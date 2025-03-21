// Created by : Pierre FRAISSE
// RecyOs => RecyOs => CommercialService.cs
// Created : 2024/03/26 - 15:20
// Updated : 2024/03/26 - 15:20

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

namespace RecyOs.ORM.Service;

public class CommercialBaseService : BaseService, ICommercialBaseService
{
    private readonly ICommercialBaseRepository _commercialBaseRepository;
    private readonly IMapper _mapper;
    private readonly ITokenInfoService _tokenInfoService;
    public CommercialBaseService(
        ICurrentContextProvider contextProvider,
        ICommercialBaseRepository commercialBaseRepository,
        IMapper mapper,
        ITokenInfoService tokenInfoService
        ) : base(contextProvider)
    {
        _commercialBaseRepository = commercialBaseRepository;
        _mapper = mapper;
        _tokenInfoService = tokenInfoService;
    }

    public async Task<CommercialDto> CreateAsync(CommercialDto dto)
    {
        var currentUser = _tokenInfoService.GetCurrentUserName();
        
        var entity = _mapper.Map<Commercial>(dto);
        entity.CreatedBy = currentUser;
        entity.CreateDate = DateTime.Now;
        
        var newEntity = await _commercialBaseRepository.CreateAsync(entity, Session);
        var newEntityDto = _mapper.Map<CommercialDto>(newEntity);
        return newEntityDto;
    }

    public async Task<IReadOnlyList<CommercialDto>> GetListAsync()
    {
        var entities = await _commercialBaseRepository.GetListAsync();
        var entitiesDto = _mapper.Map<IReadOnlyList<CommercialDto>>(entities);
        return entitiesDto;
    }
    
    public async Task<GridData<CommercialDto>> GetFilteredListAsync(CommercialFilter filter)
    {
        var (commercials, total) = await _commercialBaseRepository.GetFilteredListAsync(filter, Session);
        var commercialDtos = _mapper.Map<IEnumerable<CommercialDto>>(commercials);

        return new GridData<CommercialDto>
        {
            Items = commercialDtos,
            Paginator = new Pagination
            {
                length = total,
                size = filter.PageSize,
                page = filter.PageNumber,
                lastPage = (int)Math.Ceiling(total / (double)filter.PageSize),
                startIndex = (filter.PageNumber - 1) * filter.PageSize,
            }
        };
    }
    
    public async Task<CommercialDto> GetByIdAsync(int id)
    {
        var entity = await _commercialBaseRepository.GetByIdAsync(id, Session);
        var entityDto = _mapper.Map<CommercialDto>(entity);
        return entityDto;
    }
    
    public async Task<GridData<object>> GetClientsByCommercialIdAsync(int commercialId, ClientByCommercialFilter filter)
    {
        var (clients, totalCount) = await _commercialBaseRepository.GetClientsByCommercialIdAsyncWithCount(commercialId, filter);
        var ratio = totalCount / (double)filter.PageSize;
        var begin = filter.PageNumber  * filter.PageSize;
        
        var mappedClients = new List<object>();
        foreach (var client in clients)
        {
            switch (client)
            {
                case EtablissementClient etablissementClient:
                    mappedClients.Add(_mapper.Map<EtablissementClientDto>(etablissementClient));
                    break;
                case ClientEurope clientEurope:
                    mappedClients.Add(_mapper.Map<ClientEuropeDto>(clientEurope));
                    break;
            }
        }
        
        return new GridData<object>
        {
            Items = mappedClients,
            Paginator = new Pagination()
            {
                length = totalCount,
                size = filter.PageSize,
                page = filter.PageNumber,
                lastPage = (int)Math.Max(Math.Ceiling(ratio),1.0),
                startIndex = begin,
            }
        };
    }
    
    public async Task<CommercialDto> UpdateAsync(int id, CommercialDto dto)
    {
        var entity = _mapper.Map<Commercial>(dto);
        entity.Id = id;

        entity.UpdatedBy = _tokenInfoService.GetCurrentUserName();
        entity.UpdatedAt = DateTime.Now;

        var updatedEntity = await _commercialBaseRepository.UpdateAsync(entity, Session);
        var updatedEntityDto = _mapper.Map<CommercialDto>(updatedEntity);
        
        return updatedEntityDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {        
        return await _commercialBaseRepository.DeleteAsync(id, Session);
    }
}