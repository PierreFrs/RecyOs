using Moq;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Repository;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOsTests.Interfaces;

namespace RecyOsTests.EngineTests.Repository;

public class EngineSyncStatusRepositoryTests
{
    private readonly Mock<IDataContextEngine> _mockIDataContextEngine;
    private readonly DataContext _dataContext;

    public EngineSyncStatusRepositoryTests(IEngineDataContextTests prmEngineDataContextTests)
    {
        _mockIDataContextEngine = new Mock<IDataContextEngine>();
        _dataContext = prmEngineDataContextTests.GetContext();
        _mockIDataContextEngine.Setup(x => x.GetContext()).Returns(_dataContext);
    }

    /// <summary>
    /// Tests the GetByModuleName method of the EngineSyncStatusRepository class when a valid module name is provided.
    /// </summary>
    /// <remarks>
    /// The test verifies that the method returns the expected object with the provided module name.
    /// It asserts that the returned object's ModuleName property matches the provided module name and SyncCre property is false.
    /// </remarks>
    [Fact]
    public void GetByModuleName_ValidParam_ShouldReturnExpectedObject()
    {
        // Arrange
        var repository = new EngineSyncStatusRepository(_mockIDataContextEngine.Object);
        // Act
        var moduleName = "OdooEuropeClientModule";
        // Act
        var result = repository.GetByModuleName(moduleName);
        // Assert
        Assert.Equal(moduleName, result.ModuleName);
        Assert.False(result.SyncCre);
    }

    /// <summary>
    /// Test case to validate the behavior of the GetByModuleName method when an invalid parameter is provided.
    /// </summary>
    [Fact]
    public void GetByModuleName_InvalidParam_ShouldReturnNull()
    {
        // Arrange
        var repository = new EngineSyncStatusRepository(_mockIDataContextEngine.Object);
        // Act
        var moduleName = "InvalidModuleName";
        // Act
        var result = repository.GetByModuleName(moduleName);
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetEnabledModules_ShouldReturnListOfObjects()
    {
        // Arrange
        var repository = new EngineSyncStatusRepository(_mockIDataContextEngine.Object);
        // Act
        var result = repository.GetEnabledModules();
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<EngineSyncStatus>>(result);
        Assert.Equal(13,result.Count);
    }
}