// ClientParticulier.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 07/10/2024
// Fichier Modifié le : 07/10/2024
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClosedXML.Excel;

namespace RecyOs.ORM.Entities.hub;

public class ClientParticulier : TrackedEntity
{
    [Required]
    [Column("nom")] [MaxLength(255)] public string Nom { get; set; }

    [Required]
    [Column("prenom")] [MaxLength(255)] public string Prenom { get; set; }

    [Required]
    [Column("titre")] [MaxLength(3)] public string Titre { get; set; }

    [Required]
    [Column("adresse_facturation_1")]
    [MaxLength(255)]
    public string AdresseFacturation1 { get; set; }

    #nullable enable
    [Column("adresse_facturation_2")]
    [MaxLength(255)]
    public string? AdresseFacturation2 { get; set; }

    [Column("adresse_facturation_3")]
    [MaxLength(255)]
    public string? AdresseFacturation3 { get; set; }
    #nullable disable

    [Required]
    [Column("code_postal_facturation")]
    [MaxLength(10)]
    public string CodePostalFacturation { get; set; }

    [Required]
    [Column("ville_facturation")]
    [MaxLength(255)]
    public string VilleFacturation { get; set; }

    [Required]
    [Column("pays_facturation")]
    [MaxLength(255)]
    public string PaysFacturation { get; set; }

#nullable enable
    [Column("email_facturation")]
    [MaxLength(255)]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string? EmailFacturation { get; set; }

    [Column("telephone_facturation")]
    [MaxLength(30)]
    public string? TelephoneFacturation { get; set; }

    [Column("portable_facturation")]
    [MaxLength(30)]
    public string? PortableFacturation { get; set; }

    [Column("contact_alternatif")]
    [MaxLength(255)]
    public string? ContactAlternatif { get; set; }

    [Column("email_alternatif")]
    [MaxLength(255)]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string? EmailAlternatif { get; set; }

    [Column("telephone_alternatif")]
    [MaxLength(30)]
    public string? TelephoneAlternatif { get; set; }

    [Column("portable_alternatif")]
    [MaxLength(30)]
    public string? PortableAlternatif { get; set; }

    [Column("condition_reglement")] public int? ConditionReglement { get; set; }

    [Column("mode_reglement")] public int? ModeReglement { get; set; }

    [Column("delai_reglement")] public int? DelaiReglement { get; set; }

    [Column("taux_tva", TypeName = "decimal(4,2)")]
    public decimal? TauxTva { get; set; }

    [Column("encours_max")] public int? EncoursMax { get; set; }

    [Column("compte_comptable")]
    [MaxLength(25)]
    public string? CompteComptable { get; set; }

    [Column("client_bloque")] public bool? ClientBloque { get; set; }

    [Column("motif_blocage")]
    [MaxLength(255)]
    public string? MotifBlocage { get; set; }

    [Column("date_blocage")] 
    public DateTime? DateBlocage { get; set; }

    [Column("code_mkgt")] [MaxLength(13)] 
    public string? CodeMkgt { get; set; }

    [Column("date_cre_mkgt")] 
    public DateTime? DateCreMkgt { get; set; }

    [Column("id_odoo")] [MaxLength(10)] 
    public string? IdOdoo { get; set; }

    [Column("date_cre_odoo")] public DateTime? DateCreOdoo { get; set; }
    [Column("id_shipper_dashdoc")]
    public int? IdShipperDashdoc { get; set; }

    [Column("no_balance")]
    public bool? NoBalance { get; set; }

    #nullable disable

    public virtual ICollection<BalanceParticulier> BalanceParticuliers { get; set; }

}