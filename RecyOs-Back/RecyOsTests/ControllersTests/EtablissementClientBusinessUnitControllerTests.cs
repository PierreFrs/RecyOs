// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => EtablissementClientBusinessUnitControllerTests.cs
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

public class EtablissementClientBusinessUnitControllerTests
{
    private readonly Mock<IEtablissementClientBusinessUnitService<EtablissementClientBusinessUnitDto, BusinessUnitDto>> _etablissementClientBusinessUnitServiceMock;
    private readonly EtablissementClientBusinessUnitController _etablissementClientBusinessUnitController;
    public EtablissementClientBusinessUnitControllerTests()
    {
        _etablissementClientBusinessUnitServiceMock = new Mock<IEtablissementClientBusinessUnitService<EtablissementClientBusinessUnitDto, BusinessUnitDto>>();
        _etablissementClientBusinessUnitController = new EtablissementClientBusinessUnitController(_etablissementClientBusinessUnitServiceMock.Object);
    }
    
    /********** Getters **********/
    
    [Fact]
    public async Task GetByClientId_ShouldReturnOkObjectResult_WhenEtablissementClientBusinessUnitIsFound()
    {
        // Arrange
        var businessUnitDtoList = new List<BusinessUnitDto>
        {
            new BusinessUnitDto { }
        };
        
        _etablissementClientBusinessUnitServiceMock.Setup(x => x.GetByEtablissementClientIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(businessUnitDtoList);
        
        // Act
        var result = await _etablissementClientBusinessUnitController.GetByClientId(It.IsAny<int>());
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task GetByClientId_ShouldReturnNotFoundResult_WhenEtablissementClientBusinessUnitIsNotFound()
    {
        // Arrange
        _etablissementClientBusinessUnitServiceMock.Setup(x => x.GetByEtablissementClientIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((List<BusinessUnitDto>) null);
        
        // Act
        var result = await _etablissementClientBusinessUnitController.GetByClientId(It.IsAny<int>());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    /********** Create **********/
    
    [Fact]
    public async Task Create_ShouldReturnOkObjectResult_WhenEtablissementClientBusinessUnitIsCreated()
    {
        // Arrange
        var createdDto = new EtablissementClientBusinessUnitDto
        {
            ClientId = 1,
            BusinessUnitId = 1
        };
        
        _etablissementClientBusinessUnitServiceMock.Setup(x => x.CreateAsync(It.IsAny<EtablissementClientBusinessUnitDto>()))
            .ReturnsAsync(createdDto);
        
        // Act
        var result = await _etablissementClientBusinessUnitController.Create(It.IsAny<EtablissementClientBusinessUnitDto>());
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task Create_ShouldReturnNotFoundResult_WhenEtablissementClientBusinessUnitIsNotCreated()
    {
        // Arrange
        _etablissementClientBusinessUnitServiceMock.Setup(x => x.CreateAsync(It.IsAny<EtablissementClientBusinessUnitDto>()))
            .ReturnsAsync((EtablissementClientBusinessUnitDto) null);
        
        // Act
        var result = await _etablissementClientBusinessUnitController.Create(It.IsAny<EtablissementClientBusinessUnitDto>());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    /********** Delete **********/
    
    [Fact]
    public async Task Delete_ShouldReturnOkObjectResult_WhenEtablissementClientBusinessUnitIsDeleted()
    {
        // Arrange
        var dtoToDelete = new EtablissementClientBusinessUnitDto
        {
            ClientId = 1,
            BusinessUnitId = 1
        };
        
        _etablissementClientBusinessUnitServiceMock.Setup(x => x.DeleteAsync(It.IsAny<EtablissementClientBusinessUnitDto>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _etablissementClientBusinessUnitController.Delete(dtoToDelete);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task Delete_ShouldReturnNotFoundResult_WhenEtablissementClientBusinessUnitIsNotDeleted()
    {
        // Arrange
        _etablissementClientBusinessUnitServiceMock.Setup(x => x.DeleteAsync(It.IsAny<EtablissementClientBusinessUnitDto>()))
            .ReturnsAsync(false);
        
        // Act
        var result = await _etablissementClientBusinessUnitController.Delete(It.IsAny<EtablissementClientBusinessUnitDto>());
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}