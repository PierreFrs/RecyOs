//  ImportCouverture.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NLog;
using RecyOs.Controllers;
using RecyOs.Helpers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.Commands;

public class ImportCouverture : ICommandImportCouverture
{
    protected readonly IEntrepriseCouvertureService _entrepriseCouvertureService;
    protected readonly IEntrepriseBaseService _entrepriseBaseService;
    private readonly ILogger<ImportCouverture> _logger;

    public ImportCouverture(
        IEntrepriseCouvertureService entrepriseCouvertureService,
        IEntrepriseBaseService entrepriseBaseService, 
        ILogger<ImportCouverture> logger)
    {
        _entrepriseCouvertureService = entrepriseCouvertureService;
        _entrepriseBaseService = entrepriseBaseService;
        _logger = logger;
    }
    
    private static readonly string[] RequiredColumns = new[]
    {
        "coverID", 
        "Numéro du contrat primaire", 
        "Type de garantie", 
        "EH ID", 
        "Raison sociale", 
        "Type de réponse", 
        "Date de décision", 
        "Date de la demande", 
        "Montant de la garantie", 
        "Devise de la garantie", 
        "Décision", 
        "Type d'identifiant", 
        "Identifiant", 
        "Statut de l'entreprise", 
        "Nom de rue", 
        "Code postal", 
        "Ville", 
        "Pays", 
        "Date de l'extraction", 
        "Reprise de garantie possible"
    };

    private static readonly string[] OptionalColumns = new[]
    {
        "Numéro d'extension", 
        "Référence client", 
        "Notation", 
        "Date d'attribution de la notation", 
        "Motif de la décision", 
        "Notre commentaire", 
        "Commentaire de l'arbitre", 
        "Quotité de garantie", 
        "Délai de paiement spécifique", 
        "Date de l'effet différé", 
        "Date d'expiration de la garantie", 
        "Montant temporaire", 
        "Devise", 
        "Date d'expiration du montant temporaire", 
        "Quotité de garantie du montant temporaire", 
        "Délai de paiement du montant temporaire", 
        "Montant demandé", 
        "Devise demandée", 
        "Conditions de paiement demandées", 
        "Date d'expiration demandée", 
        "Montant temporaire demandé", 
        "Numéro de la demande", 
        "N° ID de la demande", 
        "Heure de la réponse", 
        "Temps de réponse", 
        "Demandé par", 
        "Date de la demande de montant temporaire", 
        "Etat/Région/Pays", 
        "Conditions spécifiques", 
        "Autres conditions 1", 
        "Autres conditions 2", 
        "Autres conditions 3", 
        "Autres conditions 4", 
        "Autres conditions temporaires", 
        "Date de la reprise de garantie possible", 
        "Cover group role", 
        "Cover group ID"
    };

    private static readonly HashSet<string> AllColumns = new HashSet<string>(RequiredColumns.Concat(OptionalColumns));


#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
    /// <summary>
    /// Importer les données de couverture à partir d'un fichier Excel
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
                        EntrepriseCouvertureDto entrepriseCouvertureDto = new EntrepriseCouvertureDto
                        {
                            CoverId = reader.GetString(columns, "coverID"),
                            NumeroContratPrimaire = reader.GetString(columns, "Numéro du contrat primaire"),
                            NumeroContratExtension = reader.GetNullableString(columns, "Numéro d'extension"),
                            TypeGarantie = reader.GetString(columns, "Type de garantie"),
                            EhId = reader.GetString(columns, "EH ID"),
                            RaisonSociale = reader.GetString(columns, "Raison sociale"),
                            ReferenceClient = reader.GetNullableString(columns, "Référence client"),
                            Notation = string.IsNullOrWhiteSpace(reader.GetNullableString(columns, "Notation")) ? "NA" : reader.GetString(columns, "Notation"),
                            DateAttributionNotation = reader.GetNullableDateTime(columns, "Date d'attribution de la notation"),
                            TypeReponse = reader.GetString(columns, "Type de réponse"),
                            DateDecision = reader.GetDateTime(columns, "Date de décision"),
                            DateDemande = reader.GetDateTime(columns, "Date de la demande"),
                            MontantGarantie = reader.GetInt(columns, "Montant de la garantie"),
                            DeviseGarantie = reader.GetString(columns, "Devise de la garantie"),
                            Decision = reader.GetString(columns, "Décision"),
                            MotifDecision = reader.GetNullableString(columns, "Motif de la décision"),
                            NotreCommentaire = reader.GetNullableString(columns, "Notre commentaire"),
                            CommentaireArbitre = reader.GetNullableString(columns, "Commentaire de l'arbitre"),
                            QuotiteGarantie = reader.GetNullableString(columns, "Quotité de garantie"),
                            DelaiPaiementSpecifique = reader.GetNullableInt(columns, "Délai de paiement spécifique"),
                            DateEffetDiffere = reader.GetNullableDateTime(columns, "Date de l'effet différé"),
                            DateExpirationGarantie = reader.GetNullableDateTime(columns, "Date d'expiration de la garantie"),
                            MontantTemporaire = reader.GetNullableInt(columns, "Montant temporaire"),
                            DeviseMontantTemporaire = reader.GetNullableString(columns, "Devise"),
                            DateExpirationMontantTemporaire = reader.GetNullableDateTime(columns, "Date d'expiration du montant temporaire"),
                            QuotiteGarantieMontantTemporaire = reader.GetNullableString(columns, "Quotité de garantie du montant temporaire"),
                            DelaiPaiementMontantTemporaire = reader.GetNullableInt(columns, "Délai de paiement du montant temporaire"),
                            MontantDemande = reader.GetNullableDecimal(columns, "Montant demandé"),
                            DeviseDemandee = reader.GetNullableString(columns, "Devise demandée"),
                            ConditionsPaiementDemandees = reader.GetNullableString(columns, "Conditions de paiement demandées"),
                            DateExpirationDemandee = reader.GetNullableDateTime(columns, "Date d'expiration demandée"),
                            MontantTemporaireDemande = reader.GetNullableInt(columns, "Montant temporaire demandé"),
                            NumeroDemande = reader.GetNullableString(columns, "Numéro de la demande"),
                            IdDemande = reader.GetNullableInt(columns, "N° ID de la demande"),
                            HeureReponse = reader.GetNullableString(columns, "Heure de la réponse"),
                            TempsReponse = reader.GetNullableString(columns, "Temps de réponse"),
                            Demandeur = reader.GetNullableInt(columns, "Demandé par"),
                            DateDemandeMontantTemporaire = reader.GetNullableDateTime(columns, "Date de la demande de montant temporaire"),
                            TypeIdentifiant = reader.GetString(columns, "Type d'identifiant"),
                            Siren = reader.GetString(columns, "Identifiant"),
                            StatutEntreprise = reader.GetString(columns, "Statut de l'entreprise"),
                            NomRue = reader.GetString(columns, "Nom de rue"),
                            CodePostal = reader.GetInt(columns, "Code postal"),
                            Ville = reader.GetString(columns, "Ville"),
                            EtatRegionPays = reader.GetNullableString(columns, "Etat/Région/Pays"),
                            Pays = reader.GetString(columns, "Pays"),
                            ConditionsSpecifiques = reader.GetNullableString(columns, "Conditions spécifiques"),
                            AutresConditions1 = reader.GetNullableString(columns, "Autres conditions 1"),
                            AutresConditions2 = reader.GetNullableString(columns, "Autres conditions 2"),
                            AutresConditions3 = reader.GetNullableString(columns, "Autres conditions 3"),
                            AutresConditions4 = reader.GetNullableString(columns, "Autres conditions 4"),
                            AutresConditionsTemporaires = reader.GetNullableString(columns, "Autres conditions temporaires"),
                            DateExtraction = reader.GetDateTime(columns, "Date de l'extraction"),
                            RepriseGarantiePossible = reader.GetString(columns, "Reprise de garantie possible"),
                            DateRepriseGarantiePossible = reader.GetNullableDateTime(columns, "Date de la reprise de garantie possible"),
                            CoverGroupRole = reader.GetNullableString(columns, "Cover group role"),
                            CoverGroupId = reader.GetNullableString(columns, "Cover group ID")
                        };
                        
                        // Vérifier que l'entreprise existe dans entreprise_base
                        EntrepriseBaseDto entrepriseBaseDto = await _entrepriseBaseService.GetBySiren(entrepriseCouvertureDto.Siren);
                        if (entrepriseBaseDto == null)
                        {
                            _logger.LogError("L'entreprise avec le SIREN {0} n'existe pas dans la base de données", entrepriseCouvertureDto.Siren);
                        }
                        else
                        {
                            await _entrepriseCouvertureService.Create(
                                entrepriseCouvertureDto); // La méthode Create met à jour si l'entreprise existe déjà
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
                if (!reader.Read()) return false;

                Dictionary<string, int> columns = new Dictionary<string, int>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetValue(i).ToString();
                    if (!columns.ContainsKey(columnName))
                        columns.Add(columnName, i);
                }

                foreach (var column in AllColumns)
                {
                    if (!columns.ContainsKey(column))
                        return false;
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
                    return false;
            }
        }
        return true;
    }

    private bool IsColumnDataValid(IExcelDataReader reader, Dictionary<string, int> columns, string columnName)
    {
        var value = reader.GetValue(columns[columnName])?.ToString();
        bool isMandatory = RequiredColumns.Contains(columnName);

        if (isMandatory && string.IsNullOrEmpty(value))
            return false;

        switch (columnName)
        {
            case "Date de décision":
            case "Date de la demande":
            case "Date d'attribution de la notation":
            case "Date de l'effet différé":
            case "Date d'expiration de la garantie":
            case "Date d'expiration du montant temporaire":
            case "Date d'expiration demandée":
            case "Date de la demande de montant temporaire":
            case "Date de la reprise de garantie possible":
                return isMandatory ? DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) : true;

            case "Date de l'extraction":
                return isMandatory ? DateTime.TryParseExact(value, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _) : true;
            
            case "Montant de la garantie":
            case "Montant temporaire":
            case "Montant demandé":
            case "Montant temporaire demandé":
                return isMandatory ? int.TryParse(value, out _) : true;

            case "Heure de la réponse":
            case "Temps de réponse":
                return isMandatory ? TimeSpan.TryParseExact(value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out _) : true;

            case "Délai de paiement spécifique":
            case "Délai de paiement du montant temporaire":
            case "N° ID de la demande":
            case "Demandé par":
                return isMandatory ? int.TryParse(value, out _) : true;

            case "Code postal":
                return isMandatory ? int.TryParse(value, out _) : true;

            default:
                return true;
        }
    }


#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
}