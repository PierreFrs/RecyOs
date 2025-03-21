// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => EntrepriseNDCoverControllerTests.cs
// Created : 2023/12/19 - 15:42
// Updated : 2023/12/19 - 15:42

using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOsTests.ControllersTests;

[Collection("NDCoverTests")]
public class EntrepriseNDCoverControllerTests
{
    private readonly Mock<IEntrepriseNDCoverService> _mockEntrepriseNDCoverService;
    private readonly EntrepriseNDCoverController _controller;

    public EntrepriseNDCoverControllerTests()
    {
        _mockEntrepriseNDCoverService = new Mock<IEntrepriseNDCoverService>();
        _controller = new EntrepriseNDCoverController(_mockEntrepriseNDCoverService.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenNDCoverDoesNotExist()
    {
        // Arrange
        int testId = 1;
        _mockEntrepriseNDCoverService.Setup(service => service.GetById(testId)).ReturnsAsync((EntrepriseNDCoverDto?)null);

        // Act
        var result = await _controller.Get(testId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenNDCoverExists()
    {
        // Arrange
        int testId = 1;
        var entrepriseNDCoverDto = new EntrepriseNDCoverDto { /* set properties as needed */ };
        _mockEntrepriseNDCoverService.Setup(service => service.GetById(testId)).ReturnsAsync(entrepriseNDCoverDto);

        // Act
        var result = await _controller.Get(testId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(entrepriseNDCoverDto, okResult.Value);
    }
    
    [Fact]
    public async Task GetBySiren_ShouldReturnNotFound_WhenNDCoverDoesNotExist()
    {
        // Arrange
        string testSiren = "123456789";
        _mockEntrepriseNDCoverService.Setup(service => service.GetBySiren(testSiren)).ReturnsAsync((EntrepriseNDCoverDto?)null);

        // Act
        var result = await _controller.Get(testSiren);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetBySiren_ShouldReturnOk_WhenNDCoverExists()
    {
        // Arrange
        string testSiren = "123456789";
        var entrepriseNDCoverDto = new EntrepriseNDCoverDto { /* set properties as needed */ };
        _mockEntrepriseNDCoverService.Setup(service => service.GetBySiren(testSiren)).ReturnsAsync(entrepriseNDCoverDto);

        // Act
        var result = await _controller.Get(testSiren);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(entrepriseNDCoverDto, okResult.Value);
    }

    [Fact]
    public async Task GetDataForGrid_ShouldReturnOk_WithData()
    {
        // Arrange
        var filter = new EntrepriseNDCoverGridFilter
        {
            FilteredBSiren = "someValue",
            Refus = "someRefusValue",
            Agreement = "someAgreementValue"
        };
        var expectedItems = new List<EntrepriseNDCoverDto>
        {
            new EntrepriseNDCoverDto() 
            { 
            Id = 0,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NomPolice = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            FormeJuridiqueCode = "string",
            DateChangementPosition = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            SecteurActivite = "string",
            DateSuppression = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            Statut = "string",
            PeriodeRenouvellementOuverte = "string",
            SeraRenouvele = "string",
            DateRenouvellementPrevue = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpiration = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TempsReponse = "string",
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            Pays = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),            
            },
            new EntrepriseNDCoverDto() 
            {
                Id = 0,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NomPolice = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            FormeJuridiqueCode = "string",
            DateChangementPosition = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            SecteurActivite = "string",
            DateSuppression = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            Statut = "string",
            PeriodeRenouvellementOuverte = "string",
            SeraRenouvele = "string",
            DateRenouvellementPrevue = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpiration = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TempsReponse = "string",
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            Pays = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),            
            }
        };

        var expectedPagination = new Pagination
        {
            length = expectedItems.Count,
            size = 10,
            page = 1,
            lastPage = 1,
            startIndex = 0,
            cost = 10
        };

        var expectedData = new GridData<EntrepriseNDCoverDto>
        {
            Items = expectedItems,
            Paginator = expectedPagination
        };

        _mockEntrepriseNDCoverService.Setup(s => s.GetDataForGrid(It.IsAny<EntrepriseNDCoverGridFilter>()))
            .ReturnsAsync(expectedData);
        // Act
        var result = await _controller.GetDataForGrid(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedData = okResult.Value as GridData<EntrepriseNDCoverDto>;
        Assert.Equal(expectedData, returnedData);
    }
    
    [Fact]
    public async Task GetDataForGrid_ShouldReturnOk_WithEmptyData()
    {
        // Arrange
        var filter = new EntrepriseNDCoverGridFilter
        {
            FilteredBSiren = "someValue",
            Refus = "someRefusValue",
            Agreement = "someAgreementValue"
        };
       var expectedItems = new List<EntrepriseNDCoverDto>();
       
       var expectedPagination = new Pagination
       {
           length = expectedItems.Count,
           size = 10,
           page = 1,
           lastPage = 1,
           startIndex = 0,
           cost = 10
       };
       
       var expectedData = new GridData<EntrepriseNDCoverDto>
       {
           Items = expectedItems,
           Paginator = expectedPagination
       };
       
        _mockEntrepriseNDCoverService.Setup(s => s.GetDataForGrid(It.IsAny<EntrepriseNDCoverGridFilter>())).ReturnsAsync(expectedData);

        // Act
        var result = await _controller.GetDataForGrid(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedData = okResult.Value as GridData<EntrepriseNDCoverDto>;
        Assert.NotNull(returnedData);
        Assert.Empty(returnedData.Items);
        Assert.Equal(expectedPagination, returnedData.Paginator);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_IfEntrepriseNDCoverIsNull()
    {
        // Arrange
        EntrepriseNDCoverDto? entrepriseNDCover = null;
        
        // Act
        var result = await _controller.Put(123, entrepriseNDCover);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    
    [Fact]
    public async Task Put_ShouldReturnBadRequest_IfEntrepriseNDCoverIdIsDifferent()
    {
        // Arrange
        EntrepriseNDCoverDto updatedEntrepriseNDCover = new EntrepriseNDCoverDto
        {
            Id = 0,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NomPolice = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            FormeJuridiqueCode = "string",
            DateChangementPosition = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            SecteurActivite = "string",
            DateSuppression = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            Statut = "string",
            PeriodeRenouvellementOuverte = "string",
            SeraRenouvele = "string",
            DateRenouvellementPrevue = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpiration = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TempsReponse = "string",
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            Pays = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
        };
        
        // Act
        var result = await _controller.Put(1, updatedEntrepriseNDCover);
        
        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    
    [Fact]
    public async Task Put_ShouldReturnNotFound_IfEntrepriseNDCoverServiceSendsBackNull()
    {
        // Arrange
        EntrepriseNDCoverDto updatedEntrepriseNDCover = new EntrepriseNDCoverDto {
            Id = 0,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NomPolice = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            FormeJuridiqueCode = "string",
            DateChangementPosition = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            SecteurActivite = "string",
            DateSuppression = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            Statut = "string",
            PeriodeRenouvellementOuverte = "string",
            SeraRenouvele = "string",
            DateRenouvellementPrevue = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpiration = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TempsReponse = "string",
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            Pays = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
        };
        _mockEntrepriseNDCoverService.Setup(service => service.Edit(It.IsAny<EntrepriseNDCoverDto>()))
            .ReturnsAsync((EntrepriseNDCoverDto?)null);
        
        // Act
        var result = await _controller.Put(0, updatedEntrepriseNDCover);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Put_ShouldUpdateEntrepriseNDCover_IfSentCorrectData()
    {
        // Arrange
        EntrepriseNDCoverDto updatedEntrepriseNDCover = new EntrepriseNDCoverDto {
            Id = 0,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NomPolice = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            FormeJuridiqueCode = "string",
            DateChangementPosition = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            SecteurActivite = "string",
            DateSuppression = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            Statut = "string",
            PeriodeRenouvellementOuverte = "string",
            SeraRenouvele = "string",
            DateRenouvellementPrevue = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpiration = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TempsReponse = "string",
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            Pays = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
        };
        _mockEntrepriseNDCoverService.Setup(service => service.Edit(updatedEntrepriseNDCover))
            .ReturnsAsync(updatedEntrepriseNDCover);
        
        // Act
        var result = await _controller.Put(updatedEntrepriseNDCover.Id, updatedEntrepriseNDCover);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedNDCover = okResult.Value as EntrepriseNDCoverDto;
        Assert.Equal(updatedEntrepriseNDCover, returnedNDCover);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_IfEntrepriseNDCoverIsNull()
    {
        // Arrange
        EntrepriseNDCoverDto? entrepriseNDCover = null;
        
        // Act
        var result = await _controller.Post(entrepriseNDCover);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_IfEntrepriseNDCoverServiceReturnsNull()
    {
        // Arrange
        EntrepriseNDCoverDto updatedEntrepriseNDCover = new EntrepriseNDCoverDto {
            Id = 0,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NomPolice = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            FormeJuridiqueCode = "string",
            DateChangementPosition = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            SecteurActivite = "string",
            DateSuppression = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            Statut = "string",
            PeriodeRenouvellementOuverte = "string",
            SeraRenouvele = "string",
            DateRenouvellementPrevue = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpiration = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TempsReponse = "string",
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            Pays = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
        };
        _mockEntrepriseNDCoverService.Setup(service => service.Create(It.IsAny<EntrepriseNDCoverDto>()))
            .ReturnsAsync((EntrepriseNDCoverDto?)null);
        
        // Act
        var result = await _controller.Post(updatedEntrepriseNDCover);
        
        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    
    [Fact]
    public async Task Post_ShouldCreateEntrepriseNDCover_IfSentCorrectData()
    {
        // Arrange
        EntrepriseNDCoverDto entrepriseNDCoverDto = new EntrepriseNDCoverDto {
            Id = 1,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NomPolice = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            FormeJuridiqueCode = "string",
            DateChangementPosition = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            SecteurActivite = "string",
            DateSuppression = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            Statut = "string",
            PeriodeRenouvellementOuverte = "string",
            SeraRenouvele = "string",
            DateRenouvellementPrevue = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpiration = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TempsReponse = "string",
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            Pays = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
        };
        _mockEntrepriseNDCoverService.Setup(service => service.Edit(It.IsAny<EntrepriseNDCoverDto>()))
            .ReturnsAsync(entrepriseNDCoverDto);
        
        // Act
        var result = await _controller.Post(entrepriseNDCoverDto);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedNDCover = okResult.Value as EntrepriseNDCoverDto;
        Assert.Equal(entrepriseNDCoverDto, returnedNDCover);
    }
}