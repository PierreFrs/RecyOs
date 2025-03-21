// Copyright : Pierre FRAISSE
// RecyOs>RecyOs>CountryCodeHelper.cs
// Created : 2024/05/2121 - 12:05

using System.Collections.Generic;
using System.Collections.Immutable;

namespace RecyOs.Helpers;

public static class CountryCodeHelper
{
    
    public static readonly ImmutableDictionary<string, string> CountryCodes = ImmutableDictionary.CreateRange(new Dictionary<string, string>
    {
        { "ALBANIE", "AL" },
        { "ANDORRE", "AD" },
        { "AUTRICHE", "AT" },
        { "ALLEMAGNE", "DE"},
        { "BELGIQUE", "BE" },
        { "BOSNIE-HERZEGOVINE", "BA" },
        { "BULGARIE", "BG" },
        { "CHYPRE", "CY" },
        { "CROATIE", "HR" },
        { "DANEMARK", "DK" },
        { "ESPAGNE", "ES" },
        { "ESTONIE", "EE" },
        { "FINLANDE", "FI" },
        { "FRANCE", "FR" },
        { "GRECE", "GR" },
        { "HONGRIE", "HU" },
        { "IRLANDE", "IE" },
        { "ISLANDE", "IS" },
        { "ITALIE", "IT" },
        { "LETTONIE", "LV" },
        { "LIECHTENSTEIN", "LI" },
        { "LITUANIE", "LT" },
        { "LUXEMBOURG", "LU" },
        { "MALTE", "MT" },
        { "MOLDAVIE", "MD" },
        { "MONACO", "MC" },
        { "MONTENEGRO", "ME" },
        { "NORVÈGE", "NO" },
        { "PAYS-BAS", "NL" },
        { "POLOGNE", "PL" },
        { "PORTUGAL", "PT" },
        { "REPUBLIQUE TCHEQUE", "CZ" },
        { "ROUMANIE", "RO" },
        { "ROYAUME-UNI", "GB" },
        { "RUSSIE", "RU" },
        { "SAINT-MARIN", "SM" },
        { "SERBIE", "RS" },
        { "SLOVAQUIE", "SK" },
        { "SLOVENIE", "SI" },
        { "SUEDE", "SE" },
        { "SUISSE", "CH" },
        { "UKRAINE", "UA" },
        { "VATICAN", "VA" }
    });

    public static string GetCountryCode(string countryName)
    {
        return CountryCodes.TryGetValue(countryName.ToUpper(), out var code) ? code : countryName;
    }
}