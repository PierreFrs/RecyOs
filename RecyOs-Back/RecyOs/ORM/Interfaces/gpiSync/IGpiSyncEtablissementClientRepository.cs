using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Interfaces.gpiSync;

public interface IGpiSyncEtablissementClientRepository<T> where T : EtablissementClient
 {
    Task<IList<T>> GetCreatedEtablissementClient(ContextSession session);
    Task<IList<T>> GetUpdatedEtablissementClient(ContextSession session);
}