// Created by : Pierre FRAISSE
// RecyOs => RecyOs => EmailDomainService.cs
// Created : 2024/04/19 - 16:33
// Updated : 2024/04/19 - 16:33

using System;
using System.Collections.Generic;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class EmailDomainService : IEmailDomainService
{
    private readonly HashSet<string> _bigEmailDomains;

    public EmailDomainService()
    {
        _bigEmailDomains = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "gmail.com", 
            "yahoo.com", 
            "hotmail.com", 
            "hotmail.fr", 
            "outlook.com", 
            "free.fr", 
            "icloud.com", 
            "protonmail.com",
            "aol.com",
            "orange.fr",
            "sfr.fr",
            "laposte.net",
            "wanadoo.fr",
            "bbox.fr",
            "live.fr",
            "live.com",
            "msn.com",
            "me.com",
            "mac.com",
            "gmx.com",
            "gmx.fr",
            "gmx.de",
            "gmx.net",
            "gmx.ch",
            "gmx.at",
            "gmx.co.uk",
            "gmx.us",
            "gmx.eu",
            "neuf.fr",
            "numericable.fr",
            "aliceadsl.fr",
            "nordnet.fr",
            "voila.fr",
            "free.com",
            "skynet.be",
            "test.fr",
            "outlook.fr",
            "yahoo.fr",
            "gmail.fr",
            "poste.net"
        };
    }

    public string GetEmailDomain(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return "";
        }

        var domain = email.Substring(email.IndexOf('@') + 1);

        if (_bigEmailDomains.Contains(domain))
        {
            return "";
        }

        return domain;
    }
}