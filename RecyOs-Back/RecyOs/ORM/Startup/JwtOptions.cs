// /** JwtOptions.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.ORM
//  */

using System;
using Microsoft.IdentityModel.Tokens;

namespace RecyOs.ORM.Startup;

public class JwtOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Subject { get; set; }
    public byte[] AccessSecret { get; set; }
    public byte[] RefreshSecret { get; set; }
    public DateTime IssuedAt => DateTime.UtcNow;
    public TimeSpan AccessValidFor { get; set; } = TimeSpan.FromMinutes(60);
    public TimeSpan RefreshValidFor { get; set; } = TimeSpan.FromMinutes(43200);
    public DateTime NotBefore => DateTime.UtcNow;
    public DateTime AccessExpiration => IssuedAt.Add(AccessValidFor);
    public DateTime RefreshExpiration => IssuedAt.Add(RefreshValidFor);
    public SigningCredentials AccessSigningCredentials { get; set; }
    public SigningCredentials RefreshSigningCredentials { get; set; }
}