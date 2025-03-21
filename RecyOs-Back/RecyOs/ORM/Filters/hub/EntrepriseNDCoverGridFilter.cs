// Created by : Pierre FRAISSE
// RecyOs => RecyOs => EntrepriseNDCoverGridFilter.cs
// Created : 2023/12/19 - 14:05
// Updated : 2023/12/19 - 14:05

namespace RecyOs.ORM.Filters.hub;

public class EntrepriseNDCoverGridFilter : BaseFilter
{
    public string FilteredBSiren { get; set; }
    public string Refus { get; set; }
    public string Agreement { get; set; }
}