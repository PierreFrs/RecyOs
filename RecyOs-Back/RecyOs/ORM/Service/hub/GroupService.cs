using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Filters;
using System.Linq;

namespace RecyOs.ORM.Service.hub;

public class GroupService : BaseService, IGroupService
{
    private readonly IGroupRepository _repository;
    private readonly IMapper _mapper;
    private readonly ITokenInfoService _tokenInfoService;

    public GroupService(
        ICurrentContextProvider contextProvider,
        IGroupRepository repository,
        IMapper mapper,
        ITokenInfoService tokenInfoService) : base(contextProvider)
    {
        _repository = repository;
        _mapper = mapper;
        _tokenInfoService = tokenInfoService;
    }

    public async Task<GroupDto> CreateAsync(GroupDto createDto)
    {
        var entity = _mapper.Map<Group>(createDto);
        entity.CreatedBy = _tokenInfoService.GetCurrentUserName();
        entity.CreateDate = DateTime.Now;
        
        var createdEntity = await _repository.CreateAsync(entity, new ContextSession());
        return _mapper.Map<GroupDto>(createdEntity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id, new ContextSession());
    }

    public async Task<GroupDto> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id, new ContextSession());
        return _mapper.Map<GroupDto>(entity);
    }

    public async Task<GroupDto> GetByNameAsync(string name)
    {
        var entity = await _repository.GetByNameAsync(name, new ContextSession());
        return _mapper.Map<GroupDto>(entity);
    }

    public async Task<IReadOnlyList<GroupDto>> GetListAsync()
    {
        var entities = await _repository.GetListAsync();
        return _mapper.Map<IReadOnlyList<GroupDto>>(entities);
    }

    public async Task<GroupDto> UpdateAsync(int id, GroupDto dtoUpdate)
    {
        var entity = _mapper.Map<Group>(dtoUpdate);
        entity.Id = id;
        entity.UpdatedBy = _tokenInfoService.GetCurrentUserName();
        entity.UpdatedAt = DateTime.Now;
        
        var updatedEntity = await _repository.UpdateAsync(entity, new ContextSession());
        return _mapper.Map<GroupDto>(updatedEntity);
    }

    public async Task<GridData<GroupDto>> GetFilteredListWithClientsAsync(GroupFilter filter)
    {
        var (groups, totalCount) = await _repository.GetFilteredListWithClientsAsync(filter, Session);
        var groupDtos = _mapper.Map<IEnumerable<GroupDto>>(groups);

        foreach (var groupDto in groupDtos)
        {
            groupDto.FicheCount = await GetFicheCountByGroupIdAsync(groupDto);
        }

        return new GridData<GroupDto>
        {
            Items = groupDtos,
            Paginator = new Pagination
            {
                length = totalCount,
                size = filter.PageSize,
                page = filter.PageNumber,
                lastPage = (int)Math.Ceiling(totalCount / (double)filter.PageSize),
                startIndex = (filter.PageNumber - 1) * filter.PageSize,
            }
        };
    }

    private async Task<int> GetFicheCountByGroupIdAsync(GroupDto group)
    {
        var franceFicheCount = group.EtablissementClients?.Count() ?? 0;
        var europeFicheCount = group.ClientEuropes?.Count() ?? 0;
        return franceFicheCount + europeFicheCount;
    }
} 