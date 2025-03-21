// <copyright file="FactorClientFranceBuControllerTests.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
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

public class FactorClientFranceBuControllerTests
{
    private readonly FactorClientFranceBuController _factorClientFranceBuController;
    private readonly Mock<IFactorClientFranceBuService> _factorClientFranceBuServiceMock;
    
    public FactorClientFranceBuControllerTests()
    {
        _factorClientFranceBuServiceMock = new Mock<IFactorClientFranceBuService>();
        _factorClientFranceBuController = new FactorClientFranceBuController(_factorClientFranceBuServiceMock.Object);
    }
    
    /********** Getters **********/
    
    [Fact]
    public async Task GetListAsync_WhenCalled_ReturnsOkObjectResult()
    {
        // Arrange
        var factorClientFranceBuDto = new FactorClientFranceBuDto();
        var factorClientFranceBuDtoList = new List<FactorClientFranceBuDto> { factorClientFranceBuDto };
        _factorClientFranceBuServiceMock.Setup(x => x.GetListAsync()).ReturnsAsync(factorClientFranceBuDtoList);
        
        // Act
        var result = await _factorClientFranceBuController.GetListAsync();
        
        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<IReadOnlyList<FactorClientFranceBuDto>>(okObjectResult.Value);
        Assert.Single(model);
    }
    
    [Fact]
    public async Task GetListAsync_WhenClientIsNull_ReturnsNotFound()
    {
        // Arrange
        _factorClientFranceBuServiceMock.Setup(x => x.GetListAsync()).ReturnsAsync(new List<FactorClientFranceBuDto>());
        
        // Act
        var result = await _factorClientFranceBuController.GetListAsync();
        
        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_WhenCalled_ReturnsOkObjectResult()
    {
        // Arrange
        var factorClientFranceBuDto = new FactorClientFranceBuDto();
        var factorClientFranceBuDtoList = new List<FactorClientFranceBuDto> { factorClientFranceBuDto };
        _factorClientFranceBuServiceMock.Setup(x => x.GetByClientIdAsync(It.IsAny<int>())).ReturnsAsync(factorClientFranceBuDtoList);
        
        // Act
        var result = await _factorClientFranceBuController.GetByClientIdAsync(It.IsAny<int>());
        
        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<IReadOnlyList<FactorClientFranceBuDto>>(okObjectResult.Value);
        Assert.Single(model);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_WhenClientIsNull_ReturnsNoContent()
    {
        // Arrange
        _factorClientFranceBuServiceMock.Setup(x => x.GetByClientIdAsync(It.IsAny<int>())).ReturnsAsync((IReadOnlyList<FactorClientFranceBuDto>)null);
        
        // Act
        var result = await _factorClientFranceBuController.GetByClientIdAsync(It.IsAny<int>());
        
        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }
    
    [Fact]
    public async Task GetByBuIdAsync_WhenCalled_ReturnsOkObjectResult()
    {
        // Arrange
        var factorClientFranceBuDto = new FactorClientFranceBuDto();
        var factorClientFranceBuDtoList = new List<FactorClientFranceBuDto> { factorClientFranceBuDto };
        _factorClientFranceBuServiceMock.Setup(x => x.GetByBuIdAsync(It.IsAny<int>())).ReturnsAsync(factorClientFranceBuDtoList);
        
        // Act
        var result = await _factorClientFranceBuController.GetByBuIdAsync(It.IsAny<int>());
        
        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<IReadOnlyList<FactorClientFranceBuDto>>(okObjectResult.Value);
        Assert.Single(model);
    }
    
    [Fact]
    public async Task GetByBuIdAsync_WhenClientIsNull_ReturnsNoContent()
    {
        // Arrange
        _factorClientFranceBuServiceMock.Setup(x => x.GetByBuIdAsync(It.IsAny<int>())).ReturnsAsync((IReadOnlyList<FactorClientFranceBuDto>)null);
        
        // Act
        var result = await _factorClientFranceBuController.GetByBuIdAsync(It.IsAny<int>());
        
        // Assert
        Assert.IsType<NoContentResult>(result.Result);
    }
    
    /*********** Update ***********/
    
    [Fact]
    public async Task UpdateBatchAsync_WhenCalled_ReturnsOkObjectResult()
    {
        // Arrange
        var factorBatchRequest = new FactorBatchRequest();
        var factorClientFranceBuDto = new FactorClientFranceBuDto();
        var factorClientFranceBuDtoList = new List<FactorClientFranceBuDto> { factorClientFranceBuDto };
        _factorClientFranceBuServiceMock.Setup(x => x.UpdateBatchAsync(It.IsAny<FactorBatchRequest>())).ReturnsAsync(factorClientFranceBuDtoList);
        
        // Act
        var result = await _factorClientFranceBuController.UpdateBatchAsync(factorBatchRequest);
        
        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<IEnumerable<FactorClientFranceBuDto>>(okObjectResult.Value);
        Assert.Single(model);
    }
}