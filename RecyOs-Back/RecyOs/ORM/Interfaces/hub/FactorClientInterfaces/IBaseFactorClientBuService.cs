// <copyright file="IBaseFactorClientBuService.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Interfaces;

public interface IBaseFactorClientBuService<TEntity, TDto>
where TEntity : class, new()
where TDto : FactorClientBuDto
{
    Task<TDto> CreateAsync(TDto dto);
    
    Task<IReadOnlyList<TDto>> GetListAsync();
    
    Task<IReadOnlyList<TDto>> GetByClientIdAsync(int clientId);
    
    Task<IReadOnlyList<TDto>> GetByBuIdAsync(int buId);
    
    Task<IEnumerable<TDto>> UpdateBatchAsync(FactorBatchRequest request);
    
    Task<bool> DeleteAsync(int clientId, int buId);
}