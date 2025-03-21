// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => EtablissementClientBusinessUnitServiceTests.cs
// Created : 2024/01/24 - 12:11
// Updated : 2024/01/24 - 12:11

using AutoMapper;
using Moq;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service;

namespace RecyOsTests.ORMTests.hubTests;
[Collection("EtablissementClientBusinessUnitTests")]
public class EtablissementClientBusinessUnitServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<IEtablissementClientBusinessUnitRepository<EtablissementClientBusinessUnit, BusinessUnit>> _mockRepository;
    private readonly Mock<IEtablissementClientRepository<EtablissementClient>> _mockEtablissementClientRepository;
    private readonly IMapper _mapper;
    private readonly EtablissementClientBusinessUnitService<EtablissementClientBusinessUnit, EtablissementClientBusinessUnitDto, BusinessUnit, BusinessUnitDto> _mockService;
    public EtablissementClientBusinessUnitServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockRepository = new Mock<IEtablissementClientBusinessUnitRepository<EtablissementClientBusinessUnit, BusinessUnit>>();
        _mockEtablissementClientRepository = new Mock<IEtablissementClientRepository<EtablissementClient>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<EtablissementClientBusinessUnit, EtablissementClientBusinessUnitDto>().ReverseMap();
            cfg.CreateMap<BusinessUnit, BusinessUnitDto>();
        });
        _mapper = mapperConfig.CreateMapper();
        _mockService = new EtablissementClientBusinessUnitService<EtablissementClientBusinessUnit, EtablissementClientBusinessUnitDto, BusinessUnit, BusinessUnitDto>(
            _mockCurrentContextProvider.Object,
            _mockRepository.Object,
            _mockEtablissementClientRepository.Object,
            _mapper
            );
    }
    
    /********** Getters **********/
    [Fact]
    public async Task GetByEtablissementClientIdAsync_ReturnsListOfBusinessUnitDto_WhenEtablissementClientIdIsValid()
    {
        // Arrange
        var etablissementClientId = 1;
        var etablissementClient = new EtablissementClient
        {
            Id = etablissementClientId
        };
        var mockBusinessUnitList = new List<BusinessUnit>
        {
            new BusinessUnit
            {
                Id = 1,
                Libelle = "TestLabel",
            }
        };
        _mockEtablissementClientRepository.Setup(repo => repo.GetById(etablissementClientId, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(etablissementClient);
        _mockRepository.Setup(repo => repo.GetBusinessUnitsByEtablissementClientIdAsync(etablissementClientId, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(mockBusinessUnitList);
        
        // Act
        var expectedList = await _mockService.GetByEtablissementClientIdAsync(etablissementClientId);
        
        // Assert
        Assert.IsType<List<BusinessUnitDto>>(expectedList);
    }
    
    [Fact]
    public async Task GetByEtablissementClientIdAsync_ReturnsNull_WhenEtablissementClientIdIsInvalid()
    {
        // Arrange
        var etablissementClientId = 1;
        _mockEtablissementClientRepository.Setup(repo => repo.GetById(etablissementClientId, It.IsAny<ContextSession>(), false))
            .ReturnsAsync((EtablissementClient) null);

        // Act
        var expectedList = await _mockService.GetByEtablissementClientIdAsync(etablissementClientId);
        
        // Assert
        Assert.Null(expectedList);
    }
    
    /********** Create **********/
    [Fact]
    public async Task CreateAsync_ReturnsEtablissementClientBusinessUnitDto_WhenCreationIsSuccessful()
    {
        // Arrange
        var etablissementClientId = 1;
        var businessUnitId = 1;
        
        var mockEtablissementClientBusinessUnitDto = new EtablissementClientBusinessUnitDto
        {
            ClientId = etablissementClientId,
            BusinessUnitId = businessUnitId
        };
        
        var mockEtablissementClientBusinessUnit = new EtablissementClientBusinessUnit
        {
            ClientId = etablissementClientId,
            BusinessUnitId = businessUnitId
        };
        _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<EtablissementClientBusinessUnit>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(mockEtablissementClientBusinessUnit);
        
        // Act
        var expectedEtablissementClientBusinessUnit = await _mockService.CreateAsync(mockEtablissementClientBusinessUnitDto);
        
        // Assert
        Assert.NotNull(expectedEtablissementClientBusinessUnit);
        Assert.IsType<EtablissementClientBusinessUnitDto>(expectedEtablissementClientBusinessUnit);
    }
    
    /********** Delete **********/
    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenDeletionIsSuccessful()
    {
        // Arrange
        var etablissementClientId = 1;
        var businessUnitId = 1;
        
        var mockEtablissementClientBusinessUnitDto = new EtablissementClientBusinessUnitDto
        {
            ClientId = etablissementClientId,
            BusinessUnitId = businessUnitId
        };
        
        var mockEtablissementClientBusinessUnit = new EtablissementClientBusinessUnit
        {
            ClientId = etablissementClientId,
            BusinessUnitId = businessUnitId
        };
        _mockRepository.Setup(repo => repo.GetByEtablissementClientIdAndBusinessUnitIdAsync(etablissementClientId, businessUnitId, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(mockEtablissementClientBusinessUnit);
        _mockRepository.Setup(repo => repo.DeleteAsync(etablissementClientId, businessUnitId, It.IsAny<ContextSession>()))
            .ReturnsAsync(true);
        
        // Act
        var expectedEtablissementClientBusinessUnit = await _mockService.DeleteAsync(mockEtablissementClientBusinessUnitDto);
        
        // Assert
        Assert.True(expectedEtablissementClientBusinessUnit);
    }
    
    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenEtablissementClientBusinessUnitDtoIsNull()
    {
        // Arrange
        EtablissementClientBusinessUnitDto mockEtablissementClientBusinessUnitDto = null;
        
        // Act
        var expectedEtablissementClientBusinessUnit = await _mockService.DeleteAsync(mockEtablissementClientBusinessUnitDto);
        
        // Assert
        Assert.False(expectedEtablissementClientBusinessUnit);
    }
    
    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenDeletionIsUnsuccessful()
    {
        // Arrange
        var etablissementClientId = 1;
        var businessUnitId = 1;
        
        var mockEtablissementClientBusinessUnitDto = new EtablissementClientBusinessUnitDto
        {
            ClientId = etablissementClientId,
            BusinessUnitId = businessUnitId
        };
        
        _mockRepository.Setup(repo => repo.GetByEtablissementClientIdAndBusinessUnitIdAsync(etablissementClientId, businessUnitId, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync((EtablissementClientBusinessUnit) null);
        
        // Act
        var expectedEtablissementClientBusinessUnit = await _mockService.DeleteAsync(mockEtablissementClientBusinessUnitDto);
        
        // Assert
        Assert.False(expectedEtablissementClientBusinessUnit);
    }
}