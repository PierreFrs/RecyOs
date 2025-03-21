// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => DocumentPdfRepositoryTests.cs
// Created : 2023/12/26 - 09:23
// Updated : 2023/12/26 - 09:23

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOsTests.Interfaces;
using RecyOsTests.TestFixtures;
using RecyOsTests.TestsHelpers;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests;

[Collection("RepositoryTests")]
public class DocumentPdfRepositoryTests
{
    private readonly DocumentPdfTestsHelpers _documentPdfTestsHelpers;
    private readonly DataContext _context;
    private readonly Mock<ITokenInfoService> _tokenInfoService;
    public DocumentPdfRepositoryTests(
        DocumentPdfServiceFixture fixture, 
        IDataContextTests dataContextTests)
    {
        _documentPdfTestsHelpers = fixture.DocumentPdfTestsHelpers;
        _context = dataContextTests.GetContext();
        _tokenInfoService = new Mock<ITokenInfoService>();
    }
    
    /*********** Geters ***********/
    [Fact]
    public async Task GetListAsync_ShouldReturnDocumentPdfList()
    {
        // Arrange
        var repository = new DocumentPdfRepository(_context, _tokenInfoService.Object);
        
        // Act
        var documentPdfList = await repository.GetListAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<DocumentPdf>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(documentPdfList);
        Assert.Equal(3, documentPdfList.Count);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnDocumentPdf()
    {
        // Arrange
        var repository = new DocumentPdfRepository(_context, _tokenInfoService.Object);
        
        // Act
        var expectedDocumentPdf = await repository.GetByIdAsync(1, new ContextSession());
        _context.Entry(expectedDocumentPdf).State = EntityState.Detached;

        //Assert
        Assert.NotNull(expectedDocumentPdf);
        Assert.Equal(1, expectedDocumentPdf.Id);
        Assert.Equal("pdf1.pdf", expectedDocumentPdf.FileName);
        Assert.Equal(1024, expectedDocumentPdf.FileSize);
        Assert.Equal(1, expectedDocumentPdf.EtablissementClientId);
        Assert.Equal(1, expectedDocumentPdf.TypeDocumentPdfId);
    }
    
    [Fact]
    public async Task GetByClientIdAsync_ShouldReturnDocumentPdfListBelongingToClientId()
   {
       // Arrange
       var repository = new DocumentPdfRepository(_context, _tokenInfoService.Object);
       
       // Act
       var expectedDocumentPdfList = await repository.GetByClientIdAsync(1, new ContextSession());
       var trackedEntities = _context.ChangeTracker.Entries<DocumentPdf>();
       foreach (var entity in trackedEntities)
       {
           entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
       }

       // Assert
       Assert.NotNull(expectedDocumentPdfList);
       Assert.Single(expectedDocumentPdfList);
       foreach (var doc in expectedDocumentPdfList)
       {
           Assert.Equal(1, doc.EtablissementClientId);
       }
   }

    /*********** Create ***********/
    [Fact]
    public async Task CreateAsync_ShouldAddADocumentPdf()
   {
       // Arrange
       var repository = new DocumentPdfRepository(_context, _tokenInfoService.Object);

       var newDocumentPdf = new DocumentPdf
       {
           FileName = "newPdf.pdf",
           FileSize = 4096,
           FileLocation = _documentPdfTestsHelpers.CreateTestFilePath(Directory.GetCurrentDirectory(), "newPdf.pdf"),
           TypeDocumentPdfId = 2,
           EtablissementClientId = 2,
       };

       var documentPdfList = await repository.GetListAsync(new ContextSession());
        
       // Act
       var uploadedDocumentPdf = await repository.CreateAsync(newDocumentPdf);
       var updatedDocumentPdfList = await repository.GetListAsync(new ContextSession());
       var trackedEntities = _context.ChangeTracker.Entries<DocumentPdf>();
       foreach (var entity in trackedEntities)
       {
           entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
       }
        
       // Assert
       Assert.NotNull(uploadedDocumentPdf);
       Assert.Equal(newDocumentPdf.FileName, uploadedDocumentPdf.FileName);
       Assert.Equal(newDocumentPdf.FileSize, uploadedDocumentPdf.FileSize);
       Assert.Equal(newDocumentPdf.FileLocation, uploadedDocumentPdf.FileLocation);
       Assert.Equal(newDocumentPdf.TypeDocumentPdfId, uploadedDocumentPdf.TypeDocumentPdfId);
       Assert.Equal(newDocumentPdf.EtablissementClientId, uploadedDocumentPdf.EtablissementClientId);
       Assert.Equal(updatedDocumentPdfList.Count, documentPdfList.Count + 1);
   }
    
    /*********** Update ***********/
    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedDocumentPdf()
    {
        // Arrange
        var repository = new DocumentPdfRepository(_context, _tokenInfoService.Object);
        var newDocumentPdf = new DocumentPdf()
        {
            Id = 2,
            FileSize = 2048,
            FileName = "newFileName.pdf",
            TypeDocumentPdfId = 2,
            EtablissementClientId = 2
        };

        // Act
        await repository.UpdateAsync(newDocumentPdf, new ContextSession());
        var updatedDocumentPdf = await repository.GetByIdAsync(2, new ContextSession());
        _context.Entry(updatedDocumentPdf).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(updatedDocumentPdf);
        Assert.Equal(newDocumentPdf.Id, updatedDocumentPdf.Id);
        Assert.Equal(newDocumentPdf.FileName, updatedDocumentPdf.FileName);
        Assert.Equal(newDocumentPdf.FileLocation, updatedDocumentPdf.FileLocation);
        Assert.Equal(newDocumentPdf.FileSize, updatedDocumentPdf.FileSize);
        Assert.Equal(newDocumentPdf.TypeDocumentPdfId, updatedDocumentPdf.TypeDocumentPdfId);
        Assert.Equal(newDocumentPdf.EtablissementClientId, updatedDocumentPdf.EtablissementClientId);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_IfPdfNotFound()
    {
        // Arrange
        var repository = new DocumentPdfRepository(_context, _tokenInfoService.Object);
        var newDocumentPdf = new DocumentPdf()
        {
            Id = 6,
            FileSize = 2048,
            FileName = "newFileName.pdf",
            TypeDocumentPdfId = 2,
            EtablissementClientId = 2
        };

        // Act
        var updatedDocumentPdf = await repository.UpdateAsync(newDocumentPdf, new ContextSession());
        
        // Assert
        Assert.Null(updatedDocumentPdf);
    }

    /*********** Delete ***********/
    [Fact]
    public async Task DeleteAsync_ShouldReturnTrueIfSuccessful()
    {
        // Arrange
        var repository = new DocumentPdfRepository(_context, _tokenInfoService.Object);

        // Act
        var deletedDocumentPdf = await repository.DeleteAsync(3, new ContextSession());

        // Assert
        Assert.True(deletedDocumentPdf);
        var checkDeleted = await _context.DocumentPdfs.FindAsync(3);
        Assert.True(checkDeleted?.IsDeleted);
    }
}