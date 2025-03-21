// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => CommandExportSoumissionNDCoverRepositoryTests.cs
// Created : 2024/01/23 - 13:51
// Updated : 2024/01/23 - 13:51

using Microsoft.EntityFrameworkCore;
using RecyOs.Commands;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests;

[Collection("RepositoryTests")]
public class CommandExportSoumissionNDCoverRepositoryTests
{
    private readonly DataContext _context;
    public CommandExportSoumissionNDCoverRepositoryTests(IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
    }
    
    /*********** Geters ***********/
    [Fact]
    public async Task ExportSoumissionNDCoverFranceRepositoryAsync_ShouldReturnEntrepriseBaseList_IfItExists()
    {
        // Arrange
        var repository = new CommandExportSoumissionNDCoverRepository(_context);

        // Act
        var entrepriseList = await repository.ExportSoumissionNDCoverFranceRepositoryAsync(new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<EntrepriseBase>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }

        // Assert
        Assert.NotNull(entrepriseList);
        Assert.Equal(3, entrepriseList.Count);
    }

    [Fact]
    public async Task PurgeTableAsync_ShouldRemoveAllTemporaryNnCoverExports_IfItExists()
    {
        // Arrange
        var repository = new CommandExportSoumissionNDCoverRepository(_context);
        // Add data
        _context.TemporaryNdCoverExports.Add(new TemporaryNdCoverExport()
        {
            FileRow = 1,
            Siren = "123456789",

        });
        await _context.SaveChangesAsync();
        // Make sure data were added
        var temporaryNdCoverExports = await _context.TemporaryNdCoverExports.ToListAsync();
        Assert.NotEmpty(temporaryNdCoverExports);

        // Act
        await repository.PurgeTableAsync();
        temporaryNdCoverExports = await _context.TemporaryNdCoverExports.ToListAsync();

        // Assert
        Assert.Empty(temporaryNdCoverExports);
    }

    /*********** Setters ***********/
    [Fact]
    public async Task CreateTemporaryNdCoverBatchExport_ShouldAddTemporaryNdCovertExportList_IfItDoesNotExist()
    {
        // Arrange
        var repository = new CommandExportSoumissionNDCoverRepository(_context);
        var temporaryNdCoverExport = new TemporaryNdCoverExport()
        {
            FileRow = 1,
            Siren = "123456789",
        };

        // Act
        await repository.CreateTemporaryNdCoverExportBatchAsync(new List<TemporaryNdCoverExport> { temporaryNdCoverExport });
        var temporaryNdCoverExports = await _context.TemporaryNdCoverExports.ToListAsync();

        // Assert
        Assert.NotEmpty(temporaryNdCoverExports);
        Assert.Single(temporaryNdCoverExports);
        Assert.Equal(1, temporaryNdCoverExports[0].FileRow);
        Assert.Equal("123456789", temporaryNdCoverExports[0].Siren);
    }
}