// Created by : Pierre FRAISSE
// RecyOs => RecyOs => CommercialDto.cs
// Created : 2024/03/26 - 14:50
// Updated : 2024/03/26 - 14:50

using System.Collections.Generic;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.DTO;

public class CommercialDto : TrackedDto
{
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Username { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string CodeMkgt { get; set; }
    #nullable enable
    public string? IdHubSpot { get; set; }
    public int? UserId { get; set; }
    public User? User { get; set; }
    #nullable disable
    public ICollection<EtablissementClientDto> EtablissementClients { get; set; }
    public ICollection<ClientEuropeDto> ClientEuropes { get; set; }
}