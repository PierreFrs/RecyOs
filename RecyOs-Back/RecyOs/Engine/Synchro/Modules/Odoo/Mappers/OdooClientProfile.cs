//  OdooClientProfile.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using AutoMapper;
using RecyOs.OdooDB;
using RecyOs.OdooDB.DTO;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.Engine.Modules.Odoo;

public class OdooClientProfile: Profile
{
    public OdooClientProfile()
    {
        CreateMap<ResPartnerDto, EtablissementClientDto>()
            .ForMember(dest => dest.IdOdoo, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.AdresseFacturation1, opt => opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.AdresseFacturation2, opt => opt.MapFrom(src => src.Street2))
            .ForMember(dest => dest.CodePostalFacturation, opt => opt.MapFrom(src => src.Zip))
            .ForMember(dest => dest.VilleFacturation, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.Siret, opt => opt.MapFrom(src => src.Siret))
            .ForMember(dest => dest.EmailFacturation, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.TelephoneFacturation, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.PortableFacturation, opt => opt.MapFrom(src => src.Mobile))
            .ForMember( dest => dest.CodeMkgt, opt => opt.MapFrom( src => src.CodeMkgt ))
            .ForMember( dest => dest.DelaiReglement, opt => opt.MapFrom( src => src.CustomerPaymentTermId ))
            .ForMember( dest => dest.FrnDelaiReglement, opt => opt.MapFrom( src => src.SupplierPaymentTermId ))
            .ForMember( dest => dest.CompteComptable, opt => opt.MapFrom( src => src.SellAccount ))
            .ForMember( dest => dest.FrnCompteComptable, opt => opt.MapFrom( src => src.BuyAccount ));

        CreateMap<EtablissementClientDto, ResPartnerDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ParseIdOdoo(src.IdOdoo)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nom))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.AdresseFacturation1))
            .ForMember(dest => dest.Street2, opt => opt.MapFrom(src => src.AdresseFacturation2))
            .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.CodePostalFacturation))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.VilleFacturation))
            .ForMember(dest => dest.Siret, opt => opt.MapFrom(src => src.Siret))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailFacturation))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.TelephoneFacturation))
            .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.PortableFacturation))
            .ForMember(dest => dest.Vat, opt => opt.MapFrom(src => src.EntrepriseBase.NumeroTvaIntracommunautaire))
            .ForMember(dest => dest.CodeMkgt, opt => opt.MapFrom(src => src.CodeMkgt))
            .ForMember(dest => dest.IsCompany, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => 75))
            .ForMember(dest => dest.Origine, opt => opt.MapFrom(src => "RecyOs"))
            .ForMember(dest => dest.CustomerPaymentTermId,
                opt => opt.MapFrom(src => ParsePaymentTermId(src.DelaiReglement)))
            .ForMember(dest => dest.SupplierPaymentTermId,
                opt => opt.MapFrom(src => ParsePaymentTermId(src.FrnDelaiReglement)))
            .ForMember(dest => dest.SellAccount, opt => opt.MapFrom(src => src.CompteComptable))
            .ForMember( dest => dest.BuyAccount, opt => opt.MapFrom( src => src.FrnCompteComptable ));
    }
    
    public long ParseIdOdoo(string idOdoo)
    {
        if (long.TryParse(idOdoo, out var id))
        {
            return id;
        }
        return 0;
    }
    
    public int ParsePaymentTermId(int? delaisPaiement)
    {
        switch (delaisPaiement)
        {
            case 0:
                return 1;
            
            case 30:
                return 4;
            
            case 45:
                return 29;
            
            case 60:
                return 39;
        }
        return 1;
    }
    
    public int ParseDelaisPaiement(int? paymentTermId)
    {
        switch (paymentTermId)
        {
            case 1:
                return 0;
            
            case 4:
                return 30;
            
            case 29:
                return 45;
            
            case 39:
                return 60;
        }
        return 0;
    }
}