// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IBalanceService.cs
// Created : 2024/02/26 - 12:05
// Updated : 2024/02/26 - 12:05

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

public interface IBalanceService<TDto>
{
    Task<TDto> CreateAsync(TDto dto);
    Task<IReadOnlyList<TDto>> GetAllAsync(bool includeDeleted = false);
    Task<TDto> GetByIdAsync(int id, bool includeDeleted = false);
    Task<IReadOnlyList<TDto>> GetByClientIdAsync(int clientId, bool includeDeleted = false);
    Task<TDto> UpdateAsync(int id, TDto dto);
    Task<bool> DeleteAsync(int id);
}
