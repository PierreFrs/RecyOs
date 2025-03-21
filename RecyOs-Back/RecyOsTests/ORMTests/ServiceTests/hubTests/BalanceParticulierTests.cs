// BalanceParticulierTests.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 30/01/2025
// Fichier Modifié le : 30/01/2025
// Code développé pour le projet : RecyOsTests

using AutoMapper;
using Moq;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using RecyOs.ORM.Service.hub;
using RecyOs.ORM.Service.hub.BalanceServices;

namespace RecyOsTests.ORMTests.hubTests;

[Collection("BalanceTestsCollection")]
public class BalanceParticulierTests
{
    private readonly Mock<ICurrentContextProvider> _currentContextProviderMock;
    private readonly Mock<IBalanceParticulierRepository> _balanceParticulierRepositoryMock;
    private readonly IMapper _mapper;
    private readonly BalanceParticulierService _balanceParticulier;

    public BalanceParticulierTests()
    {
        _currentContextProviderMock = new Mock<ICurrentContextProvider>();
        _balanceParticulierRepositoryMock = new Mock<IBalanceParticulierRepository>();
        var mapperConfig = new MapperConfiguration(cfg => { cfg.CreateMap<BalanceParticulier, BalanceDto>().ReverseMap(); });
        _mapper = mapperConfig.CreateMapper();
        _balanceParticulier = new BalanceParticulierService(
            _currentContextProviderMock.Object,
            _balanceParticulierRepositoryMock.Object,
            _mapper);
    }

    /* Getters */
    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfBalances_IfGetAllAsyncIsSuccessful()
    {
        // Arrange
        var balanceParticulierList = new List<BalanceParticulier>
        {
            new BalanceParticulier
            {
                ClientId = 1,
                SocieteId = 1,
                DateRecuperationBalance = DateTime.Now,
                Montant = 100
            },
            new BalanceParticulier
            {
                ClientId = 1,
                SocieteId = 2,
                DateRecuperationBalance = DateTime.Now,
                Montant = 200
            }
        };
        _balanceParticulierRepositoryMock.Setup(x => x.GetAllAsync(
                It.IsAny<ContextSession>(),
                It.IsAny<bool>()))
            .ReturnsAsync(balanceParticulierList);

        // Act
        var result = await _balanceParticulier.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<BalanceDto>>(result);
        Assert.Equal(balanceParticulierList.Count, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_IfGetAllAsyncIsUnsuccessful()
    {
        // Arrange
        _balanceParticulierRepositoryMock.Setup(x => x.GetAllAsync(
                It.IsAny<ContextSession>(),
                It.IsAny<bool>()))
            .ReturnsAsync(new List<BalanceParticulier>());

        // Act
        var result = await _balanceParticulier.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<BalanceDto>>(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBalanceParticulierDto_WhenGetByIdAsyncIsSuccessful()
    {
        // Arrange
        var balanceParticulier = new BalanceParticulier
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = DateTime.Now,
            Montant = 100
        };
        _balanceParticulierRepositoryMock.Setup(x => x.GetByIdAsync(
                It.IsAny<int>(),
                It.IsAny<ContextSession>(),
                It.IsAny<bool>()))
            .ReturnsAsync(balanceParticulier);

        // Act
        var result = await _balanceParticulier.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BalanceDto>(result);
        Assert.Equal(balanceParticulier.ClientId, result.ClientId);
        Assert.Equal(balanceParticulier.SocieteId, result.SocieteId);
        Assert.Equal(balanceParticulier.DateRecuperationBalance, result.DateRecuperationBalance);
        Assert.Equal(balanceParticulier.Montant, result.Montant);
    }

    [Fact]
    public async Task GetByClientIdAsync_ReturnsListOfBalanceParticulierDto_WhenGetByClientIdIsSuccessful()
    {
        // Arrange
        var clientId = 1;
        var includeDeleted = false;
        var balanceParticulierList = new List<BalanceParticulier>
        {
            new BalanceParticulier
            {
                ClientId = 1,
                SocieteId = 1,
                DateRecuperationBalance = DateTime.Now,
                Montant = 100
            },
            new BalanceParticulier
            {
                ClientId = 1,
                SocieteId = 2,
                DateRecuperationBalance = DateTime.Now,
                Montant = 200
            }
        };
        _balanceParticulierRepositoryMock.Setup(x => x.GetByClientIdAsync(
                It.IsAny<int>(),
                It.IsAny<ContextSession>(),
                It.IsAny<bool>()))
            .ReturnsAsync(balanceParticulierList);

        // Act
        var result = await _balanceParticulier.GetByClientIdAsync(clientId, includeDeleted);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<BalanceDto>>(result);
        Assert.Equal(balanceParticulierList.Count, result.Count);
    }

    /* Create */
    [Fact]
    public async Task CreateAsync_ShouldReturnBalanceParticulierDto_IfCreateAsyncIsSuccessful()
    {
        // Arrange
        var balanceDto = new BalanceDto
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = DateTime.Now,
            Montant = 100
        };
        var balanceParticulier = new BalanceParticulier
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = DateTime.Now,
            Montant = 100
        };
        _balanceParticulierRepositoryMock.Setup(x => x.CreateAsync(
                It.IsAny<BalanceParticulier>(),
                It.IsAny<ContextSession>()))
            .ReturnsAsync(balanceParticulier);

        // Act
        var result = await _balanceParticulier.CreateAsync(balanceDto);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BalanceDto>(result);
        Assert.Equal(balanceDto.ClientId, result.ClientId);
        Assert.Equal(balanceDto.SocieteId, result.SocieteId);
        Assert.Equal(balanceDto.Montant, result.Montant);
    }

    /* Update */
    [Fact]
    public async Task UpdateAsync_ShouldReturnBalanceParticulierDto_IfUpdateAsyncIsSuccessful()
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
        var balanceParticulier = new BalanceParticulier
        {
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = date,
            Montant = 100
        };
        _balanceParticulierRepositoryMock.Setup(x => x.UpdateAsync(
                It.IsAny<int>(),
                It.IsAny<BalanceParticulier>(),
                It.IsAny<ContextSession>()))
            .ReturnsAsync(balanceParticulier);

        // Act
        var result = await _balanceParticulier.UpdateAsync(1, balanceDto);

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
        _balanceParticulierRepositoryMock.Setup(x => x.DeleteAsync(
                It.IsAny<int>(),
                It.IsAny<ContextSession>()))
            .ReturnsAsync(true);

        // Act
        var result = await _balanceParticulier.DeleteAsync(1);

        // Assert
        Assert.True(result);
    }
}