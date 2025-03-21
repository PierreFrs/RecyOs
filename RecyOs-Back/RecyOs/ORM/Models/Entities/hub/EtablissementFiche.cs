/** EtablissementFiche.cs - Définition du modèle article
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 20/03/2023
 * Fichier Modifié le : 20/03/2023
 * Code développé pour le projet : RecOS.EtablissementFiche
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RecyOs.ORM.Entities.hub;

public class EtablissementFiche : TrackedEntity
{
    [Column("siret")]
    [MaxLength(14)]
    [Required]
    public string Siret { get; set; }

    #nullable enable
    [Column("siret_formate")]
    [MaxLength(18)]
    public string? SiretFormate { get; set; }

    [Column("nic")]
    [MaxLength(5)]
    public string? Nic { get; set; }

    [Column("code_postal")]
    [MaxLength(10)]
    public string? CodePostal { get; set; }

    [Column("ville")]
    [MaxLength(255)]
    public string? Ville { get; set; }

    [Column("pays")]
    [MaxLength(255)]
    public string? Pays { get; set; }

    [Column("code_pays")]
    [MaxLength(10)]
    public string? CodePays { get; set; }

    [Column("latitude")]
    public float? Latitude { get; set; }

    [Column("longitude")]
    public float? Longitude { get; set; }

    [Column("etablissement_cesse")]
    public bool? EtablissementCesse { get; set; }

    [Column("siege")]
    public bool? Siege { get; set; }

    [Column("etablissement_employeur")]
    public bool? EtablissementEmployeur { get; set; }

    [Column("effectif")]
    [MaxLength(50)]
    public string? Effectif { get; set; }

    [Column("effectif_min")]
    public int? EffectifMin { get; set; }

    [Column("effectif_max")]
    public int? EffectifMax { get; set; }

    [Column("tranche_effectif")]
    [MaxLength(10)]
    public string? TrancheEffectif { get; set; }

    [Column("annee_effectif")]
    public int? AnneeEffectif { get; set; }

    [Column("code_naf")]
    [MaxLength(10)]
    public string? CodeNaf { get; set; }

    [Column("libelle_code_naf")]
    [MaxLength(255)]
    public string? LibelleCodeNaf { get; set; }

    [Column("date_de_creation")]
    public DateTime? DateDeCreation { get; set; }

    [Column("numero_voie")]
    public int? NumeroVoie { get; set; }

    [Column("indice_repetition")]
    [MaxLength(5)]
    public string? IndiceRepetition { get; set; }

    [Column("type_voie")]
    [MaxLength(50)]
    public string? TypeVoie { get; set; }

    [Column("libelle_voie")]
    [MaxLength(255)]
    public string? LibelleVoie { get; set; }

    [Column("complement_adresse")]
    [MaxLength(255)]
    public string? ComplementAdresse { get; set; }

    [Column("adresse_ligne_1")]
    [MaxLength(255)]
    public string? AdresseLigne1 { get; set; }

    [Column("adresse_ligne_2")]
    [MaxLength(255)]
    public string? AdresseLigne2 { get; set; }

    [Column("date_cessation")]
    public DateTime? DateCessation { get; set; }

    [Column("enseigne")]
    [MaxLength(255)]
    public string? Enseigne { get; set; }

    [Column("nom_commercial")]
    [MaxLength(255)]
    public string? NomCommercial { get; set; }
    
    [Column("nom_domiciliation")]
    [MaxLength(255)]
    public string? NomDomiciliation { get; set; }

    [Column("siren_domiciliation")]
    [MaxLength(14)]
    public string? SirenDomiciliation { get; set; }

    [Column("predecesseurs")]
    [MaxLength]
    public string? Predecesseurs { get; set; }

    [Column("successeurs")]
    [MaxLength]
    public string? Successeurs { get; set; }
    
    public virtual EtablissementClient? EtablissementClient { get; set; }
    
    #nullable disable
}
