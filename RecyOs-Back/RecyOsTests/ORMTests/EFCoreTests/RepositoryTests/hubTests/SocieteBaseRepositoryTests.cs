// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => SocieteRepositoryTests.cs
// Created : 2024/02/26 - 11:02
// Updated : 2024/02/26 - 11:02

using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests;

[Collection("RepositoryTests")]
public class SocieteBaseRepositoryTests
{
    private readonly DataContext _context;
    public SocieteBaseRepositoryTests(IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
    }
    
    /*********** Geters ***********/
    [Fact]
    public async Task GetListAsync_ShouldReturnSocieteList_IfItExists()
    {
        // Arrange
        var repository = new SocieteBaseRepository(_context);

        // Act
        var entrepriseList = await repository.GetListAsync();
        var trackedEntities = _context.ChangeTracker.Entries<Societe>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(entrepriseList);
        Assert.Equal(3, entrepriseList.Count);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnSociete_ifIdIsValid()
    {
        // Arrange
        var repository = new SocieteBaseRepository(_context);

        // Act
        var expectedSociete = await repository.GetByIdAsync(2, new ContextSession());
        _context.Entry(expectedSociete).State = EntityState.Detached;

        //Assert
        Assert.NotNull(expectedSociete);
        Assert.Equal(2, expectedSociete.Id);
        Assert.Equal("BP Trans", expectedSociete.Nom);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_ifIdIsInvalid()
    {
        // Arrange
        var repository = new SocieteBaseRepository(_context);

        // Act
        var expectedSociete = await repository.GetByIdAsync(0, new ContextSession());

        //Assert
        Assert.Null(expectedSociete);
    }
    
    /*********** Create ***********/
    [Fact]
    public async Task CreateSocieteAsync_ShouldAddSociete_IfDataIsValid()
    {
        // Arrange
        var repository = new SocieteBaseRepository(_context);
        var newSociete = new Societe
        {
            Nom = "NewSociete",
            IdOdoo = "4",
        };
        var societeList = await repository.GetListAsync();
        var trackedEntities = _context.ChangeTracker.Entries<Societe>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        var expectedSociete = await repository.CreateAsync(newSociete, new ContextSession());
        var updatedSocieteList = await repository.GetListAsync();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Assert
        Assert.NotNull(expectedSociete);
        Assert.Equal(4, expectedSociete.Id);
        Assert.Equal(updatedSocieteList.Count, societeList.Count + 1);
    }
    
    /*********** Update ***********/
    [Fact]
    public async Task UpdateSocieteAsync_ShouldUpdateSociete_IfDataIsValid()
    {
        // Arrange
        var repository = new SocieteBaseRepository(_context);
        var updatedSociete = new Societe
        {
            Id = 1,
            Nom = "UpdatedSociete",
            IdOdoo = "1",
        };
        var societeList = await repository.GetListAsync();
        var trackedEntities = _context.ChangeTracker.Entries<Societe>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        var expectedSociete = await repository.UpdateAsync(updatedSociete, new ContextSession());
        var updatedSocieteList = await repository.GetListAsync();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Assert
        Assert.NotNull(expectedSociete);
        Assert.Equal(updatedSociete.Nom, expectedSociete.Nom);
        Assert.Equal(updatedSocieteList.Count, societeList.Count);
    }
    
    [Fact]
    public async Task UpdateSocieteAsync_ShouldReturnNull_IfSocieteDoesntExist()
    {
        // Arrange
        var repository = new SocieteBaseRepository(_context);
        var updatedSociete = new Societe
        {
            Id = 5,
            Nom = "UpdatedSociete",
            IdOdoo = "5",
        };
        var trackedEntities = _context.ChangeTracker.Entries<Societe>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        var expectedSociete = await repository.UpdateAsync(updatedSociete, new ContextSession());
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Assert
        Assert.Null(expectedSociete);
    }
    
    /*********** Delete ***********/
    [Fact]
    public async Task DeleteSocieteAsync_ShouldReturnTrue_IfSocieteExists()
    {
        // Arrange
        var repository = new SocieteBaseRepository(_context);
        var societeList = await repository.GetListAsync();
        var trackedEntities = _context.ChangeTracker.Entries<Societe>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        var isDeleted = await repository.DeleteAsync(1, new ContextSession());
        var updatedSocieteList = await repository.GetListAsync();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Assert
        Assert.True(isDeleted);
        Assert.Equal(updatedSocieteList.Count, societeList.Count - 1);
    }
    
    /*********** Exists ***********/
    [Fact]
    public async Task Exists_ShouldReturnTrue_IfSocieteExists()
    {
        // Arrange
        var repository = new SocieteBaseRepository(_context);
        var societe = new Societe
        {
            Id = 1,
            Nom = "RecyGroup",
            IdOdoo = "1",
        };
        
        // Act
        var exists = await repository.Exists(societe, new ContextSession());
        
        // Assert
        Assert.True(exists);
    }
}