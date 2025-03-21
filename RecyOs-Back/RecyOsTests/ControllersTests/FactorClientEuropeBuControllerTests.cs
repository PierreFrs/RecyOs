// <copyright file="FactorClientEuropeBuControllerTests.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOsTests.ControllersTests;

[Collection("FactorClientBuTestsCollection")]

public class FactorClientEuropeBuControllerTests
{
    private readonly FactorClientEuropeBuController _factorClientEuropeBuController;
    private readonly Mock<IFactorClientEuropeBuService> _factorClientEuropeBuServiceMock;
    
    public FactorClientEuropeBuControllerTests()
    {
        _factorClientEuropeBuServiceMock = new Mock<IFactorClientEuropeBuService>();
        _factorClientEuropeBuController = new FactorClientEuropeBuController(_factorClientEuropeBuServiceMock.Object);
    }
    
    /********** Getters **********/
    
    [Fact]
    public async Task GetListAsync_WhenCalled_ReturnsOkObjectResult()
    {
        // Arrange
        var factorClientEuropeBuDto = new FactorClientEuropeBuDto();
        var factorClientEuropeBuDtoList = new List<FactorClientEuropeBuDto> { factorClientEuropeBuDto };
        _factorClientEuropeBuServiceMock.Setup(x => x.GetListAsync()).ReturnsAsync(factorClientEuropeBuDtoList);
        
        // Act
        var result = await _factorClientEuropeBuController.GetListAsync();
        
        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<IReadOnlyList<FactorClientEuropeBuDto>>(okObjectResult.Value);
        Assert.Single(model);
    }
    
    [Fact]
    public async Task GetListAsync_WhenClientIsNull_ReturnsNotFound()
    {
        // Arrange
        _factorClientEuropeBuServiceMock.Setup(x => x.GetListAsync()).ReturnsAsync(new List<FactorClientEuropeBuDto>());
        
        // Act
        var result = await _factorClientEuropeBuController.GetListAsync();
        
        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_WhenCalled_ReturnsOkObjectResult()
    {
        // Arrange
        var factorClientEuropeBuDto = new FactorClientEuropeBuDto();
        var factorClientEuropeBuDtoList = new List<FactorClientEuropeBuDto> { factorClientEuropeBuDto };
        _factorClientEuropeBuServiceMock.Setup(x => x.GetByClientIdAsync(It.IsAny<int>())).ReturnsAsync(factorClientEuropeBuDtoList);
        
        // Act
        var result = await _factorClientEuropeBuController.GetByClientIdAsync(It.IsAny<int>());
        
        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<IReadOnlyList<FactorClientEuropeBuDto>>(okObjectResult.Value);
        Assert.Single(model);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_WhenClientIsNull_ReturnsNoContent()
    {
        // Arrange
        _factorClientEuropeBuServiceMock.Setup(x => x.GetByClientIdAsync(It.IsAny<int>())).ReturnsAsync((IReadOnlyList<FactorClientEuropeBuDto>)null);
        
        // Act
        var result = await _factorClientEuropeBuController.GetByClientIdAsync(It.IsAny<int>());
        
        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }
    
    [Fact]
    public async Task GetByBuIdAsync_WhenCalled_ReturnsOkObjectResult()
    {
        // Arrange
        var factorClientEuropeBuDto = new FactorClientEuropeBuDto();
        var factorClientEuropeBuDtoList = new List<FactorClientEuropeBuDto> { factorClientEuropeBuDto };
        _factorClientEuropeBuServiceMock.Setup(x => x.GetByBuIdAsync(It.IsAny<int>())).ReturnsAsync(factorClientEuropeBuDtoList);
        
        // Act
        var result = await _factorClientEuropeBuController.GetByBuIdAsync(It.IsAny<int>());
        
        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<IReadOnlyList<FactorClientEuropeBuDto>>(okObjectResult.Value);
        Assert.Single(model);
    }
    
    [Fact]
    public async Task GetByBuIdAsync_WhenClientIsNull_ReturnsNoContent()
    {
        // Arrange
        _factorClientEuropeBuServiceMock.Setup(x => x.GetByBuIdAsync(It.IsAny<int>())).ReturnsAsync((IReadOnlyList<FactorClientEuropeBuDto>)null);
        
        // Act
        var result = await _factorClientEuropeBuController.GetByBuIdAsync(It.IsAny<int>());
        
        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }
    
    /*********** Update ***********/
    
    [Fact]
    public async Task UpdateBatchAsync_WhenCalled_ReturnsOkObjectResult()
    {
        // Arrange
        var factorBatchRequest = new FactorBatchRequest();
        var factorClientEuropeBuDto = new FactorClientEuropeBuDto();
        var factorClientEuropeBuDtoList = new List<FactorClientEuropeBuDto> { factorClientEuropeBuDto };
        _factorClientEuropeBuServiceMock.Setup(x => x.UpdateBatchAsync(It.IsAny<FactorBatchRequest>())).ReturnsAsync(factorClientEuropeBuDtoList);
        
        // Act
        var result = await _factorClientEuropeBuController.UpdateBatchAsync(factorBatchRequest);
        
        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<IEnumerable<FactorClientEuropeBuDto>>(okObjectResult.Value);
        Assert.Single(model);
    }
}