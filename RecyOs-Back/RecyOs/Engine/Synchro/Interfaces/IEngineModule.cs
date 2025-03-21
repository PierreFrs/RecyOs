//  IEngineModule.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 16/04/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Engine.Interfaces;

public interface IEngineModule
{
    public string ModuleName { get; }
    public bool SyncData();
    
}