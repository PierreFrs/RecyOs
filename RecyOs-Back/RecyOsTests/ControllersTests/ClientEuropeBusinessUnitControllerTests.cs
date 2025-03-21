// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => ClientEuropeBusinessUnitControllerTests.cs
// Created : 2024/01/24 - 12:10
// Updated : 2024/01/24 - 12:10

using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOsTests.ControllersTests;

public class ClientEuropeBusinessUnitControllerTests
{
    private readonly Mock<IClientEuropeBusinessUnitService<ClientEuropeBusinessUnitDto, BusinessUnitDto>> _clientEuropeBusinessUnitServiceMock;
    private readonly ClientEuropeBusinessUnitController _clientEuropeBusinessUnitController;
    public ClientEuropeBusinessUnitControllerTests()
    {
        _clientEuropeBusinessUnitServiceMock = new Mock<IClientEuropeBusinessUnitService<ClientEuropeBusinessUnitDto, BusinessUnitDto>>();
        _clientEuropeBusinessUnitController = new ClientEuropeBusinessUnitController(_clientEuropeBusinessUnitServiceMock.Object);
    }
    
    /********** Getters **********/
    
    [Fact]
    public async Task GetByClientId_ShouldReturnOkObjectResult_WhenClientEuropeBusinessUnitIsFound()
    {
        // Arrange
        var businessUnitDtoList = new List<BusinessUnitDto>
        {
            new BusinessUnitDto { }
        };
        
        _clientEuropeBusinessUnitServiceMock.Setup(x => x.GetByClientEuropeIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(businessUnitDtoList);
        
        // Act
        var result = await _clientEuropeBusinessUnitController.GetByClientId(It.IsAny<int>());
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task GetByClientId_ShouldReturnNotFoundResult_WhenClientEuropeBusinessUnitIsNotFound()
    {
        // Arrange
        _clientEuropeBusinessUnitServiceMock.Setup(x => x.GetByClientEuropeIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((List<BusinessUnitDto>) null);
        
        // Act
        var result = await _clientEuropeBusinessUnitController.GetByClientId(It.IsAny<int>());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    /********** Create **********/
    
    [Fact]
    public async Task Create_ShouldReturnOkObjectResult_WhenClientEuropeBusinessUnitIsCreated()
    {
        // Arrange
        var createdDto = new ClientEuropeBusinessUnitDto
        {
            ClientId = 1,
            BusinessUnitId = 1
        };
        
        _clientEuropeBusinessUnitServiceMock.Setup(x => x.CreateAsync(It.IsAny<ClientEuropeBusinessUnitDto>()))
            .ReturnsAsync(createdDto);
        
        // Act
        var result = await _clientEuropeBusinessUnitController.Create(It.IsAny<ClientEuropeBusinessUnitDto>());
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task Create_ShouldReturnNotFoundResult_WhenClientEuropeBusinessUnitIsNotCreated()
    {
        // Arrange
        _clientEuropeBusinessUnitServiceMock.Setup(x => x.CreateAsync(It.IsAny<ClientEuropeBusinessUnitDto>()))
            .ReturnsAsync((ClientEuropeBusinessUnitDto) null);
        
        // Act
        var result = await _clientEuropeBusinessUnitController.Create(It.IsAny<ClientEuropeBusinessUnitDto>());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    /********** Delete **********/
    
    [Fact]
    public async Task Delete_ShouldReturnOkObjectResult_WhenClientEuropeBusinessUnitIsDeleted()
    {
        // Arrange
        var dtoToDelete = new ClientEuropeBusinessUnitDto
        {
            ClientId = 1,
            BusinessUnitId = 1
        };
        
        _clientEuropeBusinessUnitServiceMock.Setup(x => x.DeleteAsync(It.IsAny<ClientEuropeBusinessUnitDto>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _clientEuropeBusinessUnitController.Delete(dtoToDelete);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task Delete_ShouldReturnNotFoundResult_WhenClientEuropeBusinessUnitIsNotDeleted()
    {
        // Arrange
        _clientEuropeBusinessUnitServiceMock.Setup(x => x.DeleteAsync(It.IsAny<ClientEuropeBusinessUnitDto>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _clientEuropeBusinessUnitController.Delete(It.IsAny<ClientEuropeBusinessUnitDto>());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}