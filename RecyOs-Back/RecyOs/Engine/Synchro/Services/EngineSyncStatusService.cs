using System.Collections.Generic;
using AutoMapper;
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.DTO;

namespace RecyOs.Engine.Services;

public class EngineSyncStatusService : IEngineSyncStatusService
{
    private readonly IEngineSyncStatusRepository _repository;
    private readonly IMapper _mapper;
    
    public EngineSyncStatusService(IEngineSyncStatusRepository prmRepo, IMapper mapper)
    {
        _mapper = mapper;
        _repository = prmRepo;
    }

    /// <summary>
    /// Retrieves the engine sync status DTO by module name.
    /// </summary>
    /// <param name="prmValue">The module name.</param>
    /// <returns>The engine sync status DTO with the specified module name.</returns>
    public EngineSyncStatusDto GetByModuleName(string prmValue)
    {
        var entity = _repository.GetByModuleName(prmValue);
        var dto = _mapper.Map<EngineSyncStatusDto>(entity);
        return dto;
    }
    
    /// <summary>
    /// Retrieves a list of enabled engine sync status DTOs.
    /// </summary>
    /// <returns>A list of enabled engine sync status DTOs.</returns>
    public IList<EngineSyncStatusDto> GetEnabledModules()
    {
        var entities = _repository.GetEnabledModules();
        var dtos = _mapper.Map<IList<EngineSyncStatusDto>>(entities);
        return dtos;
    }
}