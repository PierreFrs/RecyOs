// Created by : Pierre FRAISSE
// RecyOs => RecyOs => DateFormater.cs
// Created : 2024/04/22 - 11:57
// Updated : 2024/04/22 - 11:57

using System;
using System.Globalization;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class DateFormater : IDateFormater
{
    public string FormatToYear(string date)
    {
        if (DateTime.TryParse(date, new CultureInfo("fr-FR"), DateTimeStyles.None, out DateTime parsedDate))
        {
            return parsedDate.Year.ToString();
        }
        else
        {
            return "";
        }
    }
}