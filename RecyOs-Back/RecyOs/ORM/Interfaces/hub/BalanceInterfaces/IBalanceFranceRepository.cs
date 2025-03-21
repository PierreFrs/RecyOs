// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IBalanceFranceRepository.cs
// Created : 2024/02/26 - 12:13
// Updated : 2024/02/26 - 12:13

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

public interface IBalanceFranceRepository : IBalanceRepository<BalanceFrance>
{
    Task<ServiceResult> UpdateClientIdInBalancesAsync(int oldEtablissementClientId, int newEtablissementId, ContextSession session);
    Task<(IEnumerable<BalanceFrance>,int, decimal)> GetFilteredListWithCount(BalanceFranceGridFilter filter, bool includeDeleted = false);
}