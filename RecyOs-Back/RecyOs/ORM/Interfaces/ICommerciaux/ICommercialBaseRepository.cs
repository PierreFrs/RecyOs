// Created by : Pierre FRAISSE
// RecyOs => RecyOs => ICommercialRepository.cs
// Created : 2024/03/26 - 15:06
// Updated : 2024/03/26 - 15:06

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters;

namespace RecyOs.ORM.Interfaces;

#nullable enable
public interface ICommercialBaseRepository : IBaseRepository<Commercial>
{
    Task<(IEnumerable<Commercial>, int)> GetFilteredListAsync(CommercialFilter filter, ContextSession session, bool includeDeleted = false);
    Task<(IEnumerable<object>, int)> GetClientsByCommercialIdAsyncWithCount(int id, ClientByCommercialFilter filter);
    Task<Commercial?> GetByMailAsync(string mail, ContextSession session);
}
#nullable disable