// ClientParticulierControllerTests.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 08/10/2024
// Fichier Modifié le : 08/10/2024
// Code développé pour le projet : RecyOsTests

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RecyOs.Controllers;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Models.DTO.hub;
using ILogger = NLog.ILogger;

namespace RecyOsTests.ControllersTests;

[Collection("ClientParticulierTestsCollection")]
public class ClientParticulierControllerTests
{
    private readonly Mock<IClientParticulierService> _clientParticulierServiceMock;
    private readonly Mock<ILogger<ClientParticulierController>> _loggerMock;  // Mock for ILogger
    private readonly ClientParticulierController _clientParticulierController;

    public ClientParticulierControllerTests()
    {
        _clientParticulierServiceMock = new Mock<IClientParticulierService>();
        _loggerMock = new Mock<ILogger<ClientParticulierController>>();  // Mock the logger
        _clientParticulierController = new ClientParticulierController(
            _clientParticulierServiceMock.Object
            , _loggerMock.Object);  // Pass the mocked logger
    }

    /********** Getters **********/

    [Fact]
    public async Task GetClientParticulierById_ShouldReturnOk_IfClientParticulierExists()
    {
        // Arrange
        var clientParticulierDto = new ClientParticulierDto();
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(clientParticulierDto);

        // Act
        var result = await _clientParticulierController.GetClientParticulierById(1);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetClientParticulierById_ShouldReturnNotFound_IfClientParticulierDoesNotExist()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ThrowsAsync(new EntityNotFoundException("Client not found"));

        // Act
        var result = await _clientParticulierController.GetClientParticulierById(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetClientParticulierById_ShouldReturnStatusCode500_IfSomethingElseGoesWrong()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _clientParticulierController.GetClientParticulierById(1);

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, (result as ObjectResult)?.StatusCode);
    }
    
    [Fact]
    public async Task GetFilteredListWithCount_ShouldReturnOk_IfClientParticulierEntitiesExistInTheDatabase()
    {
        // Arrange
        var clientParticulierDto = new ClientParticulierDto();
        var gridData = new GridData<ClientParticulierDto>
        {
            Items = new List<ClientParticulierDto> { clientParticulierDto },
            Paginator = new Pagination()
        };
        _clientParticulierServiceMock.Setup(x => x.GetFilteredListWithCountAsync(It.IsAny<ClientParticulierGridFilter>(), It.IsAny<bool>()))
            .ReturnsAsync(gridData);

        // Act
        var result = await _clientParticulierController.GetFilteredListWithCount(new ClientParticulierGridFilter());

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task GetFilteredListWithCount_ShouldReturnBadRequest_IfInvalidOperationExceptionIsThrown()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.GetFilteredListWithCountAsync(It.IsAny<ClientParticulierGridFilter>(), It.IsAny<bool>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        var result = await _clientParticulierController.GetFilteredListWithCount(new ClientParticulierGridFilter());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task GetFilteredListWithCount_ShouldReturnStatusCode500_IfSomethingElseGoesWrong()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.GetFilteredListWithCountAsync(It.IsAny<ClientParticulierGridFilter>(), It.IsAny<bool>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _clientParticulierController.GetFilteredListWithCount(new ClientParticulierGridFilter());

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, (result as ObjectResult)?.StatusCode);
    }

    [Fact]
    public async Task GetClientParticulierByNameAndCityAsync_ShouldReturnOk_IfClientParticulierExists()
    {
        // Arrange
        var clientParticulierDto = new ClientParticulierDto();
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByNameAndCityAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(clientParticulierDto);

        // Act
        var result = await _clientParticulierController.GetClientParticulierByNameAndCityAsync("test", "test", "test");

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetClientParticulierByNameAndCityAsync_NoContent_IfClientDoesNotExist()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByNameAndCityAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ThrowsAsync(new EntityNotFoundException("Client not found"));

        // Act
        var result = await _clientParticulierController.GetClientParticulierByNameAndCityAsync("test", "test", "test");

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetClientParticulierByName_ShouldReturnStatusCode500_IfSomethingElseGoesWrong()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByNameAndCityAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _clientParticulierController.GetClientParticulierByNameAndCityAsync("test", "test", "test");

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, (result as ObjectResult)?.StatusCode);
    }

    [Fact]
    public async Task GetClientParticulierByCodeMkgt_ShouldReturnOk_IfClientParticulierExists()
    {
        // Arrange
        var clientParticulierDto = new ClientParticulierDto();
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByCodeMkgtAsync(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(clientParticulierDto);

        // Act
        var result = await _clientParticulierController.GetClientParticulierByCodeMkgt("test");

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetClientParticulierByCodeMkgt_ShouldReturnNotFound_IfClientParticulierDoesNotExist()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByCodeMkgtAsync(It.IsAny<string>(), It.IsAny<bool>()))
            .ThrowsAsync(new EntityNotFoundException("Client not found"));

        // Act
        var result = await _clientParticulierController.GetClientParticulierByCodeMkgt("test");

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetClientParticulierByCodeMkgt_ShouldReturnStatusCode500_IfSomethingElseGoesWrong()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByCodeMkgtAsync(It.IsAny<string>(), It.IsAny<bool>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _clientParticulierController.GetClientParticulierByCodeMkgt("test");

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, (result as ObjectResult)?.StatusCode);
    }
    
    /********** Posters **********/
    
    [Fact]
    public async Task CreateClientParticulier_ShouldReturnOk_IfClientParticulierIsCreated()
    {
        // Arrange
        var clientParticulierDto = new ClientParticulierDto();
        _clientParticulierServiceMock.Setup(x => x.CreateClientParticulierAsync(It.IsAny<ClientParticulierDto>()))
            .ReturnsAsync(clientParticulierDto);

        // Act
        var result = await _clientParticulierController.CreateClientParticulier(clientParticulierDto);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task CreateClientParticulier_ShouldReturnBadRequest_IfInvalidOperationExceptionIsThrown()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.CreateClientParticulierAsync(It.IsAny<ClientParticulierDto>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        var result = await _clientParticulierController.CreateClientParticulier(new ClientParticulierDto());

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task CreateClientParticulier_ShouldReturnStatusCode500_IfSomethingElseGoesWrong()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.CreateClientParticulierAsync(It.IsAny<ClientParticulierDto>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _clientParticulierController.CreateClientParticulier(new ClientParticulierDto());

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, (result as ObjectResult)?.StatusCode);
    }
    
    /********** Updaters **********/
    
    [Fact]
    public async Task UpdateClientParticulier_ShouldReturnOk_IfClientParticulierIsUpdated()
    {
        // Arrange
        var clientParticulierDto = new ClientParticulierDto();
        _clientParticulierServiceMock.Setup(x => x.UpdateClientParticulierAsync(It.IsAny<int>(), It.IsAny<ClientParticulierDto>()))
            .ReturnsAsync(clientParticulierDto);

        // Act
        var result = await _clientParticulierController.UpdateClientParticulier(1, clientParticulierDto);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task UpdateClientParticulier_ShouldReturnNotFound_IfEntityNotFoundExceptionIsThrown()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.UpdateClientParticulierAsync(It.IsAny<int>(), It.IsAny<ClientParticulierDto>()))
            .ThrowsAsync(new EntityNotFoundException("Client not found"));

        // Act
        var result = await _clientParticulierController.UpdateClientParticulier(1, new ClientParticulierDto());

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Fact]
    public async Task UpdateClientParticulier_ShouldReturnStatusCode500_IfSomethingElseGoesWrong()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.UpdateClientParticulierAsync(It.IsAny<int>(), It.IsAny<ClientParticulierDto>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _clientParticulierController.UpdateClientParticulier(1, new ClientParticulierDto());

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, (result as ObjectResult)?.StatusCode);
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ShouldReturnOk_IfClientParticulierIsDeleted()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.DeleteErpCodeAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(new ClientParticulierDto());

        // Act
        var result = await _clientParticulierController.DeleteErpCodeAsync(1, "mkgt");

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ShouldReturnNotFound_IfEntityNotFoundExceptionIsThrown()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.DeleteErpCodeAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((ClientParticulierDto)null);

        // Act
        var result = await _clientParticulierController.DeleteErpCodeAsync(1, "mkgt");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    /********** Deleters **********/
    
    [Fact]
    public async Task SoftDeleteClientParticulier_ShouldReturnOk_IfClientParticulierIsSoftDeleted()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.SoftDeleteClientParticulierAsync(It.IsAny<int>()))
            .ReturnsAsync(true);

        // Act
        var result = await _clientParticulierController.SoftDeleteClientParticulier(1);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task SoftDeleteClientParticulier_ShouldReturnNotFound_IfEntityNotFoundExceptionIsThrown()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.SoftDeleteClientParticulierAsync(It.IsAny<int>()))
            .ThrowsAsync(new EntityNotFoundException("Client not found"));

        // Act
        var result = await _clientParticulierController.SoftDeleteClientParticulier(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Fact]
    public async Task SoftDeleteClientParticulier_ShouldReturnBadRequest_IfArgumentExceptionIsThrown()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.SoftDeleteClientParticulierAsync(It.IsAny<int>()))
            .ThrowsAsync(new ArgumentException("Client not found"));

        // Act
        var result = await _clientParticulierController.SoftDeleteClientParticulier(1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task SoftDeleteClientParticulier_ShouldReturnStatusCode500_IfSomethingElseGoesWrong()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.SoftDeleteClientParticulierAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _clientParticulierController.SoftDeleteClientParticulier(1);

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, (result as ObjectResult)?.StatusCode);
    }
    
    [Fact]
    public async Task HardDeleteClientParticulier_ShouldReturnOk_IfClientParticulierIsHardDeleted()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.HardDeleteClientParticulierAsync(It.IsAny<int>()))
            .ReturnsAsync(true);

        // Act
        var result = await _clientParticulierController.HardDeleteClientParticulier(1);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task HardDeleteClientParticulier_ShouldReturnNotFound_IfEntityNotFoundExceptionIsThrown()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.HardDeleteClientParticulierAsync(It.IsAny<int>()))
            .ThrowsAsync(new EntityNotFoundException("Client not found"));

        // Act
        var result = await _clientParticulierController.HardDeleteClientParticulier(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Fact]
    public async Task HardDeleteClientParticulier_ShouldReturnBadRequest_IfArgumentExceptionIsThrown()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.HardDeleteClientParticulierAsync(It.IsAny<int>()))
            .ThrowsAsync(new ArgumentException("Client not found"));

        // Act
        var result = await _clientParticulierController.HardDeleteClientParticulier(1);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    [Fact]
    public async Task HardDeleteClientParticulier_ShouldReturnStatusCode500_IfSomethingElseGoesWrong()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.HardDeleteClientParticulierAsync(It.IsAny<int>()))
            .ThrowsAsync(new Exception());

        // Act
        var result = await _clientParticulierController.HardDeleteClientParticulier(1);

        // Assert
        Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, (result as ObjectResult)?.StatusCode);
    }
}