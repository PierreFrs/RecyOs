// StringNormalizerHelper.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 16/10/2024
// Fichier Modifié le : 16/10/2024
// Code développé pour le projet : RecyOs

using System.Globalization;
using System.Text;

namespace RecyOs.Helpers;

public class StringNormalizerHelper
{
    protected StringNormalizerHelper()
    {
    }

    public static string RemoveAccentsAndUppercase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Convert to uppercase
        input = input.ToUpper();

        // Normalize the input string to decompose accents
        string normalizedString = input.Normalize(NormalizationForm.FormD);

        // Create a StringBuilder to collect characters without diacritical marks
        StringBuilder stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            // Check if the character is a non-spacing mark (accent)
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        // Return the cleaned string (remove all combining characters)
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}