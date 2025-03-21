// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => SocieteControllerTests.cs
// Created : 2024/02/26 - 10:25
// Updated : 2024/02/26 - 10:25

using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces;

namespace RecyOsTests.ControllersTests;

[Collection("SocieteUnitTests")]
public class SocieteControllerTests
{
    private readonly Mock<ISocieteBaseService> _mockSocieteService;
    private readonly SocieteController _controller;
    public SocieteControllerTests()
    {
        _mockSocieteService = new Mock<ISocieteBaseService>();
        _controller = new SocieteController(_mockSocieteService.Object);
    }
    
    /* Getters */
    
    [Fact]
    public async Task Get_ReturnsSocietesList()
    {
        // Arrange
        var societes = new List<SocieteDto>();
        _mockSocieteService.Setup(service => service.GetListAsync())
            .ReturnsAsync(societes);
        
        // Act
        var result = await _controller.Get();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(societes, okResult.Value);
    }
    
    [Fact]
    public async Task GetById_ReturnsNotFound_WhenSocieteIsNull()
    {
        // Arrange
        _mockSocieteService.Setup(service => service.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((SocieteDto?)null);
        
        // Act
        var result = await _controller.GetById(1);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnOk_IfSocieteIsValid()
    {
        // Arrange
        var societeDto = new SocieteDto();
        _mockSocieteService.Setup(service => service.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(societeDto);
        
        // Act
        var result = await _controller.GetById(1);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(societeDto, okResult.Value);
    }
    
    /* Create */
    
    [Fact]
    public async Task Create_ReturnsNotFound_WhenSocieteIsNull()
    {
        // Arrange
        _mockSocieteService.Setup(service => service.CreateAsync(It.IsAny<SocieteDto>()))
            .ReturnsAsync((SocieteDto?)null);
        
        // Act
        var result = await _controller.Create(new SocieteDto());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Create_ShouldReturnOk_IfSocieteIsValid()
    {
        // Arrange
        var societeDto = new SocieteDto();
        _mockSocieteService.Setup(service => service.CreateAsync(It.IsAny<SocieteDto>()))
            .ReturnsAsync(societeDto);
        
        // Act
        var result = await _controller.Create(societeDto);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(societeDto, okResult.Value);
    }
    
    /* Update */
    
    [Fact]
    public async Task Update_ReturnsNotFound_WhenSocieteIsNull()
    {
        // Arrange
        _mockSocieteService.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<SocieteDto>()))
            .ReturnsAsync((SocieteDto?)null);
        
        // Act
        var result = await _controller.Update(1, new SocieteDto());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Update_ShouldReturnOk_IfSocieteIsValid()
    {
        // Arrange
        var societeDto = new SocieteDto();
        _mockSocieteService.Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<SocieteDto>()))
            .ReturnsAsync(societeDto);
        
        // Act
        var result = await _controller.Update(1, new SocieteDto());
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(societeDto, okResult.Value);
    }
    
    /* Delete */
    
    [Fact]
    public async Task Delete_ReturnsNotFound_WhenSocieteIsNull()
    {
        // Arrange
        _mockSocieteService.Setup(service => service.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _controller.Delete(1);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Delete_ShouldReturnOk_IfSocieteIsValid()
    {
        // Arrange
        _mockSocieteService.Setup(service => service.DeleteAsync(It.IsAny<int>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _controller.Delete(1);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value);
    }
}