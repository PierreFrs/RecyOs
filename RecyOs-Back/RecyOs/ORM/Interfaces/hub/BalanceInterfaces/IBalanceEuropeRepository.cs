// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IBalanceEuropeRepository.cs
// Created : 2024/02/26 - 14:43
// Updated : 2024/02/26 - 14:43

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

public interface IBalanceEuropeRepository : IBalanceRepository<BalanceEurope>
{
    Task<(IEnumerable<BalanceEurope>, int, decimal)> GetFilteredListWithCount(BalanceEuropeGridFilter filter, bool includeDeleted = false);
}