// BalanceParticulierService.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 30/01/2025
// Fichier Modifié le : 30/01/2025
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using RecyOs.ORM.Models.DTO.hub.BalanceDtos;

namespace RecyOs.ORM.Service.hub.BalanceServices;

public class BalanceParticulierService : BalanceService<BalanceParticulier, BalanceDto>, IBalanceParticulierService
{
    private readonly IBalanceParticulierRepository _balanceParticulierRepository;
    private readonly IMapper _mapper;
    public BalanceParticulierService(
        ICurrentContextProvider contextProvider,
        IBalanceParticulierRepository balanceParticulierRepository,
        IMapper mapper
        ) : base(contextProvider, balanceParticulierRepository, mapper)
    {
        _balanceParticulierRepository = balanceParticulierRepository;
        _mapper = mapper;
    }

    public async Task<GridData<BalanceParticuliersDto>> GetDataForGrid(BalanceParticulierGridFilter filter, bool includeDeleted = false)
    {
        var tuple = await _balanceParticulierRepository.GetFilteredListWithCount(filter, includeDeleted);
        var ratio = tuple.Item2 / (double)filter.PageSize;
        var begin = filter.PageNumber  * filter.PageSize;
        
        return new GridData<BalanceParticuliersDto>
        {
            Items = _mapper.Map<IEnumerable<BalanceParticuliersDto>>(tuple.Item1),
             Paginator = new Pagination()
            {
                length = tuple.Item2,
                size = filter.PageSize,
                page = filter.PageNumber,
                lastPage = (int)Math.Max(Math.Ceiling(ratio),1.0),
                startIndex = begin,
            },
            SommeTotal = tuple.Item3
        };
    }
}