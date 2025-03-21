// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IDataValidationService.cs
// Created : 2024/03/26 - 16:55
// Updated : 2024/03/26 - 16:55

namespace RecyOs.ORM.Interfaces;

public interface IDataValidationService
{
    bool ValidatePhoneNumber(string phoneNumber);
    bool ValidateEmailAddress(string emailAddress);
}