// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => DocumentPdfEuropeRepositoryTests.cs
// Created : 2023/12/26 - 13:50
// Updated : 2023/12/26 - 13:50

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOsTests.Interfaces;
using RecyOsTests.TestFixtures;
using RecyOsTests.TestsHelpers;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests;

[Collection("RepositoryTests")]
public class DocumentPdfEuropeRepositoryTests
{
    private readonly DocumentPdfTestsHelpers _documentPdfTestsHelpers;
    private readonly DataContext _context;
    public DocumentPdfEuropeRepositoryTests(DocumentPdfServiceFixture fixture, IDataContextTests dataContextTests)
    {
        _documentPdfTestsHelpers = fixture.DocumentPdfTestsHelpers;
        _context = dataContextTests.GetContext();
    }

    /*********** Geters ***********/
    [Fact]
    public async Task GetListAsync_ShouldReturnDocumentPdfList()
    {
        // Arrange
        var repository = new DocumentPdfEuropeRepository(_context);
        
        // Act
        var documentPdfList = await repository.GetListAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<DocumentPdfEurope>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(documentPdfList);
        Assert.Equal(3, documentPdfList.Count);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnDocumentPdfEurope()
    {
        // Arrange
        var repository = new DocumentPdfEuropeRepository(_context);
        
        // Act
        var expectedDocumentPdfEurope = await repository.GetByIdAsync(1, new ContextSession());
        _context.Entry(expectedDocumentPdfEurope).State = EntityState.Detached;

        //Assert
        Assert.NotNull(expectedDocumentPdfEurope);
        Assert.Equal(1, expectedDocumentPdfEurope.Id);
        Assert.Equal("pdf1.pdf", expectedDocumentPdfEurope.FileName);
        Assert.Equal(1024, expectedDocumentPdfEurope.FileSize);
        Assert.Equal(1, expectedDocumentPdfEurope.EtablissementClientEuropeId);
        Assert.Equal(1, expectedDocumentPdfEurope.TypeDocumentPdfId);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_ShouldReturnDocumentPdfEuropeListBelongingToClientId()
   {
       // Arrange
       var repository = new DocumentPdfEuropeRepository(_context);
       
       // Act
       var expectedDocumentPdfEuropeList = await repository.GetByClientIdAsync(1, new ContextSession());
       var trackedEntities = _context.ChangeTracker.Entries<DocumentPdfEurope>();
       foreach (var entity in trackedEntities)
       {
           entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
       }

       // Assert
       Assert.NotNull(expectedDocumentPdfEuropeList);
       Assert.Single(expectedDocumentPdfEuropeList);
       foreach (var doc in expectedDocumentPdfEuropeList)
       {
           Assert.Equal(1, doc.EtablissementClientEuropeId);
       }
   }

    /*********** Create ***********/
    [Fact]
    public async Task CreateAsync_ShouldAddADocumentPdfEurope()
   {
       // Arrange
       var repository = new DocumentPdfEuropeRepository(_context);

       var newDocumentPdfEurope = new DocumentPdfEurope
       {
           FileName = "newPdf.pdf",
           FileSize = 4096,
           FileLocation = _documentPdfTestsHelpers.CreateTestFilePath(Directory.GetCurrentDirectory(), "newPdf.pdf"),
           TypeDocumentPdfId = 2,
           EtablissementClientEuropeId = 2,
       };

       var documentPdfList = await repository.GetListAsync(new ContextSession());
        
       // Act
       var uploadedDocumentPdfEurope = await repository.CreateAsync(newDocumentPdfEurope);
       var updatedDocumentPdfEuropeList = await repository.GetListAsync(new ContextSession());
       var trackedEntities = _context.ChangeTracker.Entries<DocumentPdfEurope>();
       foreach (var entity in trackedEntities)
       {
           entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
       }
        
       // Assert
       Assert.NotNull(uploadedDocumentPdfEurope);
       Assert.Equal(newDocumentPdfEurope.FileName, uploadedDocumentPdfEurope.FileName);
       Assert.Equal(newDocumentPdfEurope.FileSize, uploadedDocumentPdfEurope.FileSize);
       Assert.Equal(newDocumentPdfEurope.FileLocation, uploadedDocumentPdfEurope.FileLocation);
       Assert.Equal(newDocumentPdfEurope.TypeDocumentPdfId, uploadedDocumentPdfEurope.TypeDocumentPdfId);
       Assert.Equal(newDocumentPdfEurope.EtablissementClientEuropeId, uploadedDocumentPdfEurope.EtablissementClientEuropeId);
       Assert.Equal(updatedDocumentPdfEuropeList.Count, documentPdfList.Count + 1);
   }
    
    /*********** Update ***********/
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedDocumentPdfEurope()
    {
        // Arrange
        var repository = new DocumentPdfEuropeRepository(_context);
        var newDocumentPdfEurope = new DocumentPdfEurope()
        {
            Id = 2,
            FileSize = 2048,
            FileName = "newFileName.pdf",
            TypeDocumentPdfId = 2,
            EtablissementClientEuropeId = 2
        };

        // Act
        await repository.UpdateAsync(newDocumentPdfEurope, new ContextSession());
        var updatedDocumentPdfEurope = await repository.GetByIdAsync(2, new ContextSession());
        _context.Entry(updatedDocumentPdfEurope).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(updatedDocumentPdfEurope);
        Assert.Equal(newDocumentPdfEurope.Id, updatedDocumentPdfEurope.Id);
        Assert.Equal(newDocumentPdfEurope.FileName, updatedDocumentPdfEurope.FileName);
        Assert.Equal(newDocumentPdfEurope.FileLocation, updatedDocumentPdfEurope.FileLocation);
        Assert.Equal(newDocumentPdfEurope.FileSize, updatedDocumentPdfEurope.FileSize);
        Assert.Equal(newDocumentPdfEurope.TypeDocumentPdfId, updatedDocumentPdfEurope.TypeDocumentPdfId);
        Assert.Equal(newDocumentPdfEurope.EtablissementClientEuropeId, updatedDocumentPdfEurope.EtablissementClientEuropeId);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_IfPdfNotFound()
    {
        // Arrange
        var repository = new DocumentPdfEuropeRepository(_context);
        var newDocumentPdfEurope = new DocumentPdfEurope()
        {
            Id = 6,
            FileSize = 2048,
            FileName = "newFileName.pdf",
            TypeDocumentPdfId = 2,
            EtablissementClientEuropeId = 2
        };

        // Act
        var updatedDocumentPdfEurope = await repository.UpdateAsync(newDocumentPdfEurope, new ContextSession());
        
        // Assert
        Assert.Null(updatedDocumentPdfEurope);
    }

    /*********** Delete ***********/
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrueIfSuccessful()
    {
        // Arrange
        var repository = new DocumentPdfEuropeRepository(_context);

        // Act
        var deletedDocumentPdfEurope = await repository.DeleteAsync(3, new ContextSession());

        // Assert
        Assert.True(deletedDocumentPdfEurope);
        var checkDeleted = await _context.DocumentPdfEuropes.FindAsync(3);
        Assert.True(checkDeleted?.IsDeleted);
    }
}