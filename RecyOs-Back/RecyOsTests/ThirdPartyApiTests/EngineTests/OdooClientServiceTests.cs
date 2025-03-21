using Moq;
using RecyOs.Engine.Modules.Odoo.Services;
using RecyOs.OdooDB.DTO;
using RecyOs.OdooDB.Interfaces;

namespace RecyOsTests.EngineTests;

public class OdooClientServiceTests
{
    [Fact]
    public void AddItem_ShouldCallRepositoryMethod()
    {
        // Arrange
        var mockRepository = new Mock<IResPartnerService>();
        var service = new OdooClientService(mockRepository.Object);
        var item = new ResPartnerDto();

        // Act
        service.AddItem(item);

        // Assert
        mockRepository.Verify(repo => repo.InsertPartnerAsync(item), Times.Once);
    }
    
    [Fact]
    public void UpdateItem_ShouldCallRepositoryMethod()
    {
        // Arrange
        var mockRepository = new Mock<IResPartnerService>();
        var service = new OdooClientService(mockRepository.Object);
        var item = new ResPartnerDto();

        // Act
        service.UpdateItem(item);

        // Assert
        mockRepository.Verify(repo => repo.UpdatePartnerAsync(item), Times.Once);
    }
    
    [Fact]
    public void UpdateItem_ShouldReturnResults_WhenRepositoryReturnsResults()
    {
        // Arrange
        var mockRepository = new Mock<IResPartnerService>();
        var service = new OdooClientService(mockRepository.Object);
        var item = new ResPartnerDto();
        mockRepository.Setup(repo => repo.UpdatePartnerAsync(item)).ReturnsAsync(new ResPartnerDto());

        // Act
        var result = service.UpdateItem(item);

        // Assert
        Assert.NotNull(result);
    }
}