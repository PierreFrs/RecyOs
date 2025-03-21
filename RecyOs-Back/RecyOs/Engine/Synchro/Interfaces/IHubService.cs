//  IHubService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 17/05/2023
// Fichier Modifié le : 30/11/2024
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;

namespace RecyOs.Engine.Interfaces;

public interface IHubService<TSourceDTO> where TSourceDTO : class
{
    public IList<TSourceDTO> GetCreatedItems(string prmModuleName);
    public IList<TSourceDTO> GetUpdatedItems(string prmModuleName);
    public IList<TSourceDTO> CallBackDestIdCreation(string prmModuleName, IList<TSourceDTO> prmItems);
}