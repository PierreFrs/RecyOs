// /** EntrepriseBaseService.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 21/03/2023
//  * Fichier Modifié le : 21/03/2023
//  * Code développé pour le projet : RecyOs.EntrepriseBaseService
//  */

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

public class EntrepriseBaseService<TEntrepriseBase> : BaseService, IEntrepriseBaseService where TEntrepriseBase : EntrepriseBase, new()
{
    private readonly IEntrepriseBaseRepository<TEntrepriseBase> _repository;
    private readonly IMapper _mapper;

    public EntrepriseBaseService(ICurrentContextProvider contextProvider, 
        IEntrepriseBaseRepository<TEntrepriseBase> entrepriseBaseRepository, IMapper mapper) : base(contextProvider)
    {
        _repository = entrepriseBaseRepository;
        _mapper = mapper;
    }


    public async Task<GridData<EntrepriseBaseDto>> GetDataForGrid(EntrepriseBaseGridFilter filter, bool includeDeleted = false)
    {
        var tuple = await _repository.GetFiltredListWithCount(filter, Session, includeDeleted);
        var ratio = tuple.Item2 / (double)filter.PageSize;
        var begin = filter.PageNumber  * filter.PageSize;

        return new GridData<EntrepriseBaseDto>
        {
            Items = _mapper.Map<IEnumerable<EntrepriseBaseDto>>(tuple.Item1),
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

    public async Task<EntrepriseBaseDto> GetById(int id, bool includeDeleted = false)
    {
        var entrepriseBase = await _repository.Get(id, Session, includeDeleted);
        return _mapper.Map<EntrepriseBaseDto>(entrepriseBase);
    }

    public async Task<EntrepriseBaseDto> GetBySiren(string siren, bool includeDeleted = false)
    {
        var entrepriseBase = await _repository.GetBySiren(siren, Session, includeDeleted);
        return _mapper.Map<EntrepriseBaseDto>(entrepriseBase);
    }

    public async Task<bool> Delete(int id)
    {
        await _repository.Delete(id, Session);
        return true;
    }

    public async Task<EntrepriseBaseDto> Edit(EntrepriseBaseDto dto)
    {
        var entrepriseBase = _mapper.Map<TEntrepriseBase>(dto);
        var result = await _repository.Update(entrepriseBase, Session);
        return _mapper.Map<EntrepriseBaseDto>(result);
    }
    
    public async Task<EntrepriseBaseDto> Create(EntrepriseBaseDto dto)
    {
        var entrepriseBase = _mapper.Map<TEntrepriseBase>(dto);
        var result = await _repository.Create(entrepriseBase, Session);
        return _mapper.Map<EntrepriseBaseDto>(result);
    }
}
