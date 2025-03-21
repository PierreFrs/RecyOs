// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => BalanceEuropeControllerTests.cs
// Created : 2024/02/26 - 14:48
// Updated : 2024/02/26 - 14:48

using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

namespace RecyOsTests.ControllersTests;

[Collection("BalanceTestsCollection")]
public class BalanceEuropeControllerTests
{
    private readonly BalanceEuropeController _balanceEuropeController;
    private readonly Mock<IBalanceEuropeService> _balanceEuropeServiceMock;
    private readonly Mock<IClientEuropeService> _clientEuropeServiceMock;
    public BalanceEuropeControllerTests()
    {
        _balanceEuropeServiceMock = new Mock<IBalanceEuropeService>();
        _clientEuropeServiceMock = new Mock<IClientEuropeService>();
        _balanceEuropeController =
            new BalanceEuropeController(
                _balanceEuropeServiceMock.Object, 
                _clientEuropeServiceMock.Object);
    }
    
    /* Getters */
    [Fact]
    public async Task GetAllAsync_ShouldReturnBalanceEuropeList_IfItExists()
    {
        // Arrange
        var balanceEuropeList = new List<BalanceDto>();
        _balanceEuropeServiceMock.Setup(service => service.GetAllAsync(It.IsAny<bool>()))
            .ReturnsAsync(balanceEuropeList);
        
        // Act
        var result = await _balanceEuropeController.GetAllAsync();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(balanceEuropeList, okResult.Value);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_IfItDoesNotExist()
    {
        // Arrange
        _balanceEuropeServiceMock.Setup(service => service.GetAllAsync(It.IsAny<bool>()))
            .ReturnsAsync(new List<BalanceDto>());
        
        // Act
        var result = await _balanceEuropeController.GetAllAsync();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Empty((List<BalanceDto>)okResult.Value);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnBalanceEurope_IfItExists()
    {
        // Arrange
        var balanceEurope = new BalanceDto();
        _balanceEuropeServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(balanceEurope);
        
        // Act
        var result = await _balanceEuropeController.GetByIdAsync(It.IsAny<int>());
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(balanceEurope, okResult.Value);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNotFound_IfItDoesNotExist()
    {
        // Arrange
        _balanceEuropeServiceMock.Setup(service => service.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((BalanceDto?)null);
        
        // Act
        var result = await _balanceEuropeController.GetByIdAsync(It.IsAny<int>());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Get_ReturnsBalanceEuropeList()
    {
        // Arrange
        var balanceEurope = new List<BalanceDto>();
        _clientEuropeServiceMock.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(new ClientEuropeDto());
        _balanceEuropeServiceMock.Setup(service => service.GetByClientIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(balanceEurope);
        
        // Act
        var result = await _balanceEuropeController.GetByClientIdAsync(It.IsAny<int>());
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(balanceEurope, okResult.Value);
    }
    
    [Fact]
    public async Task GetByClientId_ReturnsNotFound_WhenClientIsNull()
    {
        // Arrange
        _clientEuropeServiceMock.Setup(service => service.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto?)null);
        
        // Act
        var result = await _balanceEuropeController.GetByClientIdAsync(1);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    /* Create */
    
    [Fact]
    public async Task CreateAsync_ReturnsBalanceEurope_WhenCreated()
    {
        // Arrange
        var balanceEurope = new BalanceDto();
        _balanceEuropeServiceMock.Setup(service => service.CreateAsync(It.IsAny<BalanceDto>()))
            .ReturnsAsync(balanceEurope);
        
        // Act
        var result = await _balanceEuropeController.CreateAsync(balanceEurope);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(balanceEurope, okResult.Value);
    }
    
    [Fact]
    public async Task CreateAsync_ReturnsNotFound_WhenBalanceEuropeIsNull()
    {
        // Arrange
        _balanceEuropeServiceMock.Setup(service => service.CreateAsync(It.IsAny<BalanceDto>()))
            .ReturnsAsync((BalanceDto?)null);
        
        // Act
        var result = await _balanceEuropeController.CreateAsync(null);
        
        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    
    /* Update */
    
    [Fact]
    public async Task UpdateAsync_ReturnsBalanceEurope_WhenUpdated()
    {
        // Arrange
        var balanceEurope = new BalanceDto();
        _balanceEuropeServiceMock.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<BalanceDto>()))
            .ReturnsAsync(balanceEurope);
        
        // Act
        var result = await _balanceEuropeController.UpdateAsync(1, balanceEurope);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(balanceEurope, okResult.Value);
    }
    
    [Fact]
    public async Task UpdateAsync_ReturnsNotFound_WhenBalanceEuropeIsNull()
    {
        // Arrange
        _balanceEuropeServiceMock.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<BalanceDto>()))
            .ReturnsAsync((BalanceDto?)null);
        
        // Act
        var result = await _balanceEuropeController.UpdateAsync(1, null);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    /* Delete */
    [Fact]
    public async Task DeleteAsync_ReturnsOk_WhenDeleted()
    {
        // Arrange
        _balanceEuropeServiceMock.Setup(service => service.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _balanceEuropeController.DeleteAsync(1);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    
    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenBalanceEuropeIsNull()
    {
        // Arrange
        _balanceEuropeServiceMock.Setup(service => service.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _balanceEuropeController.DeleteAsync(1);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}