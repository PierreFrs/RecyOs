// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => CategorieClientControllerTests.cs
// Created : 2023/12/26 - 14:55
// Updated : 2023/12/26 - 14:55

using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Interfaces;

namespace RecyOsTests.ControllersTests;

[Collection("CategorieClientTests")]
public class CategorieClientControllerTests
{
    private readonly Mock<ICategorieClientService<CategorieClientDto>> _mockCategorieClientService;
    private readonly CategorieClientController _controller;
    public CategorieClientControllerTests()
    {
        _mockCategorieClientService = new Mock<ICategorieClientService<CategorieClientDto>>();
        _controller = new CategorieClientController(_mockCategorieClientService.Object);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task CreateCategory_ReturnsNotFound_WhenTypeIsNullOrEmpty(string typeCategorieLabel)
    {
        // Arrange
        _mockCategorieClientService.Setup(service => service.CreateCategoryAsync(typeCategorieLabel))
            .ReturnsAsync((CategorieClientDto?)null);
        
        // Act
        var result = await _controller.CreateCategory(typeCategorieLabel);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnOk_IfCategoryIsValid()
    {
        // Arrange
        string testCategory = "testCategory";

        var categorieClientDto = new CategorieClientDto();
        _mockCategorieClientService.Setup(service => service.CreateCategoryAsync(It.IsAny<string>()))
            .ReturnsAsync(categorieClientDto);
        
        // Act
        var result = await _controller.CreateCategory(testCategory);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(categorieClientDto, okResult.Value);
    }
    
    [Fact]
    public async Task Get_ReturnsCategorieClientsList()
    {
        
        // Arrange
        var label = "SampleCategorieLabel";
        var mockUser = "TestUser";
        var mockCategorieClients = new List<CategorieClientDto>
        {
            new CategorieClientDto
            {
                CategorieLabel = label,
                CreateDate = DateTime.Now,
                CreatedBy = mockUser
            },
            new CategorieClientDto
            {
                CategorieLabel = label,
                CreateDate = DateTime.Now,
                CreatedBy = mockUser
            },
        };
        _mockCategorieClientService.Setup((service => service.GetListAsync())).ReturnsAsync(mockCategorieClients);
        
        // Act
        var result = await _controller.GetList();
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedTypes = Assert.IsAssignableFrom<List<CategorieClientDto>>(okResult.Value);
        Assert.Equal(mockCategorieClients.Count, returnedTypes.Count);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_IfTypeIsNull()
    {
        // Arrange
        var id = 1;
        _mockCategorieClientService.Setup(service => service.GetByIdAsync(id)).ReturnsAsync((CategorieClientDto?)null);
        
        // Act
        var result = await _controller.GetById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_IfCategoryIsValid()
    {
        // Arrange
        int categorieClientId = 1;
        string categoryLabel = "testCategory";

        var mockCategorieClientDto = new CategorieClientDto
        {
            Id = categorieClientId,
            CategorieLabel = categoryLabel
        };
        
        _mockCategorieClientService.Setup(service => service.GetByIdAsync(categorieClientId))
            .ReturnsAsync(mockCategorieClientDto);
        
        // Act
        var result = await _controller.GetById(categorieClientId);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsAssignableFrom<CategorieClientDto>(okResult.Value);
        Assert.Equal(categoryLabel, returnedDto.CategorieLabel);
    }
    
    [Fact]
    public async Task Update_ReturnsBadRequest_IfLabelIsNullOrWhiteSpace()
    {
        // Arrange
        var label = "";
        var id = 1;
        _mockCategorieClientService.Setup(service => service.UpdateCategorieClientAsync(id, label)).ReturnsAsync((CategorieClientDto?)null);
        
        // Act
        var result = await _controller.UpdateCategorieClient(id, label);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task Update_ReturnsNotFound_IfTypeIsNull()
    {
        // Arrange
        var label = "test type";
        var id = 1;
        _mockCategorieClientService.Setup(service => service.UpdateCategorieClientAsync(id, label)).ReturnsAsync((CategorieClientDto?)null);
        
        // Act
        var result = await _controller.UpdateCategorieClient(id, label);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Update_ShouldReturnOk_IfCategoryIsValid()
    {
        // Arrange
        int categorieClientId = 1;
        string categoryLabel = "testCategory";

        var newMockCategorieClientDto = new CategorieClientDto
        {
            Id = categorieClientId,
            CategorieLabel = categoryLabel
        };
        
        _mockCategorieClientService.Setup(service => service.UpdateCategorieClientAsync(categorieClientId, categoryLabel))
            .ReturnsAsync(newMockCategorieClientDto);
        
        // Act
        var result = await _controller.UpdateCategorieClient(categorieClientId, categoryLabel);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsAssignableFrom<CategorieClientDto>(okResult.Value);
        Assert.Equal(categoryLabel, returnedDto.CategorieLabel);
    }
    
    [Fact]
    public async Task DeleteById_ReturnsNotFound_IfDeleteFails()
    {
        // Arrange
        var id = 1;
        _mockCategorieClientService.Setup(service => service.DeleteAsync(id)).ReturnsAsync(false);
        
        // Act
        var result = await _controller.DeleteById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task DeleteById_ReturnsOk_IfDeleteSucceeds()
    {
        // Arrange
        var id = 1;
        _mockCategorieClientService.Setup(service => service.DeleteAsync(id)).ReturnsAsync(true);
        
        // Act
        var result = await _controller.DeleteById(id);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}