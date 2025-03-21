using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Controllers;
using RecyOs.Engine.Interfaces;
using RecyOs.Helpers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Requests;
using RecyOs.ORM.Results;
using RecyOs.ORM.Service.pappers;

namespace RecyOsTests.ControllersTests;

[Collection("EtablissementClientTestsCollection")]
public class EtablissementClientControllerTests
{
    private readonly Mock<IEtablissementClientService> _etablissementClientServiceMock;
    private readonly Mock<IPappersUtilitiesService> _pappersUtilitiesServiceMock;
    private readonly Mock<ISynchroWaitingToken> _synchroServiceMock;
    private readonly Mock<IConfiguration> _configMock;
    private readonly EtablissementClientController _controller;

    public EtablissementClientControllerTests()
    {
        _etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        _pappersUtilitiesServiceMock = new Mock<IPappersUtilitiesService>();
        _synchroServiceMock = new Mock<ISynchroWaitingToken>();
        _configMock = new Mock<IConfiguration>();

        // Configuration par défaut pour engine:writeSync
        var configSection = new Mock<IConfigurationSection>();
        configSection.Setup(x => x.Value).Returns("true");
        _configMock.Setup(x => x.GetSection("engine:writeSync"))
            .Returns(configSection.Object);

        _controller = new EtablissementClientController(
            _etablissementClientServiceMock.Object,
            _pappersUtilitiesServiceMock.Object,
            _synchroServiceMock.Object,
            _configMock.Object
        );
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenEtablissementExists()
    {
        // Arrange
        var etablissement = new EtablissementClientDto();
        _etablissementClientServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(etablissement, okResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenEtablissementDoesNotExist()
    {
        // Arrange
        _etablissementClientServiceMock.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync((EtablissementClientDto?)null);

        // Act
        var result = await _controller.Get(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetDataForGrid_ReturnsOk()
    {
        // Arrange
        var filter = new EtablissementClientGridFilter();
        var gridData = new GridData<EtablissementClientDto>
        {
            Items = new List<EtablissementClientDto>(),
            Paginator = new Pagination { page = 1, size = 10 }
        };
        _etablissementClientServiceMock.Setup(x => x.GetDataForGrid(filter, It.IsAny<bool>()))
            .ReturnsAsync(gridData);

        // Act
        var result = await _controller.GetDataForGrid(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(gridData, okResult.Value);
    }

    [Fact]
    public async Task GetBySiret_ReturnsOk_WhenEtablissementExists()
    {
        // Arrange
        var etablissement = new EtablissementClientDto();
        _etablissementClientServiceMock.Setup(x => x.GetBySiret(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.GetBySiret("12345678901234");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(etablissement, okResult.Value);
    }

    [Fact]
    public async Task GetBySiret_ReturnsNotFound_WhenEtablissementDoesNotExist()
    {
        // Arrange
        _etablissementClientServiceMock.Setup(x => x.GetBySiret(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((EtablissementClientDto?)null);

        // Act
        var result = await _controller.GetBySiret("12345678901234");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetGroup_ReturnsOk_WhenGroupExists()
    {
        // Arrange
        var group = new GroupDto();
        _etablissementClientServiceMock.Setup(x => x.GetGroup(It.IsAny<int>()))
            .ReturnsAsync(group);

        // Act
        var result = await _controller.GetGroup(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(group, okResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsOk()
    {
        // Arrange
        var etablissement = new EtablissementClientDto();
        var createdEtablissement = new EtablissementClientDto();
        _etablissementClientServiceMock.Setup(x => x.Edit(etablissement))
            .ReturnsAsync(createdEtablissement);

        // Act
        var result = await _controller.Create(etablissement);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(createdEtablissement, okResult.Value);
    }

    [Fact]
    public async Task Edit_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var etablissement = new EtablissementClientDto { Id = 1 };
        var updatedEtablissement = new EtablissementClientDto();
        _etablissementClientServiceMock.Setup(x => x.Edit(etablissement))
            .ReturnsAsync(updatedEtablissement);

        // Act
        var result = await _controller.Edit(1, etablissement);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(updatedEtablissement, okResult.Value);
    }

    [Fact]
    public async Task Edit_ReturnsBadRequest_WhenIdsDoNotMatch()
    {
        // Arrange
        var etablissement = new EtablissementClientDto { Id = 2 };

        // Act
        var result = await _controller.Edit(1, etablissement);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Les identifiants ne correspondent pas", badRequestResult.Value);
    }

    [Fact]
    public async Task Edit_Returns480_WhenDuplicateCodeMkgt()
    {
        // Arrange
        var etablissement = new EtablissementClientDto { Id = 1 };
        _etablissementClientServiceMock.Setup(x => x.Edit(etablissement))
            .ThrowsAsync(new Exception("", new Exception("", new Exception("", new Exception("Cannot insert duplicate key row in object 'dbo.EtablissementClient' with unique index 'IX_EtablissementClient_code_mkgt'.")))));

        // Act
        var result = await _controller.Edit(1, etablissement);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(480, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Edit_Returns500_WhenExceptionOccurs()
    {
        // Arrange
        var etablissement = new EtablissementClientDto { Id = 1 };
        _etablissementClientServiceMock.Setup(x => x.Edit(etablissement))
            .ThrowsAsync(new Exception("Erreur webService"));

        // Act
        var result = await _controller.Edit(1, etablissement);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task ChangeSiret_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var request = new SiretUpdateRequest { Siret = "12345678901234" };
        var result = new ServiceResult { Success = true };
        _etablissementClientServiceMock.Setup(x => x.ChangeSiretAsync(1, request.Siret, true, false, It.IsAny<ContextSession>()))
            .ReturnsAsync(result);

        // Act
        var actionResult = await _controller.ChangeSiret(1, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Same(result, okResult.Value);
    }

    [Fact]
    public async Task ChangeSiret_ReturnsBadRequest_WhenSiretIsEmpty()
    {
        // Arrange
        var request = new SiretUpdateRequest { Siret = "" };

        // Act
        var result = await _controller.ChangeSiret(1, request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ChangeSiret_Returns404_WhenEtablissementDoesNotExist()
    {
        // Arrange
        var request = new SiretUpdateRequest { Siret = "12345678901234" };
        _etablissementClientServiceMock.Setup(x => x.ChangeSiretAsync(1, request.Siret, true, false, It.IsAny<ContextSession>()))
            .ReturnsAsync(new ServiceResult { Success = false, Message = "Etablissement non trouvé", StatusCode = StatusCodes.Status404NotFound });

        // Act
        var result = await _controller.ChangeSiret(1, request);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ChangeSiret_Returns400_WhenSiretIsInvalid()
    {
        // Arrange
        var request = new SiretUpdateRequest { Siret = "invalid" };
        _etablissementClientServiceMock.Setup(x => x.ChangeSiretAsync(1, request.Siret, true, false, It.IsAny<ContextSession>()))
            .ReturnsAsync(new ServiceResult { Success = false, Message = "Siret invalide", StatusCode = StatusCodes.Status400BadRequest });

        // Act
        var result = await _controller.ChangeSiret(1, request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        _etablissementClientServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenEtablissementDoesNotExist()
    {
        // Arrange
        _etablissementClientServiceMock.Setup(x => x.DeleteAsync(It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByCodeKerlog_ReturnsOk_WhenEtablissementExists()
    {
        // Arrange
        var etablissement = new EtablissementClientDto();
        _etablissementClientServiceMock.Setup(x => x.GetByCodeKerlog(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.GetByCodeKerlog("TEST123");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(etablissement, okResult.Value);
    }


    [Fact]
    public async Task GetByCodeKerlog_Returns404_WhenEtablissementDoesNotExist()
    {
        // Arrange
        _etablissementClientServiceMock.Setup(x => x.GetByCodeKerlog(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((EtablissementClientDto?)null);

        // Act
        var result = await _controller.GetByCodeKerlog("TEST123");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByCodeMkgt_ReturnsOk_WhenEtablissementExists()
    {
        // Arrange
        var etablissement = new EtablissementClientDto();
        _etablissementClientServiceMock.Setup(x => x.GetByCodeMkgt(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.GetByCodeMkgt("TEST123");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(etablissement, okResult.Value);
    }

    [Fact]
    public async Task GetByCodeMkgt_Returns404_WhenEtablissementDoesNotExist()
    {
        // Arrange
        _etablissementClientServiceMock.Setup(x => x.GetByCodeMkgt(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((EtablissementClientDto?)null);

        // Act
        var result = await _controller.GetByCodeMkgt("TEST123");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetByOdooId_ReturnsOk_WhenEtablissementExists()
    {
        // Arrange
        var etablissement = new EtablissementClientDto();
        _etablissementClientServiceMock.Setup(x => x.GetByIdOdoo(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.GetByOdooId("123");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(etablissement, okResult.Value);
    }

    [Fact]
    public async Task GetByOdooId_Returns404_WhenEtablissementDoesNotExist()
    {
        // Arrange
        _etablissementClientServiceMock.Setup(x => x.GetByIdOdoo(It.IsAny<string>(), It.IsAny<bool>()))
            .ReturnsAsync((EtablissementClientDto?)null);

        // Act
        var result = await _controller.GetByOdooId("123");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task AddBySiret_ReturnsOk_WhenSiretIsValid()
    {
        // Arrange
        var siret = "12345678901234";
        var etablissement = new EtablissementClientDto();
        _pappersUtilitiesServiceMock.Setup(x => x.CheckSiret(siret))
            .Returns(true);
        _pappersUtilitiesServiceMock.Setup(x => x.CreateEtablissementClientBySiret(siret, true, false, false)) 
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.AddBySiret(siret);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(etablissement, okResult.Value);
    }

    [Fact]
    public async Task AddBySiret_ReturnsBadRequest_WhenSiretIsInvalid()
    {
        // Arrange
        var siret = "invalid";
        _pappersUtilitiesServiceMock.Setup(x => x.CheckSiret(siret))
            .Returns(false);

        // Act
        var result = await _controller.AddBySiret(siret);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Le siret n'est pas valide", badRequestResult.Value);
    }

     [Fact]
    public async Task AddBySiret_Returns400_WhenEtablissementAlreadyExists()
    {
        // Arrange
        var siret = "12345678901234";
        var etablissement = new EtablissementClientDto(){
            Client = true,
            Fournisseur = false,
            IsDeleted = false,
            Id = 1,
            CodeKerlog = "TEST123",
            CodeMkgt = "TEST123",
            IdOdoo = "123",
            Siret = siret,
            Nom = "TEST",
            AdresseFacturation1 = "TEST"
        };
        _pappersUtilitiesServiceMock.Setup(x => x.CheckSiret(siret))
            .Returns(true);
        _etablissementClientServiceMock.Setup(x => x.GetBySiret(siret, false))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.AddBySiret(siret);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("L'etablissementClient existe déjà", badRequestResult.Value);
    }

    [Fact]
    public async Task AddBySiret_Returns404_WhenPappersUtilitiesServiceThrowsHttpRequestExceptionShouldBeCatchAndReturn404()
    {
        // Arrange
        var siret = "12345678901234";
        _pappersUtilitiesServiceMock.Setup(x => x.CheckSiret(siret))
            .Throws(new HttpRequestException("Erreur webService", null, HttpStatusCode.NotFound));  //404 status code

        // Act
        var result = await _controller.AddBySiret(siret);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task AddBySiret_Returns500_WhenExceptionOccurs()
    {
        // Arrange
        var siret = "12345678901234";
        _pappersUtilitiesServiceMock.Setup(x => x.CheckSiret(siret))
            .Throws(new Exception("Erreur webService"));

        // Act
        var result = await _controller.AddBySiret(siret);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
    
    [Fact]
    public async Task CreateFromScratch_ReturnsOk()
    {
        // Arrange
        var siret = "12345678901234";
        var etablissement = new EtablissementClientDto();
        _etablissementClientServiceMock.Setup(x => x.CreateFromScratchAsync(siret))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.CreateFromScratch(siret);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(etablissement, okResult.Value);
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var etablissement = new EtablissementClientDto();
        _etablissementClientServiceMock.Setup(x => x.DeleteErpCodeAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.DeleteErpCodeAsync(1, "mkgt");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Same(etablissement, okResult.Value);
    }

    [Fact]
    public async Task DeleteErpCodeAsync_ReturnsNotFound_WhenEtablissementDoesNotExist()
    {
        // Arrange
        _etablissementClientServiceMock.Setup(x => x.DeleteErpCodeAsync(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((EtablissementClientDto?)null);

        // Act
        var result = await _controller.DeleteErpCodeAsync(1, "mkgt");

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
