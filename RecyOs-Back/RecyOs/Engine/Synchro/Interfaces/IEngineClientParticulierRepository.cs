using System.Collections.Generic;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Engine.Interfaces;

public interface IEngineClientParticulierRepository
{
    public IList<ClientParticulier> GetUpdatedEntities(string moduleName);
    public IList<ClientParticulier> GetCreatedEntities(string moduleName);
    public IList<ClientParticulier> CallBackDestIdCreation(string moduleName, IList<ClientParticulier> items);
}