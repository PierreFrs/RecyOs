// Created by : Pierre FRAISSE
// RecyOs => RecyOs => ICommercialClient.cs
// Created : 2024/05/02 - 11:13
// Updated : 2024/05/02 - 11:14

namespace RecyOs.ORM.Interfaces;

public interface ICommercialClient
{
    string CodeMkgt { get; set; }
    string CodeGpi { get; set; }
    string IdOdoo { get; set; }
    string Nom { get; set; }
    string Identifiant { get; }
}