using System;
using System.Collections.Generic;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

namespace RecyOs.ORM.DTO.hub;

public class EtablissementClientDto : IClientBalanceDto
{
    public int Id { get; set; }
    #nullable enable
    public string? Nom { get; set; }
    public string Siret { get; set; }
    public string Siren { get; set; }
    public string? IdOdoo { get; set; }
    public string? CodeKerlog { get; set; }
    public string? CodeMkgt { get; set; }
    public string? CodeGpi { get; set; }
    public string? FrnCodeGpi { get; set; }
    public string? ContactFacturation { get; set; }
    public string? EmailFacturation { get; set; }
    public string? TelephoneFacturation { get; set; }
    public string? PortableFacturation { get; set; }
    public string? ContactAlternatif { get; set; }
    public string? EmailAlternatif { get; set; }
    public string? TelephoneAlternatif { get; set; }
    public string? PortableAlternatif { get; set; }
    public string? AdresseFacturation1 { get; set; }
    public string? AdresseFacturation2 { get; set; }
    public string? AdresseFacturation3 { get; set; }
    public string? CodePostalFacturation { get; set; }
    public string? VilleFacturation { get; set; }
    public string? PaysFacturation { get; set; }
    public int? ConditionReglement { get; set; }
    public int? ModeReglement { get; set; }
    public int? DelaiReglement { get; set; }
    public decimal? TauxTva { get; set; }
    public int? EncoursMax { get; set; }
    public string? CompteComptable { get; set; }
    public int? FrnConditionReglement { get; set; }
    public int? FrnModeReglement { get; set; }
    public int? FrnDelaiReglement { get; set; }
    public decimal? FrnTauxTva { get; set; }
    public int? FrnEncoursMax { get; set; }
    public string? FrnCompteComptable { get; set; }
    public string? Iban { get; set; }
    public string? Bic { get; set; }
    public bool? ClientBloque { get; set; }
    public string? MotifBlocage { get; set; }
    public DateTime? DateBlocage { get; set; }
    public bool? Radie { get; set; }
    public int? CommercialId { get; set; }
    public CommercialDto? Commercial { get; set; }
    public string? IdHubspot { get; set; }
    public int? CategorieId { get; set; }
    public int? IdShipperDashdoc { get; set; }
    public int? GroupId { get; set; }
    public bool? NoBalance { get; set; }
    public bool? ClientGroupe { get; set; }
    #nullable disable
    public string CreateDate { get; set; }
    public string UpdatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string UpdatedBy { get; set; }
    public string DateCreMKGT { get; set; }
    public string DateCreGpi { get; set; }
    public string DateCreOdoo { get; set; }
    public int? IdDashdoc { get; set; }
    public DateTime? DateCreDashdoc { get; set; }
    public bool Client { get; set; }
    public bool Fournisseur { get; set; }
    public bool IsDeleted { get; set; }
    public EntrepriseBaseDto EntrepriseBase { get; set; }
    public EtablissementFicheDto EtablissementFiche { get; set; }
    public ICollection<FactorClientFranceBuDto> FactorClientFranceBus { get; set; }
    public EtablissementClientDto()
    {
    }
    
    public EtablissementClientDto(EntrepriseBaseDto entrepriseBaseDTO, EtablissementFicheDto etablissementFicheDTO
        , bool estClient, bool estFournisseur)
    {
        this.Nom = entrepriseBaseDTO.NomEntreprise;
        this.Siret = etablissementFicheDTO.Siret;
        this.Siren = entrepriseBaseDTO.Siren;
        this.AdresseFacturation1 = etablissementFicheDTO.AdresseLigne1;
        this.AdresseFacturation2 = etablissementFicheDTO.AdresseLigne2;
        this.CodePostalFacturation = etablissementFicheDTO.CodePostal;
        this.VilleFacturation = etablissementFicheDTO.Ville;
        this.PaysFacturation = etablissementFicheDTO.Pays;
        this.ConditionReglement = 1;  // 1 = comptant
        this.ModeReglement = 4; //
        this.DelaiReglement = 0;
        this.TauxTva = 20;
        this.EncoursMax = 0;
        this.CompteComptable = "411103";
        this.FrnConditionReglement = 0;
        this.FrnModeReglement = 0;
        this.FrnDelaiReglement = 30;
        this.FrnTauxTva = 20;
        this.FrnEncoursMax = 1000;
        this.FrnCompteComptable = "401101";
        this.CreateDate = DateTime.Now.ToString();
        this.CreatedBy = entrepriseBaseDTO.CreatedBy;
        this.Radie = etablissementFicheDTO.EtablissementCesse;
        this.CategorieId = 1; // Default value for N/A category
        this.Client = estClient;
        this.Fournisseur = estFournisseur;
    }

    public EtablissementClientDto(string prmSiret, bool estClient, bool estFournisseur)
    {
        this.Siret = prmSiret;
        this.Siren = prmSiret.Substring(0, 9);
        this.PaysFacturation = "FRANCE";
        this.ConditionReglement = 1;
        this.ModeReglement = 4;
        this.DelaiReglement = 0;
        this.TauxTva = 20;
        this.EncoursMax = 0;
        this.CompteComptable = "411103";
        this.FrnConditionReglement = 0;
        this.FrnModeReglement = 0;
        this.FrnDelaiReglement = 30;
        this.FrnTauxTva = 20;
        this.FrnEncoursMax = 1000;
        this.FrnCompteComptable = "401101";
        this.CategorieId = 1; // Default value for N/A category
        this.CreateDate = DateTime.Now.ToString();
        this.Radie = false;
        this.Client = estClient;
        this.Fournisseur = estFournisseur;
    }

    public EtablissementClientDto(EtablissementClientDto obj)
    {
        this.Id = obj.Id;
        this.Nom = obj.Nom;
        this.Siret = obj.Siret;
        this.Siren = obj.Siren;
        this.IdOdoo = obj.IdOdoo;
        this.CodeKerlog = obj.CodeKerlog;
        this.CodeMkgt = obj.CodeMkgt;
        this.ContactFacturation = obj.ContactFacturation;
        this.EmailFacturation = obj.EmailFacturation;
        this.TelephoneFacturation = obj.TelephoneFacturation;
        this.PortableFacturation = obj.PortableFacturation;
        this.ContactAlternatif = obj.ContactAlternatif;
        this.EmailAlternatif = obj.EmailAlternatif;
        this.TelephoneAlternatif = obj.TelephoneAlternatif;
        this.PortableAlternatif = obj.PortableAlternatif;
        this.AdresseFacturation1 = obj.AdresseFacturation1;
        this.AdresseFacturation2 = obj.AdresseFacturation2;
        this.AdresseFacturation3 = obj.AdresseFacturation3;
        this.CodePostalFacturation = obj.CodePostalFacturation;
        this.VilleFacturation = obj.VilleFacturation;
        this.PaysFacturation = obj.PaysFacturation;
        this.ConditionReglement = obj.ConditionReglement;
        this.ModeReglement = obj.ModeReglement;
        this.DelaiReglement = obj.DelaiReglement;
        this.TauxTva = obj.TauxTva;
        this.EncoursMax = obj.EncoursMax;
        this.CompteComptable = obj.CompteComptable;
        this.FrnConditionReglement = obj.FrnConditionReglement;
        this.FrnModeReglement = obj.FrnModeReglement;
        this.FrnDelaiReglement = obj.FrnDelaiReglement;
        this.FrnTauxTva = obj.FrnTauxTva;
        this.FrnEncoursMax = obj.FrnEncoursMax;
        this.FrnCompteComptable = obj.FrnCompteComptable;
        this.Iban = obj.Iban;
        this.Bic = obj.Bic;
        this.ClientBloque = obj.ClientBloque;
        this.MotifBlocage = obj.MotifBlocage;
        this.DateBlocage = obj.DateBlocage;
        this.Radie = obj.Radie;
        this.CreateDate = obj.CreateDate;
        this.UpdatedAt = obj.UpdatedAt;
        this.CreatedBy = obj.CreatedBy;
        this.UpdatedBy = obj.UpdatedBy;
        this.DateCreMKGT = obj.DateCreMKGT;
        this.DateCreOdoo = obj.DateCreOdoo;
        this.DateCreGpi = obj.DateCreGpi;
        this.CategorieId = obj.CategorieId;
        this.Client = obj.Client;
        this.Fournisseur = obj.Fournisseur;
        this.IdHubspot = obj.IdHubspot;
    }
}