// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => ClientEuropeBusinessUnitServiceTests.cs
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
[Collection("ClientEuropeBusinessUnitTests")]
public class ClientEuropeBusinessUnitServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<IClientEuropeBusinessUnitRepository<ClientEuropeBusinessUnit, BusinessUnit>> _mockRepository;
    private readonly Mock<IClientEuropeRepository<ClientEurope>> _mockClientEuropeRepository;
    private readonly IMapper _mapper;
    private readonly ClientEuropeBusinessUnitService<ClientEuropeBusinessUnit, ClientEuropeBusinessUnitDto, BusinessUnit, BusinessUnitDto> _mockService;
    public ClientEuropeBusinessUnitServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockRepository = new Mock<IClientEuropeBusinessUnitRepository<ClientEuropeBusinessUnit, BusinessUnit>>();
        _mockClientEuropeRepository = new Mock<IClientEuropeRepository<ClientEurope>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ClientEuropeBusinessUnit, ClientEuropeBusinessUnitDto>().ReverseMap();
            cfg.CreateMap<BusinessUnit, BusinessUnitDto>();
        });
        _mapper = mapperConfig.CreateMapper();
        _mockService = new ClientEuropeBusinessUnitService<ClientEuropeBusinessUnit, ClientEuropeBusinessUnitDto, BusinessUnit, BusinessUnitDto>(
            _mockCurrentContextProvider.Object,
            _mockRepository.Object,
            _mockClientEuropeRepository.Object,
            _mapper
            );
    }
    
    /********** Getters **********/
    [Fact]
    public async Task GetBusinessUnitsByClientEuropeIdAsync_ReturnsListOfBusinessUnitDto_WhenClientEuropeIdIsValid()
    {
        // Arrange
        var clientEuropeId = 1;
        var clientEurope = new ClientEurope
        {
            Id = clientEuropeId
        };
        var mockBusinessUnitList = new List<BusinessUnit>
        {
            new BusinessUnit
            {
                Id = 1,
                Libelle = "TestLabel",
            }
        };
        _mockClientEuropeRepository.Setup(repo => repo.GetById(clientEuropeId, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(clientEurope);
        _mockRepository.Setup(repo => repo.GetBusinessUnitsByClientEuropeIdAsync(clientEuropeId, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(mockBusinessUnitList);
        
        // Act
        var expectedList = await _mockService.GetByClientEuropeIdAsync(clientEuropeId);
        
        // Assert
        Assert.IsType<List<BusinessUnitDto>>(expectedList);
    }
    
    [Fact]
    public async Task GetByClientEuropeIdAsync_ReturnsNull_WhenClientEuropeIdIsInvalid()
    {
        // Arrange
        var clientEuropeId = 1;
        _mockClientEuropeRepository.Setup(repo => repo.GetById(clientEuropeId, It.IsAny<ContextSession>(), false))
            .ReturnsAsync((ClientEurope) null);

        // Act
        var expectedList = await _mockService.GetByClientEuropeIdAsync(clientEuropeId);
        
        // Assert
        Assert.Null(expectedList);
    }
    
    /********** Create **********/
    [Fact]
    public async Task CreateAsync_ReturnsClientEuropeBusinessUnitDto_WhenCreationIsSuccessful()
    {
        // Arrange
        var clientEuropeId = 1;
        var businessUnitId = 1;
        
        var mockClientEuropeBusinessUnitDto = new ClientEuropeBusinessUnitDto
        {
            ClientId = clientEuropeId,
            BusinessUnitId = businessUnitId
        };
        
        var mockClientEuropeBusinessUnit = new ClientEuropeBusinessUnit
        {
            ClientId = clientEuropeId,
            BusinessUnitId = businessUnitId
        };
        _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<ClientEuropeBusinessUnit>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(mockClientEuropeBusinessUnit);
        
        // Act
        var expectedClientEuropeBusinessUnit = await _mockService.CreateAsync(mockClientEuropeBusinessUnitDto);
        
        // Assert
        Assert.NotNull(expectedClientEuropeBusinessUnit);
        Assert.IsType<ClientEuropeBusinessUnitDto>(expectedClientEuropeBusinessUnit);
    }
    
    /********** Delete **********/
    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenDeletionIsSuccessful()
    {
        // Arrange
        var clientEuropeId = 1;
        var businessUnitId = 1;
        
        var mockClientEuropeBusinessUnitDto = new ClientEuropeBusinessUnitDto
        {
            ClientId = clientEuropeId,
            BusinessUnitId = businessUnitId
        };
        
        var mockClientEuropeBusinessUnit = new ClientEuropeBusinessUnit
        {
            ClientId = clientEuropeId,
            BusinessUnitId = businessUnitId
        };
        _mockRepository.Setup(repo => repo.GetByClientEuropeIdAndBusinessUnitIdAsync(clientEuropeId, businessUnitId, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(mockClientEuropeBusinessUnit);
        _mockRepository.Setup(repo => repo.DeleteAsync(clientEuropeId, businessUnitId, It.IsAny<ContextSession>()))
            .ReturnsAsync(true);
        
        // Act
        var expectedClientEuropeBusinessUnit = await _mockService.DeleteAsync(mockClientEuropeBusinessUnitDto);
        
        // Assert
        Assert.True(expectedClientEuropeBusinessUnit);
    }
    
    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenClientEuropeBusinessUnitDtoIsNull()
    {
        // Arrange
        ClientEuropeBusinessUnitDto mockClientEuropeBusinessUnitDto = null;
        
        // Act
        var expectedClientEuropeBusinessUnit = await _mockService.DeleteAsync(mockClientEuropeBusinessUnitDto);
        
        // Assert
        Assert.False(expectedClientEuropeBusinessUnit);
    }
    
    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenDeletionIsUnsuccessful()
    {
        // Arrange
        var clientEuropeId = 1;
        var businessUnitId = 1;
        
        var mockClientEuropeBusinessUnitDto = new ClientEuropeBusinessUnitDto
        {
            ClientId = clientEuropeId,
            BusinessUnitId = businessUnitId
        };
        
        _mockRepository.Setup(repo => repo.GetByClientEuropeIdAndBusinessUnitIdAsync(clientEuropeId, businessUnitId, It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync((ClientEuropeBusinessUnit) null);
        
        // Act
        var expectedClientEuropeBusinessUnit = await _mockService.DeleteAsync(mockClientEuropeBusinessUnitDto);
        
        // Assert
        Assert.False(expectedClientEuropeBusinessUnit);
    }
}