// <copyright file="IBaseFactorClientBuRepository.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces;

public interface IBaseFactorClientBuRepository<TFactorClientBu>
{
    Task<TFactorClientBu> CreateAsync(TFactorClientBu entity, ContextSession session);
    
    Task<IReadOnlyList<TFactorClientBu>> GetListAsync(ContextSession session, bool includeDeleted = false);
    
    Task<IReadOnlyList<TFactorClientBu>> GetByClientIdAsync(ContextSession session, int clientId, bool includeDeleted = false);
    
    Task<IReadOnlyList<TFactorClientBu>> GetByBuIdAsync(ContextSession session, int buId, bool includeDeleted = false);
    
    Task<bool> DeleteAsync(int clientId, int buId, ContextSession session);
}