using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.Service.hub;

public class EntrepriseNDCoverService<TEntrepriseNDCover> : BaseService, IEntrepriseNDCoverService
    where TEntrepriseNDCover : EntrepriseNDCover, new()
{
    private readonly IEntrepriseNDCoverRepository<TEntrepriseNDCover> _entrepriseNDCoverRepository;
    private readonly IMapper _mapper;
    public EntrepriseNDCoverService(ICurrentContextProvider contextProvider, IEntrepriseNDCoverRepository<TEntrepriseNDCover> entreprisendcoverRepository, IMapper mapper) : base(contextProvider)
    {
        _entrepriseNDCoverRepository = entreprisendcoverRepository;
        _mapper = mapper;
    }

    public async Task<EntrepriseNDCoverDto> GetById(int id)
    {
        var entrepriseNDCover = await _entrepriseNDCoverRepository.Get(id, Session);
        return _mapper.Map<EntrepriseNDCoverDto>(entrepriseNDCover);
    }
    
    public async Task<EntrepriseNDCoverDto> GetBySiren(string siren)
    {
        var entrepriseNDCover = await _entrepriseNDCoverRepository.GetBySiren(siren, Session);
        return _mapper.Map<EntrepriseNDCoverDto>(entrepriseNDCover);
    }
    
    public async Task<GridData<EntrepriseNDCoverDto>> GetDataForGrid(EntrepriseNDCoverGridFilter filter)
    {
        var tuple = await _entrepriseNDCoverRepository.GetFilteredListWithCount(filter, Session);
        var ratio = tuple.Item2 / (double)filter.PageSize;
        var begin = filter.PageNumber  * filter.PageSize;
        
        return new GridData<EntrepriseNDCoverDto>
        {
            Items = _mapper.Map<IEnumerable<EntrepriseNDCoverDto>>(tuple.Item1),
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

    public async Task<EntrepriseNDCoverDto> Edit(EntrepriseNDCoverDto dto)
    {
        if (string.IsNullOrEmpty(dto.Siren))
        {
            var existingEntity = await _entrepriseNDCoverRepository.Get(dto.Id, new ContextSession());
            if (existingEntity != null)
            {
                dto.Siren = existingEntity.Siren;
            }
            else
            {
                throw new Exception("Entity does not exist");
            }
        }
        var entrepriseNDCover = _mapper.Map<TEntrepriseNDCover>(dto);
        await _entrepriseNDCoverRepository.Update(entrepriseNDCover, Session);
        return _mapper.Map<EntrepriseNDCoverDto>(entrepriseNDCover);
    }

    
    public async Task<EntrepriseNDCoverDto> Create(EntrepriseNDCoverDto dto)
    {
        return await Edit(dto);
    }
}