using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.OdooDB.DTO;
using RecyOs.OdooDB.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Models.DTO.hub;

namespace RecyOsTests.ControllersTests;

[Collection("PartnerOdooTestsCollection")]
public class PartnerOdooControllerTests
{
    private readonly Mock<IEtablissementClientService> _etablissementClientServiceMock;
    private readonly Mock<IClientEuropeService> _clientEuropeServiceMock;
    private readonly Mock<IClientParticulierService> _clientParticulierServiceMock;
    private readonly Mock<IResPartnerService> _resPartnerServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IEtablissementFournisseurService> _etablissementFournisseurServiceMock;
    private readonly Mock<IFournisseurEuropeService> _fournisseurEuropeServiceMock;
    private readonly PartnerOdooController _controller;

    public PartnerOdooControllerTests()
    {
        _etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        _clientEuropeServiceMock = new Mock<IClientEuropeService>();
        _clientParticulierServiceMock = new Mock<IClientParticulierService>();
        _resPartnerServiceMock = new Mock<IResPartnerService>();
        _mapperMock = new Mock<IMapper>();
        _etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        _fournisseurEuropeServiceMock = new Mock<IFournisseurEuropeService>();

        _controller = new PartnerOdooController(
            _etablissementClientServiceMock.Object,
            _resPartnerServiceMock.Object,
            _mapperMock.Object,
            _clientEuropeServiceMock.Object,
            _clientParticulierServiceMock.Object,
            _etablissementFournisseurServiceMock.Object,
            _fournisseurEuropeServiceMock.Object
        );
    }

    [Fact]
    public async Task CreateFrenchCli_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var etablissement = new EtablissementClientDto { IdOdoo = null };
        var resPartner = new ResPartnerDto { Id = 123 };
        _etablissementClientServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissement);
        _mapperMock.Setup(x => x.Map<ResPartnerDto>(etablissement))
            .Returns(new ResPartnerDto());
        _resPartnerServiceMock.Setup(x => x.InsertPartnerAsync(It.IsAny<ResPartnerDto>()))
            .ReturnsAsync(resPartner);

        // Act
        var result = await _controller.createFrenchCli(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEtablissement = Assert.IsType<EtablissementClientDto>(okResult.Value);
        Assert.Equal("123", returnedEtablissement.IdOdoo);
    }

    [Fact]
    public async Task CreateFrenchCli_ReturnsNotFound_WhenEtablissementNotFound()
    {
        // Arrange
        _etablissementClientServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((EtablissementClientDto?)null);

        // Act
        var result = await _controller.createFrenchCli(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateFrenchCli_ReturnsBadRequest_WhenIdOdooExists()
    {
        // Arrange
        var etablissement = new EtablissementClientDto { IdOdoo = "123" };
        _etablissementClientServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.createFrenchCli(1);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateFrenchCli_ReturnsBadRequest_WhenSiretIsInvalid()
    {
        // Arrange
        var etablissement = new EtablissementClientDto { IdOdoo = null };
        _etablissementClientServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissement);
        _mapperMock.Setup(x => x.Map<ResPartnerDto>(etablissement))
            .Returns(new ResPartnerDto());
        _resPartnerServiceMock.Setup(x => x.InsertPartnerAsync(It.IsAny<ResPartnerDto>()))
            .ReturnsAsync((ResPartnerDto?)null); // Simule un échec d'insertion

        // Act
        var result = await _controller.createFrenchCli(1);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateEuropeCli_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var clientEurope = new ClientEuropeDto { IdOdoo = null };
        var resPartner = new ResPartnerDto { Id = 123 };
        _clientEuropeServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(clientEurope);
        _mapperMock.Setup(x => x.Map<ResPartnerDto>(clientEurope))
            .Returns(new ResPartnerDto());
        _resPartnerServiceMock.Setup(x => x.InsertPartnerAsync(It.IsAny<ResPartnerDto>()))
            .ReturnsAsync(resPartner);

        // Act
        var result = await _controller.CreateEuropeCli(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedClient = Assert.IsType<ClientEuropeDto>(okResult.Value);
        Assert.Equal("123", returnedClient.IdOdoo);
    }

    [Fact]
    public async Task CreateEuropeCli_ReturnsNotFound_WhenClientNotFound()
    {
        // Arrange
        _clientEuropeServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.CreateEuropeCli(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateEuropeCli_ReturnsBadRequest_WhenSiretIsInvalid()
    {
        // Arrange
        var clientEurope = new ClientEuropeDto { IdOdoo = null };
        _clientEuropeServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(clientEurope);
        _mapperMock.Setup(x => x.Map<ResPartnerDto>(clientEurope))
            .Returns(new ResPartnerDto());
        _resPartnerServiceMock.Setup(x => x.InsertPartnerAsync(It.IsAny<ResPartnerDto>()))
            .ReturnsAsync((ResPartnerDto?)null); // Simule un échec d'insertion

        // Act
        var result = await _controller.CreateEuropeCli(1);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    

    [Fact]
    public async Task CreateEuropeCli_ReturnsBadRequest_WhenIdOdooExists()
    {
        // Arrange
        var clientEurope = new ClientEuropeDto { IdOdoo = "123" };
        _clientEuropeServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(clientEurope);

        // Act
        var result = await _controller.CreateEuropeCli(1);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateParticulierCli_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var clientParticulier = new ClientParticulierDto { IdOdoo = null };
        var resPartner = new ResPartnerDto { Id = 123 };
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(clientParticulier);
        _mapperMock.Setup(x => x.Map<ResPartnerDto>(clientParticulier))
            .Returns(new ResPartnerDto());
        _resPartnerServiceMock.Setup(x => x.InsertPartnerAsync(It.IsAny<ResPartnerDto>()))
            .ReturnsAsync(resPartner);

        // Act
        var result = await _controller.CreateParticulierCli(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedClient = Assert.IsType<ClientParticulierDto>(okResult.Value);
        Assert.Equal("123", returnedClient.IdOdoo);
    }

    [Fact]
    public async Task CreateFrenchFrn_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var etablissement = new EtablissementClientDto { IdOdoo = null };
        var resPartner = new ResPartnerDto { Id = 123 };
        _etablissementFournisseurServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissement);
        _mapperMock.Setup(x => x.Map<ResPartnerDto>(etablissement))
            .Returns(new ResPartnerDto());
        _resPartnerServiceMock.Setup(x => x.InsertPartnerAsync(It.IsAny<ResPartnerDto>()))
            .ReturnsAsync(resPartner);

        // Act
        var result = await _controller.CreateFrenchFrn(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEtablissement = Assert.IsType<EtablissementClientDto>(okResult.Value);
        Assert.Equal("123", returnedEtablissement.IdOdoo);
    }

    [Fact]
    public async Task CreateEuropeFrn_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var fournisseurEurope = new ClientEuropeDto { IdOdoo = null };
        var resPartner = new ResPartnerDto { Id = 123 };
        _fournisseurEuropeServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(fournisseurEurope);
        _mapperMock.Setup(x => x.Map<ResPartnerDto>(fournisseurEurope))
            .Returns(new ResPartnerDto());
        _resPartnerServiceMock.Setup(x => x.InsertPartnerAsync(It.IsAny<ResPartnerDto>()))
            .ReturnsAsync(resPartner);

        // Act
        var result = await _controller.CreateEuropeFrn(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedFournisseur = Assert.IsType<ClientEuropeDto>(okResult.Value);
        Assert.Equal("123", returnedFournisseur.IdOdoo);
    }

    [Fact]
    public async Task CreateEuropeFrn_ReturnsNotFound_WhenFournisseurNotFound()
    {
        // Arrange
        _fournisseurEuropeServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto?)null);

        // Act
        var result = await _controller.CreateEuropeFrn(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateParticulierCli_ReturnsBadRequest_WhenSiretIsInvalid()
    {
        // Arrange
        var clientParticulier = new ClientParticulierDto { IdOdoo = null };
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(clientParticulier);
        _mapperMock.Setup(x => x.Map<ResPartnerDto>(clientParticulier))
            .Returns(new ResPartnerDto());
        _resPartnerServiceMock.Setup(x => x.InsertPartnerAsync(It.IsAny<ResPartnerDto>()))
            .ReturnsAsync((ResPartnerDto?)null); // Simule un échec d'insertion

        // Act
        var result = await _controller.CreateParticulierCli(1);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateParticulierCli_ReturnsNotFound_WhenClientNotFound()
    {
        // Arrange
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientParticulierDto?)null);

        // Act
        var result = await _controller.CreateParticulierCli(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

     [Fact]
    public async Task CreateParticulierCli_ReturnsBadRequest_WhenAlreadyExists()
    {
        // Arrange
        var clientParticulier = new ClientParticulierDto { IdOdoo = "1342" };
        _clientParticulierServiceMock.Setup(x => x.GetClientParticulierByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(clientParticulier);

        // Act
        var result = await _controller.CreateParticulierCli(1);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateFrenchFrn_ReturnsBadRequest_WhenIdOdooExists()
    {
        // Arrange
        var etablissement = new EtablissementClientDto { IdOdoo = "123" };
        _etablissementFournisseurServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.CreateFrenchFrn(1);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateFrenchFrn_ReturnsNotFound_WhenEtablissementNotFound()
    {
        // Arrange
        _etablissementFournisseurServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((EtablissementClientDto?)null);

        // Act
        var result = await _controller.CreateFrenchFrn(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }


    [Fact]
    public async Task CreateFrenchFrn_ReturnsBadRequest_WhenParnerInsertionFails()
    {
        var etablissement = new EtablissementClientDto { IdOdoo = null };
        _etablissementFournisseurServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissement);
        _mapperMock.Setup(x => x.Map<ResPartnerDto>(etablissement))
            .Returns(new ResPartnerDto());
        _resPartnerServiceMock.Setup(x => x.InsertPartnerAsync(It.IsAny<ResPartnerDto>()))
            .ReturnsAsync((ResPartnerDto?)null);

        // Act
        var result = await _controller.CreateFrenchFrn(1);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateEuropeFrn_ReturnsBadRequest_WhenIdOdooExists()
    {
        // Arrange
        var fournisseurEurope = new ClientEuropeDto { IdOdoo = "123" };
        _fournisseurEuropeServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(fournisseurEurope);

        // Act
        var result = await _controller.CreateEuropeFrn(1);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateEuropeFrn_ReturnsBadRequest_WhenInsertionFails()
    {
        // Arrange
        var fournisseurEurope = new ClientEuropeDto { IdOdoo = null };
        _fournisseurEuropeServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(fournisseurEurope);
        _mapperMock.Setup(x => x.Map<ResPartnerDto>(fournisseurEurope))
            .Returns(new ResPartnerDto());
        _resPartnerServiceMock.Setup(x => x.InsertPartnerAsync(It.IsAny<ResPartnerDto>()))
            .ReturnsAsync((ResPartnerDto?)null); // Simule l'échec de l'insertion

        // Act
        var result = await _controller.CreateEuropeFrn(1);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}
