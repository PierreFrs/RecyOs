// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IBalanceFranceService.cs
// Created : 2024/02/26 - 12:12
// Updated : 2024/02/26 - 12:12

using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Models.DTO.hub.BalanceDtos;

namespace RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

public interface IBalanceFranceService : IBalanceService<BalanceDto>
{
    Task<GridData<BalanceFranceDto>> GetDataForGrid(BalanceFranceGridFilter filter, bool includeDeleted = false);
}