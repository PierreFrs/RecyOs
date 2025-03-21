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
public class EntrepriseCouvertureRepositoryTests
{
        private readonly DataContext _context;
    
        public EntrepriseCouvertureRepositoryTests(IDataContextTests dataContextTests)
        {
            _context = dataContextTests.GetContext();
        }

    /*********** Geters ***********/
    [Fact]
    public async Task Get_ShouldReturnEntrepriseCouvertureById()
    {
        // Arrange
        var repository = new EntrepriseCouvertureRepository(_context);

        // Act
        var expectedEntrepriseCouverture = await repository.Get(1, new ContextSession());
        _context.Entry(expectedEntrepriseCouverture).State = EntityState.Detached;

        //Assert
        Assert.NotNull(expectedEntrepriseCouverture);
        Assert.Equal(1, expectedEntrepriseCouverture.Id);
        Assert.Equal("384598009", expectedEntrepriseCouverture.Siren);
    }
    
    [Fact]
    public async Task GetFilteredListWithCount_ShouldReturnAFilteredListOfEntrepriseCouvertures()
    {
        // Arrange
            var repository = new EntrepriseCouvertureRepository(_context);
            var filter = new EntrepriseCouvertureGridFilter
            {
                PageSize = 2,
                PageNumber = 1,
            };
            
            // Act
            var entrepriseCouvertureList = await repository.GetFilteredListWithCount(filter, new ContextSession());
            var trackedEntities = _context.ChangeTracker.Entries<EntrepriseCouverture>();
            foreach (var entity in trackedEntities)
            {
                entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
            }
            var entities = entrepriseCouvertureList.Item1;
            var count = entrepriseCouvertureList.Item2;
            
            // Assert
            Assert.NotNull(entities);
            Assert.Equal(3, count);
        }

    [Fact]
    public async Task GetBySiren_ShouldReturnEntrepriseCouvertureBySiren()
    {
        // Arrange
        var repository = new EntrepriseCouvertureRepository(_context);

        // Act
        var expectedEntrepriseCouverture = await repository.GetBySiren("384598009", new ContextSession());
        _context.Entry(expectedEntrepriseCouverture).State = EntityState.Detached;

        //Assert
        Assert.NotNull(expectedEntrepriseCouverture);
        Assert.Equal(1, expectedEntrepriseCouverture.Id);
        Assert.Equal("384598009", expectedEntrepriseCouverture.Siren);
    }
    
    [Fact]
    public async Task Exists_ShouldCheckIfEntrepriseCouvertureExistsInTheDb()
    {
        // Arrange
        var repository = new EntrepriseCouvertureRepository(_context);
        var newEntrepriseCouverture = new EntrepriseCouverture
        {
            Id = 2,
            CoverId = "1602733049A5782BD2A6RU33QRXQH7U2",
            NumeroContratPrimaire = "0029558201",
            NumeroContratExtension = null,
            TypeGarantie = "Primary",
            EhId = "100429196",
            RaisonSociale = "BC BATIMENT",
            ReferenceClient = null,
            Notation = "06",
            DateAttributionNotation = DateTime.Parse("2023-11-27", CultureInfo.InvariantCulture),
            TypeReponse = "Garantie partielle",
            DateDecision = DateTime.Parse("2023-11-13", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-11-13", CultureInfo.InvariantCulture),
            MontantGarantie = 0,
            DeviseGarantie = "EUR",
            Decision = "OG de réduction",
            MotifDecision = "Notre décision a été prise en tenant compte du manque de visibilité que nous avons sur le groupe auquel appartient cette société.\n.",
            NotreCommentaire = "Nous avons tenté de prendre contact avec cette société afin d'obtenir des comptes récents, mais sans succès. Nous vous saurions gré de bien vouloir nous transmettre toute information pertinente à votre disposition. Les derniers comptes de cette société à notre disposition indiquent une trésorerie insuffisante.\n.",
            CommentaireArbitre = null,
            QuotiteGarantie = null,
            DelaiPaiementSpecifique = null,
            DateEffetDiffere = null,
            DateExpirationGarantie = null,
            MontantTemporaire = null,
            DeviseMontantTemporaire = null,
            DateExpirationMontantTemporaire = null,
            QuotiteGarantieMontantTemporaire = null,
            DelaiPaiementMontantTemporaire = null,
            MontantDemande = (decimal)40000.00,
            DeviseDemandee = "EUR",
            ConditionsPaiementDemandees = null,
            DateExpirationDemandee = null,
            MontantTemporaireDemande = null,
            NumeroDemande = "I148259244",
            IdDemande = 483,
            HeureReponse = "14:38:44",
            TempsReponse = "16:08:41",
            Demandeur = null,
            DateDemandeMontantTemporaire = null,
            TypeIdentifiant = "SIREN",
            Siren = "537379117",
            StatutEntreprise = "Actif",
            NomRue = "RUE DU BOIS BATIMENT RELAIS",
            CodePostal = 59182,
            Ville = "MONTIGNY-EN-OSTREVENT",
            EtatRegionPays = "59",
            Pays = "FR",
            ConditionsSpecifiques = null,
            AutresConditions1 = null,
            AutresConditions2 = null,
            AutresConditions3 = null,
            AutresConditions4 = null,
            AutresConditionsTemporaires = null,
            DateExtraction = DateTime.Parse("2023-11-30", CultureInfo.InvariantCulture),
            RepriseGarantiePossible = "Oui",
            DateRepriseGarantiePossible = DateTime.Parse("2023-11-27", CultureInfo.InvariantCulture),
            CoverGroupRole = null,
            CoverGroupId = null,
        };
        
        // Act
        var result = await repository.Exists(newEntrepriseCouverture, new ContextSession());

        //Assert
        Assert.True(result);
    }

    /*********** Create ***********/
    [Fact]
    public async Task Update_ShouldCreateNewEntrepriseCouverture_IfSirenIsNotRecognised()
    {
        // Arrange
        var repository = new EntrepriseCouvertureRepository(_context);
        var newEntrepriseCouverture = new EntrepriseCouverture
        {
            CoverId = "1602733049A5782BD2A6RU33QRXQH7U2",
            NumeroContratPrimaire = "0029558201",
            NumeroContratExtension = null,
            TypeGarantie = "Primary",
            EhId = "100429196",
            RaisonSociale = "BC BATIMENT",
            ReferenceClient = null,
            Notation = "06",
            DateAttributionNotation = DateTime.Parse("2023-11-27", CultureInfo.InvariantCulture),
            TypeReponse = "Garantie partielle",
            DateDecision = DateTime.Parse("2023-11-13", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-11-13", CultureInfo.InvariantCulture),
            MontantGarantie = 0,
            DeviseGarantie = "EUR",
            Decision = "OG de réduction",
            MotifDecision = "Notre décision a été prise en tenant compte du manque de visibilité que nous avons sur le groupe auquel appartient cette société.\n.",
            NotreCommentaire = "Nous avons tenté de prendre contact avec cette société afin d'obtenir des comptes récents, mais sans succès. Nous vous saurions gré de bien vouloir nous transmettre toute information pertinente à votre disposition. Les derniers comptes de cette société à notre disposition indiquent une trésorerie insuffisante.\n.",
            CommentaireArbitre = null,
            QuotiteGarantie = null,
            DelaiPaiementSpecifique = null,
            DateEffetDiffere = null,
            DateExpirationGarantie = null,
            MontantTemporaire = null,
            DeviseMontantTemporaire = null,
            DateExpirationMontantTemporaire = null,
            QuotiteGarantieMontantTemporaire = null,
            DelaiPaiementMontantTemporaire = null,
            MontantDemande = (decimal)40000.00,
            DeviseDemandee = "EUR",
            ConditionsPaiementDemandees = null,
            DateExpirationDemandee = null,
            MontantTemporaireDemande = null,
            NumeroDemande = "I148259244",
            IdDemande = 483,
            HeureReponse = "14:38:44",
            TempsReponse = "16:08:41",
            Demandeur = null,
            DateDemandeMontantTemporaire = null,
            TypeIdentifiant = "SIREN",
            Siren = "123456789",
            StatutEntreprise = "Actif",
            NomRue = "RUE DU BOIS BATIMENT RELAIS",
            CodePostal = 59182,
            Ville = "MONTIGNY-EN-OSTREVENT",
            EtatRegionPays = "59",
            Pays = "FR",
            ConditionsSpecifiques = null,
            AutresConditions1 = null,
            AutresConditions2 = null,
            AutresConditions3 = null,
            AutresConditions4 = null,
            AutresConditionsTemporaires = null,
            DateExtraction = DateTime.Parse("2023-11-30", CultureInfo.InvariantCulture),
            RepriseGarantiePossible = "Oui",
            DateRepriseGarantiePossible = DateTime.Parse("2023-11-27", CultureInfo.InvariantCulture),
            CoverGroupRole = null,
            CoverGroupId = null,
        };
        
        var initialList = await repository.GetList(new ContextSession());
        var initialCount = initialList.Count();

        // Act
        var expectedEntrepriseCouverture = await repository.Update(newEntrepriseCouverture, new ContextSession());
        var updatedList = await repository.GetList(new ContextSession());
        var updatedCount = updatedList.Count();
        
        // Assert
        Assert.NotNull(expectedEntrepriseCouverture);
        Assert.Equal(4, expectedEntrepriseCouverture.Id);
        Assert.Equal(updatedCount, initialCount + 1);
    }
    
    /*********** Update ***********/
    [Fact]
    public async Task Update_ShouldReturnUpdatedEntrepriseCouverture()
    {
        // Arrange
        var repository = new EntrepriseCouvertureRepository(_context);
        var newEntrepriseCouverture = new EntrepriseCouverture
        {
            Id = 3,
            CoverId = "1667480007A5782BD25OVW52HQDRA59I\n",
            NumeroContratPrimaire = "0029558201",
            NumeroContratExtension = null,
            TypeGarantie = "Primary",
            EhId = "130811779",
            RaisonSociale = "KREABAT TRAVAUX PUBLICS\n",
            ReferenceClient = null,
            Notation = "06",
            DateAttributionNotation = DateTime.Parse("2023-05-04", CultureInfo.InvariantCulture),
            TypeReponse = "Garantie partielle",
            DateDecision = DateTime.Parse("2023-11-06", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-11-06", CultureInfo.InvariantCulture),
            MontantGarantie = 0,
            DeviseGarantie = "EUR",
            Decision = "Refus",
            MotifDecision = "Cette décision repose sur une évaluation de la situation globale de cette société..",
            NotreCommentaire = "Nous ne bénéficions pas d'informations financières sur cette société, nous ne sommes donc pas en mesure de vous accorder le montant demandé.",
            CommentaireArbitre = null,
            QuotiteGarantie = null,
            DelaiPaiementSpecifique = null,
            DateEffetDiffere = null,
            DateExpirationGarantie = null,
            MontantTemporaire = null,
            DeviseMontantTemporaire = null,
            DateExpirationMontantTemporaire = null,
            QuotiteGarantieMontantTemporaire = null,
            DelaiPaiementMontantTemporaire = null,
            MontantDemande = (decimal)20000.00,
            DeviseDemandee = "EUR",
            ConditionsPaiementDemandees = null,
            DateExpirationDemandee = null,
            MontantTemporaireDemande = null,
            NumeroDemande = "I228663335",
            IdDemande = 482,
            HeureReponse = "13:40:15",
            TempsReponse = "11:23:10",
            Demandeur = null,
            DateDemandeMontantTemporaire = null,
            TypeIdentifiant = "SIREN",
            Siren = "902081322",
            StatutEntreprise = "Actif",
            NomRue = "RTE D'ESTAIRES N4 ZONE PTE.",
            CodePostal = 59480,
            Ville = "LA BASSEE",
            EtatRegionPays = "59",
            Pays = "FR",
            ConditionsSpecifiques = null,
            AutresConditions1 = null,
            AutresConditions2 = null,
            AutresConditions3 = null,
            AutresConditions4 = null,
            AutresConditionsTemporaires = null,
            DateExtraction = DateTime.Parse("2023-11-30", CultureInfo.InvariantCulture),
            RepriseGarantiePossible = "Non",
            DateRepriseGarantiePossible = null,
            CoverGroupRole = null,
            CoverGroupId = null,
        };
        
        foreach (var entity in _context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;  // Detache toutes les entités pour éviter les erreurs de tracking lors de la prochaine requête
        }
        // Act
        await repository.Update(newEntrepriseCouverture, new ContextSession());
        var updatedEntrepriseCouverture = await repository.Get(3, new ContextSession());
        _context.Entry(updatedEntrepriseCouverture).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(updatedEntrepriseCouverture);
        Assert.Equal(3, updatedEntrepriseCouverture.Id);
        Assert.Equal("902081322", updatedEntrepriseCouverture.Siren);
        Assert.Equal("06", updatedEntrepriseCouverture.Notation);
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateCouverture_IfSirenIsNullOrEmpty()
    {
        // Arrange
        var repository = new EntrepriseCouvertureRepository(_context);
        
        var noSirenEntrepriseCouverture = new EntrepriseCouverture
        {
            Id = 3,
            CoverId = "1667480007A5782BD25OVW52HQDRA59I",
            NumeroContratPrimaire = "0029558201",
            NumeroContratExtension = null,
            TypeGarantie = "Primary",
            EhId = "130811779",
            RaisonSociale = "KREABAT TRAVAUX PUBLICS",
            ReferenceClient = null,
            Notation = "09",
            DateAttributionNotation = DateTime.Parse("2023-05-04", CultureInfo.InvariantCulture),
            TypeReponse = "Garantie partielle",
            DateDecision = DateTime.Parse("2023-11-06", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-11-06", CultureInfo.InvariantCulture),
            MontantGarantie = 0,
            DeviseGarantie = "EUR",
            Decision = "Refus",
            MotifDecision = "Cette décision repose sur une évaluation de la situation globale de cette société..",
            NotreCommentaire = "Nous ne bénéficions pas d'informations financières sur cette société, nous ne sommes donc pas en mesure de vous accorder le montant demandé.",
            CommentaireArbitre = null,
            QuotiteGarantie = null,
            DelaiPaiementSpecifique = null,
            DateEffetDiffere = null,
            DateExpirationGarantie = null,
            MontantTemporaire = null,
            DeviseMontantTemporaire = null,
            DateExpirationMontantTemporaire = null,
            QuotiteGarantieMontantTemporaire = null,
            DelaiPaiementMontantTemporaire = null,
            MontantDemande = (decimal)20000.00,
            DeviseDemandee = "EUR",
            ConditionsPaiementDemandees = null,
            DateExpirationDemandee = null,
            MontantTemporaireDemande = null,
            NumeroDemande = "I228663335",
            IdDemande = 482,
            HeureReponse = "13:40:15",
            TempsReponse = "11:23:10",
            Demandeur = null,
            DateDemandeMontantTemporaire = null,
            TypeIdentifiant = "SIREN",
            Siren = string.Empty,
            StatutEntreprise = "Actif",
            NomRue = "RTE D'ESTAIRES N4 ZONE PTE.",
            CodePostal = 59480,
            Ville = "LA BASSEE",
            EtatRegionPays = "59",
            Pays = "FR",
            ConditionsSpecifiques = null,
            AutresConditions1 = null,
            AutresConditions2 = null,
            AutresConditions3 = null,
            AutresConditions4 = null,
            AutresConditionsTemporaires = null,
            DateExtraction = DateTime.Parse("2023-11-30", CultureInfo.InvariantCulture),
            RepriseGarantiePossible = "Non",
            DateRepriseGarantiePossible = null,
            CoverGroupRole = null,
            CoverGroupId = null,
        };

        foreach (var entity in _context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;  // Detache toutes les entités pour éviter les erreurs de tracking lors de la prochaine requête
        }
        // Act
        await repository.Update(noSirenEntrepriseCouverture, new ContextSession());
        var expectedEntrepriseCouverture = await repository.Get(3, new ContextSession());
        _context.Entry(expectedEntrepriseCouverture).State = EntityState.Detached;

        // Assert
        Assert.NotNull(expectedEntrepriseCouverture);
        Assert.Equal("902081322", expectedEntrepriseCouverture.Siren);
    }
    
    [Fact]
    public async Task Update_UpdatesTrackedEntity()
    {
        // Arrange
        var repository = new EntrepriseCouvertureRepository(_context);
        var originalEntrepriseCouverture = await repository.Get(1, new ContextSession()); // Fetch the entity to be tracked
        _context.Entry(originalEntrepriseCouverture).State = EntityState.Detached;
        
        // Modify the entity
        originalEntrepriseCouverture.Notation = "08";

        foreach (var entity in _context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;  // Detache toutes les entités pour éviter les erreurs de tracking lors de la prochaine requête
        }
        // Act
        await repository.Update(originalEntrepriseCouverture, new ContextSession());

        // Assert
        var updatedEntrepriseCouverture = await repository.Get(1, new ContextSession());
        _context.Entry(updatedEntrepriseCouverture).State = EntityState.Detached;
        Assert.Equal("08", updatedEntrepriseCouverture.Notation);
    }
}