//  MkgtClientProfile.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 12/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using AutoMapper;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.Engine.Mappers;

public class MkgtClientProfile: Profile
{
    public MkgtClientProfile()
    {
        CreateMap<EtablissementClientDto, EtablissementMkgtDto>()
            //Identification
            .ForMember(dest => dest.code, opt => opt.MapFrom(src => src.CodeMkgt))
            .ForMember(dest => dest.nom, opt => opt.MapFrom(src => src.Nom))
            .ForMember(dest => dest.adr1, opt => opt.MapFrom(src => formatAdress(src.AdresseFacturation1)))
            .ForMember(dest => dest.adr2, opt => opt.MapFrom(src => formatAdress(src.AdresseFacturation2)))
            .ForMember(dest => dest.adr3, opt => opt.MapFrom(src => formatAdress(src.AdresseFacturation3)))
            .ForMember(dest => dest.cp, opt => opt.MapFrom(src => src.CodePostalFacturation))
            .ForMember(dest => dest.ville, opt => opt.MapFrom(src => src.VilleFacturation))
            .ForMember(dest => dest.pays, opt => opt.MapFrom(src => src.PaysFacturation))
            .ForMember(dest => dest.siret, opt => opt.MapFrom(src => src.Siret))
            .ForMember(dest => dest.intl2, opt => opt.MapFrom(src => src.ContactFacturation))
            .ForMember(dest => dest.email1, opt => opt.MapFrom(src => src.EmailFacturation))
            .ForMember(dest => dest.t2, opt => opt.MapFrom(src => src.TelephoneFacturation))
            .ForMember(dest => dest.ptb2, opt => opt.MapFrom(src => src.PortableFacturation))
            .ForMember(dest => dest.intl3, opt => opt.MapFrom(src => src.ContactAlternatif))
            .ForMember(dest => dest.email2, opt => opt.MapFrom(src => src.EmailAlternatif))
            .ForMember(dest => dest.t3, opt => opt.MapFrom(src => src.TelephoneAlternatif))
            .ForMember(dest => dest.ptb3, opt => opt.MapFrom(src => src.PortableAlternatif))
            // Paramètres
            .ForMember(dest => dest.tva, opt => opt.MapFrom(src => src.TauxTva))
            .ForMember(dest => dest.tvaF, opt => opt.MapFrom(src => src.FrnTauxTva))
            .ForMember(dest => dest.secteur, opt => opt.MapFrom(src => transposeSecteur(src)))
            .ForMember(dest => dest.smTva, opt => opt.MapFrom(src => isClientTVA(src)))
            .ForMember(dest => dest.modReg, opt => opt.MapFrom(src => transposeReglement(src)))
            .ForMember(dest => dest.modRegF, opt => opt.MapFrom(src => transposeReglementF(src)))
            .ForMember(dest => dest.encours, opt => opt.MapFrom(src => src.EncoursMax))
            .ForMember(dest => dest.rib, opt => opt.MapFrom(src => src.Iban))
            .ForMember(dest => dest.tpSoc,
                opt => opt.MapFrom(src => getFormeJuridique(src.EntrepriseBase.CategorieJuridique)))
            .ForMember(dest => dest.dateCre, opt => opt.MapFrom(src => src.DateCreMKGT))
            .ForMember(dest => dest.dateMdf, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.codPay, opt => opt.MapFrom(src => getCountryCode(src)))
            .ForMember(dest => dest.ape, opt => opt.MapFrom(src => src.EntrepriseBase.CodeNaf))
            .ForMember(dest => dest.intracom, opt => opt.MapFrom(src => src.EntrepriseBase.NumeroTvaIntracommunautaire))
            .ForMember(dest => dest.cptFac,
                opt => opt.MapFrom(src => ((src.IdOdoo == "-1") || (!src.Client)) ? null : src.IdOdoo))
            .ForMember(dest => dest.cptAch,
                opt => opt.MapFrom(src => ((src.IdOdoo == "-1") || (!src.Fournisseur)) ? null : src.IdOdoo))
            .ForMember(dest => dest.cc, opt => opt.MapFrom(src => src.Commercial.CodeMkgt))
            .ForMember(dest => dest.frnCli, opt => opt.MapFrom(src => src.Fournisseur ? 'F' : 'C'))
            .ForMember(dest => dest.fam, opt => opt.MapFrom(src =>"ND"));

        CreateMap<EtablissementMkgtDto, EtablissementClientDto>()
            .ForMember(dest => dest.CodeMkgt, opt => opt.MapFrom(src => src.code))
            .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.nom.ToUpper()))
            .ForMember(dest => dest.AdresseFacturation1, opt => opt.MapFrom(src => src.adr1))
            .ForMember(dest => dest.AdresseFacturation2, opt => opt.MapFrom(src => src.adr2))
            .ForMember(dest => dest.AdresseFacturation3, opt => opt.MapFrom(src => src.adr3))
            .ForMember(dest => dest.CodePostalFacturation, opt => opt.MapFrom(src => src.cp))
            .ForMember(dest => dest.VilleFacturation, opt => opt.MapFrom(src => src.ville))
            .ForMember(dest => dest.PaysFacturation, opt => opt.MapFrom(src => src.pays))
            .ForMember(dest => dest.Siret, opt => opt.MapFrom(src => src.siret))
            .ForMember(dest => dest.Siren, opt => opt.MapFrom(src => src.siret.Substring(0, 9)))
            .ForMember(dest => dest.ContactFacturation, opt => opt.MapFrom(src => src.intl2))
            .ForMember(dest => dest.EmailFacturation, opt => opt.MapFrom(src => src.email1))
            .ForMember(dest => dest.TelephoneFacturation, opt => opt.MapFrom(src => src.t2))
            .ForMember(dest => dest.PortableFacturation, opt => opt.MapFrom(src => src.ptb2))
            .ForMember(dest => dest.ContactAlternatif, opt => opt.MapFrom(src => src.intl3))
            .ForMember(dest => dest.EmailAlternatif, opt => opt.MapFrom(src => src.email2))
            .ForMember(dest => dest.TelephoneAlternatif, opt => opt.MapFrom(src => src.t3))
            .ForMember(dest => dest.PortableAlternatif, opt => opt.MapFrom(src => src.ptb3))
            .ForMember(dest => dest.TauxTva, opt => opt.MapFrom(src => src.tva))
            .ForMember(dest => dest.Iban, opt => opt.MapFrom(src => src.rib))
            .ForMember(dest => dest.IdOdoo, opt => opt.MapFrom(src => src.cptFac))
            .ForMember( dest => dest.ConditionReglement, opt => opt.MapFrom(src => getConditionReglement(src)))
            .ForMember( dest => dest.ModeReglement, opt => opt.MapFrom(src => getModeReglement(src)))
            .ForMember( dest => dest.DelaiReglement, opt => opt.MapFrom(src => getDelaiReglement(src)))
            .ForMember( dest => dest.DateCreMKGT, opt => opt.MapFrom(src => src.dateCre))
            .ForMember( dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.dateMdf))
            .ForMember( dest => dest.EncoursMax, opt => opt.MapFrom(src => src.encours == null ? 0 : src.encours))
            .ForMember( dest => dest.CompteComptable, opt => opt.MapFrom( src => getCompteComptable(src)))
            .ForMember( dest => dest.CreateDate, opt => opt.MapFrom( src => DateTime.Now))
            .ForMember( dest => dest.CreatedBy, opt => opt.MapFrom( src => "MKGT-DB"))
            .ForMember( dest => dest.CategorieId, opt => opt.MapFrom( src => 1))
            .ForMember( dest => dest.Fournisseur, opt => opt.MapFrom( src => src.frnCli == "F"));
    }

    /*
    Cette méthode permet de déterminer le code de secteur à utiliser pour un établissement client donné en fonction de son code postal.
    Elle prend en entrée un objet EtablissementClientDTO qui contient les informations sur notament code postal facturation.
    Le code de secteur retourné par la méthode sont en fait les 2 premièrs caractères du code postal.
    Cette méthode retourne une chaîne de caractères représentant le code de secteur à utiliser.
    */
    private string transposeSecteur(EtablissementClientDto prm)
    {
        string secteur = prm.CodePostalFacturation.Substring(0, 2);
        return secteur;
    }
    
    /*
    Cette méthode permet de déterminer le code de réglement à utiliser pour un établissement client donné en fonction de ses conditions de règlement.
    Elle prend en entrée un objet EtablissementClientDTO qui contient les informations sur les conditions de règlement de l'établissement.
    Le code de réglement retourné par la méthode est une concaténation de la valeur "V30" ou "V" (selon les cas), et du délai de règlement (sous forme de chaîne de caractères).
    Cette méthode retourne une chaîne de caractères représentant le code de règlement à utiliser.
    */
    private string transposeReglement(EtablissementClientDto prm)
    {
        string reglement = "V30";
        FactorClientFranceBuDto obj = new FactorClientFranceBuDto(){IdBu = 1, IdClient = prm.Id, IsDeleted = false};
        
        if (prm.FactorClientFranceBus.Contains(obj)) return "FAC";
            
        switch (prm.ConditionReglement)
        {
            // en compte
            case 0 :
                switch (prm.ModeReglement)
                {
                    // Virement
                    case 0 :
                        reglement = "V";
                        break;
                    
                    // LCR
                    case 1 :
                        reglement = "L";
                        break;
                    
                    // Traite
                    case 5 :
                        reglement = "T";
                        break;
                    
                    default :
                        reglement = "V";
                        break;
                        
                }
                break;
            
            // comptant
            case 1 :
                switch (prm.ModeReglement)
                {
                    // Chèque
                    case 2 :
                        reglement = "CHE";
                        return reglement;

                    // Espèce
                    case 3 :
                        reglement = "ESP";
                        return reglement;

                    //CB
                    case 4 :
                        reglement = "CB";
                        return reglement;
                }
                break;
                
            
            // Affacturage
            case 2 :
                reglement = "V";
                break;
        }
        
        reglement += prm.DelaiReglement.ToString();
        return reglement;
    }
    
    /*
     * Cette méthode permet de déterminer si un client est sous régime de TVA ou non.
     * Elle prend en entrée un objet EtablissementClientDTO qui contient les informations sur le taux de TVA
     * si le taux de TVA est différent de 0, le client est sous régime de TVA.
     * Elle retourne un string qui vaut 'oui' si le client est sous régime de TVA, 'non' sinon.
     */
    private string isClientTVA(EtablissementClientDto prm)
    {
        if (prm.TauxTva != 0)
        {
            return "oui";
        }

        return "non";
    }
    
    /*
     * Cette méthode permet de déterminer la forme juridique d'un client.
     * Elle prend en entrée un objet EtablissementClientExDTO qui contient les informations sur la forme juridique.
     * Elle retourne un string qui repésente le type de société.
     */
    private string getFormeJuridique(string prmcategorieJuridique)
    {
        switch (prmcategorieJuridique)
        {
            case "1000" :
                return "E.I";

            case "5202":
                return "S.N.C";

            case "5498" :
                return "E.U.R.L";

            case "5499" :
                return "S.A.R.L";

            case "5599" :
                return "S.A";

            case "5710" :
                return "S.A.S";

            case "5720" :
                return "S.A.S.U";

            case "6540" :
                return "S.C.I";

            case "6533" :
                return "G.A.E.C";

            case "9220":
                return "ASSOC";
        }

        return null;
    }
    
    /*
     * Cette méthode permet de déterminer le code du pays d'un client.
     * Elle prend en entrée un objet EtablissementClientDTO qui contient les informations sur le pays.
     * Elle retourne un string qui repésente le code du pays ce sont les 3 premiers caractères du pays.
     */
    private string getCountryCode(EtablissementClientDto prm)
    {
        int substringLength = Math.Min(3, prm.PaysFacturation.Length);
        string code = prm.PaysFacturation.Substring(0, substringLength);
        return code.ToUpper();
    }
    
    /*
     * Cette méthode permet de déterminer les conditions de règlement d'un client.
     * Elle prend en entrée un objet EtablissementMkgtDTO qui contient le code de la condition de règlement. Qui sert à
     * déterminer le délai de règlement.
     * Elle retourne un int qui repésente les conditions de règlement.
     * 0 : en compte
     * 1 : comptant
     * 2 : affacturage
     */
    private int getConditionReglement(EtablissementMkgtDto prm)
    {
        switch (prm.modReg)
        {
            case "CHE" :
                return 1;

            case "ESP" :
                return 1;

            case "CB" :
                return 1;

            default:
                // if format is letter number number
                if (IsValidFormat(prm.modReg))
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
        }
    }
    
    /*
     * Cette méthode permet de vérifier le format d'une chaîne de caractères.
     * Elle prend en entrée un string qui représente la chaîne de caractères à vérifier.
     * Elle retourne un booléen qui vaut true si la chaîne de caractères est au bon format, false sinon.
     */
    private static bool IsValidFormat(string input) {
        // Utilisez une expression régulière pour vérifier le format letre chiffre chiffre
        Regex regex = new Regex(@"^[A-Za-z]\d\d$");
        return regex.IsMatch(input);
    }
    
    /*
     * Cette méthode permet de déterminer le mode de règlement d'un client.
     * Elle prend en entrée un objet EtablissementMkgtDTO qui contient le code de la condition de règlement. Qui sert à
     * déterminer le mode de règlement.
     * Elle retourne un int qui repésente les conditions de règlement.
     * 0 : virement
     * 1 : LCR
     * 2 : chèque
     * 3 : espèce
     * 4 : CB
     * 5 : traite
     */
    private int getModeReglement(EtablissementMkgtDto prm)
    {
        switch (prm.modReg)
        {

            case "CHE" :
                return 2;

            case "ESP" :
                return 3;

            case "CB" :
                return 4;

            default:
                // if format is letter number number
                if (IsValidFormat(prm.modReg))
                {
                    var modeReglement = prm.modReg.Substring(0,1);
                    switch (modeReglement)
                    {
                        case "V" :
                            return 0;

                        case "L" :
                            return 1;

                        case "T" :
                            return 5;
                        
                        case "C" :
                            return 2;
                    }
                }
            
                return 0;
        }
    }
    
    /*
     * Cette méthode permet de déterminer le délai de règlement d'un client.
     * Elle prend en entrée un objet EtablissementMkgtDTO qui contient le code de la condition de règlement. Qui sert à
     * déterminer le délai de règlement.
     * Elle retourne un int qui repésente le délai de règlement en jours.
     */
    private int getDelaiReglement(EtablissementMkgtDto prm)
    {
        switch (prm.modReg)
        {
            case "CHE" :
                return 0;

            case "ESP" :
                return 0;

            case "CB" :
                return 0;

            default:
                // if format is letter number number
                if (IsValidFormat(prm.modReg))
                {
                    var delaiReglement = prm.modReg.Substring(1,2);
                    return Int32.Parse(delaiReglement);
                }
                return 0;
        }
    }
    
    /*
     * Cette méthode permet de formater les adresses d'un client au format MKGT.
     * Elle prend en entrée un objet sting qui contient les informations.
     * Elle retourne un string qui repésente le type de client. le cas null est remplacé par une chaîne vide.
     */
    private string formatAdress(string prm)
    {
        if (prm == null)
        {
            return "";
        }
        else
        {
            return prm;
        }
    }
    
    /*
     * Cette méthode permet de déterminer le compte comptable à affercter en fonction du taux de TVA du client.
     * 20 -> 411103 , 10 -> 411102 , 5.5 -> 411101, 0 -> 411104
     * NE PAS UTILISER CETTE METHODE POUR LES CLIENTS ETRANGERS
     */
    private string getCompteComptable(EtablissementMkgtDto prm)
    {
        if (prm.tva == 20.0m)
            return "411103";

        if (prm.tva == 10.0m)
            return "411102";

        if (prm.tva == 5.5m)
            return "411101";

        if (prm.tva == 0.0m)
            return "411104";  // Attention pour les clients Etrangers, il faut mettre 411107 et ne pas utiliser cette méthode

        return "411103";
    }
    
    private string transposeReglementF(EtablissementClientDto prm)
    {
        string reglement = "V30";
        switch (prm.FrnConditionReglement)
        {
            // en compte
            case 0 :
                switch (prm.FrnModeReglement)
                {
                    // Virement
                    case 0 :
                        reglement = "V";
                        break;
                    
                    // LCR
                    case 1 :
                        reglement = "L";
                        break;
                    
                    // Traite
                    case 5 :
                        reglement = "T";
                        break;
                    
                    default :
                        reglement = "V";
                        break;
                        
                }
                break;
            
            // comptant
            case 1 :
                switch (prm.FrnModeReglement)
                {
                    // Chèque
                    case 2 :
                        reglement = "CHE";
                        return reglement;

                    // Espèce
                    case 3 :
                        reglement = "ESP";
                        return reglement;

                    //CB
                    case 4 :
                        reglement = "CB";
                        return reglement;
                }
                break;
                
            
            // Affacturage
            case 2 :
                reglement = "V";
                break;
        }
        
        reglement += prm.FrnDelaiReglement.ToString();
        return reglement;
    }
    
}