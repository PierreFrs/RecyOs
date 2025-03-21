// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IEmailDomainService.cs
// Created : 2024/04/19 - 16:33
// Updated : 2024/04/19 - 16:33

namespace RecyOs.ORM.Interfaces;

public interface IEmailDomainService
{
    public string GetEmailDomain(string email);
}