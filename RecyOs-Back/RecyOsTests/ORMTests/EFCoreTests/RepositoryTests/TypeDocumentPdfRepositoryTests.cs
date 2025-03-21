using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests;

[Collection("TypeDocumentPdfTests")]
public class TypeDocumentPdfRepositoryTests
{
    private readonly DataContext _context;
    
    public TypeDocumentPdfRepositoryTests(IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
    }
    
    /*********** Geters ***********/
    [Fact]
    public async Task GetListAsync_ShouldReturnTypeDocumentPdfList()
    {
        // Arrange
        var repository = new TypeDocumentPdfRepository(_context);

        // Act
        var expectedTypeList = await repository.GetListAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<TypeDocumentPdf>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(expectedTypeList);
        Assert.Equal(3, expectedTypeList.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTypeDocumentPdf()
    {
        // Arrange
        var repository = new TypeDocumentPdfRepository(_context);

        // Act
        var expectedType = await repository.GetByIdAsync(1, new ContextSession());
        _context.Entry(expectedType).State = EntityState.Detached;

        //Assert
        Assert.NotNull(expectedType);
        Assert.Equal(1, expectedType.Id);
        Assert.Equal("TypeLabel1", expectedType.Label);
    
    }

    [Fact]
    public async Task GetByLabelAsync_ShouldReturnTypeDocumentPdf()
    {
        // Arrange
        var repository = new TypeDocumentPdfRepository(_context);

        // Act
        var expectedType = await repository.GetByLabelAsync("TypeLabel1", new ContextSession());
        _context.Entry(expectedType).State = EntityState.Detached;

        //Assert
        Assert.NotNull(expectedType);
        Assert.Equal("TypeLabel1", expectedType.Label);
        Assert.Equal(1, expectedType.Id);
    }

    /*********** Create ***********/
    [Fact]
    public async Task CreateTypeAsync_ShouldAddTypeDocumentPdf()
    {
        // Arrange
        var repository = new TypeDocumentPdfRepository(_context);
        var newTypeDocument = new TypeDocumentPdf
        {
            Label = "NewTestLabel"
        };
        var typeDocumentPdfList = await repository.GetListAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<TypeDocumentPdf>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Act
        var expectedTypeDocumentPdf = await repository.CreateTypeAsync(newTypeDocument);
        var updatedTypeDocumentPdfList = await repository.GetListAsync(new ContextSession());
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(expectedTypeDocumentPdf);
        Assert.Equal(updatedTypeDocumentPdfList.Count, typeDocumentPdfList.Count + 1);
        Assert.Equal(newTypeDocument.Label, expectedTypeDocumentPdf.Label);
    }
    
    /*********** Update ***********/
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedTypeDocumentPdf()
    {
        // Arrange
        var repository = new TypeDocumentPdfRepository(_context);
        var newTypeDocumentPdf = new TypeDocumentPdf
        {
            Id = 3,
            Label = "UpdatedLabel"
        };
        
        // Act
        await repository.UpdateAsync(newTypeDocumentPdf, new ContextSession());
        var updatedTypeDocumentPdf = await repository.GetByIdAsync(3, new ContextSession());
        _context.Entry(updatedTypeDocumentPdf).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(updatedTypeDocumentPdf);
        Assert.Equal(newTypeDocumentPdf.Id, updatedTypeDocumentPdf.Id);
        Assert.Equal("UpdatedLabel", updatedTypeDocumentPdf.Label);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_IfNewTypeIsNull()
    {
        // Arrange
        var repository = new TypeDocumentPdfRepository(_context);
        var newTypeDocumentPdf = new TypeDocumentPdf
        {
            Id = 300,
            Label = "UpdatedLabel"
        };
        
        // Act
        var updatedTypeDocumentPdf = await repository.UpdateAsync(newTypeDocumentPdf, new ContextSession());
        
        // Assert
        Assert.Null(updatedTypeDocumentPdf);
    }

    /*********** Delete ***********/
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrueIfSuccessful()
    {
        // Arrange
        var repository = new TypeDocumentPdfRepository(_context);

        // Act
        var expectedResult = await repository.DeleteAsync(2, new ContextSession());

        // Assert
        Assert.True(expectedResult);
    }
}