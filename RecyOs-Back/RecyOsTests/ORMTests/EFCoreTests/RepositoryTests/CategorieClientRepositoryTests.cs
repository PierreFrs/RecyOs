// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => CategorieClientRepository.cs
// Created : 2023/12/26 - 15:17
// Updated : 2023/12/26 - 15:17

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests;

[Collection("RepositoryTests")]
public class CategorieClientRepositoryTests
{
    
    private DataContext _context;
    public CategorieClientRepositoryTests(IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
    }
    
    /*********** Geters ***********/
    [Fact]
    public async Task GetListAsync_ShouldReturnCategorieClientList_IfItExists()
    {
        // Arrange
        var repository = new CategorieClientRepository(_context);

        // Act
        var entrepriseList = await repository.GetListAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<CategorieClient>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(entrepriseList);
        Assert.Equal(2, entrepriseList.Count);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategorieClient_ifIdIsValid()
    {
        // Arrange
        var repository = new CategorieClientRepository(_context);

        // Act
        var expectedCategorieClient = await repository.GetByIdAsync(1, new ContextSession());
        _context.Entry(expectedCategorieClient).State = EntityState.Detached;

        //Assert
        Assert.NotNull(expectedCategorieClient);
        Assert.Equal(1, expectedCategorieClient.Id);
        Assert.Equal("TestCategorieLabel1", expectedCategorieClient.CategorieLabel);
    }

    /*********** Create ***********/
    [Fact]
    public async Task CreateCategoryAsync_ShouldAddCategorieClient_IfDataIsValid()
    {
        // Arrange
        var repository = new CategorieClientRepository(_context);
        var newTypeDocument = new CategorieClient
        {
            CategorieLabel = "NewTestCategorieLabel"
        };

        var categorieClientList = await repository.GetListAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<CategorieClient>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Act
        var expectedCategorieClient = await repository.CreateCategoryAsync(newTypeDocument);
        var updatedCategorieClientList = await repository.GetListAsync(new ContextSession());
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(expectedCategorieClient);
        Assert.Equal(newTypeDocument.CategorieLabel, expectedCategorieClient.CategorieLabel);
        Assert.Equal(updatedCategorieClientList.Count, categorieClientList.Count + 1);
    }
    
    /*********** Update ***********/
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedCategorieClient()
    {
        // Arrange
        var repository = new CategorieClientRepository(_context);
        var newCategorieClient = new CategorieClient
        {
            Id = 1,
            CategorieLabel = "UpdatedLabel"
        };

        // Act
        await repository.UpdateAsync(newCategorieClient, new ContextSession());
        var updatedCategorieClient = await repository.UpdateAsync(newCategorieClient, new ContextSession());
        _context.Entry(updatedCategorieClient).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(updatedCategorieClient);
        Assert.Equal(newCategorieClient.Id, updatedCategorieClient.Id);
        Assert.Equal("UpdatedLabel", updatedCategorieClient.CategorieLabel);
    }

    /*********** Delete ***********/
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrueIfSuccessful()
    {
        // Arrange
        var repository = new CategorieClientRepository(_context);

        // Act
        var expectedResult = await repository.DeleteAsync(2, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<CategorieClient>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.True(expectedResult);
    }
}