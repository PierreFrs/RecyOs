using System;
using System.Linq;
using AutoMapper;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.OdooDB.DTO;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.OdooDB.Resolvers;

public class CountryNameToIdResolver: IValueResolver<ClientEuropeDto, ResPartnerDto, long>
{
    private readonly IResCountryRepository<ResCountryOdooModel> _countryRepository;

    public CountryNameToIdResolver(IResCountryRepository<ResCountryOdooModel> countryRepository)
    {
        _countryRepository = countryRepository;
    }
    
    public long Resolve(ClientEuropeDto source, ResPartnerDto destination, long destMember, ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.Nom))
        {
            return 0;
        }

        // Récupérer tous les pays
        var allCountries = _countryRepository.GetAllCountries().Result;

        // Recherche insensible à la casse
        var paysFacturationCode = PaysFacturationToCode(source.PaysFacturation).ToUpperInvariant();
        var country = Array.Find(allCountries, c => c.Code?.ToUpperInvariant() == paysFacturationCode);
        
        return country?.Id ?? 0;
    }
    
    private string PaysFacturationToCode(string paysFacturation)
    {
        switch (paysFacturation.ToLower())
        {
            case "belgique":
                return "BE";
            case "luxembourg":
                return "LU";
            case "suisse":
                return "CH";
            case "allemagne":
                return "DE";
            case "espagne":
                return "ES";
            case "italie":
                return "IT";
            case "portugal":
                return "PT";
            case "royaume-uni":
                return "GB";
            case "pays-bas":
                return "NL";
            case "irlande":
                return "IE";
            case "autriche":
                return "AT";
            case "bulgarie":
                return "BG";
            case "chypre":
                return "CY";
            case "croatie":
                return "HR";
            case "danemark":
                return "DK";
            case "estonie":
                return "EE";
            case "finlande":
                return "FI";
            case "grèce":
                return "GR";
            case "hongrie":
                return "HU";
            case "lettonie":
                return "LV";
            case "lituanie":
                return "LT";
            case "malte":
                return "MT";
            case "pologne":
                return "PL";
            case "république tchèque":
                return "CZ";
            case "roumanie":
                return "RO";
            case "slovaquie":
                return "SK";
            case "slovénie":
                return "SI";
            case "suède":
                return "SE";
            default:
                return "XX";
        }
    }
}