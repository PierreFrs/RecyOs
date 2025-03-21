// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => ClientEuropeBusinessUnitRepositoryTests.cs
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
public class ClientEuropeBusinessUnitRepositoryTests
{
    private readonly DataContext _context;
    private readonly Mock<ITokenInfoService> _tokenInfoService;

    public ClientEuropeBusinessUnitRepositoryTests(IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
        _tokenInfoService = new Mock<ITokenInfoService>();
    }

    /*********** Geters ***********/
    [Fact]
    public async Task GetByClientEuropeIdAsync_ShouldReturnBusinessUnitList_IfClientEuropeIdIsValid()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        var clientEuropeId = 1;

        // Act
        var expectedBusinessUnitList = await repository.GetBusinessUnitsByClientEuropeIdAsync(clientEuropeId, new ContextSession());

        //Assert
        Assert.NotNull(expectedBusinessUnitList);
        Assert.Equal(2, expectedBusinessUnitList.Count());
    }
    
    [Fact]
    public async Task GetByClientEuropeIdAsync_ShouldReturnNull_IfClientEuropeIdIsZero()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        var clientEuropeId = 0;

        // Act
        var expectedBusinessUnitList = await repository.GetBusinessUnitsByClientEuropeIdAsync(clientEuropeId, new ContextSession());

        //Assert
        Assert.Null(expectedBusinessUnitList);
    }
    
    [Fact]
    public async Task GetByClientEuropeIdAsync_ShouldReturnNull_IfClientEuropeIdIsNegative()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        var clientEuropeId = -1;

        // Act
        var expectedBusinessUnitList = await repository.GetBusinessUnitsByClientEuropeIdAsync(clientEuropeId, new ContextSession());

        //Assert
        Assert.Null(expectedBusinessUnitList);
    }
    
    [Fact]
    public async Task GetByClientEuropeIdAndBusinessUnitIdAsync_ShouldReturnClientEuropeBusinessUnit_IfClientEuropeIdAndBusinessUnitIdAreValid()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        var clientEuropeId = 1;
        var businessUnitId = 1;

        // Act
        var expectedClientEuropeBusinessUnit = await repository.GetByClientEuropeIdAndBusinessUnitIdAsync(clientEuropeId, businessUnitId, new ContextSession());

        //Assert
        Assert.NotNull(expectedClientEuropeBusinessUnit);
        Assert.Equal(1, expectedClientEuropeBusinessUnit.ClientId);
        Assert.Equal(1, expectedClientEuropeBusinessUnit.BusinessUnitId);
    }
    
    [Fact]
    public async Task GetByClientEuropeIdAndBusinessUnitIdAsync_ShouldReturnNull_IfClientEuropeIdIsZero()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        var clientEuropeId = 0;
        var businessUnitId = 1;

        // Act
        var expectedClientEuropeBusinessUnit = await repository.GetByClientEuropeIdAndBusinessUnitIdAsync(clientEuropeId, businessUnitId, new ContextSession());

        //Assert
        Assert.Null(expectedClientEuropeBusinessUnit);
    }
    
    [Fact]
    public async Task GetByClientEuropeIdAndBusinessUnitIdAsync_ShouldReturnNull_IfClientEuropeIdIsNegative()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        var clientEuropeId = -1;
        var businessUnitId = 1;

        // Act
        var expectedClientEuropeBusinessUnit = await repository.GetByClientEuropeIdAndBusinessUnitIdAsync(clientEuropeId, businessUnitId, new ContextSession());

        //Assert
        Assert.Null(expectedClientEuropeBusinessUnit);
    }
    
    [Fact]
    public async Task GetByClientEuropeIdAndBusinessUnitIdAsync_ShouldReturnNull_IfBusinessUnitIdIsZero()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        var clientEuropeId = 1;
        var businessUnitId = 0;

        // Act
        var expectedClientEuropeBusinessUnit = await repository.GetByClientEuropeIdAndBusinessUnitIdAsync(clientEuropeId, businessUnitId, new ContextSession());

        //Assert
        Assert.Null(expectedClientEuropeBusinessUnit);
    }
    
    [Fact]
    public async Task GetByClientEuropeIdAndBusinessUnitIdAsync_ShouldReturnNull_IfBusinessUnitIdIsNegative()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        var clientEuropeId = 1;
        var businessUnitId = -1;

        // Act
        var expectedClientEuropeBusinessUnit = await repository.GetByClientEuropeIdAndBusinessUnitIdAsync(clientEuropeId, businessUnitId, new ContextSession());

        //Assert
        Assert.Null(expectedClientEuropeBusinessUnit);
    }
    
    /*********** Create ***********/
    
    [Fact]
    public async Task CreateAsync_ShouldReturnClientEuropeBusinessUnit_IfClientEuropeBusinessUnitIsValid()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        var clientEuropeBusinessUnit = new ClientEuropeBusinessUnit
        {
            ClientId = 4,
            BusinessUnitId = 1
        };

        // Act
        var expectedClientEuropeBusinessUnit = await repository.CreateAsync(clientEuropeBusinessUnit, new ContextSession());

        //Assert
        Assert.NotNull(expectedClientEuropeBusinessUnit);
        Assert.Equal(4, expectedClientEuropeBusinessUnit.ClientId);
        Assert.Equal(1, expectedClientEuropeBusinessUnit.BusinessUnitId);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnNull_IfClientEuropeBusinessUnitIsNull()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        ClientEuropeBusinessUnit clientEuropeBusinessUnit = null;

        // Act
        var expectedClientEuropeBusinessUnit = await repository.CreateAsync(clientEuropeBusinessUnit, new ContextSession());

        //Assert
        Assert.Null(expectedClientEuropeBusinessUnit);
    }
    
    /********** Delete **********/
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_IfClientEuropeBusinessUnitIsDeleted()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        var clientEuropeId = 2;
        var businessUnitId = 1;

        // Act
        var expectedClientEuropeBusinessUnit = await repository.DeleteAsync(clientEuropeId, businessUnitId, new ContextSession());

        //Assert
        Assert.True(expectedClientEuropeBusinessUnit);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_IfEtablissementIdsAreLessThanOne()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        var clientEuropeId = 0;
        var businessUnitId = 0;

        // Act
        var expectedClientEuropeBusinessUnit = await repository.DeleteAsync(clientEuropeId, businessUnitId, new ContextSession());

        //Assert
        Assert.False(expectedClientEuropeBusinessUnit);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_IfEtablissementIdsAreNegative()
    {
        // Arrange
        var repository = new ClientEuropeBusinessUnitRepository(_context, _tokenInfoService.Object);
        var clientEuropeId = 333;
        var businessUnitId = 1;

        // Act
        var expectedClientEuropeBusinessUnit = await repository.DeleteAsync(clientEuropeId, businessUnitId, new ContextSession());

        //Assert
        Assert.False(expectedClientEuropeBusinessUnit);
    }
}