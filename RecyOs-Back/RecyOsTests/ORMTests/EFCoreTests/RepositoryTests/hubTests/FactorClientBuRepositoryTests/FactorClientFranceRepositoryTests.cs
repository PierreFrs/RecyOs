// <copyright file="FactorClientFranceRepositoryTests.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Moq;
using RecyOs.Exceptions;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository.hub.FactorClientBuRepositories;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests.FactorClientBuRepositoryTests;

[Collection("RepositoryTests")]

public class FactorClientFranceRepositoryTests
{
    private DataContext _context;
    private readonly Mock<ITokenInfoService> _tokenInfoService;

    public FactorClientFranceRepositoryTests(
        IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
        _tokenInfoService = new Mock<ITokenInfoService>();
    }
    
    /*********** Geters ***********/
    
    [Fact]
    public async Task GetListAsync_ShouldReturnFactorClientFranceList()
    {
        // Arrange
        var repository = new FactorClientFranceBuRepository(_context, _tokenInfoService.Object);

        // Act
        var factorClientFranceList = await repository.GetListAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<FactorClientFranceBu>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(factorClientFranceList);
        Assert.Equal(3, factorClientFranceList.Count);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_ShouldReturnFactorClientFranceList_IfItExists()
    {
        // Arrange
        var repository = new FactorClientFranceBuRepository(_context, _tokenInfoService.Object);

        // Act
        var factorClientFranceList = await repository.GetByClientIdAsync(new ContextSession(), 1);
        var trackedEntities = _context.ChangeTracker.Entries<FactorClientFranceBu>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(factorClientFranceList);
        Assert.Equal(2, factorClientFranceList.Count);
    }

    [Fact]
    public async Task GetByClientIdAsync_ShouldReturnFactorClientFranceList_IfItExistsAndIncludeDeleted()
    {
        // Arrange
        var repository = new FactorClientFranceBuRepository(_context, _tokenInfoService.Object);

        // Act
        var factorClientFranceList = await repository.GetByClientIdAsync(new ContextSession(), 1, true);
        var trackedEntities = _context.ChangeTracker.Entries<FactorClientFranceBu>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(factorClientFranceList);
        Assert.Equal(2, factorClientFranceList.Count);
    }
    
    [Fact]
    public async Task GetByBuIdAsync_ShouldReturnFactorClientFranceList_IfItExists()
    {
        // Arrange
        var repository = new FactorClientFranceBuRepository(_context, _tokenInfoService.Object);

        // Act
        var factorClientFranceList = await repository.GetByBuIdAsync(new ContextSession(), 1, false);
        var trackedEntities = _context.ChangeTracker.Entries<FactorClientFranceBu>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(factorClientFranceList);
        Assert.Equal(2, factorClientFranceList.Count);
    }
    
    [Fact]
public async Task GetByBuIdAsync_ShouldThrowAnException_IfItDoesNotExist()
    {
        // Arrange
        var repository = new FactorClientFranceBuRepository(_context, _tokenInfoService.Object);

        // Act
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await repository.GetByBuIdAsync(new ContextSession(), 99);
        });
    }

    /********** Setters **********/
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFactorClientFrance_IfItDoesNotExist()
    {
        // Arrange
        var repository = new FactorClientFranceBuRepository(_context, _tokenInfoService.Object);
        var factorClientFrance = new FactorClientFranceBu
        {
            IdClient = 3,
            IdBu = 3
        };

        // Act
        var result = await repository.CreateAsync(factorClientFrance, new ContextSession());
        var factorClientFranceList = await repository.GetListAsync(new ContextSession());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, factorClientFranceList.Count);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFactorClientFrance_IfItExistsAndIsDeleted()
    {
        // Arrange
        var repository = new FactorClientFranceBuRepository(_context, _tokenInfoService.Object);
        
        var deletedEntity = new FactorClientFranceBu
        {
            IdClient = 5,
            IdBu = 5,
            CreateDate = DateTime.Now,
            IsDeleted = true
        };
        
        await repository.CreateAsync(deletedEntity, new ContextSession());
        
        var factorClientFrance = new FactorClientFranceBu
        {
            IdClient = 5,
            IdBu = 5
        };

        // Act
        var result = await repository.CreateAsync(factorClientFrance, new ContextSession());
        var factorClientFranceList = await repository.GetListAsync(new ContextSession());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, factorClientFranceList.Count);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldThrowInvalidOperationException_IfFactorClientFranceExists()
    {
        // Arrange
        var repository = new FactorClientFranceBuRepository(_context, _tokenInfoService.Object);
        var factorClientFrance = new FactorClientFranceBu
        {
            IdClient = 4,
            IdBu = 4
        };

        // Act
        await repository.CreateAsync(factorClientFrance, new ContextSession());
        var factorClientFranceList = await repository.GetListAsync(new ContextSession());

        // Assert
        await Assert.ThrowsAsync<RepositoryException>(() => repository.CreateAsync(factorClientFrance, new ContextSession()));
        Assert.Equal(4, factorClientFranceList.Count);
    }

    /*********** Delete ***********/
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_IfFactorClientFranceExists()
    {
        // Arrange
        var repository = new FactorClientFranceBuRepository(_context, _tokenInfoService.Object);

        var entity = new FactorClientFranceBu
        {
            IdClient = 9,
            IdBu = 9,
            CreateDate = DateTime.Now,
        };

        await repository.CreateAsync(entity, new ContextSession());

        // Act
        var result = await repository.DeleteAsync(9, 9, new ContextSession());
        var factorClientFranceList = await repository.GetListAsync(new ContextSession());

        // Assert
        Assert.True(result);
        Assert.Equal(3, factorClientFranceList.Count);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldThrowInvalidOperationException_IfFactorClientFranceDoesNotExist()
    {
        // Arrange
        var repository = new FactorClientFranceBuRepository(_context, _tokenInfoService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await repository.DeleteAsync(99, 99, new ContextSession());
        });
    }

    
}