// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => BalanceFranceControllerTests.cs
// Created : 2024/02/26 - 13:49
// Updated : 2024/02/26 - 13:49

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

namespace RecyOsTests.ControllersTests;

[Collection("BalanceTestsCollection")]
public class BalanceFranceControllerTests
{
    private readonly BalanceFranceController _balanceFranceController;
    private readonly Mock<IBalanceFranceService> _balanceFranceServiceMock;
    private readonly Mock<IEtablissementClientService> _etablissementClientServiceMock;
    public BalanceFranceControllerTests()
    {
        _balanceFranceServiceMock = new Mock<IBalanceFranceService>();
        _etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        _balanceFranceController =
            new BalanceFranceController(
                _balanceFranceServiceMock.Object, 
                _etablissementClientServiceMock.Object);
    }
    
    /* Getters */
    [Fact]
    public async Task GetAllAsync_ShouldReturnBalanceFranceList_IfItExists()
    {
        // Arrange
        var balanceFranceList = new List<BalanceDto>();
        _balanceFranceServiceMock.Setup(service => service.GetAllAsync(It.IsAny<bool>()))
            .ReturnsAsync(balanceFranceList);
        
        // Act
        var result = await _balanceFranceController.GetAllAsync();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(balanceFranceList, okResult.Value);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_IfItDoesNotExist()
    {
        // Arrange
        _balanceFranceServiceMock.Setup(service => service.GetAllAsync(It.IsAny<bool>()))
            .ReturnsAsync(new List<BalanceDto>());
        
        // Act
        var result = await _balanceFranceController.GetAllAsync();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Empty((List<BalanceDto>)okResult.Value);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnBalanceFrance_IfItExists()
    {
        // Arrange
        var balanceFrance = new BalanceDto();
        _balanceFranceServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(balanceFrance);
        
        // Act
        var result = await _balanceFranceController.GetByIdAsync(It.IsAny<int>());
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(balanceFrance, okResult.Value);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNotFound_IfItDoesNotExist()
    {
        // Arrange
        _balanceFranceServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((BalanceDto?)null);
        
        // Act
        var result = await _balanceFranceController.GetByIdAsync(It.IsAny<int>());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_SHouldReturnOk_WhenClientExists()
    {
        // Arrange
        var balanceFrance = new List<BalanceDto>();
        _etablissementClientServiceMock.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(new EtablissementClientDto());
        _balanceFranceServiceMock.Setup(service => service.GetByClientIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(balanceFrance);
        
        // Act
        var result = await _balanceFranceController.GetByClientIdAsync(It.IsAny<int>());
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(balanceFrance, okResult.Value);
    }
    
    [Fact]
    public async Task GetByClientId_ReturnsNotFound_WhenClientIsNull()
    {
        // Arrange
        _etablissementClientServiceMock.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((EtablissementClientDto?)null);
        
        // Act
        var result = await _balanceFranceController.GetByClientIdAsync(1);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    /* Create */
    
    [Fact]
    public async Task CreateAsync_ReturnsBalanceFrance_WhenCreated()
    {
        // Arrange
        var balanceFrance = new BalanceDto();
        _balanceFranceServiceMock.Setup(service => service.CreateAsync(It.IsAny<BalanceDto>()))
            .ReturnsAsync(balanceFrance);
        
        // Act
        var result = await _balanceFranceController.CreateAsync(balanceFrance);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(balanceFrance, okResult.Value);
    }
    
    [Fact]
    public async Task CreateAsync_ReturnsNotFound_WhenBalanceFranceIsNull()
    {
        // Arrange
        _balanceFranceServiceMock.Setup(service => service.CreateAsync(It.IsAny<BalanceDto>()))
            .ReturnsAsync((BalanceDto?)null);
        
        // Act
        var result = await _balanceFranceController.CreateAsync(null);
        
        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    
    /* Update */
    
    [Fact]
    public async Task UpdateAsync_ReturnsBalanceFrance_WhenUpdated()
    {
        // Arrange
        var balanceFrance = new BalanceDto();
        _balanceFranceServiceMock.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<BalanceDto>()))
            .ReturnsAsync(balanceFrance);
        
        // Act
        var result = await _balanceFranceController.UpdateAsync(1, balanceFrance);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(balanceFrance, okResult.Value);
    }
    
    [Fact]
    public async Task UpdateAsync_ReturnsNotFound_WhenBalanceFranceIsNull()
    {
        // Arrange
        _balanceFranceServiceMock.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<BalanceDto>()))
            .ReturnsAsync((BalanceDto?)null);
        
        // Act
        var result = await _balanceFranceController.UpdateAsync(1, null);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    /* Delete */
    [Fact]
    public async Task DeleteAsync_ReturnsOk_WhenDeleted()
    {
        // Arrange
        _balanceFranceServiceMock.Setup(service => service.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _balanceFranceController.DeleteAsync(1);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    
    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenBalanceFranceIsNull()
    {
        // Arrange
        _balanceFranceServiceMock.Setup(service => service.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _balanceFranceController.DeleteAsync(1);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}