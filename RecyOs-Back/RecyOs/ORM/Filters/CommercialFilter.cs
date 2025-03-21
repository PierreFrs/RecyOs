// Created by : Pierre FRAISSE
// RecyOs => RecyOs => CommercialFilter.cs
// Created : 2024/03/28 - 11:50
// Updated : 2024/03/28 - 11:50

namespace RecyOs.ORM.Filters;

public class CommercialFilter: BaseFilter
{
    public string FilteredByNom { get; set; }
    public string FilteredByPrenom { get; set; }
}