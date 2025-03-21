using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEtablissementClientRepository<TEtablissementClient> : IEtablissementRepository<TEtablissementClient>
    where TEtablissementClient : EtablissementClient
{
    
}
