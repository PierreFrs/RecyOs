using System;
using System.Collections.Generic;
using System.Text.Json;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.DTO.hub;

public class EntrepriseBaseDto
{
    public int Id { get; set; }
    public string Siren { get; set; }
    public string SirenFormate { get; set; }
    public string NomEntreprise { get; set; }
    public bool PersonneMorale { get; set; }
    public string Denomination { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Sexe { get; set; }
    public string CodeNaf { get; set; }
    public string LibelleCodeNaf { get; set; }
    public string DomaineActivite { get; set; }
    public string DateCreation { get; set; }
    public string DateCreationFormate { get; set; }
    public bool EntrepriseCessee { get; set; }
    public string DateCessation { get; set; }
    public bool EntrepriseEmployeuse { get; set; }
    public string CategorieJuridique { get; set; }
    public string FormeJuridique { get; set; }
    public string Effectif { get; set; }
    public int? EffectifMin { get; set; }
    public int? EffectifMax { get; set; }
    public string TrancheEffectif { get; set; }
    public int? AnneeEffectif { get; set; }
    public decimal? Capital { get; set; }
    public string StatutRcs { get; set; }
    public string Greffe { get; set; }
    public string CodeGreffe { get; set; }
    public string NumeroRcs { get; set; }
    public string DateImmatriculationRcs { get; set; }
    public string NumeroTvaIntracommunautaire { get; set; }
    public string DateRadiationRcs { get; set; }
    public bool Diffusable { get; set; }
    public string DateDebutActivite { get; set; }
    public string CreateDate { get; set; }
    public string UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
#nullable enable
    public string? ObjetSocial { get; set; }
    public int? NDCoverError { get; set; }
#nullable disable

    public EntrepriseNDCoverDto EntrepriseNDCover { get; set; }
    public EntrepriseBaseDto()
    {
    }

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
    public EntrepriseBaseDto(string prmPappersJson, string username)
    {
        if (!string.IsNullOrEmpty(prmPappersJson))
        {
            JsonElement root = JsonSerializer.Deserialize<JsonElement>(prmPappersJson);
            Boolean diffu= root.GetProperty("diffusable").GetBoolean();
            if (diffu)
            {
                this.Siren = root.TryGetProperty("siren", out var siren) && siren.ValueKind != JsonValueKind.Null
                    ? siren.GetString()
                    : null;

                this.SirenFormate = root.TryGetProperty("siren_formate", out var sirenFormate) && sirenFormate.ValueKind != JsonValueKind.Null
                    ? sirenFormate.GetString()
                    : null;

                this.NomEntreprise = root.TryGetProperty("nom_entreprise", out var nomEntreprise) && nomEntreprise.ValueKind != JsonValueKind.Null
                    ? nomEntreprise.GetString()
                    : null;

                this.PersonneMorale = root.TryGetProperty("personne_morale", out var personneMorale) && 
                                      personneMorale.ValueKind != JsonValueKind.Null && personneMorale.GetBoolean();


                this.Denomination = root.TryGetProperty("denomination", out var denomination) && denomination.ValueKind != JsonValueKind.Null
                    ? denomination.GetString()
                    : null;

                this.Nom = root.TryGetProperty("nom", out var nom) && nom.ValueKind != JsonValueKind.Null
                    ? nom.GetString()
                    : null;

                this.Prenom = root.TryGetProperty("prenom", out var prenom) && prenom.ValueKind != JsonValueKind.Null
                    ? prenom.GetString()
                    : null;

                this.Sexe = root.TryGetProperty("sexe", out var sexe) && sexe.ValueKind != JsonValueKind.Null
                    ? sexe.GetString()
                    : null;

                this.CodeNaf = root.TryGetProperty("code_naf", out var codeNaf) && codeNaf.ValueKind != JsonValueKind.Null
                    ? codeNaf.GetString()
                    : null;

                this.LibelleCodeNaf = root.TryGetProperty("libelle_code_naf", out var libelleCodeNaf) && libelleCodeNaf.ValueKind != JsonValueKind.Null
                    ? libelleCodeNaf.GetString()
                    : null;

                this.DomaineActivite = root.TryGetProperty("domaine_activite", out var domaineActivite) && domaineActivite.ValueKind != JsonValueKind.Null
                    ? domaineActivite.GetString()
                    : null;

                this.DateCreation = root.TryGetProperty("date_creation", out var dateCreation) && dateCreation.ValueKind != JsonValueKind.Null
                    ? dateCreation.GetString()
                    : null;

                this.DateCreationFormate = root.TryGetProperty("date_creation_formate", out var dateCreationFormate) && dateCreationFormate.ValueKind != JsonValueKind.Null
                    ? dateCreationFormate.GetString()
                    : null;

                this.EntrepriseCessee = root.TryGetProperty("entreprise_cessee", out var entrepriseCessee) && 
                                        entrepriseCessee.ValueKind != JsonValueKind.Null && entrepriseCessee.GetBoolean();


                this.DateCessation = root.TryGetProperty("date_cessation", out var dateCessation) && dateCessation.ValueKind != JsonValueKind.Null
                    ? dateCessation.GetString()
                    : null;
                
                this.EntrepriseEmployeuse = root.TryGetProperty("entreprise_employeuse", out var entrepriseEmployeuse) && 
                                            entrepriseEmployeuse.ValueKind != JsonValueKind.Null && entrepriseEmployeuse.GetBoolean();


                this.CategorieJuridique = root.TryGetProperty("categorie_juridique", out var categorieJuridique) && categorieJuridique.ValueKind != JsonValueKind.Null
                    ? categorieJuridique.GetString()
                    : null;

                this.FormeJuridique = root.TryGetProperty("forme_juridique", out var formeJuridique) && formeJuridique.ValueKind != JsonValueKind.Null
                    ? formeJuridique.GetString()
                    : null;

                this.Effectif = root.TryGetProperty("effectif", out var effectif) && effectif.ValueKind != JsonValueKind.Null
                    ? effectif.GetString()
                    : null;

                this.EffectifMin = root.TryGetProperty("effectif_min", out var effectifMin) && effectifMin.ValueKind != JsonValueKind.Null
                    ? effectifMin.GetInt32()
                    : (int?)null;

                this.EffectifMax = root.TryGetProperty("effectif_max", out var effectifMax) && effectifMax.ValueKind != JsonValueKind.Null
                    ? effectifMax.GetInt32()
                    : (int?)null;

                this.TrancheEffectif = root.TryGetProperty("tranche_effectif", out var trancheEffectif) && trancheEffectif.ValueKind != JsonValueKind.Null
                    ? trancheEffectif.GetString()
                    : null;

                this.AnneeEffectif = root.TryGetProperty("annee_effectif", out var anneeEffectif) && anneeEffectif.ValueKind != JsonValueKind.Null
                    ? anneeEffectif.GetInt32()
                    : (int?)null;

                this.Capital = root.TryGetProperty("capital", out var capital) && capital.ValueKind != JsonValueKind.Null
                    ? capital.GetDecimal()
                    : (decimal?)null;

                this.StatutRcs = root.TryGetProperty("statut_rcs", out var statutRcs) && statutRcs.ValueKind != JsonValueKind.Null
                    ? statutRcs.GetString()
                    : null;

                this.Greffe = root.TryGetProperty("greffe", out var greffe) && greffe.ValueKind != JsonValueKind.Null
                    ? greffe.GetString()
                    : null;

                this.CodeGreffe = root.TryGetProperty("code_greffe", out var codeGreffe) && codeGreffe.ValueKind != JsonValueKind.Null
                    ? codeGreffe.GetString()
                    : null;

                this.NumeroRcs = root.TryGetProperty("numero_rcs", out var numeroRcs) && numeroRcs.ValueKind != JsonValueKind.Null
                    ? numeroRcs.GetString()
                    : null;

                this.DateImmatriculationRcs = root.TryGetProperty("date_immatriculation_rcs", out var dateImmatriculationRcs) && dateImmatriculationRcs.ValueKind != JsonValueKind.Null
                    ? dateImmatriculationRcs.GetString()
                    : null;

                this.NumeroTvaIntracommunautaire = root.TryGetProperty("numero_tva_intracommunautaire", out var numeroTvaIntracommunautaire) && numeroTvaIntracommunautaire.ValueKind != JsonValueKind.Null
                    ? numeroTvaIntracommunautaire.GetString()
                    : null;

                this.DateRadiationRcs = root.TryGetProperty("date_radiation_rcs", out var dateRadiationRcs) && dateRadiationRcs.ValueKind != JsonValueKind.Null
                    ? dateRadiationRcs.GetString()
                    : null;
                
                this.DateDebutActivite = root.TryGetProperty("date_debut_activite", out var dateDebutActivite) && dateDebutActivite.ValueKind != JsonValueKind.Null
                    ? dateDebutActivite.GetString()
                    : null;
                
                this.ObjetSocial = root.TryGetProperty("objet_social", out var objetSocial) && objetSocial.ValueKind != JsonValueKind.Null
                    ? objetSocial.GetString()
                    : null;
                
                this.Diffusable = root.TryGetProperty("diffusable", out var diffusable) && 
                                  diffusable.ValueKind != JsonValueKind.Null && diffusable.GetBoolean();

            }
            else
            {
                this.Diffusable = false;
            }
            
            this.CreateDate = DateTime.Now.ToString();
            this.CreatedBy = username;
        }


    }
#pragma warning restore S3776
}