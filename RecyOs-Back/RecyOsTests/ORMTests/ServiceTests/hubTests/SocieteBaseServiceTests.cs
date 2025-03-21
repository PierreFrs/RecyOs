// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => SocieteServiceTests.cs
// Created : 2024/02/26 - 10:52
// Updated : 2024/02/26 - 10:52

using AutoMapper;
using Moq;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOsTests.ORMTests.hubTests;

[Collection("SocieteUnitTests")]
public class SocieteBaseServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<ISocieteBaseRepository> _mockSocieteRepository;
    private readonly IMapper _mapper;
    private readonly Mock<ITokenInfoService> _mockTokenInfoService;
    private readonly SocieteBaseService _societeBaseService;
    public SocieteBaseServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockSocieteRepository = new Mock<ISocieteBaseRepository>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Societe, SocieteDto>();
            cfg.CreateMap<Societe, SocieteDto>();
        });
        _mapper = mapperConfig.CreateMapper();
        _mockTokenInfoService = new Mock<ITokenInfoService>();
        _societeBaseService = new SocieteBaseService(
            _mockCurrentContextProvider.Object,
            _mockSocieteRepository.Object, 
            _mapper, 
            _mockTokenInfoService.Object);
    }
    
    /* Getters */
    [Fact]
    public async Task GetListAsync_ReturnsListOfSocieteDto_WhenGetListIsSuccessful()
    {
        // Arrange
        var mockSocieteList = new List<Societe>
        {
            new Societe
            {
                Id = 1,
                Nom = "Societe1",
                IdOdoo = "1",
                CreateDate = DateTime.Now,
                CreatedBy = "TestUser"
            },
            new Societe
            {
                Id = 2,
                Nom = "Societe2",
                IdOdoo = "2",
                CreateDate = DateTime.Now,
                CreatedBy = "TestUser"
            }
        };
        _mockSocieteRepository.Setup(repo => repo.GetListAsync(It.IsAny<bool>()))
            .ReturnsAsync(mockSocieteList);
        
        // Act
        var result = await _societeBaseService.GetListAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<SocieteDto>>(result);
        Assert.Equal(mockSocieteList.Count, result.Count);
    }
    
    [Fact]
    public async Task GetByIdAsync_ReturnsSocieteDto_WhenGetByIdIsSuccessful()
    {
        // Arrange
        var mockSociete = new Societe
        {
            Id = 1,
            Nom = "Societe1",
            IdOdoo = "1",
            CreateDate = DateTime.Now,
            CreatedBy = "TestUser"
        };
        _mockSocieteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(mockSociete);
        
        // Act
        var result = await _societeBaseService.GetByIdAsync(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<SocieteDto>(result);
        Assert.Equal(mockSociete.Id, result.Id);
    }
    
    /* Create */
    [Fact]
    public async Task CreateAsync_ReturnsSocieteDto_WhenCreateIsSuccessful()
    {
        // Arrange
        var mockSocieteDto = new SocieteDto
        {
            Nom = "Societe1",
            IdOdoo = "1"
        };
        var mockSociete = new Societe
        {
            Id = 1,
            Nom = "Societe1",
            IdOdoo = "1",
            CreateDate = DateTime.Now,
            CreatedBy = "TestUser"
        };
        _mockSocieteRepository.Setup(repo => repo.CreateAsync(It.IsAny<Societe>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(mockSociete);
        
        // Act
        var result = await _societeBaseService.CreateAsync(mockSocieteDto);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<SocieteDto>(result);
        Assert.Equal(mockSociete.Nom, result.Nom);
    }
    
    /* Update */
    [Fact]
    public async Task UpdateAsync_ReturnsSocieteDto_WhenUpdateIsSuccessful()
    {
        // Arrange
        var mockSocieteDto = new SocieteDto
        {
            Nom = "Societe1",
            IdOdoo = "1"
        };
        var mockSociete = new Societe
        {
            Id = 1,
            Nom = "Societe1",
            IdOdoo = "1",
            CreateDate = DateTime.Now,
            CreatedBy = "TestUser"
        };
        _mockSocieteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(mockSociete);
        _mockSocieteRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Societe>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(mockSociete);
        
        // Act
        var result = await _societeBaseService.UpdateAsync(1, mockSocieteDto);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<SocieteDto>(result);
        Assert.Equal(mockSociete.Nom, result.Nom);
    }
    
    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenSocieteDoesNotExist()
    {
        // Arrange
        var mockSocieteDto = new SocieteDto
        {
            Nom = "Societe1",
            IdOdoo = "1"
        };
        _mockSocieteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync((Societe?)null);
        
        // Act
        var result = await _societeBaseService.UpdateAsync(1, mockSocieteDto);
        
        // Assert
        Assert.Null(result);
    }
    
    /* Delete */
    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenDeleteIsSuccessful()
    {
        // Arrange
        var mockSociete = new Societe
        {
            Id = 1,
            Nom = "Societe1",
            IdOdoo = "1",
            CreateDate = DateTime.Now,
            CreatedBy = "TestUser"
        };
        _mockSocieteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(mockSociete);
        _mockSocieteRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(true);
        
        // Act
        var result = await _societeBaseService.DeleteAsync(1);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenSocieteDoesNotExist()
    {
        // Arrange
        _mockSocieteRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync((Societe?)null);
        
        // Act
        var result = await _societeBaseService.DeleteAsync(1);
        
        // Assert
        Assert.False(result);
    }
}