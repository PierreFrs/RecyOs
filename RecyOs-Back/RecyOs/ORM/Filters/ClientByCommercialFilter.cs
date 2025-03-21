// Created by : Pierre FRAISSE
// RecyOs => RecyOs => ClientByCommercialFilter.cs
// Created : 2024/03/27 - 10:05
// Updated : 2024/03/27 - 10:05

namespace RecyOs.ORM.Filters;

public class ClientByCommercialFilter : BaseFilter
{
    public string SearchByNom { get; set; }
    public string SearchByIdentifiant { get; set; }
    public string SearchByCodeMkgt { get; set; }
    public string SearchByIdOdoo { get; set; }
    public string SearchByCodeGpi { get; set; }
    public bool FilterByMkgt { get; set; } = false;
    public bool FilterByGpi { get; set; } = false;
}