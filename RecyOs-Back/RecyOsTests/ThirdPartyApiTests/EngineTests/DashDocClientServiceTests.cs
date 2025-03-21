using Microsoft.Extensions.Logging;
using Moq;
using RecyOs.Engine.Modules.DashDoc.Services;
using RecyOs.HubSpotDB.DTO;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

namespace RecyOsTests.THirdPartyApiTests.EngineTests;

public class DashDocClientServiceTests
{
    [Fact]
    public void AddItem_ShouldCallRepositoryMethod()
    {
        // Arrange
        var mockRepository = new Mock<ITransportDashdocService>();
        var mockLogger = new Mock<ILogger<DashDocClientService>>();
        var service = new DashDocClientService(mockRepository.Object, mockLogger.Object);
        var item = new DashdocCompanyDto()
        {
            Name = "Test",
            PK = 1
            
        };

        // Act
        service.AddItem(item);

        // Assert
        mockRepository.Verify(repo => repo.CreateDashdocCompanyAsync(item), Times.Once);
    }
    
    [Fact]
    public void UpdateItem_ShouldCallRepositoryMethod()
    {
        // Arrange
        var mockRepository = new Mock<ITransportDashdocService>();
        var mockLogger = new Mock<ILogger<DashDocClientService>>();
        var service = new DashDocClientService(mockRepository.Object, mockLogger.Object);
        var item = new DashdocCompanyDto()
        {
            Name = "Test",
            PK = 1
            
        };

        // Act
        service.UpdateItem(item);

        // Assert
        mockRepository.Verify(repo => repo.UpdateDashdocCompanyAsync(item), Times.Once);
    }
}