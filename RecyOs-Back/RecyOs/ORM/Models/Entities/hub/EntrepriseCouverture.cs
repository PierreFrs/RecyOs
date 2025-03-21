using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace RecyOs.ORM.Entities.hub;

public class EntrepriseCouverture: BaseEntity
{
    [Column("cover_id")]
    [MaxLength(50)]
    [Required]
    public string CoverId { get; set; }
    
    [Column("numero_contrat_primaire")]
    [MaxLength(10)]
    [Required]
    public string NumeroContratPrimaire { get; set; }
    
    [Column("type_garantie")]
    [MaxLength(7)]
    [Required]
    public string TypeGarantie { get; set; }
    
    [Column("eh_id")]
    [MaxLength(13)]
    [Required]
    public string EhId { get; set; }
    
    [Column("raison_sociale")]
    [MaxLength(255)]
    [Required]
    public string RaisonSociale { get; set; }
    
    [Column("type_reponse")]
    [MaxLength(18)]
    [Required]
    public string TypeReponse { get; set; }
    
    [Column("date_decision")]
    [Required]
    public DateTime DateDecision { get; set; }
    
    [Column("date_demande")]
    [Required]
    public DateTime DateDemande { get; set; }
    
    [Column("montant_garantie")]
    [Required]
    public int MontantGarantie { get; set; }
    
    [Column("devise_garantie")]
    [MaxLength(3)]
    [Required]
    public string DeviseGarantie { get; set; }
    
    [Column("decision")]
    [MaxLength(255)]
    [Required]
    public string Decision { get; set; }
    [Column("type_identifiant")]
    [MaxLength(10)]
    [Required]
    public string TypeIdentifiant { get; set; }
    
    [Column("siren")]
    [MaxLength(9)]
    [Required]
    public string Siren { get; set; }
    
    [Column("statut_entreprise")]
    [Required]
    [MaxLength(7)]
    public string StatutEntreprise { get; set; }
    
    [Column("nom_rue")]
    [Required]
    [MaxLength(50)]
    public string NomRue { get; set; }
    
    [Column("code_postal")]
    [Required]
    [MaxLength(5)]
    public int CodePostal { get; set; }
    
    [Column("ville")]
    [Required]
    [MaxLength(50)]
    public string Ville { get; set; }
    
    [Column("pays")]
    [Required]
    [MaxLength(2)]
    public string Pays { get; set; }
    
    [Column("date_extraction")]
    [Required]
    public DateTime DateExtraction { get; set; }
    
    [Column("reprise_garantie_possible")]
    [MaxLength(3)]
    [Required]
    public string RepriseGarantiePossible { get; set; }
    
    #nullable enable
    
    [Column("numero_contrat_extension")]
    [MaxLength(10)]
    public string? NumeroContratExtension { get; set; }
    
    [Column("reference_client")]
    [MaxLength(50)]
    public string? ReferenceClient { get; set; }
    
    [Column("notation")]
    [MaxLength(2)]
    public string? Notation { get; set; }
    
    [Column("date_attribution_notation")]
    public DateTime? DateAttributionNotation { get; set; }
    
    [Column("motif_decision")]
    [MaxLength(8192)]
    public string? MotifDecision { get; set; }
    
    [Column("notre_commentaire")]
    [MaxLength(8192)]
    public string? NotreCommentaire { get; set; }
    
    [Column("commentaire_arbitre")]
    [MaxLength(8192)]
    public string? CommentaireArbitre { get; set; }
    
    [Column("quotite_garantie")]
    [MaxLength(10)]
    public string? QuotiteGarantie { get; set; }
    
    [Column("delai_paiement_specifique")]
    public int? DelaiPaiementSpecifique { get; set; }
    
    [Column("date_effet_differe")]
    public DateTime? DateEffetDiffere { get; set; }
    
    [Column("date_expiration_garantie")]
    public DateTime? DateExpirationGarantie { get; set; }
    
    [Column("montant_temporaire")]
    public int? MontantTemporaire { get; set; }
    
    [Column("devise_montant_temporaire")]
    [MaxLength(3)]
    public string? DeviseMontantTemporaire { get; set; }
    
    [Column("date_expiration_montant_temporaire")]
    public DateTime? DateExpirationMontantTemporaire { get; set; }
    
    [Column("quotite_garantie_montant_temporaire")]
    [MaxLength(10)]
    public string? QuotiteGarantieMontantTemporaire { get; set; }
    
    [Column("delai_paiement_montant_temporaire")]
    public int? DelaiPaiementMontantTemporaire { get; set; }
    
    [Column("montant_demande", TypeName = "decimal(10,2)")]
    public decimal? MontantDemande { get; set; }
    
    [Column("devise_demandee")]
    [MaxLength(3)]
    public string? DeviseDemandee { get; set; }
    
    [Column("conditions_paiement_demandees")]
    [MaxLength(255)]
    public string? ConditionsPaiementDemandees { get; set; }
    
    [Column("date_expiration_demandee")]
    public DateTime? DateExpirationDemandee { get; set; }
    
    [Column("montant_temporaire_demande")]
    public int? MontantTemporaireDemande { get; set; }
    
    [Column("numero_demande")]
    [MaxLength(10)]
    public string? NumeroDemande { get; set; }
    
    [Column("id_demande")]
    public int? IdDemande { get; set; }
    
    [Column("heure_reponse")]
    public string? HeureReponse { get; set; }
    
    [Column("temps_reponse")]
    public string? TempsReponse { get; set; }
    
    [Column("demandeur")]
    [MaxLength(9)]
    public int? Demandeur { get; set; }
    
    [Column("date_demande_montant_temporaire")]
    public DateTime? DateDemandeMontantTemporaire { get; set; }
    
    [Column("etat_region_pays")]
    [MaxLength(50)]
    public string? EtatRegionPays { get; set; }
    
    [Column("conditions_specifiques")]
    [MaxLength(255)]
    public string? ConditionsSpecifiques { get; set; }
    
    [Column("autres_conditions_1")]
    [MaxLength(255)]
    public string? AutresConditions1 { get; set; }
    
    [Column("autres_conditions_2")]
    [MaxLength(255)]
    public string? AutresConditions2 { get; set; }
    
    [Column("autres_conditions_3")]
    [MaxLength(255)]
    public string? AutresConditions3 { get; set; }
    
    [Column("autres_conditions_4")]
    [MaxLength(255)]
    public string? AutresConditions4 { get; set; }
    
    [Column("autres_conditions_temporaires")]
    [MaxLength(255)]
    public string? AutresConditionsTemporaires { get; set; }
    
    [Column("date_reprise_garantie_possible")]
    public DateTime? DateRepriseGarantiePossible { get; set; }
    
    [Column("cover_group_role")]
    public string? CoverGroupRole { get; set; }
    
    [Column("cover_group_id")]
    public string? CoverGroupId { get; set; }
    
    #nullable disable
    public virtual EntrepriseBase EntrepriseBase { get; set; }
}