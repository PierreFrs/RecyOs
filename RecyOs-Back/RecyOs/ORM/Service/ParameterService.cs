using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.IParameters;

namespace RecyOs.ORM.Service;

public class ParameterService<TParameter> : BaseService, IParameterService 
    where TParameter : Parameter, new()
{
    protected readonly IParameterRepository<TParameter> _repository;
    private readonly IMapper _mapper;
    
    public ParameterService(ICurrentContextProvider contextProvider, IParameterRepository<TParameter> repository, 
        IMapper mapper) : base(contextProvider)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<GridData<ParameterDto>> GetDataForGrid(ParameterFilter filter, bool includeDeleted = false)
    {
        var tuple = await _repository.GetDataForGrid(filter, Session, includeDeleted);
        var ratio = tuple.Item2 / (double)filter.PageSize;
        var begin = filter.PageNumber  * filter.PageSize;

        return new GridData<ParameterDto>
        {
            Items = _mapper.Map<IEnumerable<ParameterDto>>(tuple.Item1),
            Paginator = new Pagination()
            {
                length = tuple.Item2,
                size = filter.PageSize,
                page = filter.PageNumber,
                lastPage = (int)Math.Max(Math.Ceiling(ratio),1.0),
                startIndex = begin,
            }
        };
    }
    
    public async Task<ParameterDto> GetById(int id, bool includeDeleted = false)
    {
        var parameter = await _repository.GetAsync(id, Session, includeDeleted);
        return _mapper.Map<ParameterDto>(parameter);
    }
    
    public async Task<ParameterDto> GetByNom(string module, string nom, bool includeDeleted = false)
    {
        var parameter = await _repository.GetByNom(module, nom, Session, includeDeleted);
        return _mapper.Map<ParameterDto>(parameter);
    }
    
    public async Task<ParameterDto> CreateAsync(ParameterDto dto)
    {
        var parameter = _mapper.Map<TParameter>(dto);
        parameter = await _repository.CreateAsync(parameter, Session);
        return _mapper.Map<ParameterDto>(parameter);
    }
    
    public async Task<ParameterDto> UpdateAsync(ParameterDto dto)
    {
        var parameter = _mapper.Map<TParameter>(dto);
        parameter = await _repository.UpdateAsync(parameter, Session);
        return _mapper.Map<ParameterDto>(parameter);
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id, Session);
    }

    public async Task<IList<string>> GetAllModulesAsync()
    {
        return await _repository.GetAllModulesAsync(Session);
    }   

    public async Task<IList<ParameterDto>> GetAllByModuleAsync(string module)
    {
        var parameters = await _repository.GetAllByModuleAsync(module, Session);
        return _mapper.Map<IList<ParameterDto>>(parameters);
    }

    public async Task<ParameterDto> GetByNomAsync(string module, string nom, bool includeDeleted = false)
    {
        var parameter = await _repository.GetByNomAsync(module, nom, Session, includeDeleted);
        return _mapper.Map<ParameterDto>(parameter);
    }
}