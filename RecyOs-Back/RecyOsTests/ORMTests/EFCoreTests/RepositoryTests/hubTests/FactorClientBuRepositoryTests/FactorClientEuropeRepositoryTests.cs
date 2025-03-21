// <copyright file="FactorClientEuropeRepositoryTests.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using Moq;
using RecyOs.Exceptions;
using RecyOs.Helpers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.EFCore.Repository.hub.FactorClientBuRepositories;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests.FactorClientBuRepositoryTests;

[Collection("RepositoryTests")]

public class FactorClientEuropeRepositoryTests
{
    private DataContext _context;
    private readonly Mock<ITokenInfoService> _tokenInfoService;

    public FactorClientEuropeRepositoryTests(
        IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
        _tokenInfoService = new Mock<ITokenInfoService>();
    }
    
    /*********** Geters ***********/
    
    [Fact]
    public async Task GetListAsync_ShouldReturnFactorClientEuropeList()
    {
        // Arrange
        var repository = new FactorClientEuropeBuRepository(_context, _tokenInfoService.Object);

        // Act
        var factorClientEuropeList = await repository.GetListAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<FactorClientEuropeBu>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(factorClientEuropeList);
        Assert.Equal(3, factorClientEuropeList.Count);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_ShouldReturnFactorClientEuropeList_IfItExists()
    {
        // Arrange
        var repository = new FactorClientEuropeBuRepository(_context, _tokenInfoService.Object);

        // Act
        var factorClientEuropeList = await repository.GetByClientIdAsync(new ContextSession(), 1);
        var trackedEntities = _context.ChangeTracker.Entries<FactorClientEuropeBu>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(factorClientEuropeList);
        Assert.Equal(2, factorClientEuropeList.Count);
    }

    [Fact]
    public async Task GetByClientIdAsync_ShouldReturnFactorClientEuropeList_IfItExistsAndIncludeDeleted()
    {
        // Arrange
        var repository = new FactorClientEuropeBuRepository(_context, _tokenInfoService.Object);

        // Act
        var factorClientEuropeList = await repository.GetByClientIdAsync(new ContextSession(), 1, true);
        var trackedEntities = _context.ChangeTracker.Entries<FactorClientEuropeBu>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(factorClientEuropeList);
        Assert.Equal(2, factorClientEuropeList.Count);
    }
    
    [Fact]
    public async Task GetByBuIdAsync_ShouldReturnFactorClientEuropeList_IfItExists()
    {
        // Arrange
        var repository = new FactorClientEuropeBuRepository(_context, _tokenInfoService.Object);

        // Act
        var factorClientEuropeList = await repository.GetByBuIdAsync(new ContextSession(), 1, false);
        var trackedEntities = _context.ChangeTracker.Entries<FactorClientEuropeBu>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(factorClientEuropeList);
        Assert.Equal(2, factorClientEuropeList.Count);
    }
    
    [Fact]
public async Task GetByBuIdAsync_ShouldThrowAnException_IfItDoesNotExist()
    {
        // Arrange
        var repository = new FactorClientEuropeBuRepository(_context, _tokenInfoService.Object);

        // Act
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await repository.GetByBuIdAsync(new ContextSession(), 99);
        });
    }

    /********** Setters **********/
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFactorClientEurope_IfItDoesNotExist()
    {
        // Arrange
        var repository = new FactorClientEuropeBuRepository(_context, _tokenInfoService.Object);
        var factorClientEurope = new FactorClientEuropeBu
        {
            IdClient = 3,
            IdBu = 3
        };

        // Act
        var result = await repository.CreateAsync(factorClientEurope, new ContextSession());
        var factorClientEuropeList = await repository.GetListAsync(new ContextSession());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, factorClientEuropeList.Count);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFactorClientEurope_IfItExistsAndIsDeleted()
    {
        // Arrange
        var repository = new FactorClientEuropeBuRepository(_context, _tokenInfoService.Object);
        
        var deletedEntity = new FactorClientEuropeBu
        {
            IdClient = 5,
            IdBu = 5,
            CreateDate = DateTime.Now,
            IsDeleted = true
        };
        
        await repository.CreateAsync(deletedEntity, new ContextSession());
        
        var factorClientEurope = new FactorClientEuropeBu
        {
            IdClient = 5,
            IdBu = 5
        };

        // Act
        var result = await repository.CreateAsync(factorClientEurope, new ContextSession());
        var factorClientEuropeList = await repository.GetListAsync(new ContextSession());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(4, factorClientEuropeList.Count);
    }
    
    [Fact]
    public async Task CreateAsync_ShouldThrowInvalidOperationException_IfFactorClientEuropeExists()
    {
        // Arrange
        var repository = new FactorClientEuropeBuRepository(_context, _tokenInfoService.Object);
        var factorClientEurope = new FactorClientEuropeBu
        {
            IdClient = 4,
            IdBu = 4
        };

        // Act
        await repository.CreateAsync(factorClientEurope, new ContextSession());
        var factorClientEuropeList = await repository.GetListAsync(new ContextSession());

        // Assert
        await Assert.ThrowsAsync<RepositoryException>(() => repository.CreateAsync(factorClientEurope, new ContextSession()));
        Assert.Equal(4, factorClientEuropeList.Count);
    }



    /*********** Delete ***********/
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_IfFactorClientEuropeExists()
    {
        // Arrange
        var repository = new FactorClientEuropeBuRepository(_context, _tokenInfoService.Object);

        var entity = new FactorClientEuropeBu
        {
            IdClient = 9,
            IdBu = 9,
            CreateDate = DateTime.Now,
        };

        await repository.CreateAsync(entity, new ContextSession());

        // Act
        var result = await repository.DeleteAsync(9, 9, new ContextSession());
        var factorClientEuropeList = await repository.GetListAsync(new ContextSession());

        // Assert
        Assert.True(result);
        Assert.Equal(3, factorClientEuropeList.Count);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldThrowInvalidOperationException_IfFactorClientEuropeDoesNotExist()
    {
        // Arrange
        var repository = new FactorClientEuropeBuRepository(_context, _tokenInfoService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await repository.DeleteAsync(99, 99, new ContextSession());
        });
    }

    
}