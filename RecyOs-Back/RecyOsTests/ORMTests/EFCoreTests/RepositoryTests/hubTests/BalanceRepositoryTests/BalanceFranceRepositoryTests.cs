// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => BalanceFranceRepositoryTests.cs
// Created : 2024/02/26 - 14:08
// Updated : 2024/02/26 - 14:08

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
public class BalanceFranceRepositoryTests
{
    private readonly DataContext _context;
    private readonly Mock<IRoleRepository<Role>> _mockRoleRepository;
    private readonly Mock<IUserRepository<User>> _mockUserRepository;
    private readonly Mock<ITokenInfoService> _mockTokenInfoService;

    public BalanceFranceRepositoryTests(
        IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
        _mockTokenInfoService = new Mock<ITokenInfoService>();
        _mockUserRepository = new Mock<IUserRepository<User>>();
        _mockRoleRepository = new Mock<IRoleRepository<Role>>();
    }
    
    /*********** Geters ***********/
    [Fact]
    public async Task GetAllAsync_ShouldReturnBalanceFranceList_IfItExists()
    {
        // Arrange
        var repository = new BalanceFranceRepository(
            _context,
            _mockRoleRepository.Object,
            _mockUserRepository.Object,
            _mockTokenInfoService.Object);

        // Act
        var balanceFranceList = await repository.GetAllAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceFrance>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(balanceFranceList);
        Assert.Equal(3, balanceFranceList.Count);
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldNotReturnDeletedEntities_IfTheyExist()
    {
        // Arrange
        var repository = new BalanceFranceRepository(
            _context,
            _mockRoleRepository.Object,
            _mockUserRepository.Object,
            _mockTokenInfoService.Object);
        var deletedBalance = new BalanceFrance
        {
            Id = 4,
            IsDeleted = true
        };
        
        _context.BalanceFrances.Add(deletedBalance);

        // Act
        var balanceFranceList = await repository.GetAllAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceFrance>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(balanceFranceList);
        Assert.DoesNotContain(balanceFranceList, x => x.IsDeleted);
    }

    [Fact]
    public async Task GetFilteredListWithCount_ShouldReturnBalanceFranceListForCommercial_IfUserIsCommercial()
    {
        // Arrange
        var repository = new BalanceFranceRepository(_context, _mockRoleRepository.Object, _mockUserRepository.Object, _mockTokenInfoService.Object);

        _mockTokenInfoService.Setup(t => t.GetCurrentUserId()).Returns(1);
        _mockRoleRepository.Setup(r => r.GetListByUserId(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new List<Role> { new() { Id = 1, Name = "commercial" } });
        _mockUserRepository.Setup(u => u.Get(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new User { Id = 1, SocieteId = 1 });

        // Act
        var (balances, count, total) = await repository.GetFilteredListWithCount(new BalanceFranceGridFilter(), false);
        foreach (var r in balances)
        {
            _context.Entry(r).State = EntityState.Detached;
        }
    
        // Assert
        Assert.NotNull(balances);
        Assert.Equal(1, count);
        Assert.Equal(10000, total);
        Assert.All(balances, b => Assert.Equal(1, b.SocieteId));
    }

    [Fact]
    public async Task GetFilteredListWithCount_ShouldReturnBalanceFranceListForBuDirecteur_IfUserIsBuDirecteur()
    {
        // Arrange
        var repository = new BalanceFranceRepository(_context, _mockRoleRepository.Object, _mockUserRepository.Object, _mockTokenInfoService.Object);

        _mockTokenInfoService.Setup(t => t.GetCurrentUserId()).Returns(1);
        _mockRoleRepository.Setup(r => r.GetListByUserId(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new List<Role> { new() { Id = 2, Name = "responsable_bu" } });
        _mockUserRepository.Setup(u => u.Get(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new User { Id = 1, SocieteId = 1 });

        // Act
        var (balances, count, total) = await repository.GetFilteredListWithCount(new BalanceFranceGridFilter(), false);
        foreach (var r in balances)
        {
            _context.Entry(r).State = EntityState.Detached;
        }

        // Assert
        Assert.NotNull(balances);
        Assert.Equal(1, count);
        Assert.Equal(10000, total);
        Assert.All(balances, b => Assert.Equal(1, b.SocieteId));
    }

    [Fact]
    public async Task GetFilteredListWithCount_ShouldReturnBalanceFranceListForCompta_IfUserIsCompta()
    {
        // Arrange
        var repository = new BalanceFranceRepository(_context, _mockRoleRepository.Object, _mockUserRepository.Object, _mockTokenInfoService.Object);

        _mockTokenInfoService.Setup(t => t.GetCurrentUserId()).Returns(1);
        _mockRoleRepository.Setup(r => r.GetListByUserId(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new List<Role> { new() { Id = 3, Name = "compta" } });
        _mockUserRepository.Setup(u => u.Get(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new User { Id = 1, SocieteId = 2 });

        // Act
        var (balances, count, total) = await repository.GetFilteredListWithCount(new BalanceFranceGridFilter(), false);
        foreach (var r in balances)
        {
            _context.Entry(r).State = EntityState.Detached;
        }

        // Assert
        Assert.NotNull(balances);
        Assert.Equal(2, count);
        Assert.Equal(50000, total);
        Assert.All(balances, b => Assert.NotEqual(1, b.SocieteId));
    }

    [Fact]
    public async Task GetFilteredListWithCount_ShouldThrowUnauthorizedAccessException_IfUserHasNoValidRole()
    {
        // Arrange
        var repository = new BalanceFranceRepository(_context, _mockRoleRepository.Object, _mockUserRepository.Object, _mockTokenInfoService.Object);

        _mockTokenInfoService.Setup(t => t.GetCurrentUserId()).Returns(1);
        _mockRoleRepository.Setup(r => r.GetListByUserId(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new List<Role> { new() { Id = 4, Name = "invalid_role" } });
        _mockUserRepository.Setup(u => u.Get(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new User { Id = 1, SocieteId = 2 });

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => 
            repository.GetFilteredListWithCount(new BalanceFranceGridFilter(), false));
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnBalanceFrance_IfItExists()
    {
        // Arrange
        var repository = new BalanceFranceRepository(
            _context,
            _mockRoleRepository.Object,
            _mockUserRepository.Object,
            _mockTokenInfoService.Object);

        // Act
        var balanceFrance = await repository.GetByIdAsync(1, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceFrance>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(balanceFrance);
        Assert.Equal(1, balanceFrance.Id);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldNotReturnDeletedEntity_IfItExists()
    {
        // Arrange
        var repository = new BalanceFranceRepository(
            _context,
            _mockRoleRepository.Object,
            _mockUserRepository.Object,
            _mockTokenInfoService.Object);
        var deletedBalance = new BalanceFrance
        {
            Id = 4,
            IsDeleted = true
        };
        
        _context.BalanceFrances.Add(deletedBalance);

        // Act
        var balanceFrance = await repository.GetByIdAsync(4, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceFrance>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.Null(balanceFrance);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_ShouldReturnBalanceFranceList_IfItExists()
    {
        // Arrange
        var repository = new BalanceFranceRepository(
            _context,
            _mockRoleRepository.Object,
            _mockUserRepository.Object,
            _mockTokenInfoService.Object);

        // Act
        var balanceFranceList = await repository.GetByClientIdAsync(1, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceFrance>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(balanceFranceList);
        Assert.Equal(3, balanceFranceList.Count);
    }
    
    /* Create */
    [Fact]
    public async Task CreateAsync_ShouldReturnBalanceFrance_IfItIsCreated()
    {
        // Arrange
        _mockTokenInfoService.Setup(t => t.GetCurrentUserName()).Returns("TestUser");

        var repository = new BalanceFranceRepository(
            _context,
            _mockRoleRepository.Object,
            _mockUserRepository.Object,
            _mockTokenInfoService.Object);
        var balanceFrance = new BalanceFrance
        {
            Id = 4,
            ClientId = 1,
            SocieteId = 7,
            Montant = 100,
        };


        // Act
        var createdBalanceFrance = await repository.CreateAsync(balanceFrance, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceFrance>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(createdBalanceFrance);
        Assert.Equal(4, createdBalanceFrance.Id);
    }
    
    /* Update */
    [Fact]
    public async Task UpdateAsync_ShouldReturnBalanceFrance_IfItIsUpdated()
    {
        // Arrange
        var repository = new BalanceFranceRepository(
            _context,
            _mockRoleRepository.Object,
            _mockUserRepository.Object,
            _mockTokenInfoService.Object);
        var balanceFrance = new BalanceFrance
        {
            Id = 1,
            ClientId = 1,
            SocieteId = 1,
            Montant = 200,
            CreatedBy = "Test",
            UpdatedBy = "Test"
        };

        // Act
        var updatedBalanceFrance = await repository.UpdateAsync(balanceFrance.Id, balanceFrance, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceFrance>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(updatedBalanceFrance);
        Assert.Equal(1, updatedBalanceFrance.Id);
    }
    
    /* Delete */
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_IfItIsDeleted()
    {
        // Arrange
        var repository = new BalanceFranceRepository(
            _context,
            _mockRoleRepository.Object,
            _mockUserRepository.Object,
            _mockTokenInfoService.Object);

        // Act
        var isDeleted = await repository.DeleteAsync(1, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceFrance>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.True(isDeleted);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_IfItIsNotDeleted()
    {
        // Arrange
        var repository = new BalanceFranceRepository(
            _context,
            _mockRoleRepository.Object,
            _mockUserRepository.Object,
            _mockTokenInfoService.Object);

        // Act
        var isDeleted = await repository.DeleteAsync(4, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceFrance>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.False(isDeleted);
    }
}