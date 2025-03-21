using System.Reflection;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Engine;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Services;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Interfaces;

namespace RecyOsTests;

public class SynchroServiceTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly ISynchroWaitingToken _mockSynchroWaitingToken;
    private readonly IConfiguration _configuration;
    private readonly Mock<IEngineSyncStatusService> _engineSyncStatus;
    private readonly List<EngineSyncStatusDto> _modules;
    private readonly Mock<IMigrationStatusService> _migrationStatusService;

        
    public SynchroServiceTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockSynchroWaitingToken = new SynchroWaitingToken();
        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"engine:enabled", "true"},
                {"engine:delay", "15"},
            })
            .Build();
        _modules = new List<EngineSyncStatusDto>();
        var obj = new EngineSyncStatusDto()
        {
            ModuleName = "TestModule",
            ModuleEnabled = true
        };
        _modules.Add(obj);
        obj = new EngineSyncStatusDto()
        {
            ModuleName = null,
            ModuleEnabled = true
        };
        _modules.Add(obj);
        obj = new EngineSyncStatusDto()
        {
            ModuleName = "ModuleInexistant",
            ModuleEnabled = true
        };
        _modules.Add(obj);
        _engineSyncStatus = new Mock<IEngineSyncStatusService>();
        _engineSyncStatus.Setup(m => m.GetEnabledModules()).Returns(_modules);
        _migrationStatusService = new Mock<IMigrationStatusService>();
        _migrationStatusService.Setup(m => m.IsMigrationCompleted).Returns(true);
    }
    
    [Fact]
    public void AddModule_NullModule_ThrowsArgumentNullException()
    {
        // Arrange
        var engineService = new SynchroService(_configuration, _mockServiceProvider.Object, _mockSynchroWaitingToken, _engineSyncStatus.Object, _migrationStatusService.Object);
        IEngineModule? nullModule = null;
            
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => engineService.AddModule(nullModule));
    }
    
    [Fact]
    public void AddModule_ValidModule_AddsModuleToList()
    {
        // Arrange
        var engineService = new SynchroService(_configuration, _mockServiceProvider.Object, _mockSynchroWaitingToken, _engineSyncStatus.Object, _migrationStatusService.Object);
        var mockModule = new Mock<IEngineModule>();
            
        // Act
        engineService.AddModule(mockModule.Object);
        engineService.bypassInitModules();
            
        // Assert
        // Vous pouvez utiliser une méthode pour récupérer la liste des modules si nécessaire.
        Assert.Contains(mockModule.Object, engineService.engineModules);
    }

    [Fact]
    public void ExecuteAsync_GivenStoppingToken_ShouldCallSyncDataOnEachModule()
    {
        // Arrange
        var moduleMock1 = new Mock<IEngineModule>();
        var moduleMock2 = new Mock<IEngineModule>();
        var waitingTokenMock = new Mock<ISynchroWaitingToken>();
        var engineService = new SynchroService(_configuration, _mockServiceProvider.Object, waitingTokenMock.Object, _engineSyncStatus.Object, _migrationStatusService.Object);
        engineService.AddModule(moduleMock1.Object);
        engineService.AddModule(moduleMock2.Object);
        engineService.bypassInitModules();
        
        var cts = new CancellationTokenSource();
        cts.CancelAfter(500); // Ajuste
        
        // Act
        MethodInfo? methodInfo = typeof(SynchroService).GetMethod("ExecuteAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        methodInfo.Invoke(engineService, new object[] {cts.Token});
        
        // Assert
        moduleMock1.Verify(m => m.SyncData(), Times.Once);
        moduleMock2.Verify(m => m.SyncData(), Times.Once);
        
        // Clean
        cts.Dispose();
    }

    [Fact]
    public void ExecuteAsync_GivenIEngineWaitingToken_ShouldInterceptExceptionDataOnEachModule()
    {
        var moduleMock1 = new Mock<IEngineModule>();
        var moduleMock2 = new Mock<IEngineModule>();
        var waitingTokenMock = new Mock<ISynchroWaitingToken>();
        var engineService = new SynchroService(_configuration, _mockServiceProvider.Object, waitingTokenMock.Object, _engineSyncStatus.Object, _migrationStatusService.Object);
        engineService.AddModule(moduleMock1.Object);
        engineService.AddModule(moduleMock2.Object);
        engineService.bypassInitModules();
        
        var cts = new CancellationTokenSource();
        cts.CancelAfter(500); // Ajuste
        
        moduleMock1.Setup(m => m.SyncData()).Throws(new Exception("Test"));
        MethodInfo? methodInfo = typeof(SynchroService).GetMethod("ExecuteAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        methodInfo.Invoke(engineService, new object[] {cts.Token});
        
        // Assert
        moduleMock1.Verify(m => m.SyncData(), Times.Once);
        moduleMock2.Verify(m => m.SyncData(), Times.Once);
        
        // Clean
        cts.Dispose();
        
    }

    [Fact]
    public void ExecuteAsync_GivenIEngineWaitingToken_ShouldSkip5minutesDelay()
    {
        var moduleMock1 = new Mock<IEngineModule>();
        var moduleMock2 = new Mock<IEngineModule>();
        var engineService = new SynchroService(_configuration, _mockServiceProvider.Object, _mockSynchroWaitingToken, _engineSyncStatus.Object, _migrationStatusService.Object);
        engineService.AddModule(moduleMock1.Object);
        engineService.AddModule(moduleMock2.Object);
        engineService.bypassInitModules();
        
        var cts = new CancellationTokenSource();
        moduleMock1.Setup(m => m.SyncData()).Throws(new Exception("Test"));
        MethodInfo? methodInfo = typeof(SynchroService).GetMethod("ExecuteAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        methodInfo.Invoke(engineService, new object[] {cts.Token});
        _mockSynchroWaitingToken.StopWaiting();
        Thread.Sleep(1000);
        
        // Assert
        moduleMock1.Verify(m => m.SyncData(), Times.Exactly(2));
        moduleMock2.Verify(m => m.SyncData(), Times.Exactly(2));
        
        // Clean
        cts.Dispose();
    }
    
}
