// BalanceService.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 29/08/2024
// Fichier Modifié le : 29/08/2024
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

namespace RecyOs.ORM.Service.hub;

public class BalanceService<TEntity, TDto> : IBalanceService<TDto>
    where TEntity : class
    where TDto : class
{
    private readonly IBalanceRepository<TEntity> _balanceRepository;
    private readonly IMapper _mapper;

    public BalanceService(
        ICurrentContextProvider contextProvider,
        IBalanceRepository<TEntity> balanceRepository,
        IMapper mapper
    )
    {
        _balanceRepository = balanceRepository;
        _mapper = mapper;
    }

    public async Task<TDto> CreateAsync(TDto dto)
    {
        try
        {
            var entity = _mapper.Map<TEntity>(dto);
            entity = await _balanceRepository.CreateAsync(entity, new ContextSession());
            return _mapper.Map<TDto>(entity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<IReadOnlyList<TDto>> GetAllAsync(bool includeDeleted = false)
    {
        var balances = await _balanceRepository.GetAllAsync(new ContextSession(), includeDeleted);
        return _mapper.Map<IReadOnlyList<TDto>>(balances);
    }
    
    public async Task<TDto> GetByIdAsync(int id, bool includeDeleted = false)
    {
        var entity = await _balanceRepository.GetByIdAsync(id, new ContextSession(), includeDeleted);
        return _mapper.Map<TDto>(entity);
    }
    
    public async Task<IReadOnlyList<TDto>> GetByClientIdAsync(int clientId, bool includeDeleted = false)
    {
        var balances = await _balanceRepository.GetByClientIdAsync(clientId, new ContextSession(), includeDeleted);
        return _mapper.Map<IReadOnlyList<TDto>>(balances);
    }
    
    public async Task<TDto> UpdateAsync(int id, TDto dto)
    {
        var entity = _mapper.Map<TEntity>(dto);
        entity = await _balanceRepository.UpdateAsync(id, entity, new ContextSession());
        return _mapper.Map<TDto>(entity);
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _balanceRepository.DeleteAsync(id, new ContextSession());
    }
}