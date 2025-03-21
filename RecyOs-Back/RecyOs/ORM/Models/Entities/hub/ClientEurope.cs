/** EtablissementClient.cs - Définition du modèle ClientEurope
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 20/09/2023
 * Fichier Modifié le : 20/09/2023
 * Code développé pour le projet : RecOS.ClientEurope
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Entities.hub;

public class ClientEurope: TrackedEntity, ICommercialClient
{
    [Column("vat_number")]
    [MaxLength(14)]
    [Required]
    public string Vat { get; set; }
    
    public string Identifiant { get; }
    
  #nullable enable
    [Column("nom")]
    [MaxLength(255)]
    public string? Nom { get; set; }
    
    [Column("id_odoo")]
    [MaxLength(10)]
    public string? IdOdoo { get; set; }

    [Column("code_kerlog")]
    [MaxLength(8)]
    public string? CodeKerlog { get; set; }

    [Column("code_mkgt")]
    [MaxLength(13)]
    public string? CodeMkgt { get; set; }
    
    [Column("code_gpi")]
    [MaxLength(10)]
    public string? CodeGpi { get; set; }
    
    [Column("frn_code_gpi")]
    [MaxLength(10)]
    public string? FrnCodeGpi { get; set; }
    
    [Column("contact_facturation")]
    [MaxLength(255)]
    public string? ContactFacturation { get; set; }
    
    [Column("email_facturation")]
    [MaxLength(255)]
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
    public string? EmailAlternatif { get; set; }
    
    [Column("telephone_alternatif")]
    [MaxLength(30)]
    public string? TelephoneAlternatif { get; set; }
    
    [Column("portable_alternatif")]
    [MaxLength(30)]
    public string? PortableAlternatif { get; set; } 
    
    [Column("adresse_facturation_1")]
    [MaxLength(255)]
    public string? AdresseFacturation1 { get; set; }
    
    [Column("adresse_facturation_2")]
    [MaxLength(255)]
    public string? AdresseFacturation2 { get; set; }
    
    [Column("adresse_facturation_3")]
    [MaxLength(255)]
    public string? AdresseFacturation3 { get; set; }
    
    [Column("code_postal_facturation")]
    [MaxLength(50)]
    public string? CodePostalFacturation { get; set; }
    
    [Column("ville_facturation")]
    [MaxLength(255)]
    public string? VilleFacturation { get; set; }
    
    [Column("pays_facturation")]
    [MaxLength(255)]
    public string? PaysFacturation { get; set; }
    
    [Column("condition_reglement")]
    public int? ConditionReglement { get; set; }
    
    [Column("mode_reglement")]
    public int? ModeReglement { get; set; }
    
    [Column("delai_reglement")]
    public int? DelaiReglement { get; set; }
    
    [Column("taux_tva", TypeName = "decimal(4,2)")]
    public decimal? TauxTva { get; set; }
    
    [Column("encours_max")]
    public int? EncoursMax { get; set; }
    
    [Column("compte_comptable")]
    [MaxLength(25)]
    public string? CompteComptable { get; set; }
    
    [Column("frn_condition_reglement")]
    public int? FrnConditionReglement { get; set; }
    
    [Column("frn_mode_reglement")]
    public int? FrnModeReglement { get; set; }
    
    [Column("frn_delai_reglement")]
    public int? FrnDelaiReglement { get; set; }
    
    [Column("frn_taux_tva", TypeName = "decimal(4,2)")]
    public decimal? FrnTauxTva { get; set; }
    
    [Column("frn_encours_max")]
    public int? FrnEncoursMax { get; set; }
    
    [Column("frn_compte_comptable")]
    [MaxLength(25)]
    public string? FrnCompteComptable { get; set; }
    
    [Column("iban")]
    [MaxLength(34)]
    public string? Iban { get; set; }
    
    [Column("bic")]
    [MaxLength(11)]
    public string? Bic { get; set; }
    
    [Column("client_bloque")]
    public bool? ClientBloque { get; set; }
    
    [Column("motif_blocage")]
    [MaxLength(255)]
    public string? MotifBlocage { get; set; }
    
    [Column("date_blocage")]
    public DateTime? DateBlocage { get; set; }
    
    [Column("date_cre_mkgt")]
    public DateTime? DateCreMkgt { get; set; }

    [Column("date_cre_odoo")]
    public DateTime? DateCreOdoo { get; set; }
    
    [Column("date_cre_gpi")]
    public DateTime? DateCreGpi { get; set; }
    
    [Column("radie")]
    public bool? Radie { get; set; }
    
    [Column("CategorieId")]
    [ForeignKey("CategorieClient")]
    public int? CategorieId { get; set; }
    
    [Column("id_hubspot")]
    public string? IdHubspot { get; set; }
    
    [Column("id_dashdoc")]
    public int? IdDashdoc { get; set; }
    
    [Column("id_shipper_dashdoc")]
    public int? IdShipperDashdoc { get; set; }
    
    [Column("date_cre_dashdoc")]
    public DateTime? DateCreDashdoc { get; set; }

    [Column("no_balance")]
    public bool? NoBalance { get; set; }

    #nullable disable
    [Column("client")]
    [Required]
    public bool Client { get; set; }
    
    [Column("fournisseur")]
    [Required]
    public bool Fournisseur { get; set; }
    
    
    [Column("Documents")]
    public virtual ICollection<DocumentPdfEurope> DocumentPdfEuropes { get; set; }
    
    public virtual CategorieClient CategorieClient { get; set; }
    public virtual ICollection<ClientEuropeBusinessUnit> ClientEuropeBusinessUnits { get; set; }
    
    public virtual ICollection<FactorClientEuropeBu> FactorClientEuropeBus { get; set; }
    public virtual ICollection<BalanceEurope> BalanceEuropes { get; set; }
    
    public int? CommercialId { get; set; }
    [ForeignKey("CommercialId")]
    public virtual Commercial Commercial { get; set; }

    [Column("GroupId")]
    [ForeignKey("Group")]
    public int? GroupId { get; set; }
    public virtual Group Group { get; set; }
}
