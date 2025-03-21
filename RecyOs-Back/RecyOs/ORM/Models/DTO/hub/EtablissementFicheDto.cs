using System;
using System.Text.Json;

namespace RecyOs.ORM.DTO.hub;

public class EtablissementFicheDto
{
    public int Id { get; set; }
    public string Siret { get; set; }
    public string SiretFormate { get; set; }
    public string Nic { get; set; }
    public string CodePostal { get; set; }
    public string Ville { get; set; }
    public string Pays { get; set; }
    public string CodePays { get; set; }
    public float? Latitude { get; set; }
    public float? Longitude { get; set; }
    public bool EtablissementCesse { get; set; }
    public bool Siege { get; set; }
    public bool EtablissementEmployeur { get; set; }
    public string Effectif { get; set; }
    public int? EffectifMin { get; set; }
    public int? EffectifMax { get; set; }
    public string TrancheEffectif { get; set; }
    public int? AnneeEffectif { get; set; }
    public string CodeNaf { get; set; }
    public string LibelleCodeNaf { get; set; }
    public string DateDeCreation { get; set; }
    public int? NumeroVoie { get; set; }
    public string IndiceRepetition { get; set; }
    public string TypeVoie { get; set; }
    public string LibelleVoie { get; set; }
    public string ComplementAdresse { get; set; }
    public string AdresseLigne1 { get; set; }
    public string AdresseLigne2 { get; set; }
    public string DateCessation { get; set; }
    public string Enseigne { get; set; }
    public bool Diffusable { get; set; }
    public string NomCommercial { get; set; }
    public string NomDomiciliation { get; set; }
    public string SirenDomiciliation { get; set; }
    public string Predecesseurs { get; set; }
    public string Successeurs { get; set; }
    public string CreateDate { get; set; }
    public string UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    
    public EtablissementFicheDto()
    {
    }

#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
    /// <summary>
    /// Constructeur pour la classe EtablissementFicheDTO.
    /// Initialise les propriétés de l'objet à partir d'une chaîne JSON en paramètre.
    /// Vérifie si les données sont diffusables et extrait les informations d'établissement.
    /// </summary>
    /// <param name="prmPappersJson">La chaîne JSON contenant les informations d'établissement à extraire.</param>
    /// <param name="prmUsername">Le nom de l'utilisateur ayant demandé l'acction sera le créateur de la fiche </param>
    public EtablissementFicheDto(string prmPappersJson, string prmUsername)
    {
        if (!string.IsNullOrEmpty(prmPappersJson))
        {
            JsonElement root = JsonSerializer.Deserialize<JsonElement>(prmPappersJson);
            Boolean diffusable = root.GetProperty("diffusable").GetBoolean();
            if (diffusable)
            {
                var etablissement = root.GetProperty("etablissement");
                this.Siret = etablissement.GetProperty("siret").GetString();
                this.SiretFormate = etablissement.TryGetProperty("siret_formate", out var siretFormate) && siretFormate.ValueKind != JsonValueKind.Null
                    ? siretFormate.GetString()
                    : null;

                this.Nic = etablissement.TryGetProperty("nic", out var nic) && nic.ValueKind != JsonValueKind.Null
                    ? nic.GetString()
                    : null;

                this.CodePostal = etablissement.TryGetProperty("code_postal", out var codePostal) && codePostal.ValueKind != JsonValueKind.Null
                    ? codePostal.GetString()
                    : null;

                this.Ville = etablissement.TryGetProperty("ville", out var ville) && ville.ValueKind != JsonValueKind.Null
                    ? ville.GetString()
                    : null;

                this.Pays = etablissement.TryGetProperty("pays", out var pays) && pays.ValueKind != JsonValueKind.Null
                    ? pays.GetString()
                    : null;

                this.CodePays = etablissement.TryGetProperty("code_pays", out var codePays) && codePays.ValueKind != JsonValueKind.Null
                    ? codePays.GetString()
                    : null;

                this.Latitude = etablissement.TryGetProperty("latitude", out var latitudeElement) && latitudeElement.ValueKind == JsonValueKind.Number
                    ? latitudeElement.GetSingle()
                    : (float?)null;
                
                this.Longitude = etablissement.TryGetProperty("longitude", out var longitudeElement) && longitudeElement.ValueKind == JsonValueKind.Number
                    ? longitudeElement.GetSingle()
                    : (float?)null;
                
                this.EtablissementCesse = etablissement.TryGetProperty("etablissement_cesse", out var etablissementCesse) && 
                                          etablissementCesse.ValueKind != JsonValueKind.Null && etablissementCesse.GetBoolean();

                
                this.Siege = etablissement.TryGetProperty("siege", out var siege) && 
                             siege.ValueKind != JsonValueKind.Null && siege.GetBoolean();

                
                this.EtablissementEmployeur = etablissement.TryGetProperty("etablissement_employeur", out var etablissementEmployeur) && 
                                              etablissementEmployeur.ValueKind != JsonValueKind.Null && etablissementEmployeur.GetBoolean();

                
                this.Effectif = etablissement.TryGetProperty("effectif", out var effectif) && effectif.ValueKind != JsonValueKind.Null 
                    ? effectif.GetString() 
                    : null;
                this.EffectifMin = etablissement.TryGetProperty("effectif_min", out var effectifMin) && effectifMin.ValueKind != JsonValueKind.Null 
                    ? effectifMin.GetInt32() 
                    : (int?)null;
                
                this.EffectifMax = etablissement.TryGetProperty("effectif_max", out var effectifMax) && effectifMax.ValueKind != JsonValueKind.Null 
                    ? effectifMax.GetInt32() 
                    : (int?)null;
                
                this.TrancheEffectif = etablissement.TryGetProperty("tranche_effectif", out var trancheEffectif) && trancheEffectif.ValueKind != JsonValueKind.Null 
                    ? trancheEffectif.GetString() 
                    : null;
                
                this.AnneeEffectif = etablissement.TryGetProperty("annee_effectif", out var anneeEffectif) && anneeEffectif.ValueKind != JsonValueKind.Null 
                    ? anneeEffectif.GetInt32() 
                    : (int?)null;
                
                this.CodeNaf = etablissement.TryGetProperty("code_naf", out var codeNaf) && codeNaf.ValueKind != JsonValueKind.Null 
                    ? codeNaf.GetString() 
                    : null;
                
                this.LibelleCodeNaf = etablissement.TryGetProperty("libelle_code_naf", out var libelleCodeNaf) && libelleCodeNaf.ValueKind != JsonValueKind.Null 
                    ? libelleCodeNaf.GetString() 
                    : null;
                
                this.DateDeCreation = etablissement.TryGetProperty("date_de_creation", out var dateDeCreation) && dateDeCreation.ValueKind != JsonValueKind.Null 
                    ? dateDeCreation.GetString() 
                    : null;
                
                this.NumeroVoie = etablissement.TryGetProperty("numero_voie", out var numeroVoie) && numeroVoie.ValueKind != JsonValueKind.Null 
                    ? numeroVoie.GetInt32() 
                    : (int?)null;
                
                this.IndiceRepetition = etablissement.TryGetProperty("indice_repetition", out var indiceRepetition) && indiceRepetition.ValueKind != JsonValueKind.Null 
                    ? indiceRepetition.GetString() 
                    : null;
                
                this.TypeVoie = etablissement.TryGetProperty("type_voie", out var typeVoie) && typeVoie.ValueKind != JsonValueKind.Null 
                    ? typeVoie.GetString() 
                    : null;
                
                this.LibelleVoie = etablissement.TryGetProperty("libelle_voie", out var libelleVoie) && libelleVoie.ValueKind != JsonValueKind.Null 
                    ? libelleVoie.GetString() 
                    : null;
                
                this.ComplementAdresse = etablissement.TryGetProperty("complement_adresse", out var complementAdresse) && complementAdresse.ValueKind != JsonValueKind.Null 
                    ? complementAdresse.GetString() 
                    : null;
                
                this.AdresseLigne1 = etablissement.TryGetProperty("adresse_ligne_1", out var adresseLigne1) && adresseLigne1.ValueKind != JsonValueKind.Null 
                    ? adresseLigne1.GetString() 
                    : null;
                
                this.AdresseLigne2 = etablissement.TryGetProperty("adresse_ligne_2", out var adresseLigne2) && adresseLigne2.ValueKind != JsonValueKind.Null 
                    ? adresseLigne2.GetString() 
                    : null;
                
                this.DateCessation = etablissement.TryGetProperty("date_cessation", out var dateCessation) && dateCessation.ValueKind != JsonValueKind.Null 
                    ? dateCessation.GetString() 
                    : null;
                
                this.Enseigne = etablissement.TryGetProperty("enseigne", out var enseigne) && enseigne.ValueKind != JsonValueKind.Null 
                    ? enseigne.GetString() 
                    : null;
                
                this.NomCommercial = etablissement.TryGetProperty("nom_commercial", out var nomCommercial) && nomCommercial.ValueKind != JsonValueKind.Null 
                    ? nomCommercial.GetString() 
                    : null;

                this.Diffusable = diffusable;
            }else
            {
                this.Diffusable = false;
            }
            
            this.CreatedBy = prmUsername;
            this.CreateDate = DateTime.Now.ToString();
        }

    }
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
}