using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository.Application;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOsTests.Interfaces;
using RecyOsTests.Services;

namespace RecyOsTests;

public class ParameterRepositoryTests
{
    private readonly DataContext _context;
    public ParameterRepositoryTests(
        IDataContextTests dataContextTests
        )
    {
        _context = dataContextTests.GetContext();
    }
    /*********** Getters ***********/
    [Fact]
    public async Task GetAsync_ValidID_ShouldReturnObject()
    {
        // Arrange
        var repository = new ParameterRepository(_context);
        
        // Act
        var expectedObject = await repository.GetAsync(1, new ContextSession());
        _context.Entry(expectedObject).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(expectedObject);
        Assert.Equal("Param1", expectedObject.Nom);
        Assert.Equal("Value1", expectedObject.Valeur);
        Assert.Equal("Test", expectedObject.Module);
        Assert.Equal("Test engineer", expectedObject.CreatedBy);
    }
    
    [Fact]
    public async Task GetAsync_InvalidID_ShouldReturnNull()
    {
        // Arrange
        var repository = new ParameterRepository(_context);
        
        // Act
        var expectedObject = await repository.GetAsync(0, new ContextSession());
        
        // Assert
        Assert.Null(expectedObject);
    }
    
    [Fact]
    public async Task GetByNomAsync_ValidNom_ShouldReturnObject()
    {
        // Arrange
        var repository = new ParameterRepository(_context);
        
        // Act
        var expectedObject = await repository.GetByNom("Test","Param1", new ContextSession());
        _context.Entry(expectedObject).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(expectedObject);
        Assert.Equal("Param1", expectedObject.Nom);
        Assert.Equal("Value1", expectedObject.Valeur);
        Assert.Equal("Test", expectedObject.Module);
        Assert.Equal("Test engineer", expectedObject.CreatedBy);
    }
    
    [Fact]
    public async Task GetByNomAsync_InvalidNom_ShouldReturnNull()
    {
        // Arrange
        var repository = new ParameterRepository(_context);
        
        // Act
        var expectedObject = await repository.GetByNom("Test","Param1001", new ContextSession());
        
        // Assert
        Assert.Null(expectedObject);
    }
    
    [Fact]
    public async Task GetDataForGridAsync_ShouldReturnObject()
    {
        // Arrange
        var repository = new ParameterRepository(_context);
        var filter = new ParameterFilter
        {
            PageSize = 2,
            PageNumber = 0
        };
        
        // Act
        var (parameters, totalCount) = await repository.GetDataForGrid(filter, new ContextSession());
        
        // Assert
        Assert.NotNull(parameters);
        Assert.Equal(2, parameters.Count());
        Assert.NotEqual(0 , totalCount);
    }
    [Fact]
    public async Task GetDataForGridAsync_OutRange_ShouldReturnEmpty()
    {
        // Arrange
        var repository = new ParameterRepository(_context);
        var filter = new ParameterFilter
        {
            PageSize = 2,
            PageNumber = 10
        };
        
        // Act
        var (parameters, totalCount) = await repository.GetDataForGrid(filter, new ContextSession());
        
        // Assert
        Assert.NotNull(parameters);
        Assert.Empty(parameters);
        Assert.NotEqual(0 , totalCount);
    }
    
    /*********** Updaters ***********/
    [Fact]
    public async Task UpdateAsync_ValidObject_ShouldReturnObject()
    {
        // Arrange
        var repository = new ParameterRepository(_context);
        var expectedObject = await repository.GetAsync(2, new ContextSession());
        _context.Entry(expectedObject).State = EntityState.Detached;
        expectedObject.Valeur = "Value200";
        
        // Act
        var updatedObject = await repository.UpdateAsync(expectedObject, new ContextSession());
        _context.Entry(updatedObject).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(updatedObject);
        Assert.Equal("Param2", updatedObject.Nom);
        Assert.Equal("Value200", updatedObject.Valeur);
        Assert.Equal("Test", updatedObject.Module);
        Assert.Equal("Test engineer", updatedObject.CreatedBy);
    }
    
    [Fact]
    public async Task UpdateAsync_NewObject_ShouldCreateNewObject()
    {
        // Arrange
        var repository = new ParameterRepository(_context);
        var expectedObject = new Parameter
        {
            Nom = "Param10",
            Valeur = "Value10",
            Module = "Test",
            CreatedBy = "Test engineer"
        };
        
        // Act
        await repository.UpdateAsync(expectedObject, new ContextSession());
        var createdObject = await repository.GetByNom("Test","Param10", new ContextSession());
        
        // Assert
        Assert.NotNull(createdObject);
        Assert.Equal("Param10", createdObject.Nom);
        Assert.Equal("Value10", createdObject.Valeur);
        Assert.Equal("Test", createdObject.Module);
        Assert.Equal("Test engineer", createdObject.CreatedBy);
    }
    /*********** Creators ***********/
    [Fact]
    public async Task CreateAsync_ValidObject_ShouldReturnObject()
    {
        // Arrange
        var repository = new ParameterRepository(_context);
        var expectedObject = new Parameter
        {
            Nom = "Param11",
            Valeur = "Value11",
            Module = "Test",
            CreatedBy = "Test engineer"
        };
        
        // Act
        var createdObject = await repository.CreateAsync(expectedObject, new ContextSession());
        _context.Entry(createdObject).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(createdObject);
        Assert.Equal("Param11", createdObject.Nom);
        Assert.Equal("Value11", createdObject.Valeur);
        Assert.Equal("Test", createdObject.Module);
        Assert.Equal("Test engineer", createdObject.CreatedBy);
    }
    /*********** Deleters ***********/
    [Fact]
    public async Task DeleteAsync_ValidID_ShouldBeDeleted()
    {
        // Arrange
        var repository = new ParameterRepository(_context);
        
        // Act
        var result = await repository.DeleteAsync(3, new ContextSession());
        var deletedObject = await repository.GetAsync(3, new ContextSession());
        
        // Assert
        Assert.True(result);
        Assert.Null(deletedObject);
    }
}