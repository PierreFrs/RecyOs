using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Controllers;
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service.vatlayer;

namespace RecyOsTests.ControllersTests;

[Collection("FournisseurEuropeTestsCollection")]
public class FournisseurEuropeControllerTests
{
    private readonly Mock<IFournisseurEuropeService> _fournisseurEuropeServiceMock;
    private readonly Mock<ISynchroWaitingToken> _synchroServiceMock;
    private readonly Mock<IVatlayerUtilitiesService> _vatlayerServiceMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly FournisseurEuropeController _controller;

    public FournisseurEuropeControllerTests()
    {
        _fournisseurEuropeServiceMock = new Mock<IFournisseurEuropeService>();
        _synchroServiceMock = new Mock<ISynchroWaitingToken>();
        _vatlayerServiceMock = new Mock<IVatlayerUtilitiesService>();
        _configMock = new Mock<IConfiguration>();

        // Configuration par défaut pour engine:writeSync
        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(x => x.Value).Returns("true");
        _configMock.Setup(x => x.GetSection("engine:writeSync"))
            .Returns(configSection.Object);

        _controller = new FournisseurEuropeController(
            _fournisseurEuropeServiceMock.Object,
            _synchroServiceMock.Object,
            _vatlayerServiceMock.Object,
            _configMock.Object
        );
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenFournisseurExists()
    {
        // Arrange
        var fournisseurEurope = new ClientEuropeDto();
        _fournisseurEuropeServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(fournisseurEurope);

        // Act
        var result = await _controller.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(fournisseurEurope, okResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenFournisseurDoesNotExist()
    {
        // Arrange
        _fournisseurEuropeServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.Get(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetDataForGrid_ReturnsOk()
    {
        // Arrange
        var filter = new ClientEuropeGridFilter();
        _fournisseurEuropeServiceMock.Setup(x => x.GetGridDataAsync(filter, false));

        // Act
        var result = await _controller.GetDataForGrid(filter);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetByVat_ReturnsOk_WhenFournisseurExists()
    {
        // Arrange
        var fournisseurEurope = new ClientEuropeDto();
        _fournisseurEuropeServiceMock.Setup(x => x.GetByVat(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(fournisseurEurope);

        // Act
        var result = await _controller.GetByVat("FR123");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(fournisseurEurope, okResult.Value);
    }

    [Fact]
    public async Task GetByVat_ReturnsNotFound_WhenFournisseurDoesNotExist()
    {
        // Arrange
        _fournisseurEuropeServiceMock.Setup(x => x.GetByVat(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.GetByVat("FR123");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsOk_AndTriggersSync()
    {
        // Arrange
        var fournisseurEurope = new ClientEuropeDto();
        _fournisseurEuropeServiceMock.Setup(x => x.Create(It.IsAny<ClientEuropeDto>()))
            .ReturnsAsync(fournisseurEurope);

        // Act
        var result = await _controller.Create(fournisseurEurope);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(fournisseurEurope, okResult.Value);
        _synchroServiceMock.Verify(x => x.StopWaiting(), Times.Once);
    }

    [Fact]
    public async Task Put_ReturnsOk_AndTriggersSync_WhenFournisseurExists()
    {
        // Arrange
        var fournisseurEurope = new ClientEuropeDto();
        _fournisseurEuropeServiceMock.Setup(x => x.Update(It.IsAny<ClientEuropeDto>()))
            .ReturnsAsync(fournisseurEurope);

        // Act
        var result = await _controller.Put(1, fournisseurEurope);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(fournisseurEurope, okResult.Value);
        _synchroServiceMock.Verify(x => x.StopWaiting(), Times.Once);
    }

    [Fact]
    public async Task Put_ReturnsNotFound_WhenFournisseurDoesNotExist()
    {
        // Arrange
        var fournisseurEurope = new ClientEuropeDto();
        _fournisseurEuropeServiceMock.Setup(x => x.Update(It.IsAny<ClientEuropeDto>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.Put(1, fournisseurEurope);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsOk_AndTriggersSync()
    {
        // Arrange
        _fournisseurEuropeServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<OkResult>(result);
        _synchroServiceMock.Verify(x => x.StopWaiting(), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenFournisseurDoesNotExist()
    {
        // Arrange
        _fournisseurEuropeServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByCodeKerlog_ReturnsOk_WhenFournisseurExists()
    {
        // Arrange
        var fournisseurEurope = new ClientEuropeDto();
        _fournisseurEuropeServiceMock.Setup(x => x.GetByCodeKerlog(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(fournisseurEurope);

        // Act
        var result = await _controller.GetByCodeKerlog("TEST123");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(fournisseurEurope, okResult.Value);
    }

    [Fact]
    public async Task GetByCodeKerlog_ReturnsNotFound_WhenFournisseurDoesNotExist()
    {
        // Arrange
        _fournisseurEuropeServiceMock.Setup(x => x.GetByCodeKerlog(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.GetByCodeKerlog("TEST123");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByCodeMkgt_ReturnsOk_WhenFournisseurExists()
    {
        // Arrange
        var fournisseurEurope = new ClientEuropeDto();
        _fournisseurEuropeServiceMock.Setup(x => x.GetByCodeMkgt(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(fournisseurEurope);

        // Act
        var result = await _controller.GetByCodeMkgt("TEST123");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(fournisseurEurope, okResult.Value);
    }

    [Fact]
    public async Task GetByCodeMkgt_ReturnsNotFound_WhenFournisseurDoesNotExist()
    {
        // Arrange
        _fournisseurEuropeServiceMock.Setup(x => x.GetByCodeMkgt(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.GetByCodeMkgt("TEST123");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByOdooId_ReturnsOk_WhenFournisseurExists()
    {
        // Arrange
        var fournisseurEurope = new ClientEuropeDto();
        _fournisseurEuropeServiceMock.Setup(x => x.GetByIdOdoo(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(fournisseurEurope);

        // Act
        var result = await _controller.GetByOdooId("123");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(fournisseurEurope, okResult.Value);
    }

    [Fact]
    public async Task GetByOdooId_ReturnsNotFound_WhenFournisseurDoesNotExist()
    {
        // Arrange
        _fournisseurEuropeServiceMock.Setup(x => x.GetByIdOdoo(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.GetByOdooId("123");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task AddByVat_ReturnsOk_WhenVatIsValid()
    {
        // Arrange
        var vat = "FR123";
        var fournisseurEurope = new ClientEuropeDto();
        _vatlayerServiceMock.Setup(x => x.CheckVatNumber(vat))
            .Returns(true);
        _vatlayerServiceMock.Setup(x => x.CreateClientEurope(vat, false, true, true))
            .ReturnsAsync(fournisseurEurope);

        // Act
        var result = await _controller.AddByVat(vat);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(fournisseurEurope, okResult.Value);
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
    public async Task AddByVat_ReturnsBadRequest_WhenFournisseurAlreadyExists()
    {
        // Arrange
        var vat = "FR123";
        var existingFournisseur = new ClientEuropeDto { IsDeleted = false, Fournisseur = true };
        _vatlayerServiceMock.Setup(x => x.CheckVatNumber(vat))
            .Returns(true);
        _fournisseurEuropeServiceMock.Setup(x => x.GetByVat(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(existingFournisseur);

        // Act
        var result = await _controller.AddByVat(vat);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("L'etablissement Fournisseur existe déjà", badRequestResult.Value);
    }

    [Fact]
    public async Task AddByVat_ReturnsNotFound_WhenVatNotFound()
    {
        // Arrange
        var vat = "FR123";
        _vatlayerServiceMock.Setup(x => x.CheckVatNumber(vat))
            .Returns(true);
        _vatlayerServiceMock.Setup(x => x.CreateClientEurope(vat, false, true, true))
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
        _vatlayerServiceMock.Setup(x => x.CreateClientEurope(vat, false, true, true))
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
        var fournisseurEurope = new ClientEuropeDto();
        _fournisseurEuropeServiceMock.Setup(x => x.CreateFromScratchAsync(vat))
            .ReturnsAsync(fournisseurEurope);

        // Act
        var result = await _controller.CreateFromScratch(vat);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(fournisseurEurope, okResult.Value);
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var fournisseurEurope = new ClientEuropeDto();
        _fournisseurEuropeServiceMock.Setup(x => x.DeleteErpCodeAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(fournisseurEurope);

        // Act
        var result = await _controller.DeleteErpCodeAsync(1, "mkgt");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(fournisseurEurope, okResult.Value);
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ReturnsNotFound_WhenFournisseurDoesNotExist()
    {
        // Arrange
        _fournisseurEuropeServiceMock.Setup(x => x.DeleteErpCodeAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.DeleteErpCodeAsync(1, "mkgt");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
