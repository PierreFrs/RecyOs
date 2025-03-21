// Created by : Pierre FRAISSE
// RecyOs => RecyOs => ImportNDCover.cs
// Created : 2023/12/19 - 11:08
// Updated : 2023/12/19 - 11:09

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyOs.Controllers;
using RecyOs.Helpers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.Commands;

public class ImportNDCover : ICommandImportNDCover
{
    protected readonly IEntrepriseNDCoverService _entrepriseNDCoverService;
    protected readonly IEntrepriseBaseService _entrepriseBaseService;
    private readonly IEtablissementClientService _etablissementService;
    private readonly IEtablissementFicheService _etablissementFicheService;
    private readonly ICommandExportSoumissionNDCoverRepository<EntrepriseBase> _commandExportSoumissionNDCoverRepository;
    private readonly INotificationRepository<Notification> _notificationRepository;
    private readonly ILogger<ImportNDCover> _logger;

    public ImportNDCover(
        IEntrepriseNDCoverService entrepriseNDCoverService,
        IEntrepriseBaseService entrepriseBaseService,
        IEtablissementClientService etablissementService,
        IEtablissementFicheService etablissementFicheService,
        ICommandExportSoumissionNDCoverRepository<EntrepriseBase> commandExportSoumissionNDCoverRepository,
        INotificationRepository<Notification> notificationRepository,
        ILogger<ImportNDCover> logger)
    {
        _entrepriseNDCoverService = entrepriseNDCoverService;
        _entrepriseBaseService = entrepriseBaseService;
        _etablissementService = etablissementService;
        _etablissementFicheService = etablissementFicheService;
        _commandExportSoumissionNDCoverRepository = commandExportSoumissionNDCoverRepository;
        _notificationRepository = notificationRepository;
        _logger = logger;
    }
    
    private static readonly string[] RequiredColumns = new[]
    {
        "CoverID", 
        "Numéro du contrat primaire",
        "Nom de la police", 
        "EH ID", 
        "Raison sociale", 
        "Forme juridique / code", 
        "Secteur d'activité", 
        "Type d'identifiant", 
        "Identifiant", 
        "Statut de l'entreprise",
        "Statut", 
        "Temps de réponse", 
        "Date de changement de position",
        "Période de renouvellement ouverte (oui/non)",
        "Nom de rue", 
        "Code postal", 
        "Ville", 
        "Pays", 
        "Date de l'extraction"
    };
    
    private static readonly string[] OptionalColumns = new[]
    {
        "Numéro d'extension",
        "Référence client", 
        "Date de la suppression", 
        "Sera renouvelé (oui/non)", 
        "Date de renouvellement prévue", 
        "Date d'expiration", 
    };
    
    private static readonly HashSet<string> AllColumns = new HashSet<string>(RequiredColumns.Concat(OptionalColumns));

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
    /// <summary>
    /// Importer les données ND Cover à partir d'un fichier Excel
    /// Les met à jour si elles existent déjà
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<bool> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            using (var reader = ExcelReaderFactory.CreateReader(memoryStream))
            {
                var headerRow = reader.Read();
                if (!headerRow) return false;

                // Créer un dictionaire pour les indices des colonnes
                Dictionary<string, int> columns = new Dictionary<string, int>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetValue(i).ToString();

                    // Add the column only if it doesn't exist in the dictionary
                    if (!columns.ContainsKey(columnName))
                    {
                        columns[columnName] = i;
                    }
                }
                
                // Lire les lignes
                while (reader.Read())
                {
                    try
                    {
                        EntrepriseNDCoverDto entrepriseNDCoverDto = new EntrepriseNDCoverDto
                        {
                            CoverId = reader.GetString(columns, "CoverID"),
                            NumeroContratPrimaire = reader.GetString(columns, "Numéro du contrat primaire"),
                            NumeroContratExtension = reader.GetNullableString(columns, "Numéro d'extension"),
                            NomPolice = reader.GetString(columns, "Nom de la police"),
                            EhId = reader.GetString(columns, "EH ID"),
                            RaisonSociale = reader.GetString(columns, "Raison sociale"),
                            FormeJuridiqueCode = reader.GetString(columns, "Forme juridique / code"),
                            SecteurActivite = reader.GetString(columns, "Secteur d'activité"),
                            TypeIdentifiant = reader.GetString(columns, "Type d'identifiant"),
                            Siren = reader.GetString(columns, "Identifiant"),
                            StatutEntreprise = reader.GetString(columns, "Statut de l'entreprise"),
                            ReferenceClient = reader.GetNullableString(columns, "Référence client"),
                            Statut = reader.GetString(columns, "Statut"),
                            TempsReponse = reader.GetString(columns, "Temps de réponse"),
                            DateChangementPosition = reader.GetDateTime(columns, "Date de changement de position"),
                            DateSuppression = reader.GetNullableDateTime(columns, "Date de la suppression"),
                            DateDemande = reader.GetDateTime(columns, "Date de la demande"),
                            PeriodeRenouvellementOuverte = reader.GetString(columns, "Période de renouvellement ouverte (oui/non)"),
                            SeraRenouvele = reader.GetNullableString(columns, "Sera renouvelé (oui/non)"),
                            DateRenouvellementPrevue = reader.GetNullableDateTime(columns, "Date de renouvellement prévue"),
                            DateExpiration = reader.GetNullableDateTime(columns, "Date d'expiration"),
                            NomRue = reader.GetString(columns, "Nom de rue"),
                            CodePostal = reader.GetInt(columns, "Code postal"),
                            Ville = reader.GetString(columns, "Ville"),
                            Pays = reader.GetString(columns, "Pays"),
                            DateExtraction = reader.GetDateTime(columns, "Date de l'extraction"),
                            };
                        
                        // Vérifier que l'entreprise existe dans entreprise_base
                        EntrepriseBaseDto entrepriseBaseDto = await _entrepriseBaseService.GetBySiren(entrepriseNDCoverDto.Siren);
                        if (entrepriseBaseDto == null)
                        {
                            _logger.LogError("L'entreprise avec le SIREN {0} n'existe pas dans la base de données", entrepriseNDCoverDto.Siren);
                        }
                        else
                        {
                            await _entrepriseNDCoverService.Create(
                                entrepriseNDCoverDto); // La méthode Create met à jour si l'entreprise existe déjà
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Erreur lors de l'importation des données de couverture");
                        return false;
                    }
                }
            } 
        }
        return true;
    }

    /// <summary>
    /// Vérifier le format du fichier Excel avant importation. Verifie que les colonnes sont bien présentes
    /// Vérifie également que les lignes sont bien formatées et que les données sont valides
    /// </summary>
    /// <param name="file"></param>
    /// <returns>true si le fichier est valide</returns>
    public async Task<bool> CheckFormat(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            using (var reader = ExcelReaderFactory.CreateReader(memoryStream))
            {
                var headerRow = reader.Read();
                if (!headerRow) return false;

                // Créer un dictionaire pour les indices des colonnes
                Dictionary<string, int> columns = new Dictionary<string, int>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetValue(i).ToString();

                    if (!columns.ContainsKey(columnName))
                    {
                        columns.Add(columnName, i);
                    }
                }

                // Check if all required and optional columns are present
                foreach (var column in AllColumns)
                {
                    if (!columns.ContainsKey(column))
                    {
                        return false;
                    }
                }

                return ValidateColumnData(reader, columns);
            }
        }
    }
    private bool ValidateColumnData(IExcelDataReader reader, Dictionary<string, int> columns)
    {
        while (reader.Read())
        {
            foreach (var columnName in AllColumns)
            {
                if (!IsColumnDataValid(reader, columns, columnName))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private static bool IsColumnDataValid(IExcelDataReader reader, Dictionary<string, int> columns, string columnName)
    {
        var value = reader.GetValue(columns[columnName])?.ToString();

        // Check if the column is mandatory
        bool isMandatory = RequiredColumns.Contains(columnName);

        // General validation for null values
        if (isMandatory && string.IsNullOrEmpty(value))
            return false;

        // Specific validation based on column names
        switch (columnName)
        {
            case "Date de changement de position":
                return isMandatory ? DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) : true;

            case "Date de l'extraction":
                return isMandatory ? DateTime.TryParseExact(value, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) : true;

            case "Code postal":
                return isMandatory ? int.TryParse(value, out _) : true;

            case "Date de la suppression":
            case "Date de renouvellement prévue":
            case "Date d'expiration":
                return isMandatory ? !ImportCouvertureExtensions.IsNullOrWhiteSpaceOrInvalidDate(value) : true;

            // Add more cases as needed for other columns
            default:
                return true;
        }
    }

#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high

    public async Task ImportNdCoverErrorAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return;

        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            using (var reader = ExcelReaderFactory.CreateReader(memoryStream))
            {
                var headerRow = reader.Read();
                if (!headerRow) return;

                // Créer un dictionaire pour les indices des colonnes
                Dictionary<string, int> columns = new Dictionary<string, int>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetValue(i)?.ToString();
                    if (!string.IsNullOrEmpty(columnName) && !columns.ContainsKey(columnName))
                    {
                        columns[columnName] = i;
                    }
                }

                // Vérification que les colonnes requises existent
                if (!columns.ContainsKey("Line") ||
                    !columns.ContainsKey("CoverId") ||
                    !columns.ContainsKey("Code") ||
                    !columns.ContainsKey("Error"))
                {
                    throw new Exception(
                        "Le fichier Excel ne contient pas toutes les colonnes requises : Line, CoverId, Code, Error.");
                }

                while (reader.Read())
                {
                    try
                    {
                        int lineNumber = Convert.ToInt32(reader.GetValue(columns["Line"]));
                        int errorCode = Convert.ToInt32(reader.GetValue(columns["Code"]));
                        string errorMessage = reader.GetValue(columns["Error"])?.ToString();

                        // Déterminer le SIREN en fonction de l'erreur
                        string siren = await ExtractSiren(errorCode, errorMessage, lineNumber);

                        if (string.IsNullOrEmpty(siren))
                        {
                            Console.WriteLine(
                                $"[Warning] Impossible de déterminer le SIREN pour la ligne {lineNumber} avec l'erreur {errorMessage}");
                            continue;
                        }

                        switch (errorCode)
                        {
                            case 124:
                                await UpdateNdCoverErrorAsync(siren, 124);
                                await RadierEntreprise(siren);
                                await RadierEtablissements(siren);
                                await RadierEtablissementsFiches(siren);
                                await FindCommerciauxAndSendNotification(siren, 122);
                                break;

                            case 122:
                                await UpdateNdCoverErrorAsync(siren, 122);
                                await FindCommerciauxAndSendNotification(siren, 122);
                                break;

                            case 24700:
                                Console.WriteLine($"[Info] Code d'erreur non géré : {errorCode}");
                                break;

                            case 253:
                                await FindEntrepriseGroupeAndUpdateNdCoverError(lineNumber, errorCode);
                                break;

                            case 254:
                                await FindEntrepriseGroupeAndUpdateNdCoverError(lineNumber, errorCode);
                                break;

                            default:
                                Console.WriteLine($"[Info] Code d'erreur non géré : {errorCode}");
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Extraire le SIREN à partir du message d'erreur ou en le récupérant via la ligne dans temporaryNDCoverExport.
    /// </summary>
    private async Task<string> ExtractSiren(int errorCode, string errorMessage, int lineNumber)
    {
        if (errorCode == 124 || errorCode == 122)
        {
            // Extraction du SIREN à partir du message d'erreur
            var match = Regex.Match(errorMessage, @"SIREN,\s*([\d]+),\s*FR");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
        }
        else if (errorCode == 253 || errorCode == 254)
        {
            // Récupérer le SIREN à partir de la table temporaryNDCoverExport via le numéro de ligne
            return await _commandExportSoumissionNDCoverRepository.GetSirenFromLineNumberAsync(lineNumber);
        }

        return null;
    }

    /// <summary>
    /// Met à jour la colonne NDCoverError d'une entreprise.
    /// </summary>
    private async Task UpdateNdCoverErrorAsync(string siren, int errorCode)
    {
        EntrepriseBaseDto entreprise = await _entrepriseBaseService.GetBySiren(siren);
        if (entreprise != null)
        {
            entreprise.NDCoverError = errorCode;
            await _entrepriseBaseService.Edit(entreprise);
            Console.WriteLine($"[Success] NDCoverError mis à jour pour SIREN {siren} avec le code {errorCode}");
        }
        else
        {
            Console.WriteLine($"[Error] Entreprise avec SIREN {siren} non trouvée.");
        }
    }

    /// <summary>
    /// Radie le client en mettant à jour le statut de l'entreprise.
    /// </summary>
    private async Task RadierEntreprise(string siren)
    {
        EntrepriseBaseDto entreprise = await _entrepriseBaseService.GetBySiren(siren);
        if (entreprise != null)
        {
            entreprise.EntrepriseCessee = true;
            await _entrepriseBaseService.Edit(entreprise);
            Console.WriteLine($"[Success] Entreprise radiée avec SIREN {siren}");
        }
        else
        {
            Console.WriteLine($"[Error] Entreprise avec SIREN {siren} non trouvée.");
        }
    }

    /// <summary>
    /// Radie les etablissements rattachés à l'entreprise base
    /// </summary>
    private async Task RadierEtablissements(string siren)
    {
        IEnumerable<EtablissementClientDto> etablissements = _etablissementService.GetEtablissementGroupBySirenAsync(siren).Result;
        foreach (var etablissement in etablissements)
        {
            etablissement.Radie = true;
            await _etablissementService.Edit(etablissement);
            Console.WriteLine($"[Success] Etablissement avec SIREN {siren} flagué radié");
        }
    }

    /// <summary>
    /// Radie les établissements via leur fiche.
    /// </summary>
    private async Task RadierEtablissementsFiches(string siren)
    {
        IEnumerable<EtablissementClientDto> etablissements = _etablissementService.GetEtablissementGroupBySirenAsync(siren).Result;
        foreach (var etablissement in etablissements)
        {
            EtablissementFicheDto etablissementFiche = etablissement.EtablissementFiche;
            etablissementFiche.EtablissementCesse = true;
            await _etablissementFicheService.Edit(etablissementFiche);
            Console.WriteLine($"[Success] Fiche Etablissement avec SIREN {siren} flaguée radié");
        }
    }

    /// <summary>
    /// Récupère les établissements rattachés à l'entreprise base et envoie une notification à leur commercial.
    /// </summary>
    private async Task FindCommerciauxAndSendNotification(string siren, int errorCode)
    {
        IEnumerable<EtablissementClientDto> etablissements = _etablissementService.GetEtablissementGroupBySirenAsync(siren).Result;
        IList<int> commerciauxIds = etablissements.Select(e => e.Commercial.Id).Distinct().ToList();
        if (commerciauxIds.Count <= 0)
        {
            Console.WriteLine($"[Warning] Aucun commercial trouvé pour l'entreprise avec SIREN {siren}");
        }
        else
        {
            foreach (var commercialId in commerciauxIds)
            {
                await InsertNotificationAsync(commercialId, siren, errorCode);
                Console.WriteLine($"[Success] Notification(s) envoyée(s) pour l'entreprise avec SIREN {siren}");
            }
        }
    }

    /// <summary>
    /// Insère une alerte dans la table alert pour une entreprise donnée.
    /// </summary>
    private async Task InsertNotificationAsync(int commercialId, string siren, int errorCode)
    {
        var title = errorCode switch
        {
            122 => "Entreprise inexistante",
            124 => "Entreprise cessée",
        };
        var description =  errorCode switch
        {
            122 => $"L'entreprise au Siren {siren} est inexistante selon NDCover",
            124 => $"L'entreprise au Siren {siren} est cessée selon NDCover",
        };

        Notification notification = new Notification()
        {
            Icon = "error",
            Title = title,
            Description = description,
            Time = DateTime.Now,
            UserId = commercialId
        };

        await _notificationRepository.CreateAsync(notification, new ContextSession());
    }

    /// <summary>
    /// Trouve le Siren de l'entreprise grâce au numéro de ligne présent dans le fichier d'export et met à jour le code d'erreur.
    /// </summary>
    private async Task FindEntrepriseGroupeAndUpdateNdCoverError(int lineNumber, int errorCode)
    {
        string siren = await _commandExportSoumissionNDCoverRepository.GetSirenFromLineNumberAsync(lineNumber);
        if (string.IsNullOrEmpty(siren))
        {
            Console.WriteLine($"[Warning] Impossible de déterminer le SIREN pour la ligne {lineNumber}");
            return;
        }

        await UpdateNdCoverErrorAsync(siren, errorCode);
    }
}