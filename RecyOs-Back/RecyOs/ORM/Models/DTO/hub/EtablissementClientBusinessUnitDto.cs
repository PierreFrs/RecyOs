// Created by : Pierre FRAISSE
// RecyOs => RecyOs => EtablissementClientBusinessUnit.cs
// Created : 2024/01/24 - 09:22
// Updated : 2024/01/24 - 09:22

using RecyOs.ORM.Entities;

namespace RecyOs.ORM.DTO.hub;

public class EtablissementClientBusinessUnitDto : DeletableDtoWithoutId
{
    public int ClientId { get; set; }
    public int BusinessUnitId { get; set; }
}