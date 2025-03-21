// Created by : Pierre FRAISSE
// RecyOs => RecyOs => SocieteDtoUpdate.cs
// Created : 2024/02/23 - 15:40
// Updated : 2024/02/23 - 15:40

namespace RecyOs.ORM.DTO.hub;


public class SocieteDtoUpdate : TrackedDto
{
    #nullable enable
    public string? Nom { get; set; }
    public string? IdOdoo { get; set; }
    #nullable disable
}