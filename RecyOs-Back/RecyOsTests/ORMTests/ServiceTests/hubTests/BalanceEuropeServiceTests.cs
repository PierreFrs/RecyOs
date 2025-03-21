// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => BalanceEuropeServiceTests.cs
// Created : 2024/02/26 - 14:54
// Updated : 2024/02/26 - 14:54

using AutoMapper;
using Moq;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using RecyOs.ORM.Service.hub;

namespace RecyOsTests.ORMTests.hubTests;

[Collection("BalanceTestsCollection")]
public class BalanceEuropeServiceTests
{
    private readonly Mock<ICurrentContextProvider> _currentContextProviderMock;
    private readonly Mock<IBalanceEuropeRepository> _balanceEuropeRepositoryMock;
    private readonly IMapper _mapper;
    private readonly BalanceEuropeService _balanceEuropeService;

    public BalanceEuropeServiceTests()
    {
        _currentContextProviderMock = new Mock<ICurrentContextProvider>();
        _balanceEuropeRepositoryMock = new Mock<IBalanceEuropeRepository>();
        var mapperConfig = new MapperConfiguration(cfg => { cfg.CreateMap<BalanceEurope, BalanceDto>().ReverseMap(); });
        _mapper = mapperConfig.CreateMapper();
        _balanceEuropeService = new BalanceEuropeService(
            _currentContextProviderMock.Object,
            _balanceEuropeRepositoryMock.Object,
            _mapper);
    }

    /* Getters */
    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfBalances_IfGetAllAsyncIsSuccessful()
    {
        // Arrange
        var balanceEuropeList = new List<BalanceEurope>
        {
            new BalanceEurope
            {
                ClientId = 1,
                SocieteId = 1,
                DateRecuperationBalance = DateTime.Now,
                Montant = 100
            },
            new BalanceEurope
            {
                ClientId = 1,
                SocieteId = 2,
                DateRecuperationBalance = DateTime.Now,
                Montant = 200
            }
        };
        _balanceEuropeRepositoryMock.Setup(x => x.GetAllAsync(
                It.IsAny<ContextSession>(),
                It.IsAny<bool>()))
            .ReturnsAsync(balanceEuropeList);

        // Act
        var result = await _balanceEuropeService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<BalanceDto>>(result);
        Assert.Equal(balanceEuropeList.Count, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_IfGetAllAsyncIsUnsuccessful()
    {
        // Arrange
        _balanceEuropeRepositoryMock.Setup(x => x.GetAllAsync(
                It.IsAny<ContextSession>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new List<BalanceEurope>());

        // Act
        var result = await _balanceEuropeService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<BalanceDto>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBalanceEuropeDto_WhenGetByIdAsyncIsSuccessful()
    {
        // Arrange
        var balanceEurope = new BalanceEurope
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = DateTime.Now,
            Montant = 100
        };
        _balanceEuropeRepositoryMock.Setup(x => x.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<ContextSession>(),
                It.IsAny<bool>()))
            .ReturnsAsync(balanceEurope);

        // Act
        var result = await _balanceEuropeService.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BalanceDto>(result);
        Assert.Equal(balanceEurope.ClientId, result.ClientId);
        Assert.Equal(balanceEurope.SocieteId, result.SocieteId);
        Assert.Equal(balanceEurope.DateRecuperationBalance, result.DateRecuperationBalance);
        Assert.Equal(balanceEurope.Montant, result.Montant);
    }

    [Fact]
    public async Task GetByClientIdAsync_ReturnsListOfBalanceEuropeDto_WhenGetByClientIdIsSuccessful()
    {
        // Arrange
        var clientId = 1;
        var includeDeleted = false;
        var balanceEuropeList = new List<BalanceEurope>
        {
            new BalanceEurope
            {
                ClientId = 1,
                SocieteId = 1,
                DateRecuperationBalance = DateTime.Now,
                Montant = 100
            },
            new BalanceEurope
            {
                ClientId = 1,
                SocieteId = 2,
                DateRecuperationBalance = DateTime.Now,
                Montant = 200
            }
        };
        _balanceEuropeRepositoryMock.Setup(x => x.GetByClientIdAsync(
                It.IsAny<int>(), 
                It.IsAny<ContextSession>(), 
                It.IsAny<bool>()))
            .ReturnsAsync(balanceEuropeList);
        
        // Act
        var result = await _balanceEuropeService.GetByClientIdAsync(clientId, includeDeleted);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<BalanceDto>>(result);
        Assert.Equal(balanceEuropeList.Count, result.Count);
    }
    
    /* Create */
    [Fact]
    public async Task CreateAsync_ShouldReturnBalanceEuropeDto_IfCreateAsyncIsSuccessful()
    {
        // Arrange
        var balanceDto = new BalanceDto
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = DateTime.Now,
            Montant = 100
        };
        var balanceEurope = new BalanceEurope
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = DateTime.Now,
            Montant = 100
        };
        _balanceEuropeRepositoryMock.Setup(x => x.CreateAsync(
                It.IsAny<BalanceEurope>(),
                It.IsAny<ContextSession>()))
            .ReturnsAsync(balanceEurope);

        // Act
        var result = await _balanceEuropeService.CreateAsync(balanceDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BalanceDto>(result);
        Assert.Equal(balanceDto.ClientId, result.ClientId);
        Assert.Equal(balanceDto.SocieteId, result.SocieteId);
        Assert.Equal(balanceDto.Montant, result.Montant);
    }
    
    /* Update */
    [Fact]
    public async Task UpdateAsync_ShouldReturnBalanceEuropeDto_IfUpdateAsyncIsSuccessful()
    {
        // Arrange
        var date = DateTime.Now;
        var balanceDto = new BalanceDto
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = date,
            Montant = 100
        };
        var balanceEurope = new BalanceEurope
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = date,
            Montant = 100
        };
        _balanceEuropeRepositoryMock.Setup(x => x.UpdateAsync(
                It.IsAny<int>(),
                It.IsAny<BalanceEurope>(),
                It.IsAny<ContextSession>()))
            .ReturnsAsync(balanceEurope);

        // Act
        var result = await _balanceEuropeService.UpdateAsync(1, balanceDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BalanceDto>(result);
        Assert.Equal(balanceDto.ClientId, result.ClientId);
        Assert.Equal(balanceDto.SocieteId, result.SocieteId);
        Assert.Equal(balanceDto.DateRecuperationBalance, result.DateRecuperationBalance);
        Assert.Equal(balanceDto.Montant, result.Montant);
    }
    
    /* Delete */
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_IfDeleteAsyncIsSuccessful()
    {
        // Arrange
        _balanceEuropeRepositoryMock.Setup(x => x.DeleteAsync(
                It.IsAny<int>(),
                It.IsAny<ContextSession>()))
            .ReturnsAsync(true);

        // Act
        var result = await _balanceEuropeService.DeleteAsync(1);

        // Assert
        Assert.True(result);
    }
}