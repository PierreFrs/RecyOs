using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Hub.DTO;

namespace RecyOs.ORM.Interfaces.gpiSync;

public interface IGpiSyncService
{
    Task<IList<ClientGpiDto>> GetChangedCustomers();
}