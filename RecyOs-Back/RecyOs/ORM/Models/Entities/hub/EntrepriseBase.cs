/** EntrepriseBase.cs - Définition du modèle article
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 20/03/2023
 * Fichier Modifié le : 20/03/2023
 * Code développé pour le projet : RecOS.EntrepriseBase
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RecyOs.ORM.Entities.hub;

public class EntrepriseBase : TrackedEntity
{ 
 [Column("siren")]
 [MaxLength(9)]
 [Required]
 public string Siren { get; set; }
 
 #nullable enable
 [Column("siren_formate")]
 [MaxLength(255)]
 public string? SirenFormate  { get; set; }
 
 [Column("nom_entreprise")]
 [MaxLength(255)]
 public string? NomEntreprise { get; set; }
 
[Column("personne_morale")]
public bool? PersonneMorale { get; set; }

[Column("denomination")]
[MaxLength(255)]
public string? Denomination { get; set; }

[Column("nom")]
[MaxLength(255)]
public string? Nom { get; set; }

[Column("prenom")]
[MaxLength(255)]
public string? Prenom { get; set; }

[Column("sexe")]
[MaxLength(1)]
public string? Sexe { get; set; }

[Column("code_naf")]
[MaxLength(10)]
public string? CodeNaf { get; set; }

[Column("libelle_code_naf")]
[MaxLength(255)]
public string? LibelleCodeNaf { get; set; }

[Column("domaine_activite")]
[MaxLength(255)]
public string? DomaineActivite { get; set; }

[Column("date_creation")]
public DateTime? DateCreation { get; set; }

[Column("date_creation_formate")]
[MaxLength(10)]
public string? DateCreationFormate { get; set; }

[Column("entreprise_cessee")]
public bool? EntrepriseCessee { get; set; }

[Column("date_cessation")]
public DateTime? DateCessation { get; set; }

[Column("entreprise_employeuse")]
public bool? EntrepriseEmployeuse { get; set; }

[Column("categorie_juridique")]
[MaxLength(4)]
public string? CategorieJuridique { get; set; }

[Column("forme_juridique")]
[MaxLength(255)]
public string? FormeJuridique { get; set; }

[Column("effectif")]
[MaxLength(255)]
public string? Effectif { get; set; }

[Column("effectif_min")]
public int? EffectifMin { get; set; }

[Column("effectif_max")]
public int? EffectifMax { get; set; }

[Column("tranche_effectif")]
[MaxLength(2)]
public string? TrancheEffectif { get; set; }

[Column("annee_effectif")]
public int? AnneeEffectif { get; set; }

[Column("capital", TypeName = "decimal(18,2)")]
public decimal? Capital { get; set; }

[Column("statut_rcs")]
[MaxLength(255)]
public string? StatutRcs { get; set; }

[Column("greffe")]
[MaxLength(255)]
public string? Greffe { get; set; }

[Column("code_greffe")]
[MaxLength(255)]
public string? CodeGreffe { get; set; }

[Column("numero_rcs")]
[MaxLength(255)]
public string? NumeroRcs { get; set; }

[Column("date_immatriculation_rcs")]
[MaxLength(255)]
public string? DateImmatriculationRcs { get; set; }

[Column("numero_tva_intracommunautaire")]
[MaxLength(255)]
public string? NumeroTvaIntracommunautaire { get; set; }

[Column("date_radiation_rcs")]
[MaxLength(255)]
public string? DateRadiationRcs { get; set; }

[Column("date_debut_activite")]
[MaxLength(255)]
public string? DateDebutActivite { get; set; }

[Column("objet_social")]
[MaxLength(4096)]
public string? ObjetSocial { get; set; }

[Column("ndcover_error")]
public int? NDCoverError { get; set; }

public virtual ICollection<EtablissementClient?>? EtablissementClient { get; set; }
public virtual EntrepriseCouverture? EntrepriseCouverture { get; set; }
public virtual EntrepriseNDCover? EntrepriseNDCover { get; set; }
#nullable disable
}
