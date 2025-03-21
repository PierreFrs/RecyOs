// Created by : Pierre FRAISSE
// RecyOs => RecyOs => EntrepriseNDCover.cs
// Created : 2023/12/19 - 09:32
// Updated : 2023/12/19 - 09:32

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities.hub;

public class EntrepriseNDCover: BaseEntity
{
    [Column("cover_id")]
    [MaxLength(50)]
    [Required]
    public string CoverId { get; set; }
    
    [Column("numero_contrat_primaire")]
    [MaxLength(10)]
    [Required]
    public string NumeroContratPrimaire { get; set; }
    
    [Column("nom_police")]
    [MaxLength(50)]
    [Required]
    public string NomPolice { get; set; }
    
    [Column("eh_id")]
    [MaxLength(13)]
    [Required]
    public string EhId { get; set; }
    
    [Column("raison_sociale")]
    [MaxLength(255)]
    [Required]
    public string RaisonSociale { get; set; }
    
    [Column("forme_juridique_code")]
    [MaxLength(5)]
    [Required]
    public string FormeJuridiqueCode { get; set; }
    
    [Column("secteur_activite")]
    [MaxLength(5)]
    [Required]
    public string SecteurActivite { get; set; }
    
    [Column("type_identifiant")]
    [MaxLength(10)]
    [Required]
    public string TypeIdentifiant { get; set; }
    
    [Column("siren")]
    [MaxLength(9)]
    [Required]
    public string Siren { get; set; }
    
    [Column("statut_entreprise")]
    [MaxLength(7)]
    [Required]
    public string StatutEntreprise { get; set; }
    
    [Column("statut")]
    [MaxLength(20)]
    [Required]
    public string Statut { get; set; }
    
    [Column("temps_reponse")]
    [MaxLength(20)]
    [Required]
    public string TempsReponse { get; set; }
    
    [Column("date-changement_position")]
    [Required]
    public DateTime DateChangementPosition { get; set; }
    
    [Column("periode_renouvellement_ouverte")]
    [MaxLength(3)]
    [Required]
    public string PeriodeRenouvellementOuverte { get; set; }
    
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
    
#nullable enable

    [Column("numero_contrat_extension")]
    [MaxLength(10)]
    public string? NumeroContratExtension { get; set; }
    
    [Column("reference_client")]
    [MaxLength(50)]
    public string? ReferenceClient { get; set; }
    
    [Column("date_suppression")]
    public DateTime? DateSuppression { get; set; }
    
    [Column("sera_renouvele")]
    [MaxLength(3)]
    public string? SeraRenouvele { get; set; }
    
    [Column("date_renouvellement_prevue")]
    public DateTime? DateRenouvellementPrevue { get; set; }
    
    [Column("date_expiration")]
    public DateTime? DateExpiration { get; set; }
    
#nullable disable
    
    public virtual EntrepriseBase EntrepriseBase { get; set; }
}