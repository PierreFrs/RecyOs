// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IRepository.cs
// Created : 2024/02/23 - 15:39
// Updated : 2024/02/23 - 15:39

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces;

public interface IBaseRepository<T>
{
    Task<T> CreateAsync(T entity, ContextSession session);
    
    Task<IReadOnlyList<T>> GetListAsync(bool includeDeleted = false);
    
    Task<T> GetByIdAsync(int id, ContextSession session, bool includeDeleted = false);
    
    Task<T> UpdateAsync(T entity, ContextSession session);
    
    Task<bool> DeleteAsync(int id, ContextSession session);
}