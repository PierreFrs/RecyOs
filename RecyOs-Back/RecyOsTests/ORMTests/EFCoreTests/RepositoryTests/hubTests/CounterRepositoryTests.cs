using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests;

[Collection("CounterRepositoryTests")]
public class CounterRepositoryTests
{
    private readonly DataContext _context;
  
    public CounterRepositoryTests(IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
    }

    [Fact]
    public async Task GetCounterById_ShouldReturnCounter()
    {
        // Arrange
        var repository = new CounterRepository(_context);
        
        // Act
        var result = await repository.GetCounterById(1, new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Client_gpi", result.Name);
        Assert.Equal(260, result.Value);
        Assert.Equal("Compteur de clients gpi", result.Description);
    }
    
    [Fact]
    public async Task GetCounterByName_ShouldReturnCounter()
    {
        // Arrange
        var repository = new CounterRepository(_context);
        
        // Act
        var result = await repository.GetCounterByName("Client_gpi", new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(260, result.Value);
        Assert.Equal("Compteur de clients gpi", result.Description);
    }
    
    [Fact]
    public async Task IncrementCounterByName_ShouldReturnCounter()
    {
        // Arrange
        var repository = new CounterRepository(_context);
        
        // Act
        var result = await repository.IncrementCounterByName("Client_toto", new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
        Assert.Equal(53, result.Value);
        Assert.Equal("Compteur de clients toto", result.Description);
    }
    
    [Fact]
    public async Task IncrementCounterByName_ShouldCreateCounter()
    {
        // Arrange
        var repository = new CounterRepository(_context);
        
        // Act
        var result = await repository.IncrementCounterByName("Client_titi", new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Id);
        Assert.Equal(1, result.Value);
        Assert.Equal("Counter for Client_titi", result.Description);
    }
}