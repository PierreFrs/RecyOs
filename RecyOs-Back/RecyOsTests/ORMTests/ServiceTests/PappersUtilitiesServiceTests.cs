using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service.pappers;

namespace RecyOsTests.ORMTests.ServiceTests;

public class PappersUtilitiesServiceTests
{
    [Theory]
    [InlineData("20001759800034", true)]
    [InlineData("20001759800035", false)]
    [InlineData("200017598000", false)]
    public void CheckSiret_ValidInputs_ReturnsExpectedValue(string input, bool expected)
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();

        var myClassInstance = new PappersUtilitiesService(
            pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            );

        // Act
        var result = myClassInstance.CheckSiret(input);

        // Assert
        Assert.Equal(expected, result);

    }

    [Fact]
    public async Task CreateEtablissementClientBySiret_WithValidSiretAndDiffusible_ReturnsExpectedValue()
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();

        string valretour =
            "{\"siren\":\"200017598\",\"siren_formate\":\"200 017 598\",\"diffusable\":true,\"nom_entreprise\":\"SIDEN-SIAN\",\"personne_morale\":true,\"denomination\":\"SIDEN-SIAN\",\"sigle\":\"SIDEN-SIAN\",\"nom\":null,\"prenom\":null,\"sexe\":null,\"siege\":{\"siret\":\"20001759800018\",\"siret_formate\":\"200 017 598 00018\",\"diffusion_partielle\":false,\"nic\":\"00018\",\"numero_voie\":23,\"indice_repetition\":null,\"type_voie\":\"AV\",\"libelle_voie\":\"DE LA MARNE\",\"complement_adresse\":null,\"adresse_ligne_1\":\"23 AV DE LA MARNE\",\"adresse_ligne_2\":null,\"code_postal\":\"59290\",\"ville\":\"WASQUEHAL\",\"pays\":\"France\",\"code_pays\":\"FR\",\"latitude\":50.686666,\"longitude\":3.12194,\"code_naf\":\"37.00Z\",\"libelle_code_naf\":\"Collecte et traitement des eaux usées\",\"date_de_creation\":\"2008-11-21\",\"etablissement_employeur\":true,\"effectif\":\"Entre 10 et 19 salariés\",\"effectif_min\":10,\"effectif_max\":19,\"tranche_effectif\":\"11\",\"annee_effectif\":2020,\"etablissement_cesse\":false,\"date_cessation\":null,\"domiciliation\":null,\"siege\":true,\"enseigne\":null,\"nom_commercial\":null},\"rnm\":null,\"code_naf\":\"37.00Z\",\"libelle_code_naf\":\"Collecte et traitement des eaux usées\",\"domaine_activite\":\"Collecte et traitement des eaux usées\",\"objet_social\":null,\"conventions_collectives\":[{\"nom\":\"Statut de la Fonction publique territoriale\",\"idcc\":5021,\"confirmee\":true}],\"date_creation\":\"2008-11-21\",\"date_creation_formate\":\"21/11/2008\",\"entreprise_cessee\":false,\"date_cessation\":null,\"date_cessation_formate\":null,\"associe_unique\":null,\"categorie_juridique\":\"7354\",\"forme_juridique\":\"Syndicat mixte fermé\",\"forme_exercice\":null,\"entreprise_employeuse\":true,\"societe_a_mission\":false,\"effectif\":\"Entre 500 et 999 salariés\",\"effectif_min\":500,\"effectif_max\":999,\"annee_effectif\":2020,\"tranche_effectif\":\"41\",\"annee_tranche_effectif\":2020,\"capital\":null,\"capital_actuel_si_variable\":null,\"devise_capital\":null,\"capital_formate\":null,\"date_cloture_exercice\":null,\"date_cloture_exercice_exceptionnelle\":null,\"prochaine_date_cloture_exercice\":null,\"prochaine_date_cloture_exercice_formate\":null,\"economie_sociale_solidaire\":false,\"duree_personne_morale\":null,\"derniere_mise_a_jour_sirene\":\"2023-06-07\",\"derniere_mise_a_jour_rcs\":null,\"derniere_mise_a_jour_rne\":null,\"dernier_traitement\":\"2022-08-29\",\"date_debut_activite\":null,\"date_debut_premiere_activite\":null,\"statut_rcs\":\"Non Inscrit\",\"greffe\":null,\"code_greffe\":null,\"numero_rcs\":null,\"date_immatriculation_rcs\":null,\"date_premiere_immatriculation_rcs\":null,\"date_radiation_rcs\":null,\"statut_rne\":\"Non Inscrit\",\"date_immatriculation_rne\":null,\"date_radiation_rne\":null,\"numero_tva_intracommunautaire\":\"FR39200017598\",\"etablissement\":{\"siret\":\"20001759800034\",\"siret_formate\":\"200 017 598 00034\",\"diffusion_partielle\":false,\"nic\":\"00034\",\"numero_voie\":23,\"indice_repetition\":null,\"type_voie\":\"AV\",\"libelle_voie\":\"DE LA MARNE\",\"complement_adresse\":null,\"adresse_ligne_1\":\"23 AV DE LA MARNE\",\"adresse_ligne_2\":null,\"code_postal\":\"59290\",\"ville\":\"WASQUEHAL\",\"pays\":\"France\",\"code_pays\":\"FR\",\"latitude\":50.686666,\"longitude\":3.12194,\"code_naf\":\"36.00Z\",\"libelle_code_naf\":\"Captage, traitement et distribution d'eau\",\"etablissement_employeur\":false,\"effectif\":\"0 salarié\",\"effectif_min\":0,\"effectif_max\":0,\"tranche_effectif\":\"22\",\"annee_effectif\":2020,\"date_de_creation\":\"2019-07-01\",\"etablissement_cesse\":false,\"date_cessation\":null,\"domiciliation\":null,\"siege\":false,\"enseigne\":\"SIDEN-SIAN NOREADE EAU\",\"nom_commercial\":null},\"finances\":[],\"representants\":[],\"beneficiaires_effectifs\":[],\"depots_actes\":[],\"comptes\":[],\"publications_bodacc\":[],\"procedures_collectives\":[],\"procedure_collective_existe\":false,\"procedure_collective_en_cours\":false,\"derniers_statuts\":null,\"extrait_immatriculation\":null,\"association\":null}";
        
        var etablissementClientDto = new EtablissementClientDto
        {
            Siret = "20001759800034",
            Siren = "200017598",
            Nom = "SIDEN-SIAN",
            AdresseFacturation1 = "23 AV DE LA MARNE",
            AdresseFacturation2 = null,
            AdresseFacturation3 = null,
            CodePostalFacturation = "59290",
            VilleFacturation = "WASQUEHAL",
            PaysFacturation = "FRANCE",
        };
        
        pappersApiServiceMock.Setup(x => x.GetEtablissement(It.IsAny<String>()))
            .ReturnsAsync(valretour);

        etablissementClientServiceMock.Setup(x => x.Create(It.IsAny<EtablissementClientDto>(),
                It.IsAny<EntrepriseBaseDto>(), It.IsAny<EtablissementFicheDto>(), It.IsAny<bool>()))
            .ReturnsAsync(etablissementClientDto);
        
        etablissementClientServiceMock.Setup(x => x.Edit(It.IsAny<EtablissementClientDto>()))
            .ReturnsAsync(etablissementClientDto);

        var myClassInstance = new PappersUtilitiesService(pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            );

        // Act
        var result = await myClassInstance.CreateEtablissementClientBySiret(
            "20001759800034",true,false);

        // Assert
        Assert.Equal("20001759800034", result.Siret);
        Assert.Equal("200017598", result.Siren);
        loggerMock.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains("Création de la fiche entreprise avec les données de pappers")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Exactly(1));
    }

    [Fact]
    public async Task CreateEtablissementClientBySiret_WithInvalidSiret_ShouldReturnNull()
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();


         var myClassInstance = new PappersUtilitiesService(pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            ); // Remplacer MyClass par le nom de votre classe

        // Act
        var result = await myClassInstance.CreateEtablissementClientBySiret(
            "20001759800035",true,false);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateEtablissementClientBySiret_WithUnregistredSiret_ShouldReturnNull()
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();


         var myClassInstance = new PappersUtilitiesService(pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            ); // Remplacer MyClass par le nom de votre classe

        // Act
        var result = await myClassInstance.CreateEtablissementClientBySiret("20001759800034",true,false);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateEtablissementClientNonDiffusableBySiret_WithInvalidSiret_ShouldReturnNull()
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();


         var myClassInstance = new PappersUtilitiesService(pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            );

        // Act
        MethodInfo? methodInfo = typeof(PappersUtilitiesService).GetMethod(
            "CreateEtablissementClientNonDiffusableBySiret", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        EtablissementClientDto? task = await (Task<EtablissementClientDto?>)methodInfo.Invoke(
            myClassInstance, new object[] { "20001759800035", true, false });
        
        // Assert
        Assert.Null(task);
    }

    [Fact]
    public async Task UpdateEtablissementClientBySiret_WithBadSiret_ShouldReturnNull()
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();


         var myClassInstance = new PappersUtilitiesService(pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            );

        // Act
        var result = await myClassInstance.UpdateEtablissementClientBySiret("20001759800035");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateEtablissementClientBySiret_WithUnregistredSiret_ShouldReturnNull()
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();


         var myClassInstance = new PappersUtilitiesService(pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            );

        // Act
        var result = await myClassInstance.UpdateEtablissementClientBySiret("20001759800034");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateEtablissementClientBySiret_WithNonDiffusibleSiret_ShouldReturnExpected()
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();


        string valretour =
            "{\"siren\":\"200017598\",\"siren_formate\":\"200 017 598\",\"diffusable\":false,\"nom_entreprise\":\"SIDEN-SIAN\",\"personne_morale\":true,\"denomination\":\"SIDEN-SIAN\",\"sigle\":\"SIDEN-SIAN\",\"nom\":null,\"prenom\":null,\"sexe\":null,\"siege\":{\"siret\":\"20001759800018\",\"siret_formate\":\"200 017 598 00018\",\"diffusion_partielle\":false,\"nic\":\"00018\",\"numero_voie\":23,\"indice_repetition\":null,\"type_voie\":\"AV\",\"libelle_voie\":\"DE LA MARNE\",\"complement_adresse\":null,\"adresse_ligne_1\":\"23 AV DE LA MARNE\",\"adresse_ligne_2\":null,\"code_postal\":\"59290\",\"ville\":\"WASQUEHAL\",\"pays\":\"France\",\"code_pays\":\"FR\",\"latitude\":50.686666,\"longitude\":3.12194,\"code_naf\":\"37.00Z\",\"libelle_code_naf\":\"Collecte et traitement des eaux usées\",\"date_de_creation\":\"2008-11-21\",\"etablissement_employeur\":true,\"effectif\":\"Entre 10 et 19 salariés\",\"effectif_min\":10,\"effectif_max\":19,\"tranche_effectif\":\"11\",\"annee_effectif\":2020,\"etablissement_cesse\":false,\"date_cessation\":null,\"domiciliation\":null,\"siege\":true,\"enseigne\":null,\"nom_commercial\":null},\"rnm\":null,\"code_naf\":\"37.00Z\",\"libelle_code_naf\":\"Collecte et traitement des eaux usées\",\"domaine_activite\":\"Collecte et traitement des eaux usées\",\"objet_social\":null,\"conventions_collectives\":[{\"nom\":\"Statut de la Fonction publique territoriale\",\"idcc\":5021,\"confirmee\":true}],\"date_creation\":\"2008-11-21\",\"date_creation_formate\":\"21/11/2008\",\"entreprise_cessee\":false,\"date_cessation\":null,\"date_cessation_formate\":null,\"associe_unique\":null,\"categorie_juridique\":\"7354\",\"forme_juridique\":\"Syndicat mixte fermé\",\"forme_exercice\":null,\"entreprise_employeuse\":true,\"societe_a_mission\":false,\"effectif\":\"Entre 500 et 999 salariés\",\"effectif_min\":500,\"effectif_max\":999,\"annee_effectif\":2020,\"tranche_effectif\":\"41\",\"annee_tranche_effectif\":2020,\"capital\":null,\"capital_actuel_si_variable\":null,\"devise_capital\":null,\"capital_formate\":null,\"date_cloture_exercice\":null,\"date_cloture_exercice_exceptionnelle\":null,\"prochaine_date_cloture_exercice\":null,\"prochaine_date_cloture_exercice_formate\":null,\"economie_sociale_solidaire\":false,\"duree_personne_morale\":null,\"derniere_mise_a_jour_sirene\":\"2023-06-07\",\"derniere_mise_a_jour_rcs\":null,\"derniere_mise_a_jour_rne\":null,\"dernier_traitement\":\"2022-08-29\",\"date_debut_activite\":null,\"date_debut_premiere_activite\":null,\"statut_rcs\":\"Non Inscrit\",\"greffe\":null,\"code_greffe\":null,\"numero_rcs\":null,\"date_immatriculation_rcs\":null,\"date_premiere_immatriculation_rcs\":null,\"date_radiation_rcs\":null,\"statut_rne\":\"Non Inscrit\",\"date_immatriculation_rne\":null,\"date_radiation_rne\":null,\"numero_tva_intracommunautaire\":\"FR39200017598\",\"etablissement\":{\"siret\":\"20001759800034\",\"siret_formate\":\"200 017 598 00034\",\"diffusion_partielle\":false,\"nic\":\"00034\",\"numero_voie\":23,\"indice_repetition\":null,\"type_voie\":\"AV\",\"libelle_voie\":\"DE LA MARNE\",\"complement_adresse\":null,\"adresse_ligne_1\":\"23 AV DE LA MARNE\",\"adresse_ligne_2\":null,\"code_postal\":\"59290\",\"ville\":\"WASQUEHAL\",\"pays\":\"France\",\"code_pays\":\"FR\",\"latitude\":50.686666,\"longitude\":3.12194,\"code_naf\":\"36.00Z\",\"libelle_code_naf\":\"Captage, traitement et distribution d'eau\",\"etablissement_employeur\":false,\"effectif\":\"0 salarié\",\"effectif_min\":0,\"effectif_max\":0,\"tranche_effectif\":\"22\",\"annee_effectif\":2020,\"date_de_creation\":\"2019-07-01\",\"etablissement_cesse\":false,\"date_cessation\":null,\"domiciliation\":null,\"siege\":false,\"enseigne\":\"SIDEN-SIAN NOREADE EAU\",\"nom_commercial\":null},\"finances\":[],\"representants\":[],\"beneficiaires_effectifs\":[],\"depots_actes\":[],\"comptes\":[],\"publications_bodacc\":[],\"procedures_collectives\":[],\"procedure_collective_existe\":false,\"procedure_collective_en_cours\":false,\"derniers_statuts\":null,\"extrait_immatriculation\":null,\"association\":null}";

        pappersApiServiceMock.Setup(x => x.GetEtablissement(It.IsAny<String>()))
            .ReturnsAsync(valretour);

         var myClassInstance = new PappersUtilitiesService(pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            );

        // Act  
        await myClassInstance.UpdateEtablissementClientBySiret("20001759800018");

        // Assert
        loggerMock.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains("Les données de l'etablissement ne sont pas difusibles")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Exactly(1));

    }

    [Fact]
    public async Task UpdateEtablissementClientBySiret_WithValidSiretAndDiffusible_ReturnsExpectedValue()
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();


        string valretour =
            "{\"siren\":\"200017598\",\"siren_formate\":\"200 017 598\",\"diffusable\":true,\"nom_entreprise\":\"SIDEN-SIAN\",\"personne_morale\":true,\"denomination\":\"SIDEN-SIAN\",\"sigle\":\"SIDEN-SIAN\",\"nom\":null,\"prenom\":null,\"sexe\":null,\"siege\":{\"siret\":\"20001759800018\",\"siret_formate\":\"200 017 598 00018\",\"diffusion_partielle\":false,\"nic\":\"00018\",\"numero_voie\":23,\"indice_repetition\":null,\"type_voie\":\"AV\",\"libelle_voie\":\"DE LA MARNE\",\"complement_adresse\":null,\"adresse_ligne_1\":\"23 AV DE LA MARNE\",\"adresse_ligne_2\":null,\"code_postal\":\"59290\",\"ville\":\"WASQUEHAL\",\"pays\":\"France\",\"code_pays\":\"FR\",\"latitude\":50.686666,\"longitude\":3.12194,\"code_naf\":\"37.00Z\",\"libelle_code_naf\":\"Collecte et traitement des eaux usées\",\"date_de_creation\":\"2008-11-21\",\"etablissement_employeur\":true,\"effectif\":\"Entre 10 et 19 salariés\",\"effectif_min\":10,\"effectif_max\":19,\"tranche_effectif\":\"11\",\"annee_effectif\":2020,\"etablissement_cesse\":false,\"date_cessation\":null,\"domiciliation\":null,\"siege\":true,\"enseigne\":null,\"nom_commercial\":null},\"rnm\":null,\"code_naf\":\"37.00Z\",\"libelle_code_naf\":\"Collecte et traitement des eaux usées\",\"domaine_activite\":\"Collecte et traitement des eaux usées\",\"objet_social\":null,\"conventions_collectives\":[{\"nom\":\"Statut de la Fonction publique territoriale\",\"idcc\":5021,\"confirmee\":true}],\"date_creation\":\"2008-11-21\",\"date_creation_formate\":\"21/11/2008\",\"entreprise_cessee\":false,\"date_cessation\":null,\"date_cessation_formate\":null,\"associe_unique\":null,\"categorie_juridique\":\"7354\",\"forme_juridique\":\"Syndicat mixte fermé\",\"forme_exercice\":null,\"entreprise_employeuse\":true,\"societe_a_mission\":false,\"effectif\":\"Entre 500 et 999 salariés\",\"effectif_min\":500,\"effectif_max\":999,\"annee_effectif\":2020,\"tranche_effectif\":\"41\",\"annee_tranche_effectif\":2020,\"capital\":null,\"capital_actuel_si_variable\":null,\"devise_capital\":null,\"capital_formate\":null,\"date_cloture_exercice\":null,\"date_cloture_exercice_exceptionnelle\":null,\"prochaine_date_cloture_exercice\":null,\"prochaine_date_cloture_exercice_formate\":null,\"economie_sociale_solidaire\":false,\"duree_personne_morale\":null,\"derniere_mise_a_jour_sirene\":\"2023-06-07\",\"derniere_mise_a_jour_rcs\":null,\"derniere_mise_a_jour_rne\":null,\"dernier_traitement\":\"2022-08-29\",\"date_debut_activite\":null,\"date_debut_premiere_activite\":null,\"statut_rcs\":\"Non Inscrit\",\"greffe\":null,\"code_greffe\":null,\"numero_rcs\":null,\"date_immatriculation_rcs\":null,\"date_premiere_immatriculation_rcs\":null,\"date_radiation_rcs\":null,\"statut_rne\":\"Non Inscrit\",\"date_immatriculation_rne\":null,\"date_radiation_rne\":null,\"numero_tva_intracommunautaire\":\"FR39200017598\",\"etablissement\":{\"siret\":\"20001759800034\",\"siret_formate\":\"200 017 598 00034\",\"diffusion_partielle\":false,\"nic\":\"00034\",\"numero_voie\":23,\"indice_repetition\":null,\"type_voie\":\"AV\",\"libelle_voie\":\"DE LA MARNE\",\"complement_adresse\":null,\"adresse_ligne_1\":\"23 AV DE LA MARNE\",\"adresse_ligne_2\":null,\"code_postal\":\"59290\",\"ville\":\"WASQUEHAL\",\"pays\":\"France\",\"code_pays\":\"FR\",\"latitude\":50.686666,\"longitude\":3.12194,\"code_naf\":\"36.00Z\",\"libelle_code_naf\":\"Captage, traitement et distribution d'eau\",\"etablissement_employeur\":false,\"effectif\":\"0 salarié\",\"effectif_min\":0,\"effectif_max\":0,\"tranche_effectif\":\"22\",\"annee_effectif\":2020,\"date_de_creation\":\"2019-07-01\",\"etablissement_cesse\":false,\"date_cessation\":null,\"domiciliation\":null,\"siege\":false,\"enseigne\":\"SIDEN-SIAN NOREADE EAU\",\"nom_commercial\":null},\"finances\":[],\"representants\":[],\"beneficiaires_effectifs\":[],\"depots_actes\":[],\"comptes\":[],\"publications_bodacc\":[],\"procedures_collectives\":[],\"procedure_collective_existe\":false,\"procedure_collective_en_cours\":false,\"derniers_statuts\":null,\"extrait_immatriculation\":null,\"association\":null}";
        pappersApiServiceMock.Setup(x => x.GetEtablissement(It.IsAny<String>()))
            .ReturnsAsync(valretour);

         var myClassInstance = new PappersUtilitiesService(pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            );

        // Act  
        await myClassInstance.UpdateEtablissementClientBySiret("20001759800034");

        // Assert
        entrepriseBaseServiceMock.Verify(
            x => x.Edit(It.Is<EntrepriseBaseDto>(dto => dto.Siren == "200017598"
                                                        && dto.Diffusable
                                                        && dto.NumeroTvaIntracommunautaire == "FR39200017598"
                                                        && dto.NomEntreprise == "SIDEN-SIAN"
                                                        && dto.CodeNaf == "37.00Z" 
                                                        && dto.LibelleCodeNaf == "Collecte et traitement des eaux usées"
                                                        && dto.EntrepriseEmployeuse)), Times.Once);
        
        etablissementFicheServiceMock.Verify(x => x.Edit(It.Is<EtablissementFicheDto>(dto => dto.Siret == "20001759800034"
                                                            && dto.CodePostal == "59290" && dto.Ville == "WASQUEHAL"
                                                            && dto.CodeNaf == "36.00Z" 
                                                            && dto.LibelleCodeNaf == "Captage, traitement et distribution d'eau"
                                                            && dto.Diffusable
                                                            && dto.AdresseLigne1 == "23 AV DE LA MARNE"
                                                           )),
            Times.Once);
    }

    [Fact]
    public async Task CreateEntrepriseBySiret_WithInvalidSiret_ShouldReturnNull()
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();


         var myClassInstance = new PappersUtilitiesService(pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            );

        // Act  
        var result = await myClassInstance.CreateEntrepriseBySiret("20001759800035");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateEntrepriseBySiret_WithUnregistredSiret_ShouldReturnNull()
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();


         var myClassInstance = new PappersUtilitiesService(pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            );

        // Act
        var result = await myClassInstance.CreateEntrepriseBySiret("20001759800018");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateEntrepriseBySiret_WithValidSiretAndNonDiffusible_ReturnsExpectedValue()
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();


        string valretour =
            "{\"siren\":\"200017598\",\"siren_formate\":\"200 017 598\",\"diffusable\":false,\"nom_entreprise\":\"SIDEN-SIAN\",\"personne_morale\":true,\"denomination\":\"SIDEN-SIAN\",\"sigle\":\"SIDEN-SIAN\",\"nom\":null,\"prenom\":null,\"sexe\":null,\"siege\":{\"siret\":\"20001759800018\",\"siret_formate\":\"200 017 598 00018\",\"diffusion_partielle\":false,\"nic\":\"00018\",\"numero_voie\":23,\"indice_repetition\":null,\"type_voie\":\"AV\",\"libelle_voie\":\"DE LA MARNE\",\"complement_adresse\":null,\"adresse_ligne_1\":\"23 AV DE LA MARNE\",\"adresse_ligne_2\":null,\"code_postal\":\"59290\",\"ville\":\"WASQUEHAL\",\"pays\":\"France\",\"code_pays\":\"FR\",\"latitude\":50.686666,\"longitude\":3.12194,\"code_naf\":\"37.00Z\",\"libelle_code_naf\":\"Collecte et traitement des eaux usées\",\"date_de_creation\":\"2008-11-21\",\"etablissement_employeur\":true,\"effectif\":\"Entre 10 et 19 salariés\",\"effectif_min\":10,\"effectif_max\":19,\"tranche_effectif\":\"11\",\"annee_effectif\":2020,\"etablissement_cesse\":false,\"date_cessation\":null,\"domiciliation\":null,\"siege\":true,\"enseigne\":null,\"nom_commercial\":null},\"rnm\":null,\"code_naf\":\"37.00Z\",\"libelle_code_naf\":\"Collecte et traitement des eaux usées\",\"domaine_activite\":\"Collecte et traitement des eaux usées\",\"objet_social\":null,\"conventions_collectives\":[{\"nom\":\"Statut de la Fonction publique territoriale\",\"idcc\":5021,\"confirmee\":true}],\"date_creation\":\"2008-11-21\",\"date_creation_formate\":\"21/11/2008\",\"entreprise_cessee\":false,\"date_cessation\":null,\"date_cessation_formate\":null,\"associe_unique\":null,\"categorie_juridique\":\"7354\",\"forme_juridique\":\"Syndicat mixte fermé\",\"forme_exercice\":null,\"entreprise_employeuse\":true,\"societe_a_mission\":false,\"effectif\":\"Entre 500 et 999 salariés\",\"effectif_min\":500,\"effectif_max\":999,\"annee_effectif\":2020,\"tranche_effectif\":\"41\",\"annee_tranche_effectif\":2020,\"capital\":null,\"capital_actuel_si_variable\":null,\"devise_capital\":null,\"capital_formate\":null,\"date_cloture_exercice\":null,\"date_cloture_exercice_exceptionnelle\":null,\"prochaine_date_cloture_exercice\":null,\"prochaine_date_cloture_exercice_formate\":null,\"economie_sociale_solidaire\":false,\"duree_personne_morale\":null,\"derniere_mise_a_jour_sirene\":\"2023-06-07\",\"derniere_mise_a_jour_rcs\":null,\"derniere_mise_a_jour_rne\":null,\"dernier_traitement\":\"2022-08-29\",\"date_debut_activite\":null,\"date_debut_premiere_activite\":null,\"statut_rcs\":\"Non Inscrit\",\"greffe\":null,\"code_greffe\":null,\"numero_rcs\":null,\"date_immatriculation_rcs\":null,\"date_premiere_immatriculation_rcs\":null,\"date_radiation_rcs\":null,\"statut_rne\":\"Non Inscrit\",\"date_immatriculation_rne\":null,\"date_radiation_rne\":null,\"numero_tva_intracommunautaire\":\"FR39200017598\",\"etablissement\":{\"siret\":\"20001759800034\",\"siret_formate\":\"200 017 598 00034\",\"diffusion_partielle\":false,\"nic\":\"00034\",\"numero_voie\":23,\"indice_repetition\":null,\"type_voie\":\"AV\",\"libelle_voie\":\"DE LA MARNE\",\"complement_adresse\":null,\"adresse_ligne_1\":\"23 AV DE LA MARNE\",\"adresse_ligne_2\":null,\"code_postal\":\"59290\",\"ville\":\"WASQUEHAL\",\"pays\":\"France\",\"code_pays\":\"FR\",\"latitude\":50.686666,\"longitude\":3.12194,\"code_naf\":\"36.00Z\",\"libelle_code_naf\":\"Captage, traitement et distribution d'eau\",\"etablissement_employeur\":false,\"effectif\":\"0 salarié\",\"effectif_min\":0,\"effectif_max\":0,\"tranche_effectif\":\"22\",\"annee_effectif\":2020,\"date_de_creation\":\"2019-07-01\",\"etablissement_cesse\":false,\"date_cessation\":null,\"domiciliation\":null,\"siege\":false,\"enseigne\":\"SIDEN-SIAN NOREADE EAU\",\"nom_commercial\":null},\"finances\":[],\"representants\":[],\"beneficiaires_effectifs\":[],\"depots_actes\":[],\"comptes\":[],\"publications_bodacc\":[],\"procedures_collectives\":[],\"procedure_collective_existe\":false,\"procedure_collective_en_cours\":false,\"derniers_statuts\":null,\"extrait_immatriculation\":null,\"association\":null}";

        pappersApiServiceMock.Setup(x => x.GetEtablissement(It.IsAny<String>()))
            .ReturnsAsync(valretour);

         var myClassInstance = new PappersUtilitiesService(pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            );

        // Act  
        await myClassInstance.CreateEntrepriseBySiret("20001759800018");

        // Assert
        loggerMock.Verify(x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains("Les données de l'etablissement ne sont pas difusibles")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Exactly(1));
        entrepriseBaseServiceMock.Verify(x => x.Create(It.Is<EntrepriseBaseDto>(dto => dto.Siren == "200017598"
                                                        && !dto.Diffusable)), Times.Once);
    }

    [Fact]
    public async Task CreateEntrepriseBySiret_WithValidSiretDiffusible_ShouldReturnExpectedValue()
    {
        // Arrange
        var pappersApiServiceMock = new Mock<IPappersApiService>();
        var etablissementClientServiceMock = new Mock<IEtablissementClientService>();
        var etablissementFournisseurServiceMock = new Mock<IEtablissementFournisseurService>();
        var entrepriseBaseServiceMock = new Mock<IEntrepriseBaseService>();
        var etablissementFicheServiceMock = new Mock<IEtablissementFicheService>();
        var tokenInfoServiceMock = new Mock<ITokenInfoService>();
        var loggerMock = new Mock<ILogger<PappersUtilitiesService>>();

        
        string valretour =
            "{\"siren\":\"200017598\",\"siren_formate\":\"200 017 598\",\"diffusable\":true,\"nom_entreprise\":\"SIDEN-SIAN\",\"personne_morale\":true,\"denomination\":\"SIDEN-SIAN\",\"sigle\":\"SIDEN-SIAN\",\"nom\":null,\"prenom\":null,\"sexe\":null,\"siege\":{\"siret\":\"20001759800018\",\"siret_formate\":\"200 017 598 00018\",\"diffusion_partielle\":false,\"nic\":\"00018\",\"numero_voie\":23,\"indice_repetition\":null,\"type_voie\":\"AV\",\"libelle_voie\":\"DE LA MARNE\",\"complement_adresse\":null,\"adresse_ligne_1\":\"23 AV DE LA MARNE\",\"adresse_ligne_2\":null,\"code_postal\":\"59290\",\"ville\":\"WASQUEHAL\",\"pays\":\"France\",\"code_pays\":\"FR\",\"latitude\":50.686666,\"longitude\":3.12194,\"code_naf\":\"37.00Z\",\"libelle_code_naf\":\"Collecte et traitement des eaux usées\",\"date_de_creation\":\"2008-11-21\",\"etablissement_employeur\":true,\"effectif\":\"Entre 10 et 19 salariés\",\"effectif_min\":10,\"effectif_max\":19,\"tranche_effectif\":\"11\",\"annee_effectif\":2020,\"etablissement_cesse\":false,\"date_cessation\":null,\"domiciliation\":null,\"siege\":true,\"enseigne\":null,\"nom_commercial\":null},\"rnm\":null,\"code_naf\":\"37.00Z\",\"libelle_code_naf\":\"Collecte et traitement des eaux usées\",\"domaine_activite\":\"Collecte et traitement des eaux usées\",\"objet_social\":null,\"conventions_collectives\":[{\"nom\":\"Statut de la Fonction publique territoriale\",\"idcc\":5021,\"confirmee\":true}],\"date_creation\":\"2008-11-21\",\"date_creation_formate\":\"21/11/2008\",\"entreprise_cessee\":false,\"date_cessation\":null,\"date_cessation_formate\":null,\"associe_unique\":null,\"categorie_juridique\":\"7354\",\"forme_juridique\":\"Syndicat mixte fermé\",\"forme_exercice\":null,\"entreprise_employeuse\":true,\"societe_a_mission\":false,\"effectif\":\"Entre 500 et 999 salariés\",\"effectif_min\":500,\"effectif_max\":999,\"annee_effectif\":2020,\"tranche_effectif\":\"41\",\"annee_tranche_effectif\":2020,\"capital\":null,\"capital_actuel_si_variable\":null,\"devise_capital\":null,\"capital_formate\":null,\"date_cloture_exercice\":null,\"date_cloture_exercice_exceptionnelle\":null,\"prochaine_date_cloture_exercice\":null,\"prochaine_date_cloture_exercice_formate\":null,\"economie_sociale_solidaire\":false,\"duree_personne_morale\":null,\"derniere_mise_a_jour_sirene\":\"2023-06-07\",\"derniere_mise_a_jour_rcs\":null,\"derniere_mise_a_jour_rne\":null,\"dernier_traitement\":\"2022-08-29\",\"date_debut_activite\":null,\"date_debut_premiere_activite\":null,\"statut_rcs\":\"Non Inscrit\",\"greffe\":null,\"code_greffe\":null,\"numero_rcs\":null,\"date_immatriculation_rcs\":null,\"date_premiere_immatriculation_rcs\":null,\"date_radiation_rcs\":null,\"statut_rne\":\"Non Inscrit\",\"date_immatriculation_rne\":null,\"date_radiation_rne\":null,\"numero_tva_intracommunautaire\":\"FR39200017598\",\"etablissement\":{\"siret\":\"20001759800034\",\"siret_formate\":\"200 017 598 00034\",\"diffusion_partielle\":false,\"nic\":\"00034\",\"numero_voie\":23,\"indice_repetition\":null,\"type_voie\":\"AV\",\"libelle_voie\":\"DE LA MARNE\",\"complement_adresse\":null,\"adresse_ligne_1\":\"23 AV DE LA MARNE\",\"adresse_ligne_2\":null,\"code_postal\":\"59290\",\"ville\":\"WASQUEHAL\",\"pays\":\"France\",\"code_pays\":\"FR\",\"latitude\":50.686666,\"longitude\":3.12194,\"code_naf\":\"36.00Z\",\"libelle_code_naf\":\"Captage, traitement et distribution d'eau\",\"etablissement_employeur\":false,\"effectif\":\"0 salarié\",\"effectif_min\":0,\"effectif_max\":0,\"tranche_effectif\":\"22\",\"annee_effectif\":2020,\"date_de_creation\":\"2019-07-01\",\"etablissement_cesse\":false,\"date_cessation\":null,\"domiciliation\":null,\"siege\":false,\"enseigne\":\"SIDEN-SIAN NOREADE EAU\",\"nom_commercial\":null},\"finances\":[],\"representants\":[],\"beneficiaires_effectifs\":[],\"depots_actes\":[],\"comptes\":[],\"publications_bodacc\":[],\"procedures_collectives\":[],\"procedure_collective_existe\":false,\"procedure_collective_en_cours\":false,\"derniers_statuts\":null,\"extrait_immatriculation\":null,\"association\":null}";
        pappersApiServiceMock.Setup(x => x.GetEtablissement(It.IsAny<String>()))
            .ReturnsAsync(valretour);

         var myClassInstance = new PappersUtilitiesService(pappersApiServiceMock.Object,
            () => etablissementClientServiceMock.Object,
            () => etablissementFournisseurServiceMock.Object,
            entrepriseBaseServiceMock.Object, 
            etablissementFicheServiceMock.Object,
            tokenInfoServiceMock.Object,
            loggerMock.Object
            );

        // Act  
        await myClassInstance.CreateEntrepriseBySiret("20001759800034");
        
        // Assert
        entrepriseBaseServiceMock.Verify(
            x => x.Create(It.Is<EntrepriseBaseDto>(dto => dto.Siren == "200017598"
                                                          && dto.Diffusable
                                                          && dto.NumeroTvaIntracommunautaire == "FR39200017598" 
                                                          && dto.NomEntreprise == "SIDEN-SIAN"
                                                          && dto.CodeNaf == "37.00Z" 
                                                          && dto.LibelleCodeNaf == "Collecte et traitement des eaux usées"
                                                          && dto.EntrepriseEmployeuse)),
            Times.Once);
        etablissementFicheServiceMock.Verify(
            x => x.Create(It.Is<EtablissementFicheDto>(dto => dto.Siret == "20001759800034"
                                                              && dto.CodePostal == "59290" && dto.Ville == "WASQUEHAL" 
                                                              && dto.CodeNaf == "36.00Z" 
                                                              && dto.LibelleCodeNaf == "Captage, traitement et distribution d'eau"
                                                              && dto.Diffusable
                                                              && dto.AdresseLigne1 == "23 AV DE LA MARNE"
                                                              && dto.Ville == "WASQUEHAL"
                                                              && dto.CodePostal == "59290")),
            Times.Once);
    }
}
