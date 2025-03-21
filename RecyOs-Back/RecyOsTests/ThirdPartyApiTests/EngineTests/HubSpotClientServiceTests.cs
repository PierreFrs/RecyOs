// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => HubSpotClientServiceTests.cs
// Created : 2024/04/18 - 15:48
// Updated : 2024/04/18 - 15:48

using Microsoft.Extensions.Logging;
using Moq;
using RecyOs.Engine.Modules.HubSpot.Services;
using RecyOs.HubSpotDB.DTO;
using RecyOs.HubSpotDB.Interfaces;

namespace RecyOsTests.EngineTests;

public class HubSpotClientServiceTests
{
    [Fact]
    public void AddItem_ShouldCallRepositoryMethod()
    {
        // Arrange
        var mockRepository = new Mock<ICompaniesService>();
        var mockLogger = new Mock<ILogger<HubSpotClientService>>();
        var service = new HubSpotClientService(mockRepository.Object, mockLogger.Object);
        var item = new CompaniesDto();

        // Act
        service.AddItem(item);

        // Assert
        mockRepository.Verify(repo => repo.CreateCompany(item), Times.Once);
    }

    [Fact]
    public void UpdateItem_ShouldCallRepositoryMethod()
    {
        // Arrange
        var mockRepository = new Mock<ICompaniesService>();
        var mockLogger = new Mock<ILogger<HubSpotClientService>>();
        var service = new HubSpotClientService(mockRepository.Object, mockLogger.Object);
        var item = new CompaniesDto();

        // Act
        service.UpdateItem(item);

        // Assert
        mockRepository.Verify(repo => repo.UpdateCompany(item), Times.Once);
    }
    
    [Fact]
    public void UpdateItem_ShouldReturnResults_WhenRepositoryReturnsResults()
    {
        // Arrange
        var mockRepository = new Mock<ICompaniesService>();
        var mockLogger = new Mock<ILogger<HubSpotClientService>>();
        var service = new HubSpotClientService(mockRepository.Object, mockLogger.Object);
        var item = new CompaniesDto();
        mockRepository.Setup(repo => repo.UpdateCompany(item)).ReturnsAsync(new CompaniesDto());

        // Act
        var result = service.UpdateItem(item);

        // Assert
        Assert.NotNull(result);
    }
}