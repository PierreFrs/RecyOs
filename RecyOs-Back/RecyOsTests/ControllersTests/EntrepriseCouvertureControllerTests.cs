using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOsTests.ControllersTests;

[Collection("CouvertureTests")]
public class EntrepriseCouvertureControllerTests
{
    private readonly Mock<IEntrepriseCouvertureService> _mockEntrepriseCouvertureService;
    private readonly EntrepriseCouvertureController _controller;

    public EntrepriseCouvertureControllerTests()
    {
        _mockEntrepriseCouvertureService = new Mock<IEntrepriseCouvertureService>();
        _controller = new EntrepriseCouvertureController(_mockEntrepriseCouvertureService.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenCouvertureDoesNotExist()
    {
        // Arrange
        int testId = 1;
        _mockEntrepriseCouvertureService.Setup(service => service.GetById(testId)).ReturnsAsync((EntrepriseCouvertureDto?)null);

        // Act
        var result = await _controller.Get(testId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenCouvertureExists()
    {
        // Arrange
        int testId = 1;
        var entrepriseCouvertureDto = new EntrepriseCouvertureDto { /* set properties as needed */ };
        _mockEntrepriseCouvertureService.Setup(service => service.GetById(testId)).ReturnsAsync(entrepriseCouvertureDto);

        // Act
        var result = await _controller.Get(testId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(entrepriseCouvertureDto, okResult.Value);
    }
    
    [Fact]
    public async Task GetBySiren_ShouldReturnNotFound_WhenCouvertureDoesNotExist()
    {
        // Arrange
        string testSiren = "123456789";
        _mockEntrepriseCouvertureService.Setup(service => service.GetBySiren(testSiren)).ReturnsAsync((EntrepriseCouvertureDto?)null);

        // Act
        var result = await _controller.Get(testSiren);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetBySiren_ShouldReturnOk_WhenCouvertureExists()
    {
        // Arrange
        string testSiren = "123456789";
        var entrepriseCouvertureDto = new EntrepriseCouvertureDto { /* set properties as needed */ };
        _mockEntrepriseCouvertureService.Setup(service => service.GetBySiren(testSiren)).ReturnsAsync(entrepriseCouvertureDto);

        // Act
        var result = await _controller.Get(testSiren);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(entrepriseCouvertureDto, okResult.Value);
    }

    [Fact]
    public async Task GetDataForGrid_ShouldReturnOk_WithData()
    {
        // Arrange
        var filter = new EntrepriseCouvertureGridFilter
        {
            FilteredBSiren = "someValue",
            Refus = "someRefusValue",
            Agreement = "someAgreementValue"
        };
        var expectedItems = new List<EntrepriseCouvertureDto>
        {
            new EntrepriseCouvertureDto() 
            { 
            Id = 0,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NumeroContratExtension = "string",
            TypeGarantie = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            Notation = "string",
            DateAttributionNotation = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeReponse = "string",
            DateDecision = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantGarantie = 0,
            DeviseGarantie = "string",
            Decision = "string",
            MotifDecision = "string",
            NotreCommentaire = "string",
            CommentaireArbitre = "string",
            QuotiteGarantie = "string",
            DelaiPaiementSpecifique = 0,
            DateEffetDiffere = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpirationGarantie = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaire = 0,
            DeviseMontantTemporaire = "string",
            DateExpirationMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            QuotiteGarantieMontantTemporaire = "string",
            DelaiPaiementMontantTemporaire = 0,
            MontantDemande = 0,
            DeviseDemandee = "string",
            ConditionsPaiementDemandees = "string",
            DateExpirationDemandee = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaireDemande = 0,
            NumeroDemande = "string",
            IdDemande = 0,
            HeureReponse = "string",
            TempsReponse = "string",
            Demandeur = 0,
            DateDemandeMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            EtatRegionPays = "string",
            Pays = "string",
            ConditionsSpecifiques = "string",
            AutresConditions1 = "string",
            AutresConditions2 = "string",
            AutresConditions3 = "string",
            AutresConditions4 = "string",
            AutresConditionsTemporaires = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            RepriseGarantiePossible = "string",
            DateRepriseGarantiePossible = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            CoverGroupRole = "string",
            CoverGroupId = "string",
            EntrepriseBase = new EntrepriseBaseDto
            {
                Id = 0,
                Siren = "string",
                SirenFormate = "string",
                NomEntreprise = "string",
                PersonneMorale = true,
                Denomination = "string",
                Nom = "string",
                Prenom = "string",
                Sexe = "string",
                CodeNaf = "string",
                LibelleCodeNaf = "string",
                DomaineActivite = "string",
                DateCreation = "string",
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation = "string",
                EntrepriseEmployeuse = true,
                CategorieJuridique = "string",
                FormeJuridique = "string",
                Effectif = "string",
                EffectifMin = 0,
                EffectifMax = 0,
                TrancheEffectif = "string",
                AnneeEffectif = 0,
                Capital = 0,
                StatutRcs = "string",
                Greffe = "string",
                CodeGreffe = "string",
                NumeroRcs = "string",
                DateImmatriculationRcs = "string",
                NumeroTvaIntracommunautaire = "string",
                DateRadiationRcs = "string",
                Diffusable = true,
                DateDebutActivite = "string",
                CreateDate = "string",
                UpdatedAt = "string",
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            },
            
            },
            new EntrepriseCouvertureDto() 
            {
                Id = 0,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NumeroContratExtension = "string",
            TypeGarantie = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            Notation = "string",
            DateAttributionNotation = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeReponse = "string",
            DateDecision = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantGarantie = 0,
            DeviseGarantie = "string",
            Decision = "string",
            MotifDecision = "string",
            NotreCommentaire = "string",
            CommentaireArbitre = "string",
            QuotiteGarantie = "string",
            DelaiPaiementSpecifique = 0,
            DateEffetDiffere = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpirationGarantie = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaire = 0,
            DeviseMontantTemporaire = "string",
            DateExpirationMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            QuotiteGarantieMontantTemporaire = "string",
            DelaiPaiementMontantTemporaire = 0,
            MontantDemande = 0,
            DeviseDemandee = "string",
            ConditionsPaiementDemandees = "string",
            DateExpirationDemandee = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaireDemande = 0,
            NumeroDemande = "string",
            IdDemande = 0,
            HeureReponse = "string",
            TempsReponse = "string",
            Demandeur = 0,
            DateDemandeMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            EtatRegionPays = "string",
            Pays = "string",
            ConditionsSpecifiques = "string",
            AutresConditions1 = "string",
            AutresConditions2 = "string",
            AutresConditions3 = "string",
            AutresConditions4 = "string",
            AutresConditionsTemporaires = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            RepriseGarantiePossible = "string",
            DateRepriseGarantiePossible = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            CoverGroupRole = "string",
            CoverGroupId = "string",
            EntrepriseBase = new EntrepriseBaseDto
            {
                Id = 0,
                Siren = "string",
                SirenFormate = "string",
                NomEntreprise = "string",
                PersonneMorale = true,
                Denomination = "string",
                Nom = "string",
                Prenom = "string",
                Sexe = "string",
                CodeNaf = "string",
                LibelleCodeNaf = "string",
                DomaineActivite = "string",
                DateCreation = "string",
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation = "string",
                EntrepriseEmployeuse = true,
                CategorieJuridique = "string",
                FormeJuridique = "string",
                Effectif = "string",
                EffectifMin = 0,
                EffectifMax = 0,
                TrancheEffectif = "string",
                AnneeEffectif = 0,
                Capital = 0,
                StatutRcs = "string",
                Greffe = "string",
                CodeGreffe = "string",
                NumeroRcs = "string",
                DateImmatriculationRcs = "string",
                NumeroTvaIntracommunautaire = "string",
                DateRadiationRcs = "string",
                Diffusable = true,
                DateDebutActivite = "string",
                CreateDate = "string",
                UpdatedAt = "string",
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            },
            
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

        var expectedData = new GridData<EntrepriseCouvertureDto>
        {
            Items = expectedItems,
            Paginator = expectedPagination
        };

        _mockEntrepriseCouvertureService.Setup(s => s.GetDataForGrid(It.IsAny<EntrepriseCouvertureGridFilter>()))
            .ReturnsAsync(expectedData);
        // Act
        var result = await _controller.GetDataForGrid(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedData = okResult.Value as GridData<EntrepriseCouvertureDto>;
        Assert.Equal(expectedData, returnedData);
    }
    
    [Fact]
    public async Task GetDataForGrid_ShouldReturnOk_WithEmptyData()
    {
        // Arrange
        var filter = new EntrepriseCouvertureGridFilter
        {
            FilteredBSiren = "someValue",
            Refus = "someRefusValue",
            Agreement = "someAgreementValue"
        };
       var expectedItems = new List<EntrepriseCouvertureDto>();
       
       var expectedPagination = new Pagination
       {
           length = expectedItems.Count,
           size = 10,
           page = 1,
           lastPage = 1,
           startIndex = 0,
           cost = 10
       };
       
       var expectedData = new GridData<EntrepriseCouvertureDto>
       {
           Items = expectedItems,
           Paginator = expectedPagination
       };
       
        _mockEntrepriseCouvertureService.Setup(s => s.GetDataForGrid(It.IsAny<EntrepriseCouvertureGridFilter>())).ReturnsAsync(expectedData);

        // Act
        var result = await _controller.GetDataForGrid(filter);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedData = okResult.Value as GridData<EntrepriseCouvertureDto>;
        Assert.NotNull(returnedData);
        Assert.Empty(returnedData.Items);
        Assert.Equal(expectedPagination, returnedData.Paginator);
    }

    [Fact]
    public async Task Put_ShouldReturnBadRequest_IfEntrepriseCouvertureIsNull()
    {
        // Arrange
        EntrepriseCouvertureDto? entrepriseCouverture = null;
        
        // Act
        var result = await _controller.Put(123, entrepriseCouverture);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    
    [Fact]
    public async Task Put_ShouldReturnBadRequest_IfEntrepriseCouvertureIdIsDifferent()
    {
        // Arrange
        EntrepriseCouvertureDto updatedEntrepriseCouverture = new EntrepriseCouvertureDto
        {
            Id = 0,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NumeroContratExtension = "string",
            TypeGarantie = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            Notation = "string",
            DateAttributionNotation = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeReponse = "string",
            DateDecision = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantGarantie = 0,
            DeviseGarantie = "string",
            Decision = "string",
            MotifDecision = "string",
            NotreCommentaire = "string",
            CommentaireArbitre = "string",
            QuotiteGarantie = "string",
            DelaiPaiementSpecifique = 0,
            DateEffetDiffere = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpirationGarantie = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaire = 0,
            DeviseMontantTemporaire = "string",
            DateExpirationMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            QuotiteGarantieMontantTemporaire = "string",
            DelaiPaiementMontantTemporaire = 0,
            MontantDemande = 0,
            DeviseDemandee = "string",
            ConditionsPaiementDemandees = "string",
            DateExpirationDemandee = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaireDemande = 0,
            NumeroDemande = "string",
            IdDemande = 0,
            HeureReponse = "string",
            TempsReponse = "string",
            Demandeur = 0,
            DateDemandeMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            EtatRegionPays = "string",
            Pays = "string",
            ConditionsSpecifiques = "string",
            AutresConditions1 = "string",
            AutresConditions2 = "string",
            AutresConditions3 = "string",
            AutresConditions4 = "string",
            AutresConditionsTemporaires = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            RepriseGarantiePossible = "string",
            DateRepriseGarantiePossible = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            CoverGroupRole = "string",
            CoverGroupId = "string",
            EntrepriseBase = new EntrepriseBaseDto
            {
                Id = 0,
                Siren = "string",
                SirenFormate = "string",
                NomEntreprise = "string",
                PersonneMorale = true,
                Denomination = "string",
                Nom = "string",
                Prenom = "string",
                Sexe = "string",
                CodeNaf = "string",
                LibelleCodeNaf = "string",
                DomaineActivite = "string",
                DateCreation = "string",
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation = "string",
                EntrepriseEmployeuse = true,
                CategorieJuridique = "string",
                FormeJuridique = "string",
                Effectif = "string",
                EffectifMin = 0,
                EffectifMax = 0,
                TrancheEffectif = "string",
                AnneeEffectif = 0,
                Capital = 0,
                StatutRcs = "string",
                Greffe = "string",
                CodeGreffe = "string",
                NumeroRcs = "string",
                DateImmatriculationRcs = "string",
                NumeroTvaIntracommunautaire = "string",
                DateRadiationRcs = "string",
                Diffusable = true,
                DateDebutActivite = "string",
                CreateDate = "string",
                UpdatedAt = "string",
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            }
        };
        
        // Act
        var result = await _controller.Put(1, updatedEntrepriseCouverture);
        
        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    
    [Fact]
    public async Task Put_ShouldReturnNotFound_IfEntrepriseCouvertureServiceSendsBackNull()
    {
        // Arrange
        EntrepriseCouvertureDto updatedEntrepriseCouverture = new EntrepriseCouvertureDto {
            Id = 0,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NumeroContratExtension = "string",
            TypeGarantie = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            Notation = "string",
            DateAttributionNotation = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeReponse = "string",
            DateDecision = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantGarantie = 0,
            DeviseGarantie = "string",
            Decision = "string",
            MotifDecision = "string",
            NotreCommentaire = "string",
            CommentaireArbitre = "string",
            QuotiteGarantie = "string",
            DelaiPaiementSpecifique = 0,
            DateEffetDiffere = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpirationGarantie = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaire = 0,
            DeviseMontantTemporaire = "string",
            DateExpirationMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            QuotiteGarantieMontantTemporaire = "string",
            DelaiPaiementMontantTemporaire = 0,
            MontantDemande = 0,
            DeviseDemandee = "string",
            ConditionsPaiementDemandees = "string",
            DateExpirationDemandee = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaireDemande = 0,
            NumeroDemande = "string",
            IdDemande = 0,
            HeureReponse = "string",
            TempsReponse = "string",
            Demandeur = 0,
            DateDemandeMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            EtatRegionPays = "string",
            Pays = "string",
            ConditionsSpecifiques = "string",
            AutresConditions1 = "string",
            AutresConditions2 = "string",
            AutresConditions3 = "string",
            AutresConditions4 = "string",
            AutresConditionsTemporaires = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            RepriseGarantiePossible = "string",
            DateRepriseGarantiePossible = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            CoverGroupRole = "string",
            CoverGroupId = "string",
            EntrepriseBase = new EntrepriseBaseDto
            {
                Id = 0,
                Siren = "string",
                SirenFormate = "string",
                NomEntreprise = "string",
                PersonneMorale = true,
                Denomination = "string",
                Nom = "string",
                Prenom = "string",
                Sexe = "string",
                CodeNaf = "string",
                LibelleCodeNaf = "string",
                DomaineActivite = "string",
                DateCreation = "string",
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation = "string",
                EntrepriseEmployeuse = true,
                CategorieJuridique = "string",
                FormeJuridique = "string",
                Effectif = "string",
                EffectifMin = 0,
                EffectifMax = 0,
                TrancheEffectif = "string",
                AnneeEffectif = 0,
                Capital = 0,
                StatutRcs = "string",
                Greffe = "string",
                CodeGreffe = "string",
                NumeroRcs = "string",
                DateImmatriculationRcs = "string",
                NumeroTvaIntracommunautaire = "string",
                DateRadiationRcs = "string",
                Diffusable = true,
                DateDebutActivite = "string",
                CreateDate = "string",
                UpdatedAt = "string",
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            }
        };
        _mockEntrepriseCouvertureService.Setup(service => service.Edit(It.IsAny<EntrepriseCouvertureDto>()))
            .ReturnsAsync((EntrepriseCouvertureDto?)null);
        
        // Act
        var result = await _controller.Put(0, updatedEntrepriseCouverture);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Put_ShouldUpdateEntrepriseCouverture_IfSentCorrectData()
    {
        // Arrange
        EntrepriseCouvertureDto updatedEntrepriseCouverture = new EntrepriseCouvertureDto {
            Id = 0,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NumeroContratExtension = "string",
            TypeGarantie = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            Notation = "string",
            DateAttributionNotation = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeReponse = "string",
            DateDecision = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantGarantie = 0,
            DeviseGarantie = "string",
            Decision = "string",
            MotifDecision = "string",
            NotreCommentaire = "string",
            CommentaireArbitre = "string",
            QuotiteGarantie = "string",
            DelaiPaiementSpecifique = 0,
            DateEffetDiffere = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpirationGarantie = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaire = 0,
            DeviseMontantTemporaire = "string",
            DateExpirationMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            QuotiteGarantieMontantTemporaire = "string",
            DelaiPaiementMontantTemporaire = 0,
            MontantDemande = 0,
            DeviseDemandee = "string",
            ConditionsPaiementDemandees = "string",
            DateExpirationDemandee = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaireDemande = 0,
            NumeroDemande = "string",
            IdDemande = 0,
            HeureReponse = "string",
            TempsReponse = "string",
            Demandeur = 0,
            DateDemandeMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            EtatRegionPays = "string",
            Pays = "string",
            ConditionsSpecifiques = "string",
            AutresConditions1 = "string",
            AutresConditions2 = "string",
            AutresConditions3 = "string",
            AutresConditions4 = "string",
            AutresConditionsTemporaires = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            RepriseGarantiePossible = "string",
            DateRepriseGarantiePossible = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            CoverGroupRole = "string",
            CoverGroupId = "string",
            EntrepriseBase = new EntrepriseBaseDto
            {
                Id = 0,
                Siren = "string",
                SirenFormate = "string",
                NomEntreprise = "string",
                PersonneMorale = true,
                Denomination = "string",
                Nom = "string",
                Prenom = "string",
                Sexe = "string",
                CodeNaf = "string",
                LibelleCodeNaf = "string",
                DomaineActivite = "string",
                DateCreation = "string",
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation = "string",
                EntrepriseEmployeuse = true,
                CategorieJuridique = "string",
                FormeJuridique = "string",
                Effectif = "string",
                EffectifMin = 0,
                EffectifMax = 0,
                TrancheEffectif = "string",
                AnneeEffectif = 0,
                Capital = 0,
                StatutRcs = "string",
                Greffe = "string",
                CodeGreffe = "string",
                NumeroRcs = "string",
                DateImmatriculationRcs = "string",
                NumeroTvaIntracommunautaire = "string",
                DateRadiationRcs = "string",
                Diffusable = true,
                DateDebutActivite = "string",
                CreateDate = "string",
                UpdatedAt = "string",
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            }
        };
        _mockEntrepriseCouvertureService.Setup(service => service.Edit(updatedEntrepriseCouverture))
            .ReturnsAsync(updatedEntrepriseCouverture);
        
        // Act
        var result = await _controller.Put(updatedEntrepriseCouverture.Id, updatedEntrepriseCouverture);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCouverture = okResult.Value as EntrepriseCouvertureDto;
        Assert.Equal(updatedEntrepriseCouverture, returnedCouverture);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_IfEntrepriseCouvertureIsNull()
    {
        // Arrange
        EntrepriseCouvertureDto? entrepriseCouverture = null;
        
        // Act
        var result = await _controller.Post(entrepriseCouverture);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Post_ShouldReturnBadRequest_IfEntrepriseCouvertureServiceReturnsNull()
    {
        // Arrange
        EntrepriseCouvertureDto updatedEntrepriseCouverture = new EntrepriseCouvertureDto {
            Id = 0,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NumeroContratExtension = "string",
            TypeGarantie = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            Notation = "string",
            DateAttributionNotation = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeReponse = "string",
            DateDecision = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantGarantie = 0,
            DeviseGarantie = "string",
            Decision = "string",
            MotifDecision = "string",
            NotreCommentaire = "string",
            CommentaireArbitre = "string",
            QuotiteGarantie = "string",
            DelaiPaiementSpecifique = 0,
            DateEffetDiffere = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpirationGarantie = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaire = 0,
            DeviseMontantTemporaire = "string",
            DateExpirationMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            QuotiteGarantieMontantTemporaire = "string",
            DelaiPaiementMontantTemporaire = 0,
            MontantDemande = 0,
            DeviseDemandee = "string",
            ConditionsPaiementDemandees = "string",
            DateExpirationDemandee = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaireDemande = 0,
            NumeroDemande = "string",
            IdDemande = 0,
            HeureReponse = "string",
            TempsReponse = "string",
            Demandeur = 0,
            DateDemandeMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            EtatRegionPays = "string",
            Pays = "string",
            ConditionsSpecifiques = "string",
            AutresConditions1 = "string",
            AutresConditions2 = "string",
            AutresConditions3 = "string",
            AutresConditions4 = "string",
            AutresConditionsTemporaires = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            RepriseGarantiePossible = "string",
            DateRepriseGarantiePossible = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            CoverGroupRole = "string",
            CoverGroupId = "string",
            EntrepriseBase = new EntrepriseBaseDto
            {
                Id = 0,
                Siren = "string",
                SirenFormate = "string",
                NomEntreprise = "string",
                PersonneMorale = true,
                Denomination = "string",
                Nom = "string",
                Prenom = "string",
                Sexe = "string",
                CodeNaf = "string",
                LibelleCodeNaf = "string",
                DomaineActivite = "string",
                DateCreation = "string",
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation = "string",
                EntrepriseEmployeuse = true,
                CategorieJuridique = "string",
                FormeJuridique = "string",
                Effectif = "string",
                EffectifMin = 0,
                EffectifMax = 0,
                TrancheEffectif = "string",
                AnneeEffectif = 0,
                Capital = 0,
                StatutRcs = "string",
                Greffe = "string",
                CodeGreffe = "string",
                NumeroRcs = "string",
                DateImmatriculationRcs = "string",
                NumeroTvaIntracommunautaire = "string",
                DateRadiationRcs = "string",
                Diffusable = true,
                DateDebutActivite = "string",
                CreateDate = "string",
                UpdatedAt = "string",
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            }
        };
        _mockEntrepriseCouvertureService.Setup(service => service.Create(It.IsAny<EntrepriseCouvertureDto>()))
            .ReturnsAsync((EntrepriseCouvertureDto?)null);
        
        // Act
        var result = await _controller.Post(updatedEntrepriseCouverture);
        
        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    
    [Fact]
    public async Task Post_ShouldCreateEntrepriseCouverture_IfSentCorrectData()
    {
        // Arrange
        EntrepriseCouvertureDto entrepriseCouvertureDto = new EntrepriseCouvertureDto {
            Id = 1,
            CoverId = "string",
            NumeroContratPrimaire = "string",
            NumeroContratExtension = "string",
            TypeGarantie = "string",
            EhId = "string",
            RaisonSociale = "string",
            ReferenceClient = "string",
            Notation = "string",
            DateAttributionNotation = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeReponse = "string",
            DateDecision = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantGarantie = 0,
            DeviseGarantie = "string",
            Decision = "string",
            MotifDecision = "string",
            NotreCommentaire = "string",
            CommentaireArbitre = "string",
            QuotiteGarantie = "string",
            DelaiPaiementSpecifique = 0,
            DateEffetDiffere = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            DateExpirationGarantie = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaire = 0,
            DeviseMontantTemporaire = "string",
            DateExpirationMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            QuotiteGarantieMontantTemporaire = "string",
            DelaiPaiementMontantTemporaire = 0,
            MontantDemande = 0,
            DeviseDemandee = "string",
            ConditionsPaiementDemandees = "string",
            DateExpirationDemandee = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            MontantTemporaireDemande = 0,
            NumeroDemande = "string",
            IdDemande = 0,
            HeureReponse = "string",
            TempsReponse = "string",
            Demandeur = 0,
            DateDemandeMontantTemporaire = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
            TypeIdentifiant = "string",
            Siren = "string",
            StatutEntreprise = "string",
            NomRue = "string",
            CodePostal = 0,
            Ville = "string",
            EtatRegionPays = "string",
            Pays = "string",
            ConditionsSpecifiques = "string",
            AutresConditions1 = "string",
            AutresConditions2 = "string",
            AutresConditions3 = "string",
            AutresConditions4 = "string",
            AutresConditionsTemporaires = "string",
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            RepriseGarantiePossible = "string",
            DateRepriseGarantiePossible = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
            CoverGroupRole = "string",
            CoverGroupId = "string",
            EntrepriseBase = new EntrepriseBaseDto
            {
                Id = 0,
                Siren = "string",
                SirenFormate = "string",
                NomEntreprise = "string",
                PersonneMorale = true,
                Denomination = "string",
                Nom = "string",
                Prenom = "string",
                Sexe = "string",
                CodeNaf = "string",
                LibelleCodeNaf = "string",
                DomaineActivite = "string",
                DateCreation = "string",
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation = "string",
                EntrepriseEmployeuse = true,
                CategorieJuridique = "string",
                FormeJuridique = "string",
                Effectif = "string",
                EffectifMin = 0,
                EffectifMax = 0,
                TrancheEffectif = "string",
                AnneeEffectif = 0,
                Capital = 0,
                StatutRcs = "string",
                Greffe = "string",
                CodeGreffe = "string",
                NumeroRcs = "string",
                DateImmatriculationRcs = "string",
                NumeroTvaIntracommunautaire = "string",
                DateRadiationRcs = "string",
                Diffusable = true,
                DateDebutActivite = "string",
                CreateDate = "string",
                UpdatedAt = "string",
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            }
        };
        _mockEntrepriseCouvertureService.Setup(service => service.Edit(It.IsAny<EntrepriseCouvertureDto>()))
            .ReturnsAsync(entrepriseCouvertureDto);
        
        // Act
        var result = await _controller.Post(entrepriseCouvertureDto);
        
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCouverture = okResult.Value as EntrepriseCouvertureDto;
        Assert.Equal(entrepriseCouvertureDto, returnedCouverture);
    }
}