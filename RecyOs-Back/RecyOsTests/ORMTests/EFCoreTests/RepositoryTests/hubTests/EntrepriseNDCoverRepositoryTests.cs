// Created by : Pierre FRAISSE
// RecyOs => RecyOsTests => EntrepriseNDCoverRepositoryTests.cs
// Created : 2023/12/19 - 16:03
// Updated : 2023/12/19 - 16:03

using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests;

[Collection("RepositoryTests")]
public class EntrepriseNDCoverRepositoryTests
{
    private readonly DataContext _context;
    
    public EntrepriseNDCoverRepositoryTests(IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
    }

    /*********** Geters ***********/
    [Fact]
    public async Task Get_ShouldReturnEntrepriseNDCoverById()
    {
        // Arrange
        var repository = new EntrepriseNDCoverRepository(_context);

        // Act
        var expectedEntrepriseNDCover = await repository.Get(1, new ContextSession());
        _context.Entry(expectedEntrepriseNDCover).State = EntityState.Detached;

        //Assert
        Assert.NotNull(expectedEntrepriseNDCover);
        Assert.Equal(1, expectedEntrepriseNDCover.Id);
        Assert.Equal("504488123", expectedEntrepriseNDCover.Siren);
    }
    
    [Fact]
    public async Task GetFilteredListWithCount_ShouldReturnAFilteredListOfEntrepriseNDCovers()
    {
        // Arrange
        var repository = new EntrepriseNDCoverRepository(_context);
        var filter = new EntrepriseNDCoverGridFilter
        {
            PageSize = 2,
            PageNumber = 1,
        };
            
        // Act
        var entrepriseNDCoverList = await repository.GetFilteredListWithCount(filter, new ContextSession());
        var trackedEntities = _context.ChangeTracker.Entries<EntrepriseNDCover>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        var entities = entrepriseNDCoverList.Item1;
        var count = entrepriseNDCoverList.Item2;

        // Assert
        Assert.NotNull(entities);
        Assert.Equal(3, count);
    }

    [Fact]
    public async Task GetBySiren_ShouldReturnEntrepriseNDCoverBySiren()
    {
        // Arrange
        var repository = new EntrepriseNDCoverRepository(_context);

        // Act
        var expectedEntrepriseNDCover = await repository.GetBySiren("504488123", new ContextSession());
        _context.Entry(expectedEntrepriseNDCover).State = EntityState.Detached;

        //Assert
        Assert.NotNull(expectedEntrepriseNDCover);
        Assert.Equal(1, expectedEntrepriseNDCover.Id);
        Assert.Equal("504488123", expectedEntrepriseNDCover.Siren);
    }
    
    [Fact]
    public async Task Exists_ShouldCheckIfEntrepriseNDCoverExistsInTheDb()
    {
        // Arrange
        var repository = new EntrepriseNDCoverRepository(_context);
        var newEntrepriseNDCover = new EntrepriseNDCover
        {
            Id = 2,
            CoverId = "1702558514A5782BD2UBRTJU0APH5QEI",
            NumeroContratPrimaire = "0029558201",
            NomPolice = "RECYGROUP",
            EhId = "96762223",
            RaisonSociale = "ATMI NOUVELLEMENT CREE",
            ReferenceClient = null,
            FormeJuridiqueCode = "SARL5",
            DateChangementPosition = DateTime.Parse("2023-12-14", CultureInfo.InvariantCulture),
            SecteurActivite = "7112B",
            DateSuppression = null,
            Statut = "Pas de garantie",
            PeriodeRenouvellementOuverte = "Non",
            SeraRenouvele = null,
            DateRenouvellementPrevue = null,
            DateExpiration = null,
            TempsReponse = "Répondue",
            TypeIdentifiant = "SIREN",
            Siren = "524044880",
            StatutEntreprise = "Actif",
            NomRue = "7 RTE DE LAON",
            CodePostal = 02860,
            Ville = "PRESLES-ET-THIERNY",
            Pays = "FR",
            DateExtraction = DateTime.Parse("2023/12/19", CultureInfo.InvariantCulture)
        };

        // Act
        var result = await repository.Exists(newEntrepriseNDCover, new ContextSession());

        //Assert
        Assert.True(result);
    }
    
    /*********** Create ***********/
    [Fact]
    public async Task Update_ShouldCreateNewEntrepriseNDCoverIfSirenNotRecognised()
    {
        // Arrange
        var repository = new EntrepriseNDCoverRepository(_context);
        var newEntrepriseNDCover = new EntrepriseNDCover
        {
            CoverId = "1701937372A5782BD2CUQ7BK71VKMSO5",
            NumeroContratPrimaire = "0029558201",
            NomPolice = "RECYGROUP",
            EhId = "113913502",
            RaisonSociale = "S2O SERVICES",
            ReferenceClient = null,
            FormeJuridiqueCode = "SARL5",
            DateChangementPosition = DateTime.Parse("2023-12-07", CultureInfo.InvariantCulture),
            SecteurActivite = "4322B",
            DateSuppression = null,
            Statut = "Pas de garantie",
            PeriodeRenouvellementOuverte = "Non",
            SeraRenouvele = null,
            DateRenouvellementPrevue = null,
            DateExpiration = null,
            TempsReponse = "Répondue",
            TypeIdentifiant = "SIREN",
            Siren = "820303576",
            StatutEntreprise = "Actif",
            NomRue = "6 ALL JOHANNES VERMEER",
            CodePostal = 59270,
            Ville = "BAILLEUL",
            Pays = "FR",
            DateExtraction = DateTime.Parse("2023/12/19", CultureInfo.InvariantCulture)
        };

        var initialList = await repository.GetList(new ContextSession());
        var initialCount = initialList.Count();

        // Act
        var expectedEntrepriseNDCover = await repository.Update(newEntrepriseNDCover, new ContextSession());
        var updatedList = await repository.GetList(new ContextSession());
        var updatedCount = updatedList.Count();

        // Assert
        Assert.NotNull(expectedEntrepriseNDCover);
        Assert.Equal(4, expectedEntrepriseNDCover.Id);
        Assert.Equal(updatedCount, initialCount + 1);
    }
    
    /*********** Update ***********/
    [Fact]
    public async Task Update_ShouldReturnUpdatedEntrepriseNDCover()
    {
        // Arrange
            var repository = new EntrepriseNDCoverRepository(_context);
            var newEntrepriseNDCover = new EntrepriseNDCover
            {
                Id = 3,
                CoverId = "1702022378A5782BD2147JU3MTW9BYH4",
                NumeroContratPrimaire = "0029558201",
                NomPolice = "RECYGROUP",
                EhId = "134933418",
                RaisonSociale = "RENOV",
                ReferenceClient = null,
                FormeJuridiqueCode = "SAS",
                DateChangementPosition = DateTime.Parse("2023-12-08", CultureInfo.InvariantCulture),
                SecteurActivite = "4399C",
                DateSuppression = null,
                Statut = "Garantie totale",
                PeriodeRenouvellementOuverte = "Oui",
                SeraRenouvele = null,
                DateRenouvellementPrevue = null,
                DateExpiration = null,
                TempsReponse = "Répondue",
                TypeIdentifiant = "SIREN",
                Siren = "912804762",
                StatutEntreprise = "Actif",
                NomRue = "6 ALL JOHANNES VERMEER",
                CodePostal = 59270,
                Ville = "BAILLEUL",
                Pays = "FR",
                DateExtraction = DateTime.Parse("2023/12/19", CultureInfo.InvariantCulture)
            };

            // Act
            await repository.Update(newEntrepriseNDCover, new ContextSession());
            var updatedEntrepriseNDCover = await repository.Get(3, new ContextSession());
            _context.Entry(updatedEntrepriseNDCover).State = EntityState.Detached;

            // Assert
            Assert.NotNull(updatedEntrepriseNDCover);
            Assert.Equal(3, updatedEntrepriseNDCover.Id);
            Assert.Equal("912804762", updatedEntrepriseNDCover.Siren);
            Assert.Equal("Oui", updatedEntrepriseNDCover.PeriodeRenouvellementOuverte);
    }
    
    [Fact]
    public async Task Update_UpdatesTrackedEntity()
    {
        // Arrange
        var repository = new EntrepriseNDCoverRepository(_context);
        var originalEntrepriseNDCover = await repository.Get(1, new ContextSession()); // Fetch the entity to be tracked
        _context.Entry(originalEntrepriseNDCover).State = EntityState.Detached;
        
        // Modify the entity
        originalEntrepriseNDCover.SecteurActivite = "4120B";

        // Act
        await repository.Update(originalEntrepriseNDCover, new ContextSession());

        // Assert
        var updatedEntrepriseNDCover = await repository.Get(1, new ContextSession());
        _context.Entry(updatedEntrepriseNDCover).State = EntityState.Detached;
        Assert.Equal("4120B", updatedEntrepriseNDCover.SecteurActivite);
    }
    
}