using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Hub.DTO;

public class ClientGpiDto
{
    public string Type { get; set; }
    public string Code { get; set; }
    public string Compte { get; set; }
    public string Siret { get; set; }
    public string Vat { get; set; }
    public string Nom { get; set; }
    public string Adresse1 { get; set; }
    public string CodePostal { get; set; }
    public string ModeReglement { get; set; }
    public int DelaiReglement { get; set; }
    public string Collectif { get; set; }
    public bool ClientBloque { get; set; }
    public string Pays { get; set; }
#nullable enable
    public string? Ville { get; set; }
    public string? Departement { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? Portable { get; set; }
    public string? Adresse2 { get; set; }
    public string? Adresse3 { get; set; }
    public int CapitalSocial { get; set; }
    public string? Ape { get; set; }
#nullable disable

    public ClientGpiDto()
    {

    }

    public ClientGpiDto(EtablissementClient prmObjCli, string prmType, string prmFrnCli)
    {
        if (prmFrnCli == "Client")
        {
            Code = prmObjCli.CodeGpi;
            Collectif = prmObjCli.CompteComptable;
        }
        else if (prmFrnCli == "Fournisseur")
        {
            Code = prmObjCli.FrnCodeGpi;
            Collectif = prmObjCli.FrnCompteComptable;
        }
        if (!string.IsNullOrEmpty(prmObjCli.EmailAlternatif))
        {
            Email = prmObjCli.EmailFacturation + ";" + prmObjCli.EmailAlternatif;
        }else
        {
            Email = prmObjCli.EmailFacturation;
        }
        if (!string.IsNullOrEmpty(Email)) Email = Email.ToLower();
        Type = prmType;
        Compte = prmObjCli.IdOdoo;
        Siret = prmObjCli.Siret;
        Vat = prmObjCli.EntrepriseBase.NumeroTvaIntracommunautaire;
        Nom = prmObjCli.Nom;
        Adresse1 = prmObjCli.AdresseFacturation1;
        Adresse2 = prmObjCli.AdresseFacturation2;
        Adresse3 = prmObjCli.AdresseFacturation3;
        CodePostal = prmObjCli.CodePostalFacturation;
        if(!string.IsNullOrEmpty(prmObjCli.VilleFacturation))Ville = prmObjCli.VilleFacturation.ToUpper();
        if(!string.IsNullOrEmpty(prmObjCli.PaysFacturation))Pays = prmObjCli.PaysFacturation.ToUpper();
        ModeReglement = GetModeReglement(prmObjCli.ModeReglement);
        DelaiReglement = prmObjCli.DelaiReglement ??= 0;
        ClientBloque = prmObjCli.ClientBloque??= false;
        Telephone = prmObjCli.TelephoneFacturation;
        Portable = prmObjCli.PortableFacturation;
        Departement = prmObjCli.CodePostalFacturation.Substring(0, 2);
        CapitalSocial = convertDecimalToInt(prmObjCli.EntrepriseBase.Capital);
        if(!string.IsNullOrEmpty(prmObjCli.EntrepriseBase.CodeNaf))
            Ape = prmObjCli.EntrepriseBase.CodeNaf.Replace(".", string.Empty);
    }
    public ClientGpiDto(ClientEurope prmObjClientEurope,string prmType, string prmFrnCli)
    {
        if (prmFrnCli == "Client")
        {
            Code = prmObjClientEurope.CodeGpi;
            Collectif = prmObjClientEurope.CompteComptable;
        }
        else if (prmFrnCli == "Fournisseur")
        {
            Code = prmObjClientEurope.FrnCodeGpi;
            Collectif = prmObjClientEurope.FrnCompteComptable; 
        }
        if (!string.IsNullOrEmpty(prmObjClientEurope.EmailAlternatif))
        {
            Email = prmObjClientEurope.EmailFacturation + ";" + prmObjClientEurope.EmailAlternatif;
        }else
        {
            Email = prmObjClientEurope.EmailFacturation;
        }
        if (!string.IsNullOrEmpty(Email)) Email = Email.ToLower();
        Type = prmType;
        Compte = prmObjClientEurope.IdOdoo;
        Siret = prmObjClientEurope.Vat;
        Vat = prmObjClientEurope.Vat;
        Nom = prmObjClientEurope.Nom;
        Adresse1 = prmObjClientEurope.AdresseFacturation1;
        Adresse2 = prmObjClientEurope.AdresseFacturation2;
        Adresse3 = prmObjClientEurope.AdresseFacturation3;
        CodePostal = prmObjClientEurope.CodePostalFacturation;
        if(!string.IsNullOrEmpty(prmObjClientEurope.VilleFacturation))Ville = prmObjClientEurope.VilleFacturation.ToUpper();
        if(!string.IsNullOrEmpty(prmObjClientEurope.PaysFacturation))Pays = prmObjClientEurope.PaysFacturation.ToUpper();
        ModeReglement = GetModeReglement(prmObjClientEurope.ModeReglement);
        DelaiReglement = prmObjClientEurope.DelaiReglement ??= 0;
        ClientBloque = prmObjClientEurope.ClientBloque??= false;
        Telephone = prmObjClientEurope.TelephoneFacturation;
        Portable = prmObjClientEurope.PortableFacturation;
    }

    private string GetModeReglement(int? prmModeReglement)
    {
        prmModeReglement ??= 0;
        switch (prmModeReglement)
        {
            case 0:
                return "Virement";
            case 1:
                return "L.C.R.";
            case 2:
                return "Chèque";
            case 4:
                return "Carte bancaire";
            default:
                return "Virement";
        }
    }
    
    private int convertDecimalToInt(decimal? prmDecimal)
    {
        prmDecimal ??= 0;
        return (int)prmDecimal;
    }
}