// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => BalanceParticulierRepositoryTests.cs
// Created : 2024/03/19 - 14:08
// Updated : 2024/03/19 - 14:08

using Microsoft.EntityFrameworkCore;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests;

[Collection("RepositoryTests")]
public class BalanceParticulierRepositoryTests
{
    private readonly DataContext _context;
    private readonly Mock<ITokenInfoService> _mockTokenInfoService;
    
    public BalanceParticulierRepositoryTests(
        IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
        _mockTokenInfoService = new Mock<ITokenInfoService>();
    }
    
    /*********** Geters ***********/
    [Fact]
    public async Task GetAllAsync_ShouldReturnBalanceParticulierList_IfItExists()
    {
        // Arrange
        var repository = new BalanceParticulierRepository(_context, _mockTokenInfoService.Object);

        // Act
        var balanceParticulierList = await repository.GetAllAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.NotNull(balanceParticulierList);
        Assert.Equal(3, balanceParticulierList.Count);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldNotReturnDeletedEntities_IfTheyExist()
    {
        // Arrange
        var repository = new BalanceParticulierRepository(_context, _mockTokenInfoService.Object);
        var deletedBalance = new BalanceParticulier
        {
            Id = 4,
            IsDeleted = true
        };
        
        _context.BalanceParticuliers.Add(deletedBalance);

        // Act
        var balanceParticulierList = await repository.GetAllAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.NotNull(balanceParticulierList);
        Assert.DoesNotContain(balanceParticulierList, x => x.IsDeleted);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnBalanceParticulier_IfItExists()
    {
        // Arrange
        var repository = new BalanceParticulierRepository(_context, _mockTokenInfoService.Object);

        // Act
        var balanceParticulier = await repository.GetByIdAsync(1, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.NotNull(balanceParticulier);
        Assert.Equal(1, balanceParticulier.Id);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldNotReturnDeletedEntity_IfItExists()
    {
        // Arrange
        var repository = new BalanceParticulierRepository(_context, _mockTokenInfoService.Object);
        var deletedBalance = new BalanceParticulier
        {
            Id = 4,
            IsDeleted = true
        };
        
        _context.BalanceParticuliers.Add(deletedBalance);

        // Act
        var balanceParticulier = await repository.GetByIdAsync(4, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.Null(balanceParticulier);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_ShouldReturnBalanceParticulierList_IfItExists()
    {
        // Arrange
        var repository = new BalanceParticulierRepository(_context, _mockTokenInfoService.Object);

        // Act
        var balanceParticulierList = await repository.GetByClientIdAsync(1, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.NotNull(balanceParticulierList);
        Assert.Equal(3, balanceParticulierList.Count);
    }
    
    /* Create */
    [Fact]
    public async Task CreateAsync_ShouldReturnBalanceParticulier_IfItIsCreated()
    {
        // Arrange
        _mockTokenInfoService.Setup(t => t.GetCurrentUserName()).Returns("TestUser");

        var repository = new BalanceParticulierRepository(_context, _mockTokenInfoService.Object);
        var balanceParticulier = new BalanceParticulier
        {
            Id = 4,
            ClientId = 1,
            SocieteId = 7,
            Montant = 100,
        };

        // Act
        var createdBalanceParticulier = await repository.CreateAsync(balanceParticulier, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.NotNull(createdBalanceParticulier);
        Assert.Equal(4, createdBalanceParticulier.Id);
    }
    
    /* Update */
    [Fact]
    public async Task UpdateAsync_ShouldReturnBalanceParticulier_IfItIsUpdated()
    {
        // Arrange
        var repository = new BalanceParticulierRepository(_context, _mockTokenInfoService.Object);
        var balanceParticulier = new BalanceParticulier
        {
            Id = 1,
            ClientId = 1,
            SocieteId = 1,
            Montant = 200,
            CreatedBy = "Test",
            UpdatedBy = "Test"
        };

        // Act
        var updatedBalanceParticulier = await repository.UpdateAsync(balanceParticulier.Id, balanceParticulier, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.NotNull(updatedBalanceParticulier);
        Assert.Equal(1, updatedBalanceParticulier.Id);
    }
    
    /* Delete */
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_IfItIsDeleted()
    {
        // Arrange
        var repository = new BalanceParticulierRepository(_context, _mockTokenInfoService.Object);

        // Act
        var isDeleted = await repository.DeleteAsync(1, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.True(isDeleted);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_IfItIsNotDeleted()
    {
        // Arrange
        var repository = new BalanceParticulierRepository(_context, _mockTokenInfoService.Object);

        // Act
        var isDeleted = await repository.DeleteAsync(4, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceParticulier>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached;
        }

        // Assert
        Assert.False(isDeleted);
    }

    [Fact]
    public async Task GetFilteredListWithCount_ShouldReturnAllBalanceParticuliers()
    {
        // Arrange
        var repository = new BalanceParticulierRepository(_context, _mockTokenInfoService.Object);

        // Act
        var (balances, count, total) = await repository.GetFilteredListWithCount(new BalanceParticulierGridFilter(), false);
        foreach (var r in balances)
        {
            _context.Entry(r).State = EntityState.Detached;
        }

        // Assert
        Assert.NotNull(balances);
        Assert.Equal(3, count);  // Three balances from test data
        Assert.Equal(75000m, total);  // Sum of montants (15000 + 25000 + 35000)
        
        // Verify all expected montants are present, regardless of order
        var montants = balances.Select(b => b.Montant).ToList();
        Assert.Contains(15000m, montants);
        Assert.Contains(25000m, montants);
        Assert.Contains(35000m, montants);
    }
}
