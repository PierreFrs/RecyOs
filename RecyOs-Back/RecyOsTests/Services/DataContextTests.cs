using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Engine.Alerts.Entities;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOsTests.Interfaces;
using RecyOsTests.TestFixtures;
using RecyOsTests.TestsHelpers;
using TaskStatus = RecyOs.Engine.Alerts.Entities.TaskStatus;

namespace RecyOsTests.Services;

public class DataContextTests : IDataContextTests
{
    private readonly DocumentPdfTestsHelpers _documentPdfTestsHelpers;
    public DataContextTests(DocumentPdfServiceFixture fixture)
    {
        _documentPdfTestsHelpers = fixture.DocumentPdfTestsHelpers;
    }

    public DataContext GetContext()
    {
        var configuration = MockConfiguration();
        var env = MockWebHostEnvironment();
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
  //          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)  // EXPERIMENTAL: Disable tracking for all queries
            .Options;
        
        var context = new DataContext(options, configuration, env);
        initDatas(context);
        return context;
    }
    
    private static IConfiguration MockConfiguration()
    {
        var configuration = new Mock<IConfiguration>();
        return configuration.Object;
    }
    
    private static IWebHostEnvironment MockWebHostEnvironment()
    {
        var env = new Mock<IWebHostEnvironment>();
        env.Setup(e => e.EnvironmentName).Returns("Test");
        return env.Object;
    }
    
    private void initDatas(DataContext prmContext)
    {
        var parameter = new Parameter
        {
            Id = 1,
            Nom = "Param1",
            Valeur = "Value1",
            Module = "Test",
            CreatedBy = "Test engineer",
            CreateDate = DateTime.Now,
            UpdatedAt = null,
            UpdatedBy = null,
        };
        prmContext.Parameters.Add(parameter);
        parameter = new Parameter
        {
            Id = 2,
            Nom = "Param2",
            Valeur = "Value2",
            Module = "Test",
            CreatedBy = "Test engineer",
            CreateDate = DateTime.Now,
            UpdatedAt = null,
            UpdatedBy = null,
        };
        prmContext.Parameters.Add(parameter);
        parameter = new Parameter
        {
            Id = 3,
            Nom = "Param3",
            Valeur = "Value3",
            Module = "Test",
            CreatedBy = "Test engineer",
            CreateDate = DateTime.Now,
            UpdatedAt = null,
            UpdatedBy = null,
        };
        prmContext.Parameters.Add(parameter);
        // Clients

        var user = new User
        {
            Id = 1,
            FirstName = "User1",
            LastName = "User1",
            Email = "User1@gamil.com",
            Password = "User1",
            SocieteId = 1,
            UserName = "User1",
        };
        prmContext.Users.Add(user);

        user = new User
        {
            Id = 2,
            FirstName = "User2",
            LastName = "User2",
            Email = "User2@gamil.com",
            Password = "User2",
            SocieteId = 2,
            UserName = "User2",
        };
        prmContext.Users.Add(user);

        user = new User
        {
            Id = 3,
            FirstName = "User3",
            LastName = "User3",
            Email = "User3@gamil.com",
            Password = "User3",
            SocieteId = 3,
            UserName = "User3",
        };
        prmContext.Users.Add(user);
        
        var entrepriseBase = new EntrepriseBase
        {
            Id = 1,
            Siren = "056800659",
            SirenFormate = "056 800 659",
            NomEntreprise = "SOCIETE GENERALE",
            PersonneMorale = true,
            Denomination = "SOCIETE GENERALE",
            Nom = null,
            Prenom = null,
            Sexe = null,
            CodeNaf = "64.19Z",
            LibelleCodeNaf = "Autres intermédiations monétaires",
            DomaineActivite = "Banques, assurances, services financiers",
            DateCreation = new DateTime(1958, 1, 1, 12, 12, 12, DateTimeKind.Utc),
            DateCreationFormate = "01/01/1958",
            CreatedBy = "Test engineer",
        };
        prmContext.EntrepriseBase.Add(entrepriseBase);
        entrepriseBase = new EntrepriseBase
        {
            Id = 2,
            Siren = "200017598",
            SirenFormate = "200 017 598",
            NomEntreprise = "IDEN SIAN NOREADE ASSAINISSEMEN",
            PersonneMorale = true,
            Denomination = "IDEN SIAN NOREADE ASSAINISSEMEN",
            Nom = null,
            Prenom = null,
            Sexe = null,
            CodeNaf = "37.00Z",
            LibelleCodeNaf = "Collecte et traitement des eaux usées",
            DomaineActivite = "BCollecte et traitement des eaux usées",
            DateCreation = new DateTime(1958, 1, 1, 12, 12, 12, DateTimeKind.Utc),
            DateCreationFormate = "01/01/1958",
            CreatedBy = "Test engineer",
        };
        prmContext.EntrepriseBase.Add(entrepriseBase);
        entrepriseBase = new EntrepriseBase
        {
            Id = 3,
            Siren = "948224746",
            SirenFormate = "948 224 746",
            NomEntreprise = "AZ IMMO",
            PersonneMorale = true,
            Denomination = "AZ IMMO",
            CodeNaf = "68.20B",
            LibelleCodeNaf = "Location de terrains et d'autres biens immobiliers",
            DomaineActivite = "Immobilier",
            DateCreation = new DateTime(1958, 1, 1, 12, 12, 12, DateTimeKind.Utc),
            DateCreationFormate = "01/01/1958",
            CreatedBy = "Test engineer",
        };
        prmContext.EntrepriseBase.Add(entrepriseBase);
            
        
        var etablissementClient = new EtablissementClient
        {
            Id = 1,
            Nom = "Etablissement 1",
            Siret = "05680065900858",
            Siren = "056800659",
            CodeMkgt = "TESCLI0001",
            CodeKerlog = "000001",
            IdOdoo = "ODOO0001",
            IdHubspot = "00001",
            CodeGpi = "CodeGpi",
            FrnCodeGpi = "FrnCodeGpi",
            AdresseFacturation1 = "Rue des testeurs",
            VilleFacturation = "Testville",
            PaysFacturation = "France",
            CodePostalFacturation = "012345",
            EmailFacturation = "rest@test.fr",
            TelephoneFacturation = "+33 1 23 45 67 89",
            IsDeleted = false,
            ConditionReglement = 1,
            ModeReglement = 4,
            DelaiReglement = 0,
            TauxTva = 20,
            EncoursMax = 0,
            CompteComptable = "411103",
            CategorieId = 1, // Default value for N/A category
            CreateDate = DateTime.Now,
            Radie = false,
            CreatedBy = "Test engineer",
            DateCreMKGT = DateTime.Now,
            DateCreOdoo =  DateTime.Now,
            Client = true,
            ClientGroupe = false,
            CommercialId = 1,
        };
        prmContext.EtablissementClient.Add(etablissementClient);

        etablissementClient = new EtablissementClient
        {
            Id = 2,
            Nom = "Etablissement 2",
            Siret = "20001759800058",
            Siren = "200017598",
            AdresseFacturation1 = "Rue des testeurs",
            VilleFacturation = "Testville",
            PaysFacturation = "France",
            CodePostalFacturation = "012345",
            EmailFacturation = "test@icloud.com",
            TelephoneFacturation = "+33 1 23 45 67 89",
            IsDeleted = false,
            ConditionReglement = 1,
            ModeReglement = 4,
            DelaiReglement = 0,
            TauxTva = 20,
            EncoursMax = 0,
            CompteComptable = "411103",
            CategorieId = 1, // Default value for N/A category
            CreateDate = DateTime.Now,
            Radie = false,
            CreatedBy = "Test engineer",
            Client = true,
            ClientGroupe = false
        };
        prmContext.EtablissementClient.Add(etablissementClient);
        
        etablissementClient = new EtablissementClient
        {
            Id = 3,
            Nom = "Etablissement 3",
            Siret = "94822474600016",
            Siren = "948224746",
            AdresseFacturation1 = "Rue des testeurs",
            VilleFacturation = "Testville",
            PaysFacturation = "France",
            CodePostalFacturation = "012345",
            EmailFacturation = "test@testeurs.com",
            TelephoneFacturation = "+33 1 23 45 67 89",
            IsDeleted = false,
            ConditionReglement = 1,
            ModeReglement = 4,
            DelaiReglement = 0,
            TauxTva = 20,
            EncoursMax = 0,
            CompteComptable = "411103",
            CategorieId = 1, // Default value for N/A category
            CreateDate = DateTime.Now,
            Radie = false,
            CreatedBy = "Test engineer",
            Client = true,
            ClientGroupe = false
        };
        prmContext.EtablissementClient.Add(etablissementClient);


        var categorieClient = new CategorieClient
        {
            CategorieLabel = "TestCategorieLabel1",
        };
        prmContext.CategorieClients.Add(categorieClient);
        
        categorieClient = new CategorieClient
        {
            CategorieLabel = "TestCategorieLabel2",
        };
        prmContext.CategorieClients.Add(categorieClient);
        
        
        // Couverture data
        var entrepriseCouverture = new EntrepriseCouverture
        {
            Id = 1,
            CoverId = "1602733048A5782BD28NZRSENNCWO1IN",
            NumeroContratPrimaire = "0029558201",
            NumeroContratExtension = null,
            TypeGarantie = "Primary",
            EhId = "4473544",
            RaisonSociale = "TP PLUS",
            ReferenceClient = null,
            Notation = "07",
            DateAttributionNotation = DateTime.Parse("2023-11-13", CultureInfo.InvariantCulture),
            TypeReponse = "Garantie partielle",
            DateDecision = DateTime.Parse("2023-11-13", CultureInfo.InvariantCulture),
            DateDemande = DateTime.Parse("2023-11-13", CultureInfo.InvariantCulture),
            MontantGarantie = 12000,
            DeviseGarantie = "EUR",
            Decision = "OG de réduction",
            MotifDecision = "Cette décision a été prise après l'analyse de la situation financière des derniers comptes à notre disposition sur cette société.",
            NotreCommentaire = "Notre décision repose sur des informations confidentielles.",
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
            MontantDemande = 0,
            DeviseDemandee = null,
            ConditionsPaiementDemandees = null,
            DateExpirationDemandee = null,
            MontantTemporaireDemande = null,
            NumeroDemande = null,
            IdDemande = 484,
            HeureReponse = null,
            TempsReponse = "15:54:16",
            Demandeur = null,
            DateDemandeMontantTemporaire = null,
            TypeIdentifiant = "SIREN",
            Siren = "384598009",
            StatutEntreprise = "Actif",
            NomRue = "RTE NATIONALE 41 CELLULE 1",
            CodePostal = 59480,
            Ville = "ILLIES",
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
        prmContext.EntrepriseCouverture.Add(entrepriseCouverture);
        
        entrepriseCouverture = new EntrepriseCouverture
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
            MotifDecision = "Notre décision a été prise en tenant compte du manque de visibilité que nous avons sur le groupe auquel appartient cette société.",
            NotreCommentaire = "Nous avons tenté de prendre contact avec cette société afin d'obtenir des comptes récents, mais sans succès. Nous vous saurions gré de bien vouloir nous transmettre toute information pertinente à votre disposition. Les derniers comptes de cette société à notre disposition indiquent une trésorerie insuffisante.",
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
        prmContext.EntrepriseCouverture.Add(entrepriseCouverture);

        entrepriseCouverture = new EntrepriseCouverture
        {
            Id = 3,
            CoverId = "1667480007A5782BD25OVW52HQDRA59I\n",
            NumeroContratPrimaire = "0029558201",
            NumeroContratExtension = null,
            TypeGarantie = "Primary",
            EhId = "130811779",
            RaisonSociale = "KREABAT TRAVAUX PUBLICS\n",
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
        prmContext.EntrepriseCouverture.Add(entrepriseCouverture);

        
        var entrepriseNDCover = new EntrepriseNDCover
        {
            Id = 1,
            CoverId = "1633187268A5782BD2L17D35GY7UZP7Q",
            NumeroContratPrimaire = "0029558201",
            NumeroContratExtension = "0029558201",
            NomPolice = "RECYGROUP",
            EhId = "90481998",
            RaisonSociale = "MANUFOR FONDATIONS",
            ReferenceClient = null,
            FormeJuridiqueCode = "SARL5",
            DateChangementPosition = DateTime.Parse("2023-12-15", CultureInfo.InvariantCulture),
            SecteurActivite = "4120A",
            DateSuppression = null,
            Statut = "Pas de garantie",
            PeriodeRenouvellementOuverte = "Non",
            SeraRenouvele = null,
            DateRenouvellementPrevue = null,
            DateExpiration = null,
            TempsReponse = "Répondue",
            TypeIdentifiant = "SIREN",
            Siren = "504488123",
            StatutEntreprise = "Actif",
            NomRue = "531  D RUE  DU FIEF",
            CodePostal = 62840,
            Ville = "SAILLY SUR LA LYS",
            Pays = "FR",
            DateExtraction = DateTime.Parse("2023/12/19", CultureInfo.InvariantCulture)
        };
        prmContext.EntrepriseNDCovers.Add(entrepriseNDCover);
        
        entrepriseNDCover = new EntrepriseNDCover
        {
            Id = 2,
            CoverId = "1702558514A5782BD2UBRTJU0APH5QEI",
            NumeroContratPrimaire = "0029558201",
            NumeroContratExtension = "0029558201",
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
        prmContext.EntrepriseNDCovers.Add(entrepriseNDCover);
        
        entrepriseNDCover = new EntrepriseNDCover
        {
            Id = 3,
            CoverId = "1702022378A5782BD2147JU3MTW9BYH4",
            NumeroContratPrimaire = "0029558201",
            NumeroContratExtension = "0029558201",
            NomPolice = "RECYGROUP",
            EhId = "134933418",
            RaisonSociale = "RENOV",
            ReferenceClient = null,
            FormeJuridiqueCode = "SAS",
            DateChangementPosition = DateTime.Parse("2023-12-08", CultureInfo.InvariantCulture),
            SecteurActivite = "4399C",
            DateSuppression = null,
            Statut = "Garantie totale",
            PeriodeRenouvellementOuverte = "Non",
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
        prmContext.EntrepriseNDCovers.Add(entrepriseNDCover);
        
        
        // Documents PDF
        var typeDocumentPdf = new TypeDocumentPdf
        {
            Label = "TypeLabel1"
        };
        prmContext.TypeDocumentPdfs.Add(typeDocumentPdf);
        
        typeDocumentPdf = new TypeDocumentPdf
        {
            Label = "TypeLabel2"
        };
        prmContext.TypeDocumentPdfs.Add(typeDocumentPdf);

        typeDocumentPdf = new TypeDocumentPdf
        {
            Label = "TypeLabel3"
        };
        prmContext.TypeDocumentPdfs.Add(typeDocumentPdf);


        var documentPdf = new DocumentPdf
        {
            FileName = "pdf1.pdf",
            FileSize = 1024,
            FileLocation = _documentPdfTestsHelpers.CreateTestFilePath(Directory.GetCurrentDirectory(), "pdf1.pdf"),
            TypeDocumentPdfId = 1,
            EtablissementClientId = 1,
        };
        prmContext.DocumentPdfs.Add(documentPdf);
        
        documentPdf = new DocumentPdf
        {
            FileName = "pdf2.pdf",
            FileSize = 2048,
            FileLocation = _documentPdfTestsHelpers.CreateTestFilePath(Directory.GetCurrentDirectory(), "pdf2.pdf"),
            TypeDocumentPdfId = 2,
            EtablissementClientId = 2,
        };
        prmContext.DocumentPdfs.Add(documentPdf);

        documentPdf = new DocumentPdf
        {
            FileName = "pdf3.pdf",
            FileSize = 2048,
            FileLocation = _documentPdfTestsHelpers.CreateTestFilePath(Directory.GetCurrentDirectory(), "pdf3.pdf"),
            TypeDocumentPdfId = 2,
            EtablissementClientId = 2,
        };
        prmContext.DocumentPdfs.Add(documentPdf);
        
        
        var documentPdfEurope = new DocumentPdfEurope
        {
            FileName = "pdf1.pdf",
            FileSize = 1024,
            FileLocation = _documentPdfTestsHelpers.CreateTestFilePath(Directory.GetCurrentDirectory(), "pdf1.pdf"),
            TypeDocumentPdfId = 1,
            EtablissementClientEuropeId = 1,
        };
        prmContext.DocumentPdfEuropes.Add(documentPdfEurope);
        
        documentPdfEurope = new DocumentPdfEurope
        {
            FileName = "pdf2.pdf",
            FileSize = 2048,
            FileLocation = _documentPdfTestsHelpers.CreateTestFilePath(Directory.GetCurrentDirectory(), "pdf2.pdf"),
            TypeDocumentPdfId = 2,
            EtablissementClientEuropeId = 2,
        };
        prmContext.DocumentPdfEuropes.Add(documentPdfEurope);

        documentPdfEurope = new DocumentPdfEurope
        {
            FileName = "pdf3.pdf",
            FileSize = 2048,
            FileLocation = _documentPdfTestsHelpers.CreateTestFilePath(Directory.GetCurrentDirectory(), "pdf3.pdf"),
            TypeDocumentPdfId = 2,
            EtablissementClientEuropeId = 2,
        };
        prmContext.DocumentPdfEuropes.Add(documentPdfEurope);
        
        
        etablissementClient = new EtablissementClient
        {
            Id = 9,
            Nom = "Etablissement 9",
            Siret = "02526474600016",
            Siren = "025264746",
            CodeMkgt = "TESCLI0009",
            IdOdoo = "ODOO0009",
            CodeGpi = "GPI0001",
            AdresseFacturation1 = "Rue des testeurs",
            VilleFacturation = "Testville",
            PaysFacturation = "France",
            CodePostalFacturation = "012345",
            EmailFacturation = "test@testeurs.com",
            TelephoneFacturation = "+33 1 23 45 67 89",
            IsDeleted = false,
            ConditionReglement = 1,
            ModeReglement = 4,
            DelaiReglement = 0,
            TauxTva = 20,
            EncoursMax = 0,
            CompteComptable = "411103",
            CategorieId = 1, // Default value for N/A category
            CreateDate = DateTime.Now,
            Radie = false,
            CreatedBy = "Test engineer",
            DateCreMKGT = DateTime.Now,
            DateCreOdoo =  DateTime.Now,
            Client = true,
        };
        prmContext.EtablissementClient.Add(etablissementClient);
        
        EtablissementFiche fiche = new EtablissementFiche
        {
            Id = 1,
            Siret = "05680065900858",
            AdresseLigne1 = "Rue des testeurs",
            Ville = "Testville",
            Pays = "France",
            CodePostal = "012345",
            EtablissementCesse = false,
            Siege = true,
            CreateDate = DateTime.Now,
            CreatedBy = "Test engineer",
        };
        prmContext.EtablissementFiche.Add(fiche);
        
        fiche = new EtablissementFiche
        {
            Id = 2,
            Siret = "20001759800058",
            AdresseLigne1 = "Rue des testeurs",
            Ville = "Testville",
            Pays = "France",
            CodePostal = "012345",
            EtablissementCesse = false,
            Siege = true,
            CreateDate = DateTime.Now,
            CreatedBy = "Test engineer",
        };
        prmContext.EtablissementFiche.Add(fiche);
        
        ClientEurope clientEurope = new ClientEurope
        {
            Id = 1,
            Vat = "BE0421064033",
            CodeMkgt = "TESCLIE001",
            CodeKerlog = "900001",
            IdOdoo = "9DOO0001",
            Nom = "NV Transport Mervielde",
            AdresseFacturation1 = "Rue des testeurs",
            VilleFacturation = "Testville",
            PaysFacturation = "France",
            CodePostalFacturation = "012345",
            CreateDate = DateTime.Now,
            CreatedBy = "Test engineer",
            EmailFacturation = "test@testeurs.com",
            TelephoneFacturation = "+33 1 23 45 67 89",
            IsDeleted = false,
            ConditionReglement = 1,
            ModeReglement = 4,
            DelaiReglement = 0,
            TauxTva = 0,
            EncoursMax = 0,
            CompteComptable = "411107",
            CategorieId = 1, // Default value for N/A category
            Radie = false,
            Client = true,
            CommercialId = 1,
        };
        prmContext.ClientEurope.Add(clientEurope);
        
        clientEurope = new ClientEurope
        {
            Id = 2,
            Vat = "BE0407113453",
            CodeMkgt = "TESCLIE002",
            CodeKerlog = "900002",
            IdOdoo = "9DOO0002",
            Nom = "NV Danis",
            AdresseFacturation1 = "Rue des testeurs",
            VilleFacturation = "Testville",
            PaysFacturation = "France",
            CodePostalFacturation = "012345",
            CreateDate = DateTime.Now,
            CreatedBy = "Test engineer",
            EmailFacturation = "test@testeurs.com",
            TelephoneFacturation = "+33 1 23 45 67 89",
            IsDeleted = false,
            ConditionReglement = 1,
            ModeReglement = 4,
            DelaiReglement = 0,
            TauxTva = 0,
            EncoursMax = 0,
            CompteComptable = "411107",
            CategorieId = 1, // Default value for N/A category
            Radie = false,
            Client = true,
            DateCreMkgt = DateTime.Now,
            DateCreOdoo = DateTime.Now,
        };
        prmContext.ClientEurope.Add(clientEurope);
        clientEurope = new ClientEurope
        {
            Id = 3,
            Vat = "BE0400462916",
            CodeMkgt = "TESCLIE003",
            CodeKerlog = "900003",
            IdOdoo = "9DOO0003",
            Nom = "SA Carrières Unies de Porphyre",
            AdresseFacturation1 = "Rue des testeurs",
            VilleFacturation = "Testville",
            PaysFacturation = "France",
            CodePostalFacturation = "012345",
            CreateDate = DateTime.Now,
            CreatedBy = "Test engineer",
            EmailFacturation = "test@testeurs.com",
            TelephoneFacturation = "+33 1 23 45 67 89",
            IsDeleted = false,
            ConditionReglement = 1,
            ModeReglement = 4,
            DelaiReglement = 0,
            TauxTva = 0,
            EncoursMax = 0,
            CompteComptable = "411107",
            CategorieId = 1, // Default value for N/A category
            Radie = false,
            DateCreMkgt = DateTime.Now,
            DateCreOdoo = DateTime.Now,
            Client = true,
        };
        prmContext.ClientEurope.Add(clientEurope);
        clientEurope = new ClientEurope
        {
            Id = 4,
            Vat = "BE0474455308",
            CodeMkgt = "TESCLIE004",
            CodeKerlog = "900004",
            IdOdoo = "-1",
            Nom = "SA Carrières Unies de Porphyre",
            AdresseFacturation1 = "Rue des testeurs",
            VilleFacturation = "Testville",
            PaysFacturation = "France",
            CodePostalFacturation = "012345",
            CreateDate = DateTime.Now,
            CreatedBy = "Test engineer",
            EmailFacturation = "test@testeurs.com",
            TelephoneFacturation = "+33 1 23 45 67 89",
            IsDeleted = false,
            ConditionReglement = 1,
            ModeReglement = 4,
            DelaiReglement = 0,
            TauxTva = 0,
            EncoursMax = 0,
            CompteComptable = "411107",
            CategorieId = 1, // Default value for N/A category
            Radie = false,
            DateCreMkgt = DateTime.Now,
            DateCreOdoo = DateTime.Now,
            Client = true,
        };
        prmContext.ClientEurope.Add(clientEurope);
        
        
        var businessUnit = new BusinessUnit
        {
            Id = 1,
            Libelle = "BusinessUnit1",
        };
        prmContext.BusinessUnits.Add(businessUnit);
        businessUnit = new BusinessUnit
        {
            Id = 2,
            Libelle = "BusinessUnit2",
        };
        prmContext.BusinessUnits.Add(businessUnit);
        businessUnit = new BusinessUnit
        {
            Id = 3,
            Libelle = "BusinessUnit3",
        };
        prmContext.BusinessUnits.Add(businessUnit);
        
        
        var etablissementClientBusinessUnit = new EtablissementClientBusinessUnit
        {
            ClientId = 1,
            BusinessUnitId = 1,
        };
        prmContext.EtablissementClientBusinessUnits.Add(etablissementClientBusinessUnit);
        etablissementClientBusinessUnit = new EtablissementClientBusinessUnit
        {
            ClientId = 1,
            BusinessUnitId = 2,
        };
        prmContext.EtablissementClientBusinessUnits.Add(etablissementClientBusinessUnit);
        etablissementClientBusinessUnit = new EtablissementClientBusinessUnit
        {
            ClientId = 2,
            BusinessUnitId = 1,
        };
        prmContext.EtablissementClientBusinessUnits.Add(etablissementClientBusinessUnit);
        
        
        var clientEuropeBusinessUnit = new ClientEuropeBusinessUnit
        {
            ClientId = 1,
            BusinessUnitId = 1,
        };
        prmContext.ClientEuropeBusinessUnits.Add(clientEuropeBusinessUnit);
        clientEuropeBusinessUnit = new ClientEuropeBusinessUnit
        {
            ClientId = 1,
            BusinessUnitId = 2,
        };
        prmContext.ClientEuropeBusinessUnits.Add(clientEuropeBusinessUnit);
        clientEuropeBusinessUnit = new ClientEuropeBusinessUnit
        {
            ClientId = 2,
            BusinessUnitId = 1,
        };
        prmContext.ClientEuropeBusinessUnits.Add(clientEuropeBusinessUnit);
        prmContext.Counters.Add(new Counter()
        {
            Id = 1,
            Name = "Client_gpi",
            Value = 260,
            Description = "Compteur de clients gpi"
        });
        prmContext.Counters.Add(new Counter()
        {
            Id = 2,
            Name = "Client_toto",
            Value = 52,
            Description = "Compteur de clients toto"
        });

        var societe = new Societe
        {
            Id = 1,
            Nom = "RecyGroup",
            IdOdoo = "1",
        };
        prmContext.Societes.Add(societe);
        societe = new Societe
        {
            Id = 2,
            Nom = "BP Trans",
            IdOdoo = "2",
        };
        prmContext.Societes.Add(societe);
        societe = new Societe
        {
            Id = 3,
            Nom = "PaulPacquet",
            IdOdoo = "3",
        };
        prmContext.Societes.Add(societe);

        var balanceFrance = new BalanceFrance
        {
            Id = 1,
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = DateTime.Now,
            Montant = 10000
        };
        prmContext.BalanceFrances.Add(balanceFrance);
        balanceFrance = new BalanceFrance
        {
            ClientId = 1,
            SocieteId = 2,
            DateRecuperationBalance = DateTime.Now,
            Montant = 20000
        };
        prmContext.BalanceFrances.Add(balanceFrance);
        balanceFrance = new BalanceFrance
        {
            ClientId = 1,
            SocieteId = 3,
            DateRecuperationBalance = DateTime.Now,
            Montant = 30000
        };
        prmContext.BalanceFrances.Add(balanceFrance);
        
        var balanceEurope = new BalanceEurope
        {
            Id = 1,
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = DateTime.Now,
            Montant = 10000
        };
        prmContext.BalanceEuropes.Add(balanceEurope);
        balanceEurope = new BalanceEurope
        {
            ClientId = 1,
            SocieteId = 2,
            DateRecuperationBalance = DateTime.Now,
            Montant = 20000
        };
        prmContext.BalanceEuropes.Add(balanceEurope);
        balanceEurope = new BalanceEurope
        {
            ClientId = 1,
            SocieteId = 3,
            DateRecuperationBalance = DateTime.Now,
            Montant = 30000
        };
        prmContext.BalanceEuropes.Add(balanceEurope);

        var balanceParticulier = new BalanceParticulier
        {
            Id = 1,
            ClientId = 1,
            SocieteId = 1,
            DateRecuperationBalance = DateTime.Now,
            Montant = 15000
        };
        prmContext.BalanceParticuliers.Add(balanceParticulier);
        
        balanceParticulier = new BalanceParticulier
        {
            ClientId = 1,
            SocieteId = 2,
            DateRecuperationBalance = DateTime.Now,
            Montant = 25000
        };
        prmContext.BalanceParticuliers.Add(balanceParticulier);
        
        balanceParticulier = new BalanceParticulier
        {
            ClientId = 1,
            SocieteId = 3,
            DateRecuperationBalance = DateTime.Now,
            Montant = 35000
        };
        prmContext.BalanceParticuliers.Add(balanceParticulier);

        var commercial = new Commercial
        {
            Id = 1,
            UserId = 1,
            Firstname = "Commercial1",
            Lastname = "Commercial1",
            Username = "Commercial1",
            Phone = "0123456789",
            Email = "Commercial1@gmail.com",
            CodeMkgt = "AA",
        };
        prmContext.Commerciaux.Add(commercial);
        
        commercial = new Commercial
        {
            Id = 2,
            UserId = 2,
            Firstname = "Commercial2",
            Lastname = "Commercial2",
            Username = "Commercial2",
            Phone = "0123456789",
            Email = "Commercial2@gamil.com",
            CodeMkgt = "BB",
        };
        prmContext.Commerciaux.Add(commercial);

        commercial = new Commercial
        {
            Id = 3,
            UserId = 3,
            Firstname = "Commercial3",
            Lastname = "Commercial3",
            Username = "Commercial3",
            Phone = "0123456789",
            Email = "Commercial3@gamil.com",
            CodeMkgt = "CC",
        };
        prmContext.Commerciaux.Add(commercial);
        
        var factorClientFranceBu = new FactorClientFranceBu
        {
            IdClient = 1,
            IdBu = 1,
            CreateDate = DateTime.Now,
            ExportDate = null,
            IsDeleted = false,
        };
        prmContext.FactorClientFranceBus.Add(factorClientFranceBu);
        
        factorClientFranceBu = new FactorClientFranceBu
        {
            IdClient = 1,
            IdBu = 2,
            CreateDate = DateTime.Now,
            ExportDate = null,
            IsDeleted = false,
        };
        prmContext.FactorClientFranceBus.Add(factorClientFranceBu);
        
        factorClientFranceBu = new FactorClientFranceBu
        {
            IdClient = 2,
            IdBu = 1,
            CreateDate = DateTime.Now,
            ExportDate = null,
            IsDeleted = false,
        };
        prmContext.FactorClientFranceBus.Add(factorClientFranceBu);
        
        var factorClientEuropeBu = new FactorClientEuropeBu
        {
            IdClient = 1,
            IdBu = 1,
            CreateDate = DateTime.Now,
            ExportDate = null,
            IsDeleted = false,
        };
        prmContext.FactorClientEuropeBus.Add(factorClientEuropeBu);
        
        factorClientEuropeBu = new FactorClientEuropeBu
        {
            IdClient = 1,
            IdBu = 2,
            CreateDate = DateTime.Now,
            ExportDate = null,
            IsDeleted = false,
        };
        prmContext.FactorClientEuropeBus.Add(factorClientEuropeBu);
        
        factorClientEuropeBu = new FactorClientEuropeBu
        {
            IdClient = 2,
            IdBu = 1,
            CreateDate = DateTime.Now,
            ExportDate = null,
            IsDeleted = false,
        };
        prmContext.FactorClientEuropeBus.Add(factorClientEuropeBu);

        var clientParticulier = new ClientParticulier
        {
            Id = 1,
            Nom = "Octavianus",
            Prenom = "Caius Julius Caesar",
            Titre = "M",
            AdresseFacturation1 = "Piazza Augusto Imperatore, 30",
            AdresseFacturation2 = "",
            AdresseFacturation3 = "",
            CodePostalFacturation = "00186",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "bgdulatium@mail.it",
            TelephoneFacturation = "+33 1 23 45 67 89",
            PortableFacturation = "+33 6 12 34 56 78",
            ContactAlternatif = "Livia Drusilla",
            EmailAlternatif = "liliroma@mail.it",
            TelephoneAlternatif = "+33 1 23 45 67 89",
            PortableAlternatif = "+33 6 12 34 56 78",
            ConditionReglement = 30,
            ModeReglement = 2,
            DelaiReglement = 3,
            TauxTva = 5.5m,
            EncoursMax = 10000,
            CompteComptable = "411103",
            ClientBloque = false,
            MotifBlocage = "",
            DateBlocage = null,
            CodeMkgt = "TESCLI0001",
            DateCreMkgt = new DateTime(38, 12, 23, 0, 0, 0, DateTimeKind.Utc),
            IdOdoo = "ODOO0001",
            DateCreOdoo = new DateTime(38, 12, 24, 0, 0, 0, DateTimeKind.Utc),
        };
        prmContext.ClientParticuliers.Add(clientParticulier);

        clientParticulier = new ClientParticulier
        {
            Id = 2,
            Nom = "Tiberius",
            Prenom = "Julius Caesar Augustus",
            Titre = "M",
            AdresseFacturation1 = "Piazza Augusto Imperatore, 30",
            AdresseFacturation2 = "",
            AdresseFacturation3 = "",
            CodePostalFacturation = "00186",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "titidu186@mail.it",
            TelephoneFacturation = "+33 1 23 45 67 89",
            PortableFacturation = "+33 6 12 34 56 78",
            ContactAlternatif = "Vipsania Agrippina",
            EmailAlternatif = "vipsagri@mail.it",
            TelephoneAlternatif = "+33 1 23 45 67 89",
            PortableAlternatif = "+33 6 12 34 56 78",
            ConditionReglement = 30,
            ModeReglement = 2,
            DelaiReglement = 3,
            TauxTva = 5.5m,
            EncoursMax = 10000,
            CompteComptable = "411103",
            ClientBloque = false,
            MotifBlocage = "",
            DateBlocage = null,
            CodeMkgt = "TESCLI0001",
            DateCreMkgt = new DateTime(38, 12, 23, 0, 0, 0, DateTimeKind.Utc),
            IdOdoo = "ODOO0001",
            DateCreOdoo = new DateTime(38, 12, 24, 0, 0, 0, DateTimeKind.Utc),
        };
        prmContext.ClientParticuliers.Add(clientParticulier);

        clientParticulier = new ClientParticulier
        {
            Id = 3,
            Nom = "Caligula",
            Prenom = "Caius Julius Caesar Augustus Germanicus",
            Titre = "M",
            AdresseFacturation1 = "Piazza Augusto Imperatore, 30",
            AdresseFacturation2 = "",
            AdresseFacturation3 = "",
            CodePostalFacturation = "00186",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            EmailFacturation = "calicali@mail.it",
            TelephoneFacturation = "+33 1 23 45 67 89",
            PortableFacturation = "+33 6 12 34 56 78",
            ContactAlternatif = "Caesonia Milonia",
            EmailAlternatif = "caemilo@mail.it",
            TelephoneAlternatif = "+33 1 23 45 67 89",
            PortableAlternatif = "+33 6 12 34 56 78",
            ConditionReglement = 30,
            ModeReglement = 2,
            DelaiReglement = 3,
            TauxTva = 5.5m,
            EncoursMax = 10000,
            CompteComptable = "411103",
            ClientBloque = false,
            MotifBlocage = "",
            DateBlocage = null,
            CodeMkgt = "TESCLI0001",
            DateCreMkgt = new DateTime(38, 12, 23, 0, 0, 0, DateTimeKind.Utc),
            IdOdoo = "ODOO0001",
            DateCreOdoo = new DateTime(38, 12, 24, 0, 0, 0, DateTimeKind.Utc),
        };
        prmContext.ClientParticuliers.Add(clientParticulier);

        var messageMail = new MessageMail
        {
            Id = 1,
            Subject = "Test message",
            From = "noreply@recygroup.fr",
            To = "test@example.com",
            Status = TaskStatus.Pending,
            Priority = 1,
            DateCreated = DateTime.Now,
            Body = "Test message",
            DateSent = DateTime.Now,
        };
        prmContext.MessageMails.Add(messageMail);

        messageMail = new MessageMail
        {
            Id = 2,
            Subject = "Notification importante",
            From = "noreply@recygroup.fr",
            To = "client@example.com",
            Status = TaskStatus.Completed,
            Priority = 2,
            DateCreated = DateTime.Now,
            Body = "Contenu de la notification importante",
            DateSent = DateTime.Now,
        };
        prmContext.MessageMails.Add(messageMail);

        messageMail = new MessageMail
        {
            Id = 3,
            Subject = "Rappel mensuel",
            From = "noreply@recygroup.fr",
            To = "support@example.com",
            Status = TaskStatus.InProgress,
            Priority = 3,
            DateCreated = DateTime.Now,
            Body = "Contenu du rappel mensuel",
            DateSent = DateTime.Now,
        };
        prmContext.MessageMails.Add(messageMail);
    
        var notification = new Notification
        {
            Id = 1,
            Title = "Test notification",
            Description = "Notification de test",
            Read = false,
            UserId = 1,
            Time = DateTime.Now,
            Icon = "fa-solid fa-bell",
        };
        prmContext.Notifications.Add(notification);

        // Notification avec image
        var imageNotification = new Notification
        {
            Id = 2,
            Title = "Nouvelle photo de profil",
            Description = "Votre photo de profil a été mise à jour avec succès",
            Read = false,
            UserId = 1,
            Time = DateTime.Now.AddHours(-2),
            Icon = "fa-solid fa-image",
            Image = "https://storage.recyos.com/profiles/user1.jpg",
            UseRouter = false
        };
        prmContext.Notifications.Add(imageNotification);

        // Notification avec lien et router
        var routerNotification = new Notification
        {
            Id = 3,
            Title = "Nouveau message",
            Description = "Vous avez reçu un nouveau message de l'administrateur",
            Read = false,
            UserId = 2,
            Time = DateTime.Now.AddMinutes(-30),
            Icon = "fa-solid fa-envelope",
            Link = "/messages/inbox",
            UseRouter = true
        };
        prmContext.Notifications.Add(routerNotification);

        // Notification déjà lue
        var readNotification = new Notification
        {
            Id = 4,
            Title = "Maintenance terminée",
            Description = "La maintenance du système est terminée avec succès",
            Read = true,
            UserId = 1,
            Time = DateTime.Now.AddDays(-1),
            Icon = "fa-solid fa-check-circle",
            UseRouter = false
        };
        prmContext.Notifications.Add(readNotification);

        // Notification avec lien externe
        var externalLinkNotification = new Notification
        {
            Id = 5,
            Title = "Documentation mise à jour",
            Description = "La documentation technique a été mise à jour",
            Read = false,
            UserId = 3,
            Time = DateTime.Now.AddHours(-5),
            Icon = "fa-solid fa-book",
            Link = "https://docs.recyos.com/v2",
            UseRouter = false
        };
        prmContext.Notifications.Add(externalLinkNotification);

        // Notification système
        var systemNotification = new Notification
        {
            Id = 6,
            Title = "Mise à jour système",
            Description = "Une mise à jour importante du système est disponible",
            Read = false,
            UserId = 1,
            Time = DateTime.Now,
            Icon = "fa-solid fa-exclamation-triangle",
            Link = "/system/updates",
            UseRouter = true
        };
        prmContext.Notifications.Add(systemNotification);

        // Groups
        var group = new Group
        {
            Id = 1,
            Name = "Group1",
            CreateDate = DateTime.Now,
            CreatedBy = "Test engineer",
            UpdatedAt = DateTime.Now,
            UpdatedBy = "Test engineer",
        };
        prmContext.Groups.Add(group);

        group = new Group
        {
            Id = 2,
            Name = "Group2",
            CreateDate = DateTime.Now,
            CreatedBy = "Test engineer",
            UpdatedAt = DateTime.Now,
            UpdatedBy = "Test engineer",
        };

        prmContext.Groups.Add(group);

        group = new Group
        {
            Id = 3,
            Name = "Group3",
            CreateDate = DateTime.Now,
            CreatedBy = "Test engineer",
            UpdatedAt = DateTime.Now,
            UpdatedBy = "Test engineer",
        };
        prmContext.Groups.Add(group);

        prmContext.SaveChanges();

        // 🔥 Détacher toutes les entités pour simuler un environnement réel
        foreach (var entry in prmContext.ChangeTracker.Entries().ToList())
        {
            prmContext.Entry(entry.Entity).State = EntityState.Detached;
        }
    }
}