using System;
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

public class EtablissementFicheService<TEtablissementFiche> : BaseService, IEtablissementFicheService where TEtablissementFiche : EtablissementFiche, new()
{
    protected readonly IEtablissementFicheRepository<TEtablissementFiche> _etablissementFicheRepository;
    private readonly IMapper _mapper;
    
    public EtablissementFicheService(ICurrentContextProvider contextProvider, 
        IEtablissementFicheRepository<TEtablissementFiche> etablissementFicheRepository, IMapper mapper) 
        : base(contextProvider)
    {
        _etablissementFicheRepository = etablissementFicheRepository;
        _mapper = mapper;
    }

    public async Task<GridData<EtablissementFicheDto>> GetDataForGrid(EtablissementFicheGridFilter filter, bool includeDeleted = false)
    {
        var tuple = await _etablissementFicheRepository.GetFiltredListWithCount(filter, Session, includeDeleted);
        var ratio = tuple.Item2 / (double)filter.PageSize;
        var begin = filter.PageNumber  * filter.PageSize;
        
        return new GridData<EtablissementFicheDto>
        {
            Items = _mapper.Map<IEnumerable<EtablissementFicheDto>>(tuple.Item1),
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

    public async Task<EtablissementFicheDto> GetById(int id, bool includeDeleted = false)
    {
        var etablissementFiche = await _etablissementFicheRepository.Get(id, Session, includeDeleted);
        return _mapper.Map<EtablissementFicheDto>(etablissementFiche);
    }

    public async Task<EtablissementFicheDto> GetBySiret(string siret, bool includeDeleted = false)
    {
        var etablissementFiche = await _etablissementFicheRepository.GetBySiret(siret, Session, includeDeleted);
        return _mapper.Map<EtablissementFicheDto>(etablissementFiche);
    }

    public async Task<bool> Delete(int id)
    {
        await _etablissementFicheRepository.Delete(id, Session);
        return true;
    }

    public async Task<EtablissementFicheDto> Edit(EtablissementFicheDto dto)
    {
        var etablissementFiche = _mapper.Map<TEtablissementFiche>(dto);
        await _etablissementFicheRepository.UpdateAsync(etablissementFiche, Session);
        return _mapper.Map<EtablissementFicheDto>(etablissementFiche);
    }
    
    public async Task<EtablissementFicheDto> Create(EtablissementFicheDto dto)
    {
        var etablissementFiche = _mapper.Map<TEtablissementFiche>(dto);
        await _etablissementFicheRepository.Create(etablissementFiche, Session);
        return _mapper.Map<EtablissementFicheDto>(etablissementFiche);
    }
    
    
}