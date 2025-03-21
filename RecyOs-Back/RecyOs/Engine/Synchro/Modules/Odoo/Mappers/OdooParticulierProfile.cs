// OdooParticulierProfile.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 09/10/2024
// Fichier Modifié le : 09/10/2024
// Code développé pour le projet : RecyOs

using System.Linq;
using AutoMapper;
using RecyOs.OdooDB.DTO;
using RecyOs.ORM.Models.DTO.hub;

namespace RecyOs.Engine.Modules.Odoo;

public class OdooParticulierProfile : Profile
{
    public OdooParticulierProfile()
    {
        CreateMap<ResPartnerDto, ClientParticulierDto>()
            .ForMember(dest => dest.IdOdoo, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Titre, opt => opt.MapFrom(src => ParseTitleFromName(src.Name)))
            .ForMember(dest => dest.Prenom, opt => opt.MapFrom(src => ParseFirstNameFromName(src.Name)))
            .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => ParseLastNameFromName(src.Name)))
            .ForMember(dest => dest.AdresseFacturation1, opt => opt.MapFrom(src => src.Street))
            .ForMember(dest => dest.AdresseFacturation2, opt => opt.MapFrom(src => src.Street2))
            .ForMember(dest => dest.CodePostalFacturation, opt => opt.MapFrom(src => src.Zip))
            .ForMember(dest => dest.VilleFacturation, opt => opt.MapFrom(src => src.City))
            .ForMember(dest => dest.EmailFacturation, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.TelephoneFacturation, opt => opt.MapFrom(src => src.Phone))
            .ForMember(dest => dest.PortableFacturation, opt => opt.MapFrom(src => src.Mobile))
            .ForMember(dest => dest.CodeMkgt, opt => opt.MapFrom(src => src.CodeMkgt))
            .ForMember(dest => dest.DelaiReglement, opt => opt.MapFrom(src => src.CustomerPaymentTermId))
            .ForMember(dest => dest.CompteComptable, opt => opt.MapFrom(src => src.SellAccount));

        CreateMap<ClientParticulierDto, ResPartnerDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ParseIdOdoo(src.IdOdoo)))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.Titre} {src.Prenom} {src.Nom}"))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.AdresseFacturation1))
            .ForMember(dest => dest.Street2, opt => opt.MapFrom(src => src.AdresseFacturation2))
            .ForMember(dest => dest.Zip, opt => opt.MapFrom(src => src.CodePostalFacturation))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.VilleFacturation))
            .ForMember(dest => dest.Siret, opt => opt.Ignore())
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailFacturation))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.TelephoneFacturation))
            .ForMember(dest => dest.Mobile, opt => opt.MapFrom(src => src.PortableFacturation))
            .ForMember(dest => dest.Vat, opt => opt.Ignore())
            .ForMember(dest => dest.CodeMkgt, opt => opt.MapFrom(src => src.CodeMkgt))
            .ForMember(dest => dest.IsCompany, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => 75))
            .ForMember(dest => dest.Origine, opt => opt.MapFrom(src => "RecyOs"))
            .ForMember(dest => dest.CustomerPaymentTermId,
                opt => opt.MapFrom(src => ParsePaymentTermId(src.DelaiReglement)))
            .ForMember(dest => dest.SupplierPaymentTermId,
                opt => opt.MapFrom(src => ParsePaymentTermId(null)))
            .ForMember(dest => dest.SellAccount, opt => opt.MapFrom(src => "411103"))
            .ForMember(dest => dest.BuyAccount, opt => opt.MapFrom(src => "401101"));
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

    public string ParseTitleFromName(string fullName)
    {
        // Extract the title (if applicable) from the full name
        return fullName.Split(' ').FirstOrDefault();
    }

    public string ParseFirstNameFromName(string fullName)
    {
        // Extract the first name from the full name
        return fullName.Split(' ').Skip(1).FirstOrDefault();
    }

    public string ParseLastNameFromName(string fullName)
    {
        // Extract the last name from the full name
        return fullName.Split(' ').LastOrDefault();
    }

}