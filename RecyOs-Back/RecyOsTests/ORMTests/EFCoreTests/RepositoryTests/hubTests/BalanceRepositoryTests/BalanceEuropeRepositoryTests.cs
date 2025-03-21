// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => BalanceEuropeRepositoryTests.cs
// Created : 2024/02/26 - 14:55
// Updated : 2024/02/26 - 14:55

using Microsoft.EntityFrameworkCore;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests;

[Collection("RepositoryTests")]
public class BalanceEuropeRepositoryTests
{
    private readonly DataContext _context;
    private readonly Mock<ITokenInfoService> _mockTokenInfoService;
    private readonly Mock<IRoleRepository<Role>> _mockRoleRepository;
    private readonly Mock<IUserRepository<User>> _mockUserRepository;
    public BalanceEuropeRepositoryTests(
        IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
        _mockTokenInfoService = new Mock<ITokenInfoService>();
        _mockRoleRepository = new Mock<IRoleRepository<Role>>();
        _mockUserRepository = new Mock<IUserRepository<User>>();
    }

    /*********** Geters ***********/
    [Fact]
    public async Task GetAllAsync_ShouldReturnBalanceEuropeList_IfItExists()
    {
        // Arrange
        var repository = new BalanceEuropeRepository(
            _context,
            _mockTokenInfoService.Object,
            _mockRoleRepository.Object,
            _mockUserRepository.Object);

        // Act
        var balanceEuropeList = await repository.GetAllAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceEurope>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(balanceEuropeList);
        Assert.Equal(3, balanceEuropeList.Count);
    }

    [Fact]
    public async Task GetAllAsync_ShouldNotReturnDeletedEntities_IfTheyExist()
    {
        // Arrange
        var repository = new BalanceEuropeRepository(
            _context,
            _mockTokenInfoService.Object,
            _mockRoleRepository.Object,
            _mockUserRepository.Object);
        var deletedBalance = new BalanceEurope
        {
            Id = 4,
            IsDeleted = true
        };

        _context.BalanceEuropes.Add(deletedBalance);

        // Act
        var balanceEuropeList = await repository.GetAllAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceEurope>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(balanceEuropeList);
        Assert.DoesNotContain(balanceEuropeList, x => x.IsDeleted);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBalanceEurope_IfItExists()
    {
        // Arrange
        var repository = new BalanceEuropeRepository(
            _context,
            _mockTokenInfoService.Object,
            _mockRoleRepository.Object,
            _mockUserRepository.Object);

        // Act
        var balanceEurope = await repository.GetByIdAsync(1, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceEurope>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(balanceEurope);
        Assert.Equal(1, balanceEurope.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldNotReturnDeletedEntity_IfItExists()
    {
        // Arrange
        var repository = new BalanceEuropeRepository(
            _context,
            _mockTokenInfoService.Object,
            _mockRoleRepository.Object,
            _mockUserRepository.Object);
        var deletedBalance = new BalanceEurope
        {
            Id = 4,
            IsDeleted = true
        };

        _context.BalanceEuropes.Add(deletedBalance);

        // Act
        var balanceEurope = await repository.GetByIdAsync(4, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceEurope>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.Null(balanceEurope);
    }

    [Fact]
    public async Task GetByClientIdAsync_ShouldReturnBalanceEuropeList_IfItExists()
    {
        // Arrange
        var repository = new BalanceEuropeRepository(
            _context,
            _mockTokenInfoService.Object,
            _mockRoleRepository.Object,
            _mockUserRepository.Object);

        // Act
        var balanceEuropeList = await repository.GetByClientIdAsync(1, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceEurope>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(balanceEuropeList);
        Assert.Equal(3, balanceEuropeList.Count());
    }

    /* Create */
    [Fact]
    public async Task CreateAsync_ShouldReturnBalanceEurope_IfItIsCreated()
    {
        // Arrange
        _mockTokenInfoService.Setup(x => x.GetCurrentUserName())
            .Returns("TestUser");
        var repository = new BalanceEuropeRepository(
            _context,
            _mockTokenInfoService.Object,
            _mockRoleRepository.Object,
            _mockUserRepository.Object);
        var balanceEurope = new BalanceEurope
        {
            Id = 4,
            ClientId = 1,
            SocieteId = 7,
            Montant = 100,
            CreatedBy = "Test",
            UpdatedBy = "Test"
        };

        // Act
        var createdBalanceEurope = await repository.CreateAsync(balanceEurope, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceEurope>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(createdBalanceEurope);
        Assert.Equal(4, createdBalanceEurope.Id);
    }

    /* Update */
    [Fact]
    public async Task UpdateAsync_ShouldReturnBalanceEurope_IfItIsUpdated()
    {
        // Arrange
        var repository = new BalanceEuropeRepository(
            _context,
            _mockTokenInfoService.Object,
            _mockRoleRepository.Object,
            _mockUserRepository.Object);
        var balanceEurope = new BalanceEurope
        {
            Id = 1,
            ClientId = 1,
            SocieteId = 1,
            Montant = 200,
            CreatedBy = "Test",
            UpdatedBy = "Test"
        };

        // Act
        var updatedBalanceEurope = await repository.UpdateAsync(balanceEurope.Id, balanceEurope, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceEurope>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(updatedBalanceEurope);
        Assert.Equal(1, updatedBalanceEurope.Id);
    }

    /* Delete */
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_IfItIsDeleted()
    {
        // Arrange
        var repository = new BalanceEuropeRepository(
            _context,
            _mockTokenInfoService.Object,
            _mockRoleRepository.Object,
            _mockUserRepository.Object);

        // Act
        var isDeleted = await repository.DeleteAsync(1, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceEurope>();
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
        var repository = new BalanceEuropeRepository(
            _context,
            _mockTokenInfoService.Object,
            _mockRoleRepository.Object,
            _mockUserRepository.Object);

        // Act
        var isDeleted = await repository.DeleteAsync(4, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<BalanceEurope>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.False(isDeleted);
    }

    [Fact]
    public async Task GetFilteredListWithCount_ShouldReturnBalanceEuropeListForCommercial_IfUserIsCommercial()
    {
        // Arrange
        var repository = new BalanceEuropeRepository(_context, _mockTokenInfoService.Object, _mockRoleRepository.Object, _mockUserRepository.Object);

        _mockTokenInfoService.Setup(t => t.GetCurrentUserId()).Returns(1);
        _mockRoleRepository.Setup(r => r.GetListByUserId(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new List<Role> { new() { Id = 1, Name = "commercial" } });
        _mockUserRepository.Setup(u => u.Get(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new User { Id = 1, SocieteId = 1 });

        // Act
        var (balances, count, total) = await repository.GetFilteredListWithCount(new BalanceEuropeGridFilter(), false);
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
    public async Task GetFilteredListWithCount_ShouldReturnBalanceEuropeListForBuDirecteur_IfUserIsBuDirecteur()
    {
        // Arrange
        var repository = new BalanceEuropeRepository(_context, _mockTokenInfoService.Object, _mockRoleRepository.Object, _mockUserRepository.Object);

        _mockTokenInfoService.Setup(t => t.GetCurrentUserId()).Returns(4);
        _mockRoleRepository.Setup(r => r.GetListByUserId(4, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new List<Role> { new() { Id = 2, Name = "responsable_bu" } });
        _mockUserRepository.Setup(u => u.Get(4, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new User { Id = 4, SocieteId = 1 });

        // Act
        var (balances, count, total) = await repository.GetFilteredListWithCount(new BalanceEuropeGridFilter(), false);
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
    public async Task GetFilteredListWithCount_ShouldReturnBalanceEuropeListForCompta_IfUserIsCompta()
    {
        // Arrange
        var repository = new BalanceEuropeRepository(_context, _mockTokenInfoService.Object, _mockRoleRepository.Object, _mockUserRepository.Object);

        _mockTokenInfoService.Setup(t => t.GetCurrentUserId()).Returns(1);
        _mockRoleRepository.Setup(r => r.GetListByUserId(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new List<Role> { new() { Id = 3, Name = "compta" } });
        _mockUserRepository.Setup(u => u.Get(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new User { Id = 1, SocieteId = 2 });

        // Act
        var (balances, count, total) = await repository.GetFilteredListWithCount(new BalanceEuropeGridFilter(), false);
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
        var repository = new BalanceEuropeRepository(_context, _mockTokenInfoService.Object, _mockRoleRepository.Object, _mockUserRepository.Object);

        _mockTokenInfoService.Setup(t => t.GetCurrentUserId()).Returns(1);
        _mockRoleRepository.Setup(r => r.GetListByUserId(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new List<Role> { new() { Id = 4, Name = "invalid_role" } });
        _mockUserRepository.Setup(u => u.Get(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(new User { Id = 1, SocieteId = 2 });

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => 
            repository.GetFilteredListWithCount(new BalanceEuropeGridFilter(), false));
    }
}