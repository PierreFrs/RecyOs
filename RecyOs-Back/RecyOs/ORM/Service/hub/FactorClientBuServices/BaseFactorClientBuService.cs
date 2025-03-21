// <copyright file="BaseServiceImplementation.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.Exceptions;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class BaseFactorClientBuService<TEntity, TDto> : IBaseFactorClientBuService<TEntity, TDto>
    where TEntity : class, new()
    where TDto : FactorClientBuDto
{
    private readonly IBaseFactorClientBuRepository<TEntity> _repository;
    private readonly IMapper _mapper;
    private readonly ITokenInfoService _tokenInfoService;

    public BaseFactorClientBuService(
        IBaseFactorClientBuRepository<TEntity> repository, 
        IMapper mapper,
        ITokenInfoService tokenInfoService
        )
    {
        _repository = repository;
        _mapper = mapper;
        _tokenInfoService = tokenInfoService;
    }
    
    public virtual async Task<TDto> CreateAsync(TDto dto)
    {
        try
        {
            var entity = _mapper.Map<TEntity>(dto);
            var createdEntity = await _repository.CreateAsync(entity, new ContextSession());
            return _mapper.Map<TDto>(createdEntity);
        }
        catch (RepositoryException ex)
        {
            throw new ServiceException("Error occurred in the repository layer.", ex);
        }
        catch (Exception ex)
        {
            throw new ServiceException("An unexpected error occurred in the service layer.", ex);
        }
    }


    public async Task<IReadOnlyList<TDto>> GetListAsync()
    {
        var entities = await _repository.GetListAsync(new ContextSession());
        return _mapper.Map<IReadOnlyList<TDto>>(entities);
    }
    
    public async Task<IReadOnlyList<TDto>> GetByClientIdAsync(int clientId)
    {
        var entities = await _repository.GetByClientIdAsync(new ContextSession(), clientId);
        return _mapper.Map<IReadOnlyList<TDto>>(entities);
    }
    
    public async Task<IReadOnlyList<TDto>> GetByBuIdAsync(int buId)
    {
        var entities = await _repository.GetByBuIdAsync(new ContextSession(), buId);
        return _mapper.Map<IReadOnlyList<TDto>>(entities);
    }
    
    public async Task<IEnumerable<TDto>> UpdateBatchAsync(FactorBatchRequest request)
    {
        try
        {
            var existingBus = await GetByClientIdAsync(request.ClientId);

            var existingBuIds = existingBus.Select(b => b.IdBu).ToHashSet();
            var newBuIds = request.BuIds.ToHashSet();

            var buIdsToAdd = newBuIds.Except(existingBuIds).ToList();
            var buIdsToRemove = existingBuIds.Except(newBuIds).ToList();

            var results = new List<TDto>();

            // Remove outdated BUs
            foreach (var buId in buIdsToRemove)
            {
                var factorClientBu = existingBus.FirstOrDefault(b => b.IdBu == buId);
                if (factorClientBu != null)
                {
                    await DeleteAsync(factorClientBu.IdClient, factorClientBu.IdBu);
                }
            }

            // Add new BUs
            foreach (var buId in buIdsToAdd)
            {
                var dtoCreate = Activator.CreateInstance<TDto>();
                dtoCreate.IdClient = request.ClientId;
                dtoCreate.IdBu = buId;
                dtoCreate.CreateDate = DateTime.Now;
                dtoCreate.CreatedBy = _tokenInfoService.GetCurrentUserName();

                var result = await CreateAsync(dtoCreate);
                results.Add(result);
            }

            // Get the updated list of BUs
            var updatedBus = await GetByClientIdAsync(request.ClientId);
            return updatedBus;
        }
        catch (RepositoryException ex)
        {
           throw new ServiceException("An unexpected error occurred in the repository layer.", ex);
        }
        catch (ServiceException ex)
        {
            throw new ServiceException("An unexpected error occurred in the service layer.", ex);
        }
        catch (Exception ex)
        {
            throw new ServiceException("An unexpected error occurred while updating the batch.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int clientId, int buId)
    {
        return await _repository.DeleteAsync(clientId, buId, new ContextSession());
    }
}