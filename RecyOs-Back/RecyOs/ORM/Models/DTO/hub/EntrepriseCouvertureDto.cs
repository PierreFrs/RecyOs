using System;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.DTO.hub;

public class EntrepriseCouvertureDto
{
    public int Id { get; set; }
    public string CoverId { get; set; }
    public string NumeroContratPrimaire { get; set; }
#nullable enable
    public string? NumeroContratExtension { get; set; }
    public string? ReferenceClient { get; set; }
    public string? Notation { get; set; }
    public DateTime? DateAttributionNotation { get; set; }
    public string? MotifDecision { get; set; }
    public string? NotreCommentaire { get; set; }
    public string? CommentaireArbitre { get; set; }
    public string? QuotiteGarantie { get; set; }
    public int? DelaiPaiementSpecifique { get; set; }
    public DateTime? DateEffetDiffere { get; set; }
    public DateTime? DateExpirationGarantie { get; set; }
    public int? MontantTemporaire { get; set; }
    public string? DeviseMontantTemporaire { get; set; }
    public DateTime? DateExpirationMontantTemporaire { get; set; }
    public string? QuotiteGarantieMontantTemporaire { get; set; }
    public int? DelaiPaiementMontantTemporaire { get; set; }
    public decimal? MontantDemande { get; set; }
    public string? DeviseDemandee { get; set; }
    public string? ConditionsPaiementDemandees { get; set; }
    public DateTime? DateExpirationDemandee { get; set; }
    public int? MontantTemporaireDemande { get; set; }
    public string? NumeroDemande { get; set; }
    public int? IdDemande { get; set; }
    public string? HeureReponse { get; set; }
    public string? TempsReponse { get; set; }
    public int? Demandeur { get; set; }
    public string? EtatRegionPays { get; set; }
    public DateTime? DateDemandeMontantTemporaire { get; set; }
    public DateTime? DateRepriseGarantiePossible { get; set; }
    public string? CoverGroupRole { get; set; }
    public string? CoverGroupId { get; set; }
    public string? ConditionsSpecifiques { get; set; }
    public string? AutresConditions1 { get; set; }
    public string? AutresConditions2 { get; set; }
    public string? AutresConditions3 { get; set; }
    public string? AutresConditions4 { get; set; }
    public string? AutresConditionsTemporaires { get; set; }
#nullable disable
    public string TypeGarantie { get; set; }
    public string EhId { get; set; }
    public string RaisonSociale { get; set; }
    public string TypeReponse { get; set; }
    public DateTime DateDecision { get; set; }
    public DateTime DateDemande { get; set; }
    public int MontantGarantie { get; set; }
    public string DeviseGarantie { get; set; }
    public string Decision { get; set; }
    public string TypeIdentifiant { get; set; }
    public string Siren { get; set; }
    public string StatutEntreprise { get; set; }
    public string NomRue { get; set; }
    public int CodePostal { get; set; }
    public string Ville { get; set; }
    
    public string Pays { get; set; }
    
    public DateTime DateExtraction { get; set; }
    public string RepriseGarantiePossible { get; set; }


    public EntrepriseBaseDto EntrepriseBase { get; set; }
}