using System.Globalization;
using AutoMapper;
using Moq;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service.hub;

namespace RecyOsTests.ORMTests.hubTests;

[Collection("CouvertureTests")]
public class EntrepriseCouvertureServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<IEntrepriseCouvertureRepository<EntrepriseCouverture>> _mockEntrepriseCouvertureRepository;
    private readonly IMapper _mapper;
    private readonly EntrepriseCouvertureService<EntrepriseCouverture> _mockEntrepriseCouvertureService;

    public EntrepriseCouvertureServiceTests()
    {
        
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockEntrepriseCouvertureRepository = new Mock<IEntrepriseCouvertureRepository<EntrepriseCouverture>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<EntrepriseCouverture, EntrepriseCouvertureDto>();
            cfg.CreateMap<EntrepriseCouvertureDto, EntrepriseCouverture>();
                
            cfg.CreateMap<EntrepriseBase, EntrepriseBaseDto>()
                .ForMember(dest => dest.Diffusable, opt => opt.MapFrom(src => true));
            cfg.CreateMap<EntrepriseBaseDto, EntrepriseBase>();
        });
        _mapper = mapperConfig.CreateMapper();
        _mockEntrepriseCouvertureService = new EntrepriseCouvertureService<EntrepriseCouverture>(
            _mockCurrentContextProvider.Object, 
            _mockEntrepriseCouvertureRepository.Object, 
            _mapper);
    }

    
    [Fact]
    public async Task GetById_ShouldReturnAnEntrepriseCouvertureDto_IfGivenAValidId()
    {
        // Arrange
        var mockEntrepriseCouverture = new EntrepriseCouverture()
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
            EntrepriseBase = new EntrepriseBase
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
                DateCreation = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
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
                DateDebutActivite = "string",
                CreateDate = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
                UpdatedAt = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            }
        };
        
        _mockEntrepriseCouvertureRepository
            .Setup(repo => repo.Get(mockEntrepriseCouverture.Id, It.IsAny<ContextSession>()))
            .ReturnsAsync(mockEntrepriseCouverture);

        // Act
        var result = await _mockEntrepriseCouvertureService.GetById(mockEntrepriseCouverture.Id);
        var dtoResult = Assert.IsType<EntrepriseCouvertureDto>(result);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<EntrepriseCouvertureDto>(result);
        Assert.Equal(mockEntrepriseCouverture.Id, dtoResult.Id);
        Assert.Equal(mockEntrepriseCouverture.EntrepriseBase.Id, dtoResult.EntrepriseBase.Id);

    }
    
    [Fact]
    public async Task GetBySiren_ShouldReturnAnEntrepriseCouvertureDto_IfGivenAValidSiren() 
    {
        // Arrange
        var mockEntrepriseCouverture = new EntrepriseCouverture()
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
            EntrepriseBase = new EntrepriseBase
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
                DateCreation = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
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
                DateDebutActivite = "string",
                CreateDate = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
                UpdatedAt = DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            }
        };
        
        _mockEntrepriseCouvertureRepository
            .Setup(repo => repo.GetBySiren(mockEntrepriseCouverture.Siren, It.IsAny<ContextSession>()))
            .ReturnsAsync(mockEntrepriseCouverture);

        // Act
        var result = await _mockEntrepriseCouvertureService.GetBySiren(mockEntrepriseCouverture.Siren);
        var dtoResult = Assert.IsType<EntrepriseCouvertureDto>(result);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<EntrepriseCouvertureDto>(result);
        Assert.Equal(mockEntrepriseCouverture.Siren, dtoResult.Siren);
        Assert.Equal(mockEntrepriseCouverture.EntrepriseBase.Siren, dtoResult.EntrepriseBase.Siren);
    }
    
    [Fact]
    public async Task GetDataForGrid_ShouldReturnAFilteredGrid_IfGivenAValidEntrepriseCouvertureGridFilter() 
    {
        // Arrange
        int pageSize = 10;
        int pageNumber = 1;
        int totalItemCount = 50;
        int expectedLastPage = (int)Math.Ceiling((double)totalItemCount / pageSize);
        int expectedStartIndex = 10;
        
        var filter = new EntrepriseCouvertureGridFilter
        {
            FilteredBSiren = "someValue",
            Refus = "someRefusValue",
            Agreement = "someAgreementValue",
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var mockData = new List<EntrepriseCouverture>
        {
            new EntrepriseCouverture() 
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
            EntrepriseBase = new EntrepriseBase
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
                DateCreation =  DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation =  DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
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
                DateDebutActivite = "string",
                CreateDate =  DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
                UpdatedAt =  DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            },
            
            },
            new EntrepriseCouverture() 
            {
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
            EntrepriseBase = new EntrepriseBase
            {
                Id = 1,
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
                DateCreation =  DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation =  DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
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
                DateDebutActivite = "string",
                CreateDate =  DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
                UpdatedAt =  DateTime.Parse("2023-12-07T13:22:01.394Z", CultureInfo.InvariantCulture),
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            },
            
            }
        };
        
        _mockEntrepriseCouvertureRepository
            .Setup(repo => repo.GetFilteredListWithCount(filter, It.IsAny<ContextSession>()))
            .ReturnsAsync((mockData.AsEnumerable(), totalItemCount));

        
        // Act
        var result = await _mockEntrepriseCouvertureService.GetDataForGrid(filter);
        
        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.Equal(mockData.Count, result.Items.Count());
        Assert.Equal(totalItemCount, result.Paginator.length);
        Assert.Equal(pageSize, result.Paginator.size);
        Assert.Equal(pageNumber, result.Paginator.page);
        Assert.Equal(expectedLastPage, result.Paginator.lastPage);
        Assert.Equal(expectedStartIndex, result.Paginator.startIndex);
    }
    
    [Fact]
    public async Task Edit_ShouldReturnAnUpdatedEntrepriseCouvertureDto_IfGivenAValidEntrepriseCouvertureDto() 
    {
        // Arrange
        EntrepriseCouverture updatedEntrepriseCouverture = new EntrepriseCouverture {
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
            EntrepriseBase = new EntrepriseBase
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
                DateCreation = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
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
                DateDebutActivite = "string",
                CreateDate = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
                UpdatedAt = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            }
        };
        _mockEntrepriseCouvertureRepository.Setup(repo => repo.Update(updatedEntrepriseCouverture, It.IsAny<ContextSession>()));
        
        var updatedEntrepriseCouvertureDto = _mapper.Map<EntrepriseCouvertureDto>(updatedEntrepriseCouverture);

        // Act
        var resultDto = await _mockEntrepriseCouvertureService.Edit(updatedEntrepriseCouvertureDto);
        
        // Assert
        Assert.NotNull(resultDto);
        Assert.Equal(updatedEntrepriseCouvertureDto.Id, resultDto.Id);
    }
    
    [Fact]
    public async Task Create_ShouldReturnAnUpdatedEntrepriseCouvertureDto_IfGivenAValidEntrepriseCouvertureDto() 
    {
        // Arrange
        EntrepriseCouverture updatedEntrepriseCouverture = new EntrepriseCouverture {
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
            EntrepriseBase = new EntrepriseBase
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
                DateCreation = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
                DateCreationFormate = "string",
                EntrepriseCessee = true,
                DateCessation = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
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
                DateDebutActivite = "string",
                CreateDate = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
                UpdatedAt = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture),
                CreatedBy = "string",
                UpdatedBy = "string",
                ObjetSocial = "string"
            }
        };
        _mockEntrepriseCouvertureRepository.Setup(repo => repo.Update(updatedEntrepriseCouverture, It.IsAny<ContextSession>()));
        
        var updatedEntrepriseCouvertureDto = _mapper.Map<EntrepriseCouvertureDto>(updatedEntrepriseCouverture);

        // Act
        var resultDto = await _mockEntrepriseCouvertureService.Create(updatedEntrepriseCouvertureDto);
        
        // Assert
        Assert.NotNull(resultDto);
        Assert.Equal(updatedEntrepriseCouvertureDto.Id, resultDto.Id);
    }
}