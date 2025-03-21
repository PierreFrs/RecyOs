using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using ExcelDataReader;

namespace RecyOs.Helpers;

public static class ImportCouvertureExtensions
{
    /////////////////////////////////////
    /// Import parsing helper methods ///
    ////////////////////////////////////
    
    // Nullable DateTime
    public static DateTime? GetNullableDateTime(this IExcelDataReader reader, Dictionary<string, int> columns, string columnName, DateTime defaultValue = default)
    {
        var value = reader.GetValue(columns[columnName])?.ToString();
        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            return parsedDate;
        }

        return null;
    }

    // Mandatory DateTime
    public static DateTime GetDateTime(this IExcelDataReader reader, Dictionary<string, int> columns, string columnName)
    {
        var value = reader.GetValue(columns[columnName]).ToString();
        if (string.IsNullOrEmpty(value))
        {
            throw new NullReferenceException($"The value for '{columnName}' is empty.");
        }
        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
        {
            return parsedDate;
        }
        throw new FormatException($"Unable to parse '{columnName}' as a DateTime.");
    }

    // Nullable int
    public static int? GetNullableInt(this IExcelDataReader reader, Dictionary<string, int> columns, string columnName)
    {
        var value = reader.GetValue(columns[columnName])?.ToString();
        if (int.TryParse(value, out int parsedInteger) && parsedInteger >= 0)
        {
            return parsedInteger;
        }

        return null;
    }

    // Mandatory int
    public static int GetInt(this IExcelDataReader reader, Dictionary<string, int> columns, string columnName)
    {
        var value = reader.GetValue(columns[columnName])?.ToString();
    
        if (string.IsNullOrEmpty(value))
        {
            throw new NullReferenceException($"The value for '{columnName}' is empty.");
        }

        if (int.TryParse(value, out int parsedInteger) && parsedInteger >= 0)
        {
            return parsedInteger;
        }

        throw new FormatException($"Unable to parse '{columnName}' as a non-negative integer.");
    }
    
    // Nullable decimal
    public static decimal? GetNullableDecimal(this IExcelDataReader reader, Dictionary<string, int> columns, string columnName)
    {
        var value = reader.GetValue(columns[columnName])?.ToString();
        if (decimal.TryParse(value, out decimal parsedDecimal) && parsedDecimal >= 0)
        {
            return parsedDecimal;
        }

        return null;
    }

    // Mandatory decimal
    public static decimal GetDecimal(this IExcelDataReader reader, Dictionary<string, int> columns, string columnName)
    {
        var value = reader.GetValue(columns[columnName]).ToString();
        
        if (string.IsNullOrEmpty(value))
        {
            throw new FormatException($"The value for '{columnName}' is empty.");
        }
        
        if (decimal.TryParse(value, out decimal parsedDecimal) && parsedDecimal >= 0)
        {
            return parsedDecimal;
        }
        throw new FormatException($"Unable to parse '{columnName}' as an integer.");
    }
    
    // Nullable string
    public static string GetNullableString(this IExcelDataReader reader, Dictionary<string, int> columns, string columnName)
    {
        var value = reader.GetValue(columns[columnName]);
        if (value is string stringValue && !string.IsNullOrEmpty(stringValue))
        {
            return stringValue;
        }
        return null;
    }

    // Mandatory string
    public static string GetString(this IExcelDataReader reader, Dictionary<string, int> columns, string columnName)
    {
        var value = reader.GetValue(columns[columnName])?.ToString();
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new NullReferenceException($"The value for '{columnName}' is empty or whitespace.");
        }
        return value;
    }
    
    
    /////////////////////////////////////
    /// Check parsing helper methods ///
    ////////////////////////////////////
    
    // Validation methods
    
    public static bool IsNullOrWhiteSpaceOrInvalidDate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            // If the value is null or white space, it's considered valid for optional fields.
            return false;
        }

        string[] dateFormats = {"yyyy-MM-dd", "yyyy/MM/dd"};
        // Return true if the date is not in a valid format.
        return !ValidateDateTime(value, dateFormats);
    }
    
    public static bool ValidateDateTime(string value, string[] formats)
    {
        return DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }

    public static bool ValidateInt(string value)
    {
        return int.TryParse(value, out _);
    }

    public static bool ValidateDecimal(string value)
    {
        return decimal.TryParse(value, out _);
    }

    public static bool ValidateTimeSpan(string value, string format)
    {
        return TimeSpan.TryParseExact(value, format, CultureInfo.InvariantCulture, out _);
    }
    
    
}