//  BaseHubService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 17/05/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using RecyOs.Engine.Interfaces;

namespace RecyOs.Engine.Services;

public abstract class BaseHubService<TSourceDto> : IHubService<TSourceDto> where TSourceDto : class
{
    public abstract IList<TSourceDto> GetCreatedItems(string prmModuleName);
    public abstract IList<TSourceDto> GetUpdatedItems(string prmModuleName);
    public abstract IList<TSourceDto> CallBackDestIdCreation(string prmModuleName, IList<TSourceDto> prmItems);
}