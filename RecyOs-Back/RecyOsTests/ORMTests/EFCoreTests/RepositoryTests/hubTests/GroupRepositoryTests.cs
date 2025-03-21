// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => GroupRepositoryTests.cs
// Created : 2024/03/19 - 15:30
// Updated : 2024/03/19 - 15:30

using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOsTests.Interfaces;
using RecyOs.ORM.Filters;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests;

[Collection("RepositoryTests")]
public class GroupRepositoryTests
{
    private readonly DataContext _context;

    public GroupRepositoryTests(IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
    }

    /*********** Getters ***********/
    [Fact]
    public async Task GetAllAsync_ShouldReturnGroupList_IfItExists()
    {
        // Arrange
        var repository = new GroupRepository(_context);

        // Act
        var groups = await repository.GetListAsync(false);

        // Assert
        Assert.NotNull(groups);
        Assert.NotEmpty(groups);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnGroup_IfItExists()
    {
        // Arrange
        var repository = new GroupRepository(_context);

        // Act
        var expectedGroup = await repository.GetByIdAsync(2, new ContextSession());

        // Assert
        Assert.NotNull(expectedGroup);
        Assert.Equal(2, expectedGroup.Id);
        Assert.Equal("Group2", expectedGroup.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_IfItNotExists()
    {
        // Arrange
        var repository = new GroupRepository(_context);

        // Act
        var expectedGroup = await repository.GetByIdAsync(99, new ContextSession());

        // Assert
        Assert.Null(expectedGroup);
    }

    /*********** Create ***********/
    [Fact]
    public async Task CreateAsync_ShouldReturnGroup_WhenCreated()
    {
        // Arrange
        var repository = new GroupRepository(_context);
        var newGroup = new Group
        {
            Name = "Test Group 4",
        };

        // Act
        var createdGroup = await repository.CreateAsync(newGroup, new ContextSession());

        // Assert
        Assert.NotNull(createdGroup);
        Assert.Equal(4, createdGroup.Id);
        Assert.Equal(newGroup.Name, createdGroup.Name);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenNameIsNull()
    {
        // Arrange
        var repository = new GroupRepository(_context);
        var newGroup = new Group
        {
            Name = null,
        };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => repository.CreateAsync(newGroup, new ContextSession()));
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnGroup_WhenExists()
    {
        // Arrange
        var repository = new GroupRepository(_context);
        var name = "Group1";

        // Act
        var group = await repository.GetByNameAsync(name, new ContextSession());

        // Assert
        Assert.NotNull(group);
        Assert.Equal(name, group.Name);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var repository = new GroupRepository(_context);
        var name = "NonExistentGroup";

        // Act
        var group = await repository.GetByNameAsync(name, new ContextSession());

        // Assert
        Assert.Null(group);
    }
    
    /*********** Update ***********/
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedGroup_WhenExists()
    {
        // Arrange
        var repository = new GroupRepository(_context);
        var group = new Group
        {
            Id = 1,
            Name = "Updated Group",
        };

        // Act
        var updatedGroup = await repository.UpdateAsync(group, new ContextSession());

        // Assert
        Assert.NotNull(updatedGroup);
        Assert.Equal(group.Name, updatedGroup.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNullReferenceException_WhenEntityIsNull()
    {
        // Arrange
        var repository = new GroupRepository(_context);
        var group = new Group
        {
            Id = 99,
        };

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => repository.UpdateAsync(group, new ContextSession()));
    }

    /*********** Delete ***********/
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenDeleted()
    {
        // Arrange
        var repository = new GroupRepository(_context);
        const int groupId = 1;

        // Act
        var result = await repository.DeleteAsync(groupId, new ContextSession());

        // Assert
        Assert.True(result);
        var deletedGroup = await _context.Groups.FindAsync(groupId);
        Assert.True(deletedGroup?.IsDeleted);
    }

    [Fact]
    public async Task GetFilteredListWithClientsAsync_ShouldReturnFilteredGroups()
    {
        // Arrange
        var repository = new GroupRepository(_context);
        var filter = new GroupFilter
        {
            PageNumber = 1,
            PageSize = 10,
            FilteredByNom = "Group"
        };

        // Act
        var (groups, totalCount) = await repository.GetFilteredListWithClientsAsync(filter, new ContextSession());

        // Assert
        Assert.NotNull(groups);
        Assert.True(totalCount > 0);
        Assert.All(groups, group => Assert.Contains("Group", group.Name));
    }

    [Fact]
    public async Task GetFilteredListWithClientsAsync_WithNonExistentName_ShouldReturnEmptyList()
    {
        // Arrange
        var repository = new GroupRepository(_context);
        var filter = new GroupFilter
        {
            PageNumber = 1,
            PageSize = 10,
            FilteredByNom = "NonExistentGroup"
        };

        // Act
        var (groups, totalCount) = await repository.GetFilteredListWithClientsAsync(filter, new ContextSession());

        // Assert
        Assert.Empty(groups);
        Assert.Equal(0, totalCount);
    }

    [Fact]
    public async Task GetFilteredListWithClientsAsync_ShouldIncludeRelatedClients()
    {
        // Arrange
        var repository = new GroupRepository(_context);
        var filter = new GroupFilter
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var (groups, _) = await repository.GetFilteredListWithClientsAsync(filter, new ContextSession());

        // Assert
        Assert.All(groups, group =>
        {
            Assert.NotNull(group.ClientEuropes);
            Assert.NotNull(group.EtablissementClients);
            Assert.All(group.ClientEuropes, client => Assert.False(client.IsDeleted));
            Assert.All(group.EtablissementClients, client => Assert.False(client.IsDeleted));
        });
    }

    [Fact]
    public async Task GetFilteredListWithClientsAsync_ShouldRespectPagination()
    {
        // Arrange
        var repository = new GroupRepository(_context);
        var filter = new GroupFilter
        {
            PageNumber = 1,
            PageSize = 1
        };

        // Act
        var (groups, totalCount) = await repository.GetFilteredListWithClientsAsync(filter, new ContextSession());

        // Assert
        Assert.Single(groups);
        Assert.True(totalCount >= 1);
    }
} 