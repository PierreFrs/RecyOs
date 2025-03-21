// <copyright file="FactorFileExportRepositoryTests.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOsTests.Interfaces;
using Xunit;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests.FactorClientBuRepositoryTests;

[Collection("RepositoryTests")]
public class FactorFileExportRepositoryTests
{
    private readonly IDataContextTests _dataContextTests;

    public FactorFileExportRepositoryTests(IDataContextTests dataContextTests)
    {
        _dataContextTests = dataContextTests;
    }

    [Fact]
    public async Task ExportFactorFileRepositoryAsync_ReturnsCorrectData()
    {
        // Arrange
        await using var context = _dataContextTests.GetContext();
        var repository = new FactorFileExportRepository(context);

        var buId = 3;
        var franceClient = new FactorClientFranceBu
        {
            IdBu = buId,
            IdClient = 4,
            ExportDate = null,
            IsDeleted = false,
            EtablissementClient = new EtablissementClient
            {
                Id = 4,
                Nom = "Client1",
                Siret = "SIRET1",
                Siren = "SIREN1",
                AdresseFacturation1 = "Address1",
                CodePostalFacturation = "12345",
                VilleFacturation = "City1",
                PaysFacturation = "France",
                Client = true,
                Fournisseur = false
            }
        };
        var europeClient = new FactorClientEuropeBu
        {
            IdBu = buId,
            IdClient = 5,
            ExportDate = null,
            IsDeleted = false,
            ClientEurope = new ClientEurope
            {
                Id = 5,
                Nom = "Client2",
                AdresseFacturation1 = "Address2",
                CodePostalFacturation = "54321",
                VilleFacturation = "City2",
                PaysFacturation = "Germany",
                Vat = "VAT12345678",
                Client = true,
                Fournisseur = false
            }
        };

        context.FactorClientFranceBus.Add(franceClient);
        context.FactorClientEuropeBus.Add(europeClient);
        await context.SaveChangesAsync();

        // Detach all tracked entities to avoid conflicts
        DetachAllEntities(context);

        // Act
        var result = await repository.ExportFactorFileRepositoryAsync(buId);

        // Assert
        Assert.NotNull(result);
        var factorClientDtos = result.ToList();
        Assert.Equal(2, factorClientDtos.Count);

        var franceClientDto = factorClientDtos.Find(c => c.ClientId == franceClient.EtablissementClient.Id);
        Assert.NotNull(franceClientDto);
        Assert.Equal("Client1", franceClientDto.Nom);
        Assert.Equal("SIRET1", franceClientDto.Siret);

        var europeClientDto = factorClientDtos.Find(c => c.ClientId == europeClient.ClientEurope.Id);
        Assert.NotNull(europeClientDto);
        Assert.Equal("Client2", europeClientDto.Nom);
        Assert.Equal("Germany", europeClientDto.Adpays);
    }

    [Fact]
    public async Task UpdateExportDateForFranceClientsAsync_UpdatesExportDate()
    {
        // Arrange
        await using var context = _dataContextTests.GetContext();
        var repository = new FactorFileExportRepository(context);

        var buId = 3;
        var clientId = 6;
        var franceClient = new FactorClientFranceBu
        {
            IdClient = clientId,
            IdBu = buId,
            ExportDate = null,
            EtablissementClient = new EtablissementClient
            {
                Id = clientId,
                Siret = "SIRET1",
                Siren = "SIREN1",
                Nom = "Client1",
                AdresseFacturation1 = "Address1",
                CodePostalFacturation = "12345",
                VilleFacturation = "City1",
                PaysFacturation = "France",
                Client = true,
                Fournisseur = false
            }
        };

        context.FactorClientFranceBus.Add(franceClient);
        await context.SaveChangesAsync();

        // Detach all tracked entities to avoid conflicts
        DetachAllEntities(context);

        var clientIds = new List<int> { clientId };

        // Act
        await repository.UpdateExportDateForFranceClientsAsync(clientIds, buId);
        var updatedClient = await context.FactorClientFranceBus.FindAsync(clientId, buId);

        // Assert
        Assert.NotNull(updatedClient);
        Assert.NotNull(updatedClient.ExportDate);
        Assert.Equal(DateTime.UtcNow.Date, updatedClient.ExportDate.Value.Date);
    }

    [Fact]
    public async Task UpdateExportDateForEuropeClientsAsync_UpdatesExportDate()
    {
        // Arrange
        await using var context = _dataContextTests.GetContext();
        var repository = new FactorFileExportRepository(context);

        var buId = 4;
        var clientId = 7;
        var europeClient = new FactorClientEuropeBu
        {
            IdClient = clientId,
            IdBu = buId,
            ExportDate = null,
            ClientEurope = new ClientEurope
            {
                Id = clientId,
                Nom = "Client2",
                AdresseFacturation1 = "Address2",
                CodePostalFacturation = "54321",
                VilleFacturation = "City2",
                PaysFacturation = "Germany",
                Vat = "VAT12345678",
                Client = true,
                Fournisseur = false
            }
        };

        context.FactorClientEuropeBus.Add(europeClient);
        await context.SaveChangesAsync();

        // Detach all tracked entities to avoid conflicts
        DetachAllEntities(context);

        var clientIds = new List<int> { clientId };

        // Act
        await repository.UpdateExportDateForEuropeClientsAsync(clientIds, buId);
        var updatedClient = await context.FactorClientEuropeBus.FindAsync(clientId, buId);

        // Assert
        Assert.NotNull(updatedClient);
        Assert.NotNull(updatedClient.ExportDate);
        Assert.Equal(DateTime.UtcNow.Date, updatedClient.ExportDate.Value.Date);
    }

    private void DetachAllEntities(DbContext context)
    {
        var entries = context.ChangeTracker.Entries().ToList();
        foreach (var entry in entries)
        {
            entry.State = EntityState.Detached;
        }
    }

}
