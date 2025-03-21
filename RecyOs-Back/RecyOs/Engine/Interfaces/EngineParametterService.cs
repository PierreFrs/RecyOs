using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.Engine.Interfaces;

public class EngineParametterService : IEngineParametterService
{
    private readonly IEngineParametterRepository _repository;
    private readonly IMapper _mapper;
    
    public EngineParametterService(IEngineParametterRepository prmRepo, IMapper mapper)
    {
        _mapper = mapper;
        _repository = prmRepo;
    }
    
    public async Task<ParameterDto> GetByNomAsync(string module, string nom, bool includeDeleted = false)
    {
        var entity = await _repository.GetByNomAsync(module, nom, includeDeleted);
        var dto = _mapper.Map<ParameterDto>(entity);
        return dto;
    }
    
    public async Task<ParameterDto> GetAsync(int id, bool includeDeleted = false)
    {
        var entity = await _repository.GetAsync(id, includeDeleted);
        var dto = _mapper.Map<ParameterDto>(entity);
        return dto;
    }
    
    public async Task<ParameterDto> CreateAsync(ParameterDto prm)
    {
        var entity = _mapper.Map<Parameter>(prm);
        var ret = await _repository.CreateAsync(entity);
        var dto = _mapper.Map<ParameterDto>(ret);
        return dto;
    }
}