using System;
using AutoMapper;
using RecyOs.ORM.Models.DTO.hub;
using RecyOs.Engine.Modules.Mkgt;

namespace RecyOs.Engine.Mappers;

public class MkgtParticulierProfile : Profile
{
    public MkgtParticulierProfile()
    {
        CreateMap<ClientParticulierDto, EtablissementMkgtDto>()
            //Identification
            .ForMember(dest => dest.code, opt => opt.MapFrom(src => src.CodeMkgt))
            .ForMember(dest => dest.nom, opt => opt.MapFrom(src => src.Nom))
            .ForMember(dest => dest.adr1, opt => opt.MapFrom(src => formatAdress(src.AdresseFacturation1)))
            .ForMember(dest => dest.adr2, opt => opt.MapFrom(src => formatAdress(src.AdresseFacturation2)))
            .ForMember(dest => dest.adr3, opt => opt.MapFrom(src => formatAdress(src.AdresseFacturation3)))
            .ForMember(dest => dest.cp, opt => opt.MapFrom(src => src.CodePostalFacturation))
            .ForMember(dest => dest.ville, opt => opt.MapFrom(src => src.VilleFacturation))
            .ForMember(dest => dest.pays, opt => opt.MapFrom(src => src.PaysFacturation))
            .ForMember(dest => dest.intl2, opt => opt.MapFrom(src => src.Nom))
            .ForMember(dest => dest.email1, opt => opt.MapFrom(src => src.EmailFacturation))
            .ForMember(dest => dest.t2, opt => opt.MapFrom(src => src.TelephoneFacturation))
            .ForMember(dest => dest.ptb2, opt => opt.MapFrom(src => src.PortableFacturation))
            .ForMember(dest => dest.intl3, opt => opt.MapFrom(src => src.ContactAlternatif))
            .ForMember(dest => dest.email2, opt => opt.MapFrom(src => src.EmailAlternatif))
            .ForMember(dest => dest.t3, opt => opt.MapFrom(src => src.TelephoneAlternatif))
            .ForMember(dest => dest.ptb3, opt => opt.MapFrom(src => src.PortableAlternatif))
            // Paramètres
            .ForMember(dest => dest.tva, opt => opt.MapFrom(src => src.TauxTva))
            .ForMember(dest => dest.tvaF, opt => opt.MapFrom(src => src.TauxTva))
            .ForMember(dest => dest.secteur, opt => opt.MapFrom(src => transposeSecteur(src)))
            .ForMember(dest => dest.smTva, opt => opt.MapFrom(src => isClientTVA(src)))
            .ForMember(dest => dest.modReg, opt => opt.MapFrom(src => transposeReglement(src)))
            .ForMember(dest => dest.modRegF, opt => opt.MapFrom(src => transposeReglement(src)))
            .ForMember(dest => dest.encours, opt => opt.MapFrom(src => src.EncoursMax))
            .ForMember(dest => dest.dateCre, opt => opt.MapFrom(src => src.DateCreMkgt))
            .ForMember(dest => dest.dateMdf, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.codPay, opt => opt.MapFrom(src => getCountryCode(src)))
            .ForMember(dest => dest.frnCli, opt => opt.MapFrom(src => 'C'))
            .ForMember(dest => dest.cptFac, opt => opt.MapFrom(src => ((src.IdOdoo == "-1") ? null : src.IdOdoo)))
            .ForMember(dest => dest.cptAch, opt => opt.MapFrom(src => ((src.IdOdoo == "-1") ? null : src.IdOdoo)))
            .ForMember(dest => dest.cc, opt => opt.MapFrom(src => "PT"))
            .ForMember(dest => dest.fam, opt => opt.MapFrom(src =>"PR"));
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
Cette méthode permet de déterminer le code de secteur à utiliser pour un établissement client donné en fonction de son code postal.
Elle prend en entrée un objet EtablissementClientDTO qui contient les informations sur notament code postal facturation.
Le code de secteur retourné par la méthode sont en fait les 2 premièrs caractères du code postal.
Cette méthode retourne une chaîne de caractères représentant le code de secteur à utiliser.
*/
    private string transposeSecteur(ClientParticulierDto prm)
    {
        string secteur = prm.CodePostalFacturation.Substring(0, 2);
        return secteur;
    }
    
    /*
     * Cette méthode permet de déterminer si un client est sous régime de TVA ou non.
     * Elle prend en entrée un objet EtablissementClientDTO qui contient les informations sur le taux de TVA
     * si le taux de TVA est différent de 0, le client est sous régime de TVA.
     * Elle retourne un string qui vaut 'oui' si le client est sous régime de TVA, 'non' sinon.
     */
    private string isClientTVA(ClientParticulierDto prm)
    {
        if (prm.TauxTva != 0)
        {
            return "oui";
        }

        return "non";
    }
    
    /*
    Cette méthode permet de déterminer le code de réglement à utiliser pour un établissement client donné en fonction de ses conditions de règlement.
    Elle prend en entrée un objet EtablissementClientDTO qui contient les informations sur les conditions de règlement de l'établissement.
    Le code de réglement retourné par la méthode est une concaténation de la valeur "V30" ou "V" (selon les cas), et du délai de règlement (sous forme de chaîne de caractères).
    Cette méthode retourne une chaîne de caractères représentant le code de règlement à utiliser.
    */
    private string transposeReglement(ClientParticulierDto prm)
    {
        string reglement = "V30";
            
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
     * Cette méthode permet de déterminer le code du pays d'un client.
     * Elle prend en entrée un objet EtablissementClientDTO qui contient les informations sur le pays.
     * Elle retourne un string qui repésente le code du pays ce sont les 3 premiers caractères du pays.
     */
    private string getCountryCode(ClientParticulierDto prm)
    {
        int substringLength = Math.Min(3, prm.PaysFacturation.Length);
        string code = prm.PaysFacturation.Substring(0, substringLength);
        return code.ToUpper();
    }
}