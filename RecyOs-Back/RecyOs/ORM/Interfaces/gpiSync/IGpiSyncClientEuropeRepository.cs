using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Interfaces.gpiSync;

public interface IGpiSyncClientEuropeRepository<T> where T : ClientEurope
{
    Task<IList<T>> GetCreatedClientEurope(ContextSession session);
    Task<IList<T>> GetUpdatedClientEurope(ContextSession session);
}