using AutoMapper;
using Moq;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Hub.DTO;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.gpiSync;
using RecyOs.ORM.Service.gpiSync;

namespace RecyOsTests.ORMTests.ServiceTests.gpiSyncTests;

public class GpiSyncServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<IGpiSyncClientEuropeRepository<ClientEurope>> _mockGpiSyncClientEuropeRepository;
    private readonly Mock<IGpiSyncEtablissementClientRepository<EtablissementClient>> _mockGpiSyncEtablissementClientRepository;
    
    public GpiSyncServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockGpiSyncClientEuropeRepository = new Mock<IGpiSyncClientEuropeRepository<ClientEurope>>();
        _mockGpiSyncEtablissementClientRepository = new Mock<IGpiSyncEtablissementClientRepository<EtablissementClient>>();
    }
    
    [Fact]
    public async Task GetChangedCustomers_ShouldReturnClientGpiDto()
    {
        // Arrange
        var clientEurope = new ClientEurope()
        {
            Id = 100,
            CodeGpi = "C1000",
            VilleFacturation = "Liège",
            PaysFacturation = "Belgique",
        };
        var etablissementClient = new EtablissementClient()
        {
            Id = 101,
            CodeGpi = "C0101",
            CodePostalFacturation = "59320",
            VilleFacturation = "Lille",
            PaysFacturation = "France",
            EntrepriseBase = new EntrepriseBase()
            {
                Id = 101,
                NumeroRcs = "FR3890345873487"
            }
        };
        var tuple = new List<ClientEurope>();
        tuple.Add(clientEurope);
        var tuple2 = new List<EtablissementClient>();
        tuple2.Add(etablissementClient);
        _mockGpiSyncClientEuropeRepository.Setup(repo => repo.GetCreatedClientEurope(It.IsAny<ContextSession>()))
            .ReturnsAsync(tuple);
        _mockGpiSyncClientEuropeRepository.Setup(repo => repo.GetUpdatedClientEurope(It.IsAny<ContextSession>()))
            .ReturnsAsync(tuple);
        _mockGpiSyncEtablissementClientRepository.Setup(repo => repo.GetCreatedEtablissementClient(It.IsAny<ContextSession>()))
            .ReturnsAsync(tuple2);
        _mockGpiSyncEtablissementClientRepository.Setup(repo => repo.GetUpdatedEtablissementClient(It.IsAny<ContextSession>()))
            .ReturnsAsync(tuple2);
        var service = new GpiSyncService(_mockCurrentContextProvider.Object, _mockGpiSyncClientEuropeRepository.Object, _mockGpiSyncEtablissementClientRepository.Object);
        
        // Act
        var result = await service.GetChangedCustomers();
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<ClientGpiDto>>(result);
        _mockGpiSyncClientEuropeRepository.Verify(repo => repo.GetCreatedClientEurope(It.IsAny<ContextSession>()), Times.Once);
        _mockGpiSyncClientEuropeRepository.Verify(repo => repo.GetUpdatedClientEurope(It.IsAny<ContextSession>()), Times.Once);
        _mockGpiSyncEtablissementClientRepository.Verify(repo => repo.GetCreatedEtablissementClient(It.IsAny<ContextSession>()), Times.Once);
        _mockGpiSyncEtablissementClientRepository.Verify(repo => repo.GetUpdatedEtablissementClient(It.IsAny<ContextSession>()), Times.Once);
    }
}