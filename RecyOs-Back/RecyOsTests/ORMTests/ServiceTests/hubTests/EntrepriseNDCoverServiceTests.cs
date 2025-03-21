// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => BaseGenericEntrepriseNDCoverServiceTests.cs
// Created : 2023/12/19 - 15:55
// Updated : 2023/12/19 - 15:55

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

[Collection("NDCoverTests")]
public class EntrepriseNDCoverServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<IEntrepriseNDCoverRepository<EntrepriseNDCover>> _mockEntrepriseNDCoverRepository;
    private readonly IMapper _mapper;
    private readonly EntrepriseNDCoverService<EntrepriseNDCover> _mockEntrepriseNDCoverService;

    public EntrepriseNDCoverServiceTests()
    {
        
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockEntrepriseNDCoverRepository = new Mock<IEntrepriseNDCoverRepository<EntrepriseNDCover>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<EntrepriseNDCover, EntrepriseNDCoverDto>();
            cfg.CreateMap<EntrepriseNDCoverDto, EntrepriseNDCover>();
                
            cfg.CreateMap<EntrepriseBase, EntrepriseBaseDto>()
                .ForMember(dest => dest.Diffusable, opt => opt.MapFrom(src => true));
            cfg.CreateMap<EntrepriseBaseDto, EntrepriseBase>();
        });
        _mapper = mapperConfig.CreateMapper();
        _mockEntrepriseNDCoverService = new EntrepriseNDCoverService<EntrepriseNDCover>(
            _mockCurrentContextProvider.Object, 
            _mockEntrepriseNDCoverRepository.Object, 
            _mapper);
    }

    
    [Fact]
    public async Task GetById_ShouldReturnAnEntrepriseNDCoverDto_IfGivenAValidId()
    {
        // Arrange
        var mockEntrepriseNDCover = new EntrepriseNDCover()
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
        
        _mockEntrepriseNDCoverRepository
            .Setup(repo => repo.Get(mockEntrepriseNDCover.Id, It.IsAny<ContextSession>()))
            .ReturnsAsync(mockEntrepriseNDCover);

        // Act
        var result = await _mockEntrepriseNDCoverService.GetById(mockEntrepriseNDCover.Id);
        var dtoResult = Assert.IsType<EntrepriseNDCoverDto>(result);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<EntrepriseNDCoverDto>(result);
        Assert.Equal(mockEntrepriseNDCover.Id, dtoResult.Id);

    }
    
    [Fact]
    public async Task GetBySiren_ShouldReturnAnEntrepriseNDCoverDto_IfGivenAValidSiren() 
    {
        // Arrange
        var mockEntrepriseNDCover = new EntrepriseNDCover()
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
        
        _mockEntrepriseNDCoverRepository
            .Setup(repo => repo.GetBySiren(mockEntrepriseNDCover.Siren, It.IsAny<ContextSession>()))
            .ReturnsAsync(mockEntrepriseNDCover);

        // Act
        var result = await _mockEntrepriseNDCoverService.GetBySiren(mockEntrepriseNDCover.Siren);
        var dtoResult = Assert.IsType<EntrepriseNDCoverDto>(result);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<EntrepriseNDCoverDto>(result);
        Assert.Equal(mockEntrepriseNDCover.Siren, dtoResult.Siren);
    }
    
    [Fact]
    public async Task GetDataForGrid_ShouldReturnAFilteredGrid_IfGivenAValidEntrepriseNDCoverGridFilter() 
    {
        // Arrange
        int pageSize = 10;
        int pageNumber = 1;
        int totalItemCount = 50;
        int expectedLastPage = (int)Math.Ceiling((double)totalItemCount / pageSize);
        int expectedStartIndex = 10;
        
        var filter = new EntrepriseNDCoverGridFilter
        {
            FilteredBSiren = "someValue",
            Refus = "someRefusValue",
            Agreement = "someAgreementValue",
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var mockData = new List<EntrepriseNDCover>
        {
            new EntrepriseNDCover() 
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
            new EntrepriseNDCover() 
            {
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
        
        _mockEntrepriseNDCoverRepository
            .Setup(repo => repo.GetFilteredListWithCount(filter, It.IsAny<ContextSession>()))
            .ReturnsAsync((mockData.AsEnumerable(), totalItemCount));

        
        // Act
        var result = await _mockEntrepriseNDCoverService.GetDataForGrid(filter);
        
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
    public async Task Edit_ShouldReturnAnUpdatedEntrepriseNDCoverDto_IfGivenAValidEntrepriseNDCoverDto() 
    {
        // Arrange
        EntrepriseNDCover updatedEntrepriseNDCover = new EntrepriseNDCover {
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
            DateExtraction = DateTime.Parse("2023-12-07T13:22:01.395Z", CultureInfo.InvariantCulture)
        };
        _mockEntrepriseNDCoverRepository.Setup(repo => repo.Update(updatedEntrepriseNDCover, It.IsAny<ContextSession>()));
        
        var updatedEntrepriseNDCoverDto = _mapper.Map<EntrepriseNDCoverDto>(updatedEntrepriseNDCover);

        // Act
        var resultDto = await _mockEntrepriseNDCoverService.Edit(updatedEntrepriseNDCoverDto);
        
        // Assert
        Assert.NotNull(resultDto);
        Assert.Equal(updatedEntrepriseNDCoverDto.Id, resultDto.Id);
    }
    
    [Fact]
    public async Task Create_ShouldReturnAnUpdatedEntrepriseNDCoverDto_IfGivenAValidEntrepriseNDCoverDto() 
    {
        // Arrange
        EntrepriseNDCover updatedEntrepriseNDCover = new EntrepriseNDCover {
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
        _mockEntrepriseNDCoverRepository.Setup(repo => repo.Update(updatedEntrepriseNDCover, It.IsAny<ContextSession>()));
        
        var updatedEntrepriseNDCoverDto = _mapper.Map<EntrepriseNDCoverDto>(updatedEntrepriseNDCover);

        // Act
        var resultDto = await _mockEntrepriseNDCoverService.Create(updatedEntrepriseNDCoverDto);
        
        // Assert
        Assert.NotNull(resultDto);
        Assert.Equal(updatedEntrepriseNDCoverDto.Id, resultDto.Id);
    }
}