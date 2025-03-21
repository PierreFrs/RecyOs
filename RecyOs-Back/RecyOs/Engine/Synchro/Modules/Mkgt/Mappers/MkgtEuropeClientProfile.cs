using AutoMapper;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.Engine.Mappers;

public class MkgtEuropeClientProfile: Profile
{
    public MkgtEuropeClientProfile()
    {
        CreateMap<ClientEuropeDto, EtablissementMkgtDto>()
            //Identification
            .ForMember(dest => dest.code, opt => opt.MapFrom(src => src.CodeMkgt))
            .ForMember(dest => dest.nom, opt => opt.MapFrom(src => src.Nom))
            .ForMember(dest => dest.adr1, opt => opt.MapFrom(src => formatAdress(src.AdresseFacturation1)))
            .ForMember(dest => dest.adr2, opt => opt.MapFrom(src => formatAdress(src.AdresseFacturation2)))
            .ForMember(dest => dest.adr3, opt => opt.MapFrom(src => formatAdress(src.AdresseFacturation3)))
            .ForMember(dest => dest.cp, opt => opt.MapFrom(src => src.CodePostalFacturation))
            .ForMember(dest => dest.ville, opt => opt.MapFrom(src => src.VilleFacturation))
            .ForMember(dest => dest.pays, opt => opt.MapFrom(src => src.PaysFacturation))
            .ForMember(dest => dest.siret, opt => opt.MapFrom(src => src.Vat))
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
            .ForMember( dest => dest.tvaF, opt => opt.MapFrom(src => src.FrnTauxTva))
            .ForMember(dest => dest.secteur, opt => opt.MapFrom(src => transposeSecteur(src)))
            .ForMember(dest => dest.smTva, opt => opt.MapFrom(src => isClientTVA(src)))
            .ForMember(dest => dest.modReg, opt => opt.MapFrom(src => transposeReglement(src)))
            .ForMember(dest => dest.modRegF, opt => opt.MapFrom(src => transposeReglementF(src))) 
            .ForMember(dest => dest.encours, opt => opt.MapFrom(src => src.EncoursMax))
            .ForMember(dest => dest.rib, opt => opt.MapFrom(src => src.Iban))
            .ForMember(dest => dest.tpSoc, opt => opt.MapFrom(src => "INTER."))
            .ForMember(dest => dest.dateCre, opt => opt.MapFrom(src => src.DateCreMkgt))
            .ForMember(dest => dest.dateMdf, opt => opt.MapFrom(src => src.UpdatedAt))
            .ForMember(dest => dest.codPay, opt => opt.MapFrom(src => getCountryCode(src)))
            .ForMember(dest => dest.intracom, opt => opt.MapFrom(src => src.Vat))
            .ForMember(dest => dest.cptFac, opt => opt.MapFrom(src => ((src.IdOdoo == "-1") || (!src.Client)) ? null : src.IdOdoo))
            .ForMember( dest => dest.cptAch, opt => opt.MapFrom(src => ((src.IdOdoo == "-1") || (!src.Fournisseur)) ? null : src.IdOdoo))
            .ForMember( dest => dest.frnCli, opt => opt.MapFrom(src => src.Fournisseur ? 'F' : 'C'))
            .ForMember(dest => dest.cc, opt => opt.MapFrom(src => src.Commercial.CodeMkgt))
            .ForMember(dest => dest.fam, opt => opt.MapFrom(src => "ND"));
        
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
    Cette méthode permet de déterminer le code de réglement à utiliser pour un établissement client donné en fonction de ses conditions de règlement.
    Elle prend en entrée un objet EtablissementClientDTO qui contient les informations sur les conditions de règlement de l'établissement.
    Le code de réglement retourné par la méthode est une concaténation de la valeur "V30" ou "V" (selon les cas), et du délai de règlement (sous forme de chaîne de caractères).
    Cette méthode retourne une chaîne de caractères représentant le code de règlement à utiliser.
    */
    private string transposeReglement(ClientEuropeDto prm)
    {
        string reglement = "V30";
        FactorClientEuropeBuDto obj = new FactorClientEuropeBuDto(){IdBu = 1, IdClient = prm.Id, IsDeleted = false};
        
        if (prm.FactorClientEuropeBus.Contains(obj))
        {
            return "FAC";
        }
        
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
    private string isClientTVA(ClientEuropeDto prm)
    {
        if (prm.TauxTva != 0)
        {
            return "oui";
        }

        return "non";
    }
    
    private string transposeSecteur(ClientEuropeDto prm)
    {
        switch (prm.PaysFacturation)
        {
            case "BELGIQUE" :
                return "BE";
            case "ITALIE" :
                return "IT";
            case "ESPAGNE" :
                return "ES";
            case "PAYS-BAS" :
                return "NL";
            case "LUXEMBOURG" :
                return "LU";
        }
        string secteur = prm.CodePostalFacturation.Substring(0, 2);
        return secteur;
    }

#pragma warning disable S1479 // Unused private types or members should be removed
    private string getCountryCode(ClientEuropeDto prm)
    {
        switch (prm.PaysFacturation)
        {
            case "AUSTRALIE":
                return "AUS";
            case "AUTRICHE":
                return "AUT";
            case "BELGIQUE":
                return "BEL";
            case "BOSNIE-HERZEGOVINE":
                return "BIH";
            case "BRESIL":
                return "BRA";
            case "CANADA":
                return "CAN";
            case "SUISSE":
                return "CHE";
            case "CHINE":
                return "CHN";
            case "COTE D'IVOIRE":
                return "CIV";
            case "REPUBLIQUE CHEQUE":
                return "CZE";
            case "ALLEMAGNE":
                return "DEU";
            case "DANEMARK":
                return "DNK";
            case "ALGERIE":
                return "DZA";
            case "ECOSSE":
                return "ECO";
            case "ESPAGNE":
                return "ESP";
            case "ESTONIE":
                return "EST";
            case "FINLANDE":
                return "FIN";
            case "FRANCE":
                return "FRA";
            case "ROYAUME-UNI":
                return "GBR";
            case "HONG KONG":
                return "HKG";
            case "INDONESIE":
                return "IDN";
            case "INDE":
                return "IND";
            case "IRLANDE":
                return "IRL";
            case "ITALIE":
                return "ITA";
            case "LUXEMBOURG":
                return "LUX";
            case "MAROC":
                return "MAR";
            case "MALAISIE":
                return "MYS";
            case "NOUVELLE CALEDONIE":
                return "NCL";
            case "PAYS-BAS":
                return "NLD";
            case "NORVEGE":
                return "NOR";
            case "PAKISTAN":
                return "PAK";
            case "POLOGNE":
                return "POL";
            case "PORTUGAL":
                return "PRT";
            case "ARABIE SAOUDITE":
                return "SAU";
            case "SENEGAL":
                return "SEN";
            case "SINGAPORE":
                return "SGP";
            case "SLOVENIE":
                return "SVN";
            case "SUEDE":
                return "SWE";
            case "TAHITI":
                return "TAH";
            case "TOGO":
                return "TGO";
            case "TURQUIE":
                return "TUR";
            case "TAIWAN":
                return "TWN";
            case "ETATS-UNIS":
                return "USA";
            case "VIETNAM":
                return "VNM";
        }
        return "FRA";

    }
#pragma warning restore S1479
    private string transposeReglementF(ClientEuropeDto prm)
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