// DashdocControllerTests.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 17/09/2024
// Fichier Modifié le : 17/09/2024
// Code développé pour le projet : RecyOsTests

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Models.DTO.hub;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;
using RecyOs.ThirdPartyAPIs.DashdocDB.Entities;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

namespace RecyOsTests.ControllersTests;

[Collection("DashdocTestsCollection")]
public class DashdocControllerTests
{
    private readonly Mock<IEtablissementClientService> _etablissementClientServiceMock;
    private readonly Mock<IClientEuropeService> _clientEuropeServiceMock;
    private readonly Mock<ITransportDashdocService> _dashdocServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DashdocController _controller;

    public DashdocControllerTests()
    {
        _etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        _clientEuropeServiceMock = new Mock<IClientEuropeService>();
        _dashdocServiceMock = new Mock<ITransportDashdocService>();
        _mapperMock = new Mock<IMapper>();

        _controller = new DashdocController(
            _etablissementClientServiceMock.Object,
            _clientEuropeServiceMock.Object,
            _dashdocServiceMock.Object,
            _mapperMock.Object
        );
    }
    
    /***** Create French Dashdoc Entities *****/
    
    [Fact]
    public async Task CreateFrenchDashdocCompany_EtablissementNotFound_ReturnsNotFound()
    {
        // Arrange
        int etabId = 1;
        _etablissementClientServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync((EtablissementClientDto)null);

        // Act
        var result = await _controller.CreateFrenchDashdocCompany(etabId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateFrenchDashdocCompany_IdDashdocIsNotSet_CreationFails_ReturnsBadRequest()
    {
        // Arrange
        int etabId = 1;
        var etablissement = new EtablissementClientDto
        {
            Id = etabId,
            IdDashdoc = null
        };
        var dashdocCompanyDto = new DashdocCompanyDto();

        _etablissementClientServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync(etablissement);
        _mapperMock.Setup(m => m.Map<DashdocCompanyDto>(etablissement))
            .Returns(dashdocCompanyDto);
        _dashdocServiceMock.Setup(s => s.CreateDashdocCompanyAsync(dashdocCompanyDto))
            .ReturnsAsync((DashdocCompanyDto)null);

        // Act
        var result = await _controller.CreateFrenchDashdocCompany(etabId);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateFrenchDashdocCompany_IdDashdocIsNotSet_CreationSucceeds_ReturnsOk()
    {
        // Arrange
        int etabId = 1;
        var etablissement = new EtablissementClientDto
        {
            Id = etabId,
            IdDashdoc = null
        };
        var editedEtablissement = new EtablissementClientDto
        {
            Id = etabId,
            IdDashdoc = 12345
        };
        var dashdocCompanyDto = new DashdocCompanyDto();
        var createdDashdocCompanyDto = new DashdocCompanyDto
        {
            PK = 12345
        };

        _etablissementClientServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync(etablissement);
        _mapperMock.Setup(m => m.Map<DashdocCompanyDto>(etablissement))
            .Returns(dashdocCompanyDto);
        _dashdocServiceMock.Setup(s => s.CreateDashdocCompanyAsync(dashdocCompanyDto))
            .ReturnsAsync(createdDashdocCompanyDto);
        _etablissementClientServiceMock.Setup(s => s.Edit(It.IsAny<EtablissementClientDto>()))
            .ReturnsAsync(editedEtablissement);

        // Act
        var result = await _controller.CreateFrenchDashdocCompany(etabId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEtablissement = Assert.IsType<EtablissementClientDto>(okResult.Value);
        Assert.Equal(createdDashdocCompanyDto.PK, returnedEtablissement.IdDashdoc);
    }

    [Fact]
    public async Task CreateFrenchDashdocCompany_IdDashdocIsSet_ReturnsBadRequest()
    {
        // Arrange
        int etabId = 1;
        var etablissement = new EtablissementClientDto
        {
            Id = etabId,
            IdDashdoc = 12345
        };

        _etablissementClientServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.CreateFrenchDashdocCompany(etabId);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    /***** Create Europe Dashdoc Entities *****/
    
    [Fact]
    public async Task CreateEuropeDashdocCompany_ClientNotFound_ReturnsNotFound()
    {
        // Arrange
        int etabId = 1;
        _clientEuropeServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto)null);

        // Act
        var result = await _controller.CreateEuropeDashdocCompany(etabId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateEuropeDashdocCompany_IdDashdocIsNotSet_CreationFails_ReturnsBadRequest()
    {
        // Arrange
        int etabId = 1;
        var etablissement = new ClientEuropeDto
        {
            Id = etabId,
            IdDashdoc = null
        };
        var dashdocCompanyDto = new DashdocCompanyDto();

        _clientEuropeServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync(etablissement);
        _mapperMock.Setup(m => m.Map<DashdocCompanyDto>(etablissement))
            .Returns(dashdocCompanyDto);
        _dashdocServiceMock.Setup(s => s.CreateDashdocCompanyAsync(dashdocCompanyDto))
            .ReturnsAsync((DashdocCompanyDto)null);

        // Act
        var result = await _controller.CreateEuropeDashdocCompany(etabId);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task CreateEuropeDashdocCompany_IdDashdocIsNotSet_CreationSucceeds_ReturnsOk()
    {
        // Arrange
        int etabId = 1;
        var etablissement = new ClientEuropeDto
        {
            Id = etabId,
            IdDashdoc = null
        };
        var editedClient = new ClientEuropeDto
        {
            Id = etabId,
            IdDashdoc = 12345
        };
        var dashdocCompanyDto = new DashdocCompanyDto();
        var createdDashdocCompanyDto = new DashdocCompanyDto
        {
            PK = 12345
        };

        _clientEuropeServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync(etablissement);
        _mapperMock.Setup(m => m.Map<DashdocCompanyDto>(etablissement))
            .Returns(dashdocCompanyDto);
        _dashdocServiceMock.Setup(s => s.CreateDashdocCompanyAsync(dashdocCompanyDto))
            .ReturnsAsync(createdDashdocCompanyDto);
        _clientEuropeServiceMock.Setup(s => s.Update(It.IsAny<ClientEuropeDto>()))
            .ReturnsAsync(editedClient);

        // Act
        var result = await _controller.CreateEuropeDashdocCompany(etabId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedClient = Assert.IsType<ClientEuropeDto>(okResult.Value);
        Assert.Equal(createdDashdocCompanyDto.PK, returnedClient.IdDashdoc);
    }

    [Fact]
    public async Task CreateEuropeDashdocCompany_IdDashdocIsSet_ReturnsBadRequest()
    {
        // Arrange
        int etabId = 1;
        var etablissement = new ClientEuropeDto
        {
            Id = etabId,
            IdDashdoc = 12345
        };

        _clientEuropeServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.CreateEuropeDashdocCompany(etabId);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    
    /***** Update French Dashdoc Entities *****/

    [Fact]
    public async Task UpdateFrenchDashdocCompany_EtablissementNotFound_ReturnsNotFound()
    {
        // Arrange
        int etabId = 1;
        _etablissementClientServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync((EtablissementClientDto)null);

        // Act
        var result = await _controller.UpdateFrenchDashdocCompany(etabId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateFrenchDashdocCompany_IdDashdocIsSet_UpdateSucceeds_ReturnsOk()
    {
        // Arrange
        int etabId = 1;
        var etablissement = new EtablissementClientDto
        {
            Id = etabId,
            IdDashdoc = 12345
        };
        var dashdocCompanyDto = new DashdocCompanyDto { PK = 12345 };
        var updatedDashdocCompany = new DashdocCompanyDto { PK = 12345 };

        _etablissementClientServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync(etablissement);
        _mapperMock.Setup(m => m.Map<DashdocCompanyDto>(etablissement))
            .Returns(dashdocCompanyDto);
        _dashdocServiceMock.Setup(s => s.UpdateDashdocCompanyAsync(dashdocCompanyDto))
            .ReturnsAsync(updatedDashdocCompany);
        _etablissementClientServiceMock.Setup(s => s.Edit(It.IsAny<EtablissementClientDto>()))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.UpdateFrenchDashdocCompany(etabId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEtablissement = Assert.IsType<EtablissementClientDto>(okResult.Value);
        Assert.Equal(etabId, returnedEtablissement.Id);
    }

    [Fact]
    public async Task UpdateFrenchDashdocCompany_IdDashdocIsSet_UpdateFails_ReturnsBadRequest()
    {
        // Arrange
        int etabId = 1;
        var etablissement = new EtablissementClientDto
        {
            Id = etabId,
            IdDashdoc = 12345
        };
        var dashdocCompanyDto = new DashdocCompanyDto { PK = 12345 };

        _etablissementClientServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync(etablissement);
        _mapperMock.Setup(m => m.Map<DashdocCompanyDto>(etablissement))
            .Returns(dashdocCompanyDto);
        _dashdocServiceMock.Setup(s => s.UpdateDashdocCompanyAsync(dashdocCompanyDto))
            .ReturnsAsync((DashdocCompanyDto)null);

        // Act
        var result = await _controller.UpdateFrenchDashdocCompany(etabId);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task UpdateFrenchDashdocCompany_IdDashdocIsNotSet_ReturnsBadRequest()
    {
        // Arrange
        int etabId = 1;
        var etablissement = new EtablissementClientDto
        {
            Id = etabId,
            IdDashdoc = null
        };

        _etablissementClientServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync(etablissement);

        // Act
        var result = await _controller.UpdateFrenchDashdocCompany(etabId);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    
    /***** Update Europe Dashdoc Entities *****/

    [Fact]
    public async Task UpdateEuropeDashdocCompany_ClientNotFound_ReturnsNotFound()
    {
        // Arrange
        int etabId = 1;
        _clientEuropeServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeDto)null);

        // Act
        var result = await _controller.UpdateEuropeDashdocCompany(etabId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateEuropeDashdocCompany_IdDashdocIsSet_UpdateSucceeds_ReturnsOk()
    {
        // Arrange
        int etabId = 1;
        var clientEurope = new ClientEuropeDto
        {
            Id = etabId,
            IdDashdoc = 12345
        };
        var dashdocCompanyDto = new DashdocCompanyDto { PK = 12345 };
        var updatedDashdocCompany = new DashdocCompanyDto { PK = 12345 };

        _clientEuropeServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync(clientEurope);
        _mapperMock.Setup(m => m.Map<DashdocCompanyDto>(clientEurope))
            .Returns(dashdocCompanyDto);
        _dashdocServiceMock.Setup(s => s.UpdateDashdocCompanyAsync(dashdocCompanyDto))
            .ReturnsAsync(updatedDashdocCompany);
        _clientEuropeServiceMock.Setup(s => s.Update(It.IsAny<ClientEuropeDto>()))
            .ReturnsAsync(clientEurope);

        // Act
        var result = await _controller.UpdateEuropeDashdocCompany(etabId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedClient = Assert.IsType<ClientEuropeDto>(okResult.Value);
        Assert.Equal(etabId, returnedClient.Id);
    }

    [Fact]
    public async Task UpdateEuropeDashdocCompany_IdDashdocIsSet_UpdateFails_ReturnsBadRequest()
    {
        // Arrange
        int etabId = 1;
        var clientEurope = new ClientEuropeDto
        {
            Id = etabId,
            IdDashdoc = 12345
        };
        var dashdocCompanyDto = new DashdocCompanyDto { PK = 12345 };

        _clientEuropeServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync(clientEurope);
        _mapperMock.Setup(m => m.Map<DashdocCompanyDto>(clientEurope))
            .Returns(dashdocCompanyDto);
        _dashdocServiceMock.Setup(s => s.UpdateDashdocCompanyAsync(dashdocCompanyDto))
            .ReturnsAsync((DashdocCompanyDto)null);

        // Act
        var result = await _controller.UpdateEuropeDashdocCompany(etabId);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task UpdateEuropeDashdocCompany_IdDashdocIsNotSet_ReturnsBadRequest()
    {
        // Arrange
        int etabId = 1;
        var clientEurope = new ClientEuropeDto
        {
            Id = etabId,
            IdDashdoc = null
        };

        _clientEuropeServiceMock.Setup(s => s.GetById(etabId, It.IsAny<bool>()))
            .ReturnsAsync(clientEurope);

        // Act
        var result = await _controller.UpdateEuropeDashdocCompany(etabId);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
}