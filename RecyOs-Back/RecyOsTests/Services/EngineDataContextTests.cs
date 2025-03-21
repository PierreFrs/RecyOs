using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Engine.Alerts.Entities;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOsTests.Interfaces;
using TaskStatus = RecyOs.Engine.Alerts.Entities.TaskStatus;

namespace RecyOsTests.Services;

/// <summary>
/// A class for testing the EngineDataContext. Not use for Other Tests
/// </summary>
public class EngineDataContextTests : IEngineDataContextTests
{
    private readonly DataContext _context;
    
    public EngineDataContextTests()
    {
        var configuration = MockConfiguration();
        var env = MockWebHostEnvironment();
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new DataContext(options, configuration, env);
        _context.Database.EnsureCreated();
        initDatas();
    }
    
    public DataContext GetContext()
    {
        return _context;
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
    
    /// <summary>
    /// Create somes ClientEurope in the db
    /// </summary>
    private void initDatas()
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
        _context.Parameters.Add(parameter);
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
        _context.Parameters.Add(parameter);
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
        _context.Parameters.Add(parameter);
        var client = new ClientEurope()
        {
            Vat = "BE345678901234",
            Nom = "Client1",
            IdOdoo = "10",
            CodeKerlog = null,
            CodeMkgt = "CLIENTHAUB59",
            CodeGpi = null,
            ContactFacturation = "Contact1",
            EmailFacturation = "contact1@example.com",
            TelephoneFacturation = "1234567890",
            PortableFacturation = "1234567890",
            ContactAlternatif = "Contact2",
            EmailAlternatif = "contact2@example.com",
            TelephoneAlternatif = "1234567890",
            PortableAlternatif = "1234567890",
            AdresseFacturation1 = "Address1",
            AdresseFacturation2 = "Address2",
            AdresseFacturation3 = "Address3",
            CodePostalFacturation = "12345",
            VilleFacturation = "City",
            PaysFacturation = "Country",
            ConditionReglement = 1,
            ModeReglement = 1,
            DelaiReglement = 30,
            TauxTva = 20.0m,
            EncoursMax = 1000,
            CompteComptable = "1234567890123456789012345",
            Iban = "1234567890123456789012345678901234",
            Bic = "12345678901",
            ClientBloque = false,
            MotifBlocage = null,
            DateBlocage = null,
            DateCreMkgt = DateTime.Now,
            DateCreOdoo = null,
            Radie = false,
            CategorieId = 1,
            DocumentPdfEuropes = new List<DocumentPdfEurope>()
            {
                new DocumentPdfEurope()
                {
                    FileSize = 100,
                    FileName = "file1.pdf",
                    FileLocation = "path/to/file1.pdf",
                    TypeDocumentPdfId = 1,
                    EtablissementClientEuropeId = 1
                },
                new DocumentPdfEurope()
                {
                    FileSize = 200,
                    FileName = "file2.pdf",
                    FileLocation = "path/to/file2.pdf",
                    TypeDocumentPdfId = 2,
                    EtablissementClientEuropeId = 1
                }
            }
        };
        _context.ClientEurope.Add(client);
        client = new ClientEurope()
        {
            Vat = "BE956296056483",
            Nom = "Client2",
            IdOdoo = "12456",
            CodeKerlog = null,
            CodeMkgt = null,
            CodeGpi = null,
            ContactFacturation = "Contact1",
            EmailFacturation = "contact1@example.com",
            TelephoneFacturation = "1234567890",
            PortableFacturation = "1234567890",
            ContactAlternatif = "Contact2",
            EmailAlternatif = "contact2@example.com",
            TelephoneAlternatif = "1234567890",
            PortableAlternatif = "1234567890",
            AdresseFacturation1 = "Address1",
            AdresseFacturation2 = "Address2",
            AdresseFacturation3 = "Address3",
            CodePostalFacturation = "12345",
            VilleFacturation = "City",
            PaysFacturation = "Country",
            ConditionReglement = 1,
            ModeReglement = 1,
            DelaiReglement = 30,
            TauxTva = 20.0m,
            EncoursMax = 1000,
            CompteComptable = "1234567890123456789012345",
            Iban = "1234567890123456789012345678901234",
            Bic = "12345678901",
            ClientBloque = false,
            MotifBlocage = null,
            DateBlocage = null,
            DateCreMkgt = null,
            DateCreOdoo =  DateTime.Now - TimeSpan.FromDays(2),
            Radie = false,
            CategorieId = 1,
            DocumentPdfEuropes = new List<DocumentPdfEurope>()
            {
                new DocumentPdfEurope()
                {
                    FileSize = 100,
                    FileName = "file1.pdf",
                    FileLocation = "path/to/file1.pdf",
                    TypeDocumentPdfId = 1,
                    EtablissementClientEuropeId = 1
                },
                new DocumentPdfEurope()
                {
                    FileSize = 200,
                    FileName = "file2.pdf",
                    FileLocation = "path/to/file2.pdf",
                    TypeDocumentPdfId = 2,
                    EtablissementClientEuropeId = 1
                }
            }
        };
        _context.ClientEurope.Add(client);
        client = new ClientEurope()
        {
            Vat = "BE95693675690",
            Nom = "Client3",
            IdOdoo = "10",
            IdHubspot = "1234",
            CodeKerlog = null,
            CodeMkgt = "CLIENT3HAUB59",
            IdShipperDashdoc = 123456789,
            CodeGpi = null,
            ContactFacturation = "Contact1",
            EmailFacturation = "contact1@example.com",
            TelephoneFacturation = "1234567890",
            PortableFacturation = "1234567890",
            ContactAlternatif = "Contact2",
            EmailAlternatif = "contact2@example.com",
            TelephoneAlternatif = "1234567890",
            PortableAlternatif = "1234567890",
            AdresseFacturation1 = "Address1",
            AdresseFacturation2 = "Address2",
            AdresseFacturation3 = "Address3",
            CodePostalFacturation = "12345",
            VilleFacturation = "City",
            PaysFacturation = "Country",
            ConditionReglement = 1,
            ModeReglement = 1,
            DelaiReglement = 30,
            TauxTva = 20.0m,
            EncoursMax = 1000,
            CompteComptable = "1234567890123456789012345",
            Iban = "1234567890123456789012345678901234",
            Bic = "12345678901",
            ClientBloque = false,
            MotifBlocage = null,
            DateBlocage = null,
            DateCreMkgt = DateTime.Now - TimeSpan.FromDays(2),
            UpdatedAt = DateTime.Now - TimeSpan.FromDays(1),
            Radie = false,
            CategorieId = 1,
            DocumentPdfEuropes = new List<DocumentPdfEurope>()
            {
                new DocumentPdfEurope()
                {
                    FileSize = 100,
                    FileName = "file1.pdf",
                    FileLocation = "path/to/file1.pdf",
                    TypeDocumentPdfId = 1,
                    EtablissementClientEuropeId = 1
                },
                new DocumentPdfEurope()
                {
                    FileSize = 200,
                    FileName = "file2.pdf",
                    FileLocation = "path/to/file2.pdf",
                    TypeDocumentPdfId = 2,
                    EtablissementClientEuropeId = 1
                }
            }
        };
        _context.ClientEurope.Add(client);
        client = new ClientEurope()
        {
            Vat = "BE894968495964",
            Nom = "Client4",
            IdOdoo = null,
            IdDashdoc = 1234,
            CodeKerlog = null,
            CodeMkgt = null,
            CodeGpi = null,
            ContactFacturation = "Contact1",
            EmailFacturation = "contact1@example.com",
            TelephoneFacturation = "1234567890",
            PortableFacturation = "1234567890",
            ContactAlternatif = "Contact2",
            EmailAlternatif = "contact2@example.com",
            TelephoneAlternatif = "1234567890",
            PortableAlternatif = "1234567890",
            AdresseFacturation1 = "Address1",
            AdresseFacturation2 = "Address2",
            AdresseFacturation3 = "Address3",
            CodePostalFacturation = "12345",
            VilleFacturation = "City",
            PaysFacturation = "Country",
            ConditionReglement = 1,
            ModeReglement = 1,
            DelaiReglement = 30,
            TauxTva = 20.0m,
            EncoursMax = 1000,
            CompteComptable = "1234567890123456789012345",
            Iban = "1234567890123456789012345678901234",
            Bic = "12345678901",
            ClientBloque = false,
            MotifBlocage = null,
            DateBlocage = null,
            DateCreMkgt = null,
            UpdatedAt = DateTime.Now,
            Radie = false,
            CategorieId = 1,
            DocumentPdfEuropes = new List<DocumentPdfEurope>()
            {
                new DocumentPdfEurope()
                {
                    FileSize = 100,
                    FileName = "file1.pdf",
                    FileLocation = "path/to/file1.pdf",
                    TypeDocumentPdfId = 1,
                    EtablissementClientEuropeId = 1
                },
                new DocumentPdfEurope()
                {
                    FileSize = 200,
                    FileName = "file2.pdf",
                    FileLocation = "path/to/file2.pdf",
                    TypeDocumentPdfId = 2,
                    EtablissementClientEuropeId = 1
                }
            }
        };
        _context.ClientEurope.Add(client);
        client = new ClientEurope()
        {
            Vat = "BE8435506735",
            Nom = "Client5",
            IdOdoo = null,
            CodeKerlog = null,
            CodeMkgt = null,
            CodeGpi = "C0005",
            ContactFacturation = "Contact1",
            EmailFacturation = "contact1@example.com",
            TelephoneFacturation = "1234567890",
            PortableFacturation = "1234567890",
            ContactAlternatif = "Contact2",
            EmailAlternatif = "contact2@example.com",
            TelephoneAlternatif = "1234567890",
            PortableAlternatif = "1234567890",
            AdresseFacturation1 = "Address1",
            AdresseFacturation2 = "Address2",
            AdresseFacturation3 = "Address3",
            CodePostalFacturation = "12345",
            VilleFacturation = "City",
            PaysFacturation = "Country",
            ConditionReglement = 1,
            ModeReglement = 1,
            DelaiReglement = 30,
            TauxTva = 20.0m,
            EncoursMax = 1000,
            CompteComptable = "1234567890123456789012345",
            Iban = "1234567890123456789012345678901234",
            Bic = "12345678901",
            ClientBloque = false,
            MotifBlocage = null,
            DateBlocage = null,
            DateCreMkgt = null,
            DateCreGpi = DateTime.Now,
            UpdatedAt = null,
            Radie = false,
            CategorieId = 1,
            DocumentPdfEuropes = new List<DocumentPdfEurope>()
            {
                new DocumentPdfEurope()
                {
                    FileSize = 100,
                    FileName = "file1.pdf",
                    FileLocation = "path/to/file1.pdf",
                    TypeDocumentPdfId = 1,
                    EtablissementClientEuropeId = 1
                },
                new DocumentPdfEurope()
                {
                    FileSize = 200,
                    FileName = "file2.pdf",
                    FileLocation = "path/to/file2.pdf",
                    TypeDocumentPdfId = 2,
                    EtablissementClientEuropeId = 1
                }
            }
        };
        _context.ClientEurope.Add(client);
        client = new ClientEurope()
        {
            Vat = "BE759654036",
            Nom = "Client6",
            IdOdoo = null,
            CodeKerlog = null,
            CodeMkgt = null,
            CodeGpi = "C0006",
            ContactFacturation = "Contact1",
            EmailFacturation = "contact1@example.com",
            TelephoneFacturation = "1234567890",
            PortableFacturation = "1234567890",
            ContactAlternatif = "Contact2",
            EmailAlternatif = "contact2@example.com",
            TelephoneAlternatif = "1234567890",
            PortableAlternatif = "1234567890",
            AdresseFacturation1 = "Address1",
            AdresseFacturation2 = "Address2",
            AdresseFacturation3 = "Address3",
            CodePostalFacturation = "12345",
            VilleFacturation = "City",
            PaysFacturation = "Country",
            ConditionReglement = 1,
            ModeReglement = 1,
            DelaiReglement = 30,
            TauxTva = 20.0m,
            EncoursMax = 1000,
            CompteComptable = "1234567890123456789012345",
            Iban = "1234567890123456789012345678901234",
            Bic = "12345678901",
            ClientBloque = false,
            MotifBlocage = null,
            DateBlocage = null,
            DateCreMkgt = null,
            DateCreGpi = DateTime.Now - TimeSpan.FromDays(2),
            UpdatedAt = DateTime.Now,
            Radie = false,
            CategorieId = 1,
            DocumentPdfEuropes = new List<DocumentPdfEurope>()
            {
                new DocumentPdfEurope()
                {
                    FileSize = 100,
                    FileName = "file1.pdf",
                    FileLocation = "path/to/file1.pdf",
                    TypeDocumentPdfId = 1,
                    EtablissementClientEuropeId = 1
                },
                new DocumentPdfEurope()
                {
                    FileSize = 200,
                    FileName = "file2.pdf",
                    FileLocation = "path/to/file2.pdf",
                    TypeDocumentPdfId = 2,
                    EtablissementClientEuropeId = 1
                }
            }
        };
        _context.ClientEurope.Add(client);
        

        
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
            NumeroTvaIntracommunautaire = "FR324356547"
        };
        _context.EntrepriseBase.Add(entrepriseBase);
        
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
        _context.EntrepriseBase.Add(entrepriseBase);
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
        _context.EntrepriseBase.Add(entrepriseBase);
        
        entrepriseBase = new EntrepriseBase
        {
            Id = 4,
            Siren = "258796413",
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
        
        _context.EntrepriseBase.Add(entrepriseBase);
            
        
        var etablissementClient = new EtablissementClient
        {
            Id = 1,
            Nom = "Etablissement 1",
            Siret = "05680065900858",
            Siren = "056800659",
            CodeMkgt = "TESCLI0001",
            CodeKerlog = null,
            IdOdoo = "1",
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
            DateCreOdoo =  null,
            IdShipperDashdoc = null
        };
        _context.EtablissementClient.Add(etablissementClient);

        etablissementClient = new EtablissementClient
        {
            Id = 2,
            IdOdoo = "000002",
            CodeGpi = "C00002",
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
            DateCreOdoo = DateTime.Now,
            DateCreGpi = DateTime.Now,
        };
        _context.EtablissementClient.Add(etablissementClient);
        
        etablissementClient = new EtablissementClient
        {
            Id = 3,
            CodeMkgt = "TESCLI0003",
            IdOdoo = "000003",
            CodeGpi = "C00003",
            IdDashdoc = 1235483,
            IdHubspot = "8965879040",
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
            CreateDate = DateTime.Now- TimeSpan.FromDays(2),
            Radie = false,
            CreatedBy = "Test engineer",
            DateCreMKGT = DateTime.Now - TimeSpan.FromDays(2),
            DateCreOdoo = DateTime.Now - TimeSpan.FromDays(2),
            DateCreGpi = DateTime.Now - TimeSpan.FromDays(2),
            UpdatedAt = DateTime.Now,
            IdShipperDashdoc = 123542584
        };
        _context.EtablissementClient.Add(etablissementClient);
        etablissementClient = new EtablissementClient
        {
            Id = 9,
            Nom = "Etablissement 9",
            Siret = "02526474600016",
            Siren = "025264746",
            CodeMkgt = "TESCLI0009",
            IdOdoo = "ODOO0009",
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
            CreateDate = DateTime.Now - TimeSpan.FromDays(2),
            Radie = false,
            CreatedBy = "Test engineer",
            DateCreMKGT = DateTime.Now - TimeSpan.FromDays(2),
            DateCreOdoo =  DateTime.Now - TimeSpan.FromDays(2),
            IdShipperDashdoc = 123542589,
        };
        _context.EtablissementClient.Add(etablissementClient);
        
        var garantie = new EntrepriseCouverture
        {
            Siren = "056800659",
            MontantGarantie = 100000,
            NumeroContratPrimaire = "FR148131184512",
            NumeroContratExtension = "FR148131184512",
            CoverId = "5464",
            Decision = "oui",
            DateDecision = DateTime.Now,
            DateDemande = DateTime.Now,
            DeviseGarantie = "EUR",
            EhId = "1234567890123",
            RaisonSociale = "SOCIETE GENERALE",
            TypeGarantie = "Garantie",
            TypeIdentifiant = "SIREN",
            TypeReponse = "Garantie",
            StatutEntreprise = "ouverte",
            NomRue = "rue des tests",
            Pays = "FRANCE",
            RepriseGarantiePossible = "oui",
            Ville = "tests",
        };
        _context.EntrepriseCouverture.Add(garantie);
        
        var ndCover = new EntrepriseNDCover
        {
            Siren = "200017598",
            NumeroContratPrimaire = "FR148131184512",
            NumeroContratExtension = "FR148131184512",
            CoverId = "5464",
            EhId = "1234567890123",
            RaisonSociale = "SOCIETE GENERALE",
            TypeIdentifiant = "SIREN",
            StatutEntreprise = "ouverte",
            NomRue = "rue des tests",
            Pays = "FRANCE",
            Ville = "tests",
            FormeJuridiqueCode = "SARL",
            NomPolice = "Recynov",
            PeriodeRenouvellementOuverte = "oui",
            SecteurActivite = "A risque",
            Statut = "Garantie totale",
            TempsReponse = "2 minutes"
        };
        _context.EntrepriseNDCovers.Add(ndCover);
        ndCover = new EntrepriseNDCover
        {
            Siren = "948224746",
            NumeroContratPrimaire = "FR148131184512",
            NumeroContratExtension = "FR148131184512",
            CoverId = "5464",
            EhId = "1234567890123",
            RaisonSociale = "SOCIETE GENERALE",
            TypeIdentifiant = "SIREN",
            StatutEntreprise = "ouverte",
            NomRue = "rue des tests",
            Pays = "FRANCE",
            Ville = "tests",
            FormeJuridiqueCode = "SARL",
            NomPolice = "Recynov",
            PeriodeRenouvellementOuverte = "non",
            SecteurActivite = "A risque",
            Statut = "Pas de garantie",
            TempsReponse = "2 minutes"
        };
        
        _context.EntrepriseNDCovers.Add(ndCover);
        ndCover = new EntrepriseNDCover
        {
            Siren = "258796413",
            NumeroContratPrimaire = "FR148131184512",
            NumeroContratExtension = "FR148131184512",
            CoverId = "5464",
            EhId = "1234567890123",
            RaisonSociale = "SOCIETE GENERALE",
            TypeIdentifiant = "SIREN",
            StatutEntreprise = "ouverte",
            NomRue = "rue des tests",
            Pays = "FRANCE",
            Ville = "tests",
            FormeJuridiqueCode = "SARL",
            NomPolice = "Recynov",
            PeriodeRenouvellementOuverte = "non",
            SecteurActivite = "A risque",
            Statut = "Statut invalide",
            TempsReponse = "2 minutes"
        };
        _context.EntrepriseNDCovers.Add(ndCover);
        
        var clientParticulier = new ClientParticulier
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
            IsDeleted = false,
            EmailFacturation = "calicali@mail.it",
            TelephoneFacturation = "+33 1 23 45 67 89",
            PortableFacturation = "+33 6 12 34 56 78",
            ContactAlternatif = "Caesonia Milonia",
            EmailAlternatif = "caemilo@mail.it",
            TelephoneAlternatif = "+33 1 23 45 67 89",
            PortableAlternatif = "+33 6 12 34 56 78",
            CreateDate = DateTime.Now,
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
            DateCreMkgt = DateTime.Now,
            IdOdoo = "ODOO0001",
            DateCreOdoo = DateTime.Now,
            UpdatedAt = null
        };
        _context.ClientParticuliers.Add(clientParticulier);
        
        clientParticulier = new ClientParticulier
        {
            Id = 4,
            Nom = "Caligula",
            Prenom = "Caius Julius Caesar Augustus Germanicus",
            Titre = "M",
            AdresseFacturation1 = "Piazza Augusto Imperatore, 30",
            AdresseFacturation2 = "",
            AdresseFacturation3 = "",
            CodePostalFacturation = "00186",
            VilleFacturation = "Rome",
            PaysFacturation = "Italie",
            IsDeleted = false,
            EmailFacturation = "calicali@mail.it",
            TelephoneFacturation = "+33 1 23 45 67 89",
            PortableFacturation = "+33 6 12 34 56 78",
            ContactAlternatif = "Caesonia Milonia",
            EmailAlternatif = "caemilo@mail.it",
            TelephoneAlternatif = "+33 1 23 45 67 89",
            PortableAlternatif = "+33 6 12 34 56 78",
            CreateDate = DateTime.Now,
            ConditionReglement = 30,
            ModeReglement = 2,
            DelaiReglement = 3,
            TauxTva = 5.5m,
            EncoursMax = 10000,
            CompteComptable = "411103",
            ClientBloque = false,
            MotifBlocage = "",
            DateBlocage = null,
            CodeMkgt = "TESCLI0002",
            DateCreMkgt = new DateTime(18, 12, 23, 0, 0, 0, DateTimeKind.Utc),
            IdOdoo = "ODOO0001",
            DateCreOdoo =new DateTime(18, 12, 23, 0, 0, 0, DateTimeKind.Utc),
            UpdatedAt = DateTime.Now,
            IdShipperDashdoc = 123456,
        };
        _context.ClientParticuliers.Add(clientParticulier);
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
        _context.MessageMails.Add(messageMail);

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
        _context.MessageMails.Add(messageMail);

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
        _context.MessageMails.Add(messageMail);
                messageMail = new MessageMail
        {
            Id = 4,
            Subject = "Rappel mensuel",
            From = "noreply@recygroup.fr",
            To = "support@example.com",
            Status = TaskStatus.InProgress,
            Priority = 3,
            DateCreated = DateTime.Now,
            Body = "Contenu du rappel mensuel",
            DateSent = DateTime.Now,
        };
        _context.MessageMails.Add(messageMail);
        _context.SaveChanges();
        
        // 🔥 Détacher toutes les entités pour simuler un environnement réel
        foreach (var entry in _context.ChangeTracker.Entries().ToList())
        {
            _context.Entry(entry.Entity).State = EntityState.Detached;
        }
    }
}