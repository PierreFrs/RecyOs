// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => EtablissementClientBusinessUnitRepositoryTests.cs
// Created : 2024/01/24 - 12:11
// Updated : 2024/01/24 - 12:11

using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests;

[Collection("RepositoryTests")]
public class EtablissementClientBusinessUnitRepositoryTests
{
    private readonly DataContext _context;
    private readonly Mock<ITokenInfoService> _tokenInfoService;

    public EtablissementClientBusinessUnitRepositoryTests(
        IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
        _tokenInfoService = new Mock<ITokenInfoService>();
    }

    /*********** Geters ***********/
    [Fact]
    public async Task GetBusinessUnitsByEtablissementClientIdAsync_ShouldReturnBusinessUnitList_IfEtablissementClientIdIsValid()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        var etablissementClientId = 1;

        // Act
        var expectedBusinessUnitList = await repository.GetBusinessUnitsByEtablissementClientIdAsync(etablissementClientId, new ContextSession());

        //Assert
        Assert.NotNull(expectedBusinessUnitList);
        Assert.Equal(2, expectedBusinessUnitList.Count());
    }
    
    [Fact]
    public async Task GetBusinessUnitsByEtablissementClientIdAsync_ShouldReturnNull_IfEtablissementClientIdIsZero()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        var etablissementClientId = 0;

        // Act
        var expectedBusinessUnitList = await repository.GetBusinessUnitsByEtablissementClientIdAsync(etablissementClientId, new ContextSession());

        //Assert
        Assert.Null(expectedBusinessUnitList);
    }
    
    [Fact]
    public async Task GetBusinessUnitsByEtablissementClientIdAsync_ShouldReturnNull_IfEtablissementClientIdIsNegative()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        var etablissementClientId = -1;

        // Act
        var expectedBusinessUnitList = await repository.GetBusinessUnitsByEtablissementClientIdAsync(etablissementClientId, new ContextSession());

        //Assert
        Assert.Null(expectedBusinessUnitList);
    }
    
    [Fact]
    public async Task GetByEtablissementClientIdAndBusinessUnitIdAsync_ShouldReturnEtablissementClientBusinessUnit_IfEtablissementClientIdAndBusinessUnitIdAreValid()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        var etablissementClientId = 1;
        var businessUnitId = 1;

        // Act
        var expectedEtablissementClientBusinessUnit = await repository.GetByEtablissementClientIdAndBusinessUnitIdAsync(etablissementClientId, businessUnitId, new ContextSession());

        //Assert
        Assert.NotNull(expectedEtablissementClientBusinessUnit);
        Assert.Equal(1, expectedEtablissementClientBusinessUnit.ClientId);
        Assert.Equal(1, expectedEtablissementClientBusinessUnit.BusinessUnitId);
    }
    
    [Fact]
    public async Task GetByEtablissementClientIdAndBusinessUnitIdAsync_ShouldReturnNull_IfEtablissementClientIdIsZero()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        var etablissementClientId = 0;
        var businessUnitId = 1;

        // Act
        var expectedEtablissementClientBusinessUnit = await repository.GetByEtablissementClientIdAndBusinessUnitIdAsync(etablissementClientId, businessUnitId, new ContextSession());

        //Assert
        Assert.Null(expectedEtablissementClientBusinessUnit);
    }
    
    [Fact]
    public async Task GetByEtablissementClientIdAndBusinessUnitIdAsync_ShouldReturnNull_IfEtablissementClientIdIsNegative()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        var etablissementClientId = -1;
        var businessUnitId = 1;

        // Act
        var expectedEtablissementClientBusinessUnit = await repository.GetByEtablissementClientIdAndBusinessUnitIdAsync(etablissementClientId, businessUnitId, new ContextSession());

        //Assert
        Assert.Null(expectedEtablissementClientBusinessUnit);
    }
    
    [Fact]
    public async Task GetByEtablissementClientIdAndBusinessUnitIdAsync_ShouldReturnNull_IfBusinessUnitIdIsZero()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        var etablissementClientId = 1;
        var businessUnitId = 0;

        // Act
        var expectedEtablissementClientBusinessUnit = await repository.GetByEtablissementClientIdAndBusinessUnitIdAsync(etablissementClientId, businessUnitId, new ContextSession());

        //Assert
        Assert.Null(expectedEtablissementClientBusinessUnit);
    }
    
    [Fact]
    public async Task GetByEtablissementClientIdAndBusinessUnitIdAsync_ShouldReturnNull_IfBusinessUnitIdIsNegative()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        var etablissementClientId = 1;
        var businessUnitId = -1;

        // Act
        var expectedEtablissementClientBusinessUnit = await repository.GetByEtablissementClientIdAndBusinessUnitIdAsync(etablissementClientId, businessUnitId, new ContextSession());

        //Assert
        Assert.Null(expectedEtablissementClientBusinessUnit);
    }
    
    /*********** Create ***********/
    
    [Fact]
    public async Task CreateAsync_ShouldReturnEtablissementClientBusinessUnit_IfEtablissementClientBusinessUnitIsValid()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        var etablissementClientBusinessUnit = new EtablissementClientBusinessUnit
        {
            ClientId = 4,
            BusinessUnitId = 1
        };

        // Act
        var expectedEtablissementClientBusinessUnit = await repository.CreateAsync(etablissementClientBusinessUnit, new ContextSession());

        //Assert
        Assert.NotNull(expectedEtablissementClientBusinessUnit);
        Assert.Equal(4, expectedEtablissementClientBusinessUnit.ClientId);
        Assert.Equal(1, expectedEtablissementClientBusinessUnit.BusinessUnitId);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnNull_IfEtablissementClientBusinessUnitIsNull()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        EtablissementClientBusinessUnit etablissementClientBusinessUnit = null;

        // Act
        var expectedEtablissementClientBusinessUnit = await repository.CreateAsync(etablissementClientBusinessUnit, new ContextSession());

        //Assert
        Assert.Null(expectedEtablissementClientBusinessUnit);
    }
    
    /********** Delete **********/
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_IfEtablissementClientBusinessUnitIsDeleted()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        var etablissementClientId = 2;
        var businessUnitId = 1;

        // Act
        var expectedEtablissementClientBusinessUnit = await repository.DeleteAsync(etablissementClientId, businessUnitId, new ContextSession());

        //Assert
        Assert.True(expectedEtablissementClientBusinessUnit);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_IfEtablissementIdsAreLessThanOne()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        var etablissementClientId = 0;
        var businessUnitId = 0;

        // Act
        var expectedEtablissementClientBusinessUnit = await repository.DeleteAsync(etablissementClientId, businessUnitId, new ContextSession());

        //Assert
        Assert.False(expectedEtablissementClientBusinessUnit);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_IfEtablissementIdsAreNegative()
    {
        // Arrange
        var repository = new EtablissementClientBusinessUnitRepository(_context, _tokenInfoService.Object);
        var etablissementClientId = 333;
        var businessUnitId = 1;

        // Act
        var expectedEtablissementClientBusinessUnit = await repository.DeleteAsync(etablissementClientId, businessUnitId, new ContextSession());

        //Assert
        Assert.False(expectedEtablissementClientBusinessUnit);
    }
}