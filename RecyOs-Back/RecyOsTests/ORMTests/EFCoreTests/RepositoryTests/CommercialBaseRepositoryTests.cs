// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => CommercialRepositoryTests.cs
// Created : 2024/03/27 - 09:43
// Updated : 2024/03/27 - 09:43

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests;

[Collection("RepositoryTests")]
public class CommercialBaseRepositoryTests
{
    private readonly DataContext _context;
    public CommercialBaseRepositoryTests(
        IDataContextTests dataContextTests
        )
    {
        _context = dataContextTests.GetContext();
    }
    
    /*********** Getters ***********/
    [Fact]
    public async Task GetListAsync_ShouldReturnCommercialList()
    {
        // Arrange
        var repository = new CommercialBaseRepository(_context);
        
        // Act
        var commercialList = await repository.GetListAsync();
        var trackedEntities = _context.ChangeTracker.Entries<Commercial>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(commercialList);
        Assert.Equal(3, commercialList.Count);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnCommercial()
    {
        // Arrange
        var repository = new CommercialBaseRepository(_context);
        
        // Act
        var expectedCommercial = await repository.GetByIdAsync(1, new ContextSession());
        _context.Entry(expectedCommercial).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(expectedCommercial);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull()
    {
        // Arrange
        var repository = new CommercialBaseRepository(_context);
        
        // Act
        var expectedCommercial = await repository.GetByIdAsync(10, new ContextSession());
        
        // Assert
        Assert.Null(expectedCommercial);
    }
    
    [Fact]
    public async Task GetClientsByCommercialIdAsync_ShouldReturnClients()
    {
        // Arrange
        var repository = new CommercialBaseRepository(_context);
        
        // Act
        var (etablissementClients, _) = await repository.GetClientsByCommercialIdAsyncWithCount(1, new ClientByCommercialFilter());
        
        // Assert
        Assert.NotNull(etablissementClients);
    }
    
    [Fact]
    public async Task GetFilteredListAsync_ShouldReturnCommercialList()
    {
        // Arrange
        var repository = new CommercialBaseRepository(_context);
        var filter = new CommercialFilter
        {
            PageSize = 10,
            PageNumber = 1
        };
        
        // Act
        var (commercialList, totalCount) = await repository.GetFilteredListAsync(filter, new ContextSession());
        
        // Assert
        Assert.NotNull(commercialList);
        Assert.Equal(3, totalCount);
    }
    
    /*********** Create ***********/
    [Fact]
    public async Task CreateAsync_ShouldReturnCommercial()
    {
        // Arrange
        var repository = new CommercialBaseRepository(_context);
        var commercial = new Commercial
        {
            Firstname = "Commercial 4",
            Lastname = "Commercial 4",
            Username = "Commercial 4",
            Phone = "Phone 4",
            Email = "Email 4",
            CodeMkgt = "AA",
            CreatedBy = "Admin",
            CreateDate = DateTime.Now
        };
        
        // Act
        var newCommercial = await repository.CreateAsync(commercial, new ContextSession());
        _context.Entry(newCommercial).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(newCommercial);
        Assert.Equal(4, newCommercial.Id);
        Assert.Equal("Commercial 4", newCommercial.Firstname);
        Assert.Equal("Commercial 4", newCommercial.Lastname);
        Assert.Equal("Commercial 4", newCommercial.Username);
        Assert.Equal("Phone 4", newCommercial.Phone);
        Assert.Equal("Email 4", newCommercial.Email);
        Assert.Equal("AA", newCommercial.CodeMkgt);
        Assert.Equal("Admin", newCommercial.CreatedBy);
    }
    
    /*********** Update ***********/
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedCommercial()
    {
        // Arrange
        var repository = new CommercialBaseRepository(_context);
        var commercial = new Commercial
        {
            Id = 1,
            Firstname = "Commercial 1",
            Lastname = "Commercial 1",
            Phone = "Phone 1",
            Email = "Email 1",
            CodeMkgt = "AA",
            UpdatedBy = "Admin",
            UpdatedAt = DateTime.Now
        };
        
        // Act
        await repository.UpdateAsync(commercial, new ContextSession());
        var updatedCommercial = await repository.GetByIdAsync(1, new ContextSession());
        _context.Entry(updatedCommercial).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(updatedCommercial);
        Assert.Equal(1, updatedCommercial.Id);
        Assert.Equal("Commercial 1", updatedCommercial.Firstname);
        Assert.Equal("Commercial 1", updatedCommercial.Lastname);
        Assert.Equal("Phone 1", updatedCommercial.Phone);
        Assert.Equal("Email 1", updatedCommercial.Email);
        Assert.Equal("AA", updatedCommercial.CodeMkgt);
        Assert.Equal("Admin", updatedCommercial.UpdatedBy);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_IfCommercialNotFound()
    {
        // Arrange
        var repository = new CommercialBaseRepository(_context);
        var commercial = new Commercial
        {
            Id = 10,
            Firstname = "Commercial 10",
            Lastname = "Commercial 10",
            Phone = "Phone 10",
            Email = "Email 10",
            CreatedBy = "Admin",
            CreateDate = DateTime.Now
        };
        
        // Act
        var updatedCommercial = await repository.UpdateAsync(commercial, new ContextSession());
        
        // Assert
        Assert.Null(updatedCommercial);
    }
    
    /*********** Delete ***********/
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue()
    {
        // Arrange
        var repository = new CommercialBaseRepository(_context);
        
        // Act
        var result = await repository.DeleteAsync(1, new ContextSession());
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse()
    {
        // Arrange
        var repository = new CommercialBaseRepository(_context);
        
        // Act
        var result = await repository.DeleteAsync(10, new ContextSession());
        
        // Assert
        Assert.False(result);
    }
}