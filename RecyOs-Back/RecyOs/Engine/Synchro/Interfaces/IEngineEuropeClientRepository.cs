//  IEngineEuropeClientRepository.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 16/10/2023
// Fichier Modifié le : 30/11/2024
// Code développé pour le projet : RecyOs
using System.Collections.Generic;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Engine.Interfaces;

public interface IEngineEuropeClientRepository
{
    public IList<ClientEurope> GetUpdatedEntities(string moduleName);
    public IList<ClientEurope> GetCreatedEntities(string moduleName);
    public IList<ClientEurope> CallBackDestIdCreation(string moduleName, IList<ClientEurope> items);
}