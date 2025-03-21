using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class SocieteBaseService : BaseService,ISocieteBaseService
{
    private readonly ISocieteBaseRepository _societeBaseRepository;
    private readonly IMapper _mapper;
    private readonly ITokenInfoService _tokenInfoService;
    public SocieteBaseService(
        ICurrentContextProvider contextProvider, 
        ISocieteBaseRepository societeBaseRepository, 
        IMapper mapper, 
        ITokenInfoService tokenInfoService
        ) : base(contextProvider)
    {
        _societeBaseRepository = societeBaseRepository;
        _mapper = mapper;
        _tokenInfoService = tokenInfoService;
    }

    public async Task<SocieteDto> CreateAsync(SocieteDto dto)
    {
        var currentUser = _tokenInfoService.GetCurrentUserName();
        Societe newSociete = new Societe
        {
            Nom = dto.Nom,
            IdOdoo = dto.IdOdoo,
            CreateDate = DateTime.Now,
            CreatedBy = currentUser,
        };
        
        var createdSociete = await _societeBaseRepository.CreateAsync(newSociete, Session);
        var responseSocieteDto = _mapper.Map<SocieteDto>(createdSociete);
        return responseSocieteDto;
    }

    public async Task<IReadOnlyList<SocieteDto>> GetListAsync()
    {
        var societes = await _societeBaseRepository.GetListAsync();
        return _mapper.Map<List<SocieteDto>>(societes);
    }

    public async Task<SocieteDto> GetByIdAsync(int id)
    {
        var societe = await _societeBaseRepository.GetByIdAsync(id, Session);
        return _mapper.Map<SocieteDto>(societe);
    }

    public async Task<SocieteDto> UpdateAsync(int id, SocieteDto dto)
    {
        var currentUser = _tokenInfoService.GetCurrentUserName();
        
        var existingSociete = await _societeBaseRepository.GetByIdAsync(id, Session);
        if (existingSociete == null) return null;

        existingSociete.Nom = dto.Nom;
        existingSociete.IdOdoo = dto.IdOdoo;
        existingSociete.UpdatedAt = DateTime.Now;
        existingSociete.UpdatedBy = currentUser;

        var updatedSociete = await _societeBaseRepository.UpdateAsync(existingSociete, Session);
        var updatedSocieteDto = _mapper.Map<SocieteDto>(updatedSociete);

        return updatedSocieteDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existingSociete = await _societeBaseRepository.GetByIdAsync(id, Session);
        if (existingSociete == null) return false;

        await _societeBaseRepository.DeleteAsync(id, Session);
        return true;
    }

    public async Task<GridData<SocieteDto>> GetDataForGrid(SocieteGridFilter filter, bool includeDeleted = false)
    {
        var (societes, total) = await _societeBaseRepository.GetDataForGrid(filter, Session, includeDeleted);
        var societesDto = _mapper.Map<IEnumerable<SocieteDto>>(societes);

        return new GridData<SocieteDto>
        {
            Items = societesDto,
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
}