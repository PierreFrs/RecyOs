// Created by : Pierre FRAISSE
// RecyOs => RecyOs => DataValidationService.cs
// Created : 2024/03/26 - 16:55
// Updated : 2024/03/26 - 16:55

using System.Text.RegularExpressions;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class DataValidationService : IDataValidationService
{
    public bool ValidatePhoneNumber(string phoneNumber)
    {
        var phoneRegex = @"^\+33\s[1-9](?:\s\d{2}){4}$";
        return Regex.IsMatch(phoneNumber, phoneRegex);
    }

    public bool ValidateEmailAddress(string emailAddress)
    {
        var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(emailAddress, emailRegex);
    }
}