// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IBalanceEuropeService.cs
// Created : 2024/02/26 - 14:42
// Updated : 2024/02/26 - 14:42

using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Models.DTO.hub.BalanceDtos;


namespace RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

public interface IBalanceEuropeService : IBalanceService<BalanceDto>
{
    Task<GridData<BalanceEuropeDto>> GetDataForGrid(BalanceEuropeGridFilter filter, bool includeDeleted = false);
}