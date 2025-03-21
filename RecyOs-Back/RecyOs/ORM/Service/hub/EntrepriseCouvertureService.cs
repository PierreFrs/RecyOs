using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.Service.hub;

public class EntrepriseCouvertureService<TEntrepriseCouverture> : BaseService, IEntrepriseCouvertureService where TEntrepriseCouverture : EntrepriseCouverture, new()
{
    private readonly IMapper _mapper;
    private readonly IEntrepriseCouvertureRepository<TEntrepriseCouverture> _repository;
    
    public EntrepriseCouvertureService(ICurrentContextProvider contextProvider, IEntrepriseCouvertureRepository<TEntrepriseCouverture> repository, IMapper mapper) : base(contextProvider)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<EntrepriseCouvertureDto> GetById(int id)
    {
        var entrepriseCouverture = await _repository.Get(id, Session);
        return _mapper.Map<EntrepriseCouvertureDto>(entrepriseCouverture);
    }
    
    public async Task<EntrepriseCouvertureDto> GetBySiren(string siren)
    {
        var entrepriseCouverture = await _repository.GetBySiren(siren, Session);
        return _mapper.Map<EntrepriseCouvertureDto>(entrepriseCouverture);
    }
    
    public async Task<GridData<EntrepriseCouvertureDto>> GetDataForGrid(EntrepriseCouvertureGridFilter filter)
    {
        var tuple = await _repository.GetFilteredListWithCount(filter, Session);
        var ratio = tuple.Item2 / (double)filter.PageSize;
        var begin = filter.PageNumber  * filter.PageSize;
        
        return new GridData<EntrepriseCouvertureDto>
        {
            Items = _mapper.Map<IEnumerable<EntrepriseCouvertureDto>>(tuple.Item1),
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

    public async Task<EntrepriseCouvertureDto> Edit(EntrepriseCouvertureDto dto)
    {
        var entrepriseCouverture = _mapper.Map<TEntrepriseCouverture>(dto);
        await _repository.Update(entrepriseCouverture, Session);
        return _mapper.Map<EntrepriseCouvertureDto>(entrepriseCouverture);
    }
    
    public async Task<EntrepriseCouvertureDto> Create(EntrepriseCouvertureDto dto)
    {
        return await Edit(dto);
    }
}