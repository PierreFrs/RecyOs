// Created by : Pierre FRAISSE
// RecyOs => RecyOs => Companies.cs
// Created : 2024/04/16 - 13:44
// Updated : 2024/04/16 - 13:44

namespace RecyOs.HubSpotDB.Entities;

public class Companies
{
    #nullable enable
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; } = "PARTNER";
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? Zip { get; set; }
    public string? HubSpotOwnerId { get; set; }
    public string? FoundedYear { get; set; }
    public string? Country { get; set; }
    public string? Description { get; set; }
    public string? Domain { get; set; }
    public string? DateCreationFicheRecyOs { get; set; }
    public string? CodeMkgt { get; set; }
    public string? CouvertureClient { get; set; }
    public string? Siren { get; set; }
    #nullable disable
}