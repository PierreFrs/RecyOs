// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IBalanceRepository.cs
// Created : 2024/02/26 - 12:10
// Updated : 2024/02/26 - 12:10

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

public interface IBalanceRepository<T>
{
    Task<T> CreateAsync(T entity, ContextSession session);
    Task<T> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false);
    Task<IReadOnlyList<T>> GetAllAsync(ContextSession session, bool includeDeleted = false);
    Task<IReadOnlyList<T>> GetByClientIdAsync(int clientId, ContextSession session, bool includeDeleted = false);
    Task<T> UpdateAsync(int id, T entity, ContextSession session);
    Task<bool> DeleteAsync(int id, ContextSession session);
}
