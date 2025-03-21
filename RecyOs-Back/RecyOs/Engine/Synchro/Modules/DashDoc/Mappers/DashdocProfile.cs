// DashdocProfile.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 16/09/2024
// Fichier Modifié le : 16/09/2024
// Code développé pour le projet : RecyOs

using System;
using System.Runtime.InteropServices.JavaScript;
using AutoMapper;
using RecyOs.Helpers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Models.DTO.hub;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;
using RecyOs.ThirdPartyAPIs.DashdocDB.Entities;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.MapProfile;

public class DashdocProfile : Profile
{
    public DashdocProfile()
    {
        CreateMap<DashdocCompany, DashdocCompanyDto>().ReverseMap();

        CreateMap<DashdocCompany, DashdocShipperDto>().ReverseMap();
        
        CreateMap<DashdocCompanyDto, DashdocShipperDto>().ReverseMap();
        
        CreateMap<DashdocPrimaryAddress, DashdocPrimaryAddressDto>().ReverseMap();

        CreateMap<EtablissementClientDto, DashdocCompanyDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nom))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailFacturation))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.TelephoneFacturation))
            .ForMember(dest => dest.Siren, opt => opt.MapFrom(src => src.Siren))
            .ForMember(dest => dest.TradeNumber, opt => opt.MapFrom(src => src.Siret))
            .ForMember(dest => dest.VatNumber, opt => opt.MapFrom(src => src.EntrepriseBase.NumeroTvaIntracommunautaire))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => CountryCodeHelper.GetCountryCode(src.PaysFacturation)))
            .ForMember(dest => dest.RemoteId, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.CodeMkgt) ? src.Id.ToString() : src.CodeMkgt))
            .ForMember(dest => dest.AccountCode, opt => opt.MapFrom(src => src.CompteComptable))
            .ForMember(dest => dest.SideAccountCode, opt => opt.MapFrom(src => src.IdOdoo))
            .ForMember(dest => dest.InvoicingRemoteId, opt => opt.MapFrom(src => src.IdOdoo))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => String.Empty))
            .ForMember(dest => dest.PK, opt => opt.MapFrom(src => src.IdDashdoc))
            .ForMember(dest => dest.DashdocPrimaryAddress, opt => opt.MapFrom(src => new DashdocPrimaryAddressDto
            {
                Name = src.Nom,
                Address = $"{src.AdresseFacturation1} {src.AdresseFacturation2} {src.AdresseFacturation3}",
                City = src.VilleFacturation,
                PostCode = src.CodePostalFacturation,
                Country = CountryCodeHelper.GetCountryCode(src.PaysFacturation),
            }));
        
        CreateMap<DashdocCompanyDto, EtablissementClientDto>()
            .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.EmailFacturation, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.TelephoneFacturation, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Siret, opt => opt.MapFrom(src => src.TradeNumber))
            .ForMember(dest => dest.PaysFacturation, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.CodeMkgt, opt => opt.MapFrom(src => src.RemoteId))
            .ForMember(dest => dest.CompteComptable, opt => opt.MapFrom(src => src.AccountCode))
            .ForMember(dest => dest.IdOdoo, opt => opt.MapFrom(src => src.SideAccountCode))
            .ForMember(dest => dest.IdOdoo, opt => opt.MapFrom(src => src.InvoicingRemoteId))
            .ForMember(dest => dest.IdDashdoc, opt => opt.MapFrom(src => src.PK))
            .ForMember(dest => dest.AdresseFacturation1, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.Address))
            .ForMember(dest => dest.VilleFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.City))
            .ForMember(dest => dest.CodePostalFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.PostCode))
            .ForMember(dest => dest.PaysFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.Country));
            

        CreateMap<ClientEuropeDto, DashdocCompanyDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nom))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailFacturation))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.TelephoneFacturation))
            .ForMember(dest => dest.Siren, opt => opt.MapFrom(src => String.Empty))
            .ForMember(dest => dest.VatNumber, opt => opt.MapFrom(src => src.Vat))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => CountryCodeHelper.GetCountryCode(src.PaysFacturation)))
            .ForMember(dest => dest.RemoteId, opt => opt.MapFrom(src => src.CodeMkgt))
            .ForMember(dest => dest.AccountCode, opt => opt.MapFrom(src => src.CompteComptable))
            .ForMember(dest => dest.SideAccountCode, opt => opt.MapFrom(src => src.IdOdoo))
            .ForMember(dest => dest.InvoicingRemoteId, opt => opt.MapFrom(src => src.IdOdoo))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => String.Empty))
            .ForMember(dest => dest.PK, opt => opt.MapFrom(src => src.IdDashdoc))
            .ForMember(dest => dest.DashdocPrimaryAddress, opt => opt.MapFrom(src => new DashdocPrimaryAddressDto
            {
                Name = src.Nom,
                Address = $"{src.AdresseFacturation1} {src.AdresseFacturation2} {src.AdresseFacturation3}",
                City = src.VilleFacturation,
                PostCode = src.CodePostalFacturation,
                Country = CountryCodeHelper.GetCountryCode(src.PaysFacturation),
            }));
        
        CreateMap<DashdocCompanyDto, ClientEuropeDto>()
            .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.EmailFacturation, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.TelephoneFacturation, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Vat, opt => opt.MapFrom(src => src.VatNumber))
            .ForMember(dest => dest.PaysFacturation, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.CodeMkgt, opt => opt.MapFrom(src => src.RemoteId))
            .ForMember(dest => dest.CompteComptable, opt => opt.MapFrom(src => src.AccountCode))
            .ForMember(dest => dest.IdOdoo, opt => opt.MapFrom(src => src.SideAccountCode))
            .ForMember(dest => dest.IdOdoo, opt => opt.MapFrom(src => src.InvoicingRemoteId))
            .ForMember(dest => dest.IdDashdoc, opt => opt.MapFrom(src => src.PK))
            .ForMember(dest => dest.AdresseFacturation1, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.Address))
            .ForMember(dest => dest.VilleFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.City))
            .ForMember(dest => dest.CodePostalFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.PostCode))
            .ForMember(dest => dest.PaysFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.Country));
        
        CreateMap<ClientParticulierDto, DashdocShipperDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nom + " " + src.Prenom))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailFacturation))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.TelephoneFacturation))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => CountryCodeHelper.GetCountryCode(src.PaysFacturation)))
            .ForMember(dest => dest.RemoteId, opt => opt.MapFrom(src => src.CodeMkgt))
            .ForMember(dest => dest.AccountCode, opt => opt.MapFrom(src => src.CompteComptable))
            .ForMember(dest => dest.SideAccountCode, opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.InvoicingRemoteId, opt => opt.MapFrom(src => src.IdOdoo))
            .ForMember(dest => dest.PK, opt => opt.MapFrom(src => src.IdShipperDashdoc))
            .ForMember(dest => dest.DashdocPrimaryAddress, opt => opt.MapFrom(src => new DashdocPrimaryAddressDto
            {
                Name = src.Nom,
                Address = $"{src.AdresseFacturation1} {src.AdresseFacturation2} {src.AdresseFacturation3}",
                City = src.VilleFacturation,
                PostCode = src.CodePostalFacturation,
                Country = CountryCodeHelper.GetCountryCode(src.PaysFacturation),
            }));
        
        CreateMap<DashdocShipperDto, ClientParticulierDto>()
            .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.EmailFacturation, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.TelephoneFacturation, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.PaysFacturation, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.CodeMkgt, opt => opt.MapFrom(src => src.RemoteId))
            .ForMember(dest => dest.CompteComptable, opt => opt.MapFrom(src => src.AccountCode))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ConvertToInt(src.SideAccountCode)))
            .ForMember(dest => dest.IdOdoo, opt => opt.MapFrom(src => src.InvoicingRemoteId))
            .ForMember(dest => dest.IdShipperDashdoc, opt => opt.MapFrom(src => src.PK))
            .ForMember(dest => dest.AdresseFacturation1, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.Address))
            .ForMember(dest => dest.VilleFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.City))
            .ForMember(dest => dest.CodePostalFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.PostCode))
            .ForMember(dest => dest.PaysFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.Country));
        
         CreateMap<EtablissementClientDto, DashdocShipperDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nom))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailFacturation))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.TelephoneFacturation))
            .ForMember(dest => dest.Siren, opt => opt.MapFrom(src => src.Siren))
            .ForMember(dest => dest.TradeNumber, opt => opt.MapFrom(src => src.Siret))
            .ForMember(dest => dest.VatNumber, opt => opt.MapFrom(src => src.EntrepriseBase.NumeroTvaIntracommunautaire))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => CountryCodeHelper.GetCountryCode(src.PaysFacturation)))
            .ForMember(dest => dest.RemoteId, opt => opt.MapFrom(src => src.CodeMkgt))
            .ForMember(dest => dest.AccountCode, opt => opt.MapFrom(src => src.CompteComptable))
            .ForMember(dest => dest.SideAccountCode, opt => opt.MapFrom(src => src.IdOdoo))
            .ForMember(dest => dest.InvoicingRemoteId, opt => opt.MapFrom(src => src.IdOdoo))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => String.Empty))
            .ForMember(dest => dest.PK, opt => opt.MapFrom(src => src.IdShipperDashdoc))
            .ForMember(dest => dest.DashdocPrimaryAddress, opt => opt.MapFrom(src => new DashdocPrimaryAddressDto
            {
                Name = src.Nom,
                Address = $"{src.AdresseFacturation1} {src.AdresseFacturation2} {src.AdresseFacturation3}",
                City = src.VilleFacturation,
                PostCode = src.CodePostalFacturation,
                Country = CountryCodeHelper.GetCountryCode(src.PaysFacturation),
            }));
        
        CreateMap<DashdocShipperDto, EtablissementClientDto>()
            .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.EmailFacturation, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.TelephoneFacturation, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Siret, opt => opt.MapFrom(src => src.TradeNumber))
            .ForMember(dest => dest.PaysFacturation, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.CodeMkgt, opt => opt.MapFrom(src => src.RemoteId))
            .ForMember(dest => dest.CompteComptable, opt => opt.MapFrom(src => src.AccountCode))
            .ForMember(dest => dest.IdOdoo, opt => opt.MapFrom(src => src.SideAccountCode))
            .ForMember(dest => dest.IdOdoo, opt => opt.MapFrom(src => src.InvoicingRemoteId))
            .ForMember(dest => dest.IdShipperDashdoc, opt => opt.MapFrom(src => src.PK))
            .ForMember(dest => dest.AdresseFacturation1, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.Address))
            .ForMember(dest => dest.VilleFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.City))
            .ForMember(dest => dest.CodePostalFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.PostCode))
            .ForMember(dest => dest.PaysFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.Country));
        
                CreateMap<ClientEuropeDto, DashdocShipperDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Nom))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailFacturation))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.TelephoneFacturation))
            .ForMember(dest => dest.Siren, opt => opt.MapFrom(src => String.Empty))
            .ForMember(dest => dest.VatNumber, opt => opt.MapFrom(src => src.Vat))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => CountryCodeHelper.GetCountryCode(src.PaysFacturation)))
            .ForMember(dest => dest.RemoteId, opt => opt.MapFrom(src => src.CodeMkgt))
            .ForMember(dest => dest.AccountCode, opt => opt.MapFrom(src => src.CompteComptable))
            .ForMember(dest => dest.SideAccountCode, opt => opt.MapFrom(src => src.IdOdoo))
            .ForMember(dest => dest.InvoicingRemoteId, opt => opt.MapFrom(src => src.IdOdoo))
            .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => String.Empty))
            .ForMember(dest => dest.PK, opt => opt.MapFrom(src => src.IdShipperDashdoc))
            .ForMember(dest => dest.DashdocPrimaryAddress, opt => opt.MapFrom(src => new DashdocPrimaryAddressDto
            {
                Name = src.Nom,
                Address = $"{src.AdresseFacturation1} {src.AdresseFacturation2} {src.AdresseFacturation3}",
                City = src.VilleFacturation,
                PostCode = src.CodePostalFacturation,
                Country = CountryCodeHelper.GetCountryCode(src.PaysFacturation),
            }));
        
        CreateMap<DashdocShipperDto, ClientEuropeDto>()
            .ForMember(dest => dest.Nom, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.EmailFacturation, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.TelephoneFacturation, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Vat, opt => opt.MapFrom(src => src.VatNumber))
            .ForMember(dest => dest.PaysFacturation, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.CodeMkgt, opt => opt.MapFrom(src => src.RemoteId))
            .ForMember(dest => dest.CompteComptable, opt => opt.MapFrom(src => src.AccountCode))
            .ForMember(dest => dest.IdOdoo, opt => opt.MapFrom(src => src.SideAccountCode))
            .ForMember(dest => dest.IdOdoo, opt => opt.MapFrom(src => src.InvoicingRemoteId))
            .ForMember(dest => dest.IdShipperDashdoc, opt => opt.MapFrom(src => src.PK))
            .ForMember(dest => dest.AdresseFacturation1, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.Address))
            .ForMember(dest => dest.VilleFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.City))
            .ForMember(dest => dest.CodePostalFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.PostCode))
            .ForMember(dest => dest.PaysFacturation, opt => opt.MapFrom(src => src.DashdocPrimaryAddress.Country));
    }
    
    public static int ConvertToInt(string sideAccountCode)
    {
        return int.TryParse(sideAccountCode, out var id) ? id : 0; // Retourne 0 si la conversion échoue
    }
}