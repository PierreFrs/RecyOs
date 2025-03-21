// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => BalanceFranceServiceTests.cs
// Created : 2024/02/26 - 13:58
// Updated : 2024/02/26 - 13:58

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
public class BalanceFranceServiceTests
{
    private readonly Mock<ICurrentContextProvider> _currentContextProviderMock;
    private readonly Mock<IBalanceFranceRepository> _balanceFranceRepositoryMock;
    private readonly IMapper _mapper;
    private readonly BalanceFranceService _balanceFranceService;

    public BalanceFranceServiceTests()
    {
        _currentContextProviderMock = new Mock<ICurrentContextProvider>();
        _balanceFranceRepositoryMock = new Mock<IBalanceFranceRepository>();
        var mapperConfig = new MapperConfiguration(cfg => { cfg.CreateMap<BalanceFrance, BalanceDto>().ReverseMap(); });
        _mapper = mapperConfig.CreateMapper();
        _balanceFranceService = new BalanceFranceService(
            _currentContextProviderMock.Object,
            _balanceFranceRepositoryMock.Object,
            _mapper);
    }

    /* Getters */
    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfBalances_IfGetAllAsyncIsSuccessful()
    {
        // Arrange
        var balanceFranceList = new List<BalanceFrance>
        {
            new BalanceFrance
            {
                ClientId = 1,
                SocieteId = 1,
                DateRecuperationBalance = DateTime.Now,
                Montant = 100
            },
            new BalanceFrance
            {
                ClientId = 1,
                SocieteId = 2,
                DateRecuperationBalance = DateTime.Now,
                Montant = 200
            }
        };
        _balanceFranceRepositoryMock.Setup(x => x.GetAllAsync(
                It.IsAny<ContextSession>(),
                It.IsAny<bool>()))
            .ReturnsAsync(balanceFranceList);

        // Act
        var result = await _balanceFranceService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<BalanceDto>>(result);
        Assert.Equal(balanceFranceList.Count, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_IfGetAllAsyncIsUnsuccessful()
    {
        // Arrange
        _balanceFranceRepositoryMock.Setup(x => x.GetAllAsync(
                It.IsAny<ContextSession>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new List<BalanceFrance>());

        // Act
        var result = await _balanceFranceService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<BalanceDto>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBalanceFranceDto_WhenGetByIdAsyncIsSuccessful()
    {
        // Arrange
        var balanceFrance = new BalanceFrance
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = DateTime.Now,
            Montant = 100
        };
        _balanceFranceRepositoryMock.Setup(x => x.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<ContextSession>(),
                It.IsAny<bool>()))
            .ReturnsAsync(balanceFrance);

        // Act
        var result = await _balanceFranceService.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BalanceDto>(result);
        Assert.Equal(balanceFrance.ClientId, result.ClientId);
        Assert.Equal(balanceFrance.SocieteId, result.SocieteId);
        Assert.Equal(balanceFrance.DateRecuperationBalance, result.DateRecuperationBalance);
        Assert.Equal(balanceFrance.Montant, result.Montant);
    }

    [Fact]
    public async Task GetByClientIdAsync_ReturnsListOfBalanceFranceDto_WhenGetByClientIdIsSuccessful()
    {
        // Arrange
        var clientId = 1;
        var includeDeleted = false;
        var balanceFranceList = new List<BalanceFrance>
        {
            new BalanceFrance
            {
                ClientId = 1,
                SocieteId = 1,
                DateRecuperationBalance = DateTime.Now,
                Montant = 100
            },
            new BalanceFrance
            {
                ClientId = 1,
                SocieteId = 2,
                DateRecuperationBalance = DateTime.Now,
                Montant = 200
            }
        };
        _balanceFranceRepositoryMock.Setup(x => x.GetByClientIdAsync(
                It.IsAny<int>(), 
                It.IsAny<ContextSession>(), 
                It.IsAny<bool>()))
            .ReturnsAsync(balanceFranceList);
        
        // Act
        var result = await _balanceFranceService.GetByClientIdAsync(clientId, includeDeleted);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<BalanceDto>>(result);
        Assert.Equal(balanceFranceList.Count, result.Count);
    }
    
    /* Create */
    [Fact]
    public async Task CreateAsync_ShouldReturnBalanceFranceDto_IfCreateAsyncIsSuccessful()
    {
        // Arrange
        var balanceDto = new BalanceDto
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = DateTime.Now,
            Montant = 100
        };
        var balanceFrance = new BalanceFrance
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = DateTime.Now,
            Montant = 100
        };
        _balanceFranceRepositoryMock.Setup(x => x.CreateAsync(
                It.IsAny<BalanceFrance>(),
                It.IsAny<ContextSession>()))
            .ReturnsAsync(balanceFrance);

        // Act
        var result = await _balanceFranceService.CreateAsync(balanceDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BalanceDto>(result);
        Assert.Equal(balanceDto.ClientId, result.ClientId);
        Assert.Equal(balanceDto.SocieteId, result.SocieteId);
        Assert.Equal(balanceDto.Montant, result.Montant);
    }
    
    /* Update */
    [Fact]
    public async Task UpdateAsync_ShouldReturnBalanceFranceDto_IfUpdateAsyncIsSuccessful()
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
        var balanceFrance = new BalanceFrance
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = date,
            Montant = 100
        };
        _balanceFranceRepositoryMock.Setup(x => x.UpdateAsync(
                It.IsAny<int>(),
                It.IsAny<BalanceFrance>(),
                It.IsAny<ContextSession>()))
            .ReturnsAsync(balanceFrance);

        // Act
        var result = await _balanceFranceService.UpdateAsync(1, balanceDto);

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
        _balanceFranceRepositoryMock.Setup(x => x.DeleteAsync(
                It.IsAny<int>(),
                It.IsAny<ContextSession>()))
            .ReturnsAsync(true);

        // Act
        var result = await _balanceFranceService.DeleteAsync(1);

        // Assert
        Assert.True(result);
    }
}