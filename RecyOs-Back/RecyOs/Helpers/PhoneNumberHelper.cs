// PhoneNumberHelper.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 16/10/2024
// Fichier Modifié le : 16/10/2024
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RecyOs.Helpers;

public class PhoneNumberHelper
{
    protected PhoneNumberHelper()
    {
    }

    // Dictionary to store country phone codes
    private static readonly Dictionary<string, string> CountryCodeMap = new Dictionary<string, string>
    {
        {"FRANCE", "+33"},
        {"ALLEMAGNE", "+49"},
        {"ESPAGNE", "+34"},
        {"ITALIE", "+39"},
        {"BELGIQUE", "+32"},
        {"PAYS-BAS", "+31"},
        {"LUXEMBOURG", "+352"},
        {"SUISSE", "+41"},
        {"AUTRICHE", "+43"},
        {"ROYAUME-UNI", "+44"},
        {"PORTUGAL", "+351"},
        {"IRLANDE", "+353"},
        {"DANEMARK", "+45"},
        {"SUEDE", "+46"},
        {"NORVEGE", "+47"},
        {"FINLANDE", "+358"},
        {"POLOGNE", "+48"},
        {"REPUBLIQUE TCHEQUE", "+420"},
        {"HONGRIE", "+36"},
        {"GRECE", "+30"},
        {"TURQUIE", "+90"},
        {"BULGARIE", "+359"},
        {"ETATS-UNIS", "+1"},
        {"CANADA", "+1"},
        {"AUSTRALIE", "+61"},
        {"NOUVELLE-ZELANDE", "+64"},
        {"JAPON", "+81"},
        {"CHINE", "+86"}
    };

    public static string FormatPhoneNumber(string country, string phoneNumber)
    {
        if (string.IsNullOrEmpty(country) || string.IsNullOrEmpty(phoneNumber))
        {
            return phoneNumber;
        }

        // Si le numéro commence par un indicatif international déjà bien formaté, ne pas le reformater
        if (Regex.IsMatch(phoneNumber, @"^\+\d{1,3} \d( \d{2}){4}$"))
        {
            return phoneNumber; // Numéro déjà formaté
        }

        // Nettoyer le numéro de téléphone (supprime tous les non numériques sauf le "+")
        phoneNumber = Regex.Replace(phoneNumber, @"[^\d+]", "");

        if (CountryCodeMap.TryGetValue(country.ToUpperInvariant(), out string countryCode))
        {
            // Retirer le zéro initial s'il existe
            if (phoneNumber.StartsWith('0'))
            {
                phoneNumber = phoneNumber.Substring(1);
            }

            // Formater le numéro
            string formattedPhoneNumber = FormatPhoneNumberPattern(phoneNumber);

            return $"{countryCode} {formattedPhoneNumber}";
        }

        // Si aucun code pays ne correspond, retourne le numéro brut
        return phoneNumber;
    }

    private static string FormatPhoneNumberPattern(string phoneNumber)
    {
        if (phoneNumber.Length == 9)
        {
            // Format en X XX XX XX XX
            return $"{phoneNumber[0]} {phoneNumber.Substring(1, 2)} {phoneNumber.Substring(3, 2)} {phoneNumber.Substring(5, 2)} {phoneNumber.Substring(7, 2)}";
        }

        return phoneNumber; // Retourne le numéro brut si invalide
    }

}