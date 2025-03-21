using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Controllers;
using RecyOs.Engine.Interfaces;
using RecyOs.Helpers;  // Pour GridData
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service.vatlayer;

namespace RecyOsTests.ControllersTests;

[Collection("ClientEuropeTestsCollection")]
public class ClientEuropeControllerTests
{
    private readonly Mock<IClientEuropeService> _clientEuropeServiceMock;
    private readonly Mock<ISynchroWaitingToken> _synchroServiceMock;
    private readonly Mock<IVatlayerUtilitiesService> _vatlayerServiceMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly ClientEuropeController _controller;

    public ClientEuropeControllerTests()
    {
        _clientEuropeServiceMock = new Mock<IClientEuropeService>();
        _synchroServiceMock = new Mock<ISynchroWaitingToken>();
        _vatlayerServiceMock = new Mock<IVatlayerUtilitiesService>();
        _configMock = new Mock<IConfiguration>();
        
        // Configuration par défaut pour engine:writeSync
        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(x => x.Value).Returns("true");
        _configMock.Setup(x => x.GetSection("engine:writeSync"))
            .Returns(configSection.Object);
        
        _controller = new ClientEuropeController(
            _clientEuropeServiceMock.Object,
            _synchroServiceMock.Object,
            _vatlayerServiceMock.Object,
            _configMock.Object
        );
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenClientExists()
    {
        // Arrange
        var clientEurope = new ClientEuropeDto();
        _clientEuropeServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(clientEurope);

        // Act
        var result = await _controller.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(clientEurope, okResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        _clientEuropeServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.Get(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Get_WithFilter_ReturnsOk()
    {
        // Arrange
        var filter = new ClientEuropeGridFilter();
        var gridData = new GridData<ClientEuropeDto> 
        { 
            Items = new List<ClientEuropeDto>(),
            Paginator = new Pagination { page = 1, size = 10 }
        };
        _clientEuropeServiceMock.Setup(x => x.GetGridDataAsync(filter, It.IsAny<bool>()))
            .ReturnsAsync(gridData);

        // Act
        var result = await _controller.Get(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(gridData, okResult.Value);
    }

    [Fact]
    public async Task GetByVat_ReturnsOk_WhenClientExists()
    {
        // Arrange
        var clientEurope = new ClientEuropeDto();
        _clientEuropeServiceMock.Setup(x => x.GetByVat(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(clientEurope);

        // Act
        var result = await _controller.GetByVat("FR123");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(clientEurope, okResult.Value);
    }

    [Fact]
    public async Task GetByVat_ReturnsNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        _clientEuropeServiceMock.Setup(x => x.GetByVat(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.GetByVat("FR123");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByIdOdoo_ReturnsOk_WhenClientExists()
    {
        // Arrange
        var clientEurope = new ClientEuropeDto();
        _clientEuropeServiceMock.Setup(x => x.GetByIdOdoo(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(clientEurope);

        // Act
        var result = await _controller.GetByIdOdoo("123");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(clientEurope, okResult.Value);
    }

    [Fact]
    public async Task GetGroup_ReturnsOk_WhenClientExists()
    {
        // Arrange
        var group = new GroupDto();
        _clientEuropeServiceMock.Setup(x => x.GetGroup(It.IsAny<int>()))
            .ReturnsAsync(group);

        // Act
        var result = await _controller.GetGroup(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(group, okResult.Value);
    }

    [Fact]
    public async Task Post_ReturnsOk_AndTriggersSync_WhenConfigured()
    {
        // Arrange
        var clientEurope = new ClientEuropeDto();
        _clientEuropeServiceMock.Setup(x => x.Create(It.IsAny<ClientEuropeDto>()))
            .ReturnsAsync(clientEurope);

        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(x => x.Value).Returns("true");
        _configMock.Setup(x => x.GetSection("engine:writeSync"))
            .Returns(configSection.Object);

        // Act
        var result = await _controller.Post(clientEurope);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(clientEurope, okResult.Value);
        _synchroServiceMock.Verify(x => x.StopWaiting(), Times.Once);
    }

    [Fact]
    public async Task Put_ReturnsOk_AndTriggersSync_WhenConfigured()
    {
        // Arrange
        var clientEurope = new ClientEuropeDto();
        _clientEuropeServiceMock.Setup(x => x.Update(It.IsAny<ClientEuropeDto>()))
            .ReturnsAsync(clientEurope);

        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(x => x.Value).Returns("true");
        _configMock.Setup(x => x.GetSection("engine:writeSync"))
            .Returns(configSection.Object);

        // Act
        var result = await _controller.Put(1, clientEurope);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(clientEurope, okResult.Value);
        _synchroServiceMock.Verify(x => x.StopWaiting(), Times.Once);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        var clientEurope = new ClientEuropeDto();
        _clientEuropeServiceMock.Setup(x => x.Update(It.IsAny<ClientEuropeDto>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Configuration du mock IConfiguration
        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(x => x.Value).Returns("true");
        _configMock.Setup(x => x.GetSection("engine:writeSync"))
            .Returns(configSection.Object);

        // Act
        var result = await _controller.Put(999, clientEurope);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        _clientEuropeServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool?)okResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        _clientEuropeServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task AddByVat_ReturnsOk_WhenVatIsValid()
    {
        // Arrange
        var vat = "FR123";
        var clientEurope = new ClientEuropeDto();
        _vatlayerServiceMock.Setup(x => x.CheckVatNumber(vat))
            .Returns(true);
        _vatlayerServiceMock.Setup(x => x.CreateClientEurope(vat, true, false, true))
            .ReturnsAsync(clientEurope);

        // Act
        var result = await _controller.AddByVat(vat);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(clientEurope, okResult.Value);
    }

    [Fact]
    public async Task AddByVat_ReturnsBadRequest_WhenVatIsInvalid()
    {
        // Arrange
        var vat = "INVALID";
        _vatlayerServiceMock.Setup(x => x.CheckVatNumber(vat))
            .Returns(false);

        // Act
        var result = await _controller.AddByVat(vat);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task AddByVat_ReturnsBadRequest_WhenClientAlreadyExists()
    {
        // Arrange
        var vat = "FR123";
        var existingClient = new ClientEuropeDto { IsDeleted = false, Client = true };
        _vatlayerServiceMock.Setup(x => x.CheckVatNumber(vat))
            .Returns(true);
        _clientEuropeServiceMock.Setup(x => x.GetByVat(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(existingClient);

        // Act
        var result = await _controller.AddByVat(vat);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Client déjà existant", badRequestResult.Value);
    }

    [Fact]
    public async Task AddByVat_ReturnsNotFound_WhenVatNotFound()
    {
        // Arrange
        var vat = "FR123";
        _vatlayerServiceMock.Setup(x => x.CheckVatNumber(vat))
            .Returns(true);
        _vatlayerServiceMock.Setup(x => x.CreateClientEurope(vat, true, false, true))
            .ThrowsAsync(new HttpRequestException("Not found", null, HttpStatusCode.NotFound));

        // Act
        var result = await _controller.AddByVat(vat);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
    }

    [Fact]
    public async Task AddByVat_ReturnsStatusCode500_WhenUnexpectedError()
    {
        // Arrange
        var vat = "FR123";
        _vatlayerServiceMock.Setup(x => x.CheckVatNumber(vat))
            .Returns(true);
        _vatlayerServiceMock.Setup(x => x.CreateClientEurope(vat, true, false, true))
            .ThrowsAsync(new Exception("Unexpected error"));

        // Act
        var result = await _controller.AddByVat(vat);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task CreateFromScratch_ReturnsOk()
    {
        // Arrange
        var vat = "FR123";
        var clientEurope = new ClientEuropeDto();
        _clientEuropeServiceMock.Setup(x => x.CreateFromScratchAsync(vat))
            .ReturnsAsync(clientEurope);

        // Act
        var result = await _controller.CreateFromScratch(vat);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(clientEurope, okResult.Value);
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var clientEurope = new ClientEuropeDto();
        _clientEuropeServiceMock.Setup(x => x.DeleteErpCodeAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(clientEurope);

        // Act
        var result = await _controller.DeleteErpCodeAsync(1, "mkgt");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(clientEurope, okResult.Value);
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ReturnsNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        _clientEuropeServiceMock.Setup(x => x.DeleteErpCodeAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.DeleteErpCodeAsync(1, "mkgt");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByCodeMkgt_ReturnsOk_WhenClientExists()
    {
        // Arrange
        var clientEurope = new ClientEuropeDto();
        _clientEuropeServiceMock.Setup(x => x.GetByCodeMkgt(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(clientEurope);

        // Act
        var result = await _controller.GetByCodeMkgt("TEST123");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(clientEurope, okResult.Value);
    }

    [Fact]
    public async Task GetByCodeMkgt_ReturnsNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        _clientEuropeServiceMock.Setup(x => x.GetByCodeMkgt(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.GetByCodeMkgt("TEST123");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByCodeKerlog_ReturnsOk_WhenClientExists()
    {
        // Arrange
        var clientEurope = new ClientEuropeDto();
        _clientEuropeServiceMock.Setup(x => x.GetByCodeKerlog(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(clientEurope);

        // Act
        var result = await _controller.GetByCodeKerlog("TEST123");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(clientEurope, okResult.Value);
    }

    [Fact]
    public async Task GetByCodeKerlog_ReturnsNotFound_WhenClientDoesNotExist()
    {
        // Arrange
        _clientEuropeServiceMock.Setup(x => x.GetByCodeKerlog(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.GetByCodeKerlog("TEST123");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
