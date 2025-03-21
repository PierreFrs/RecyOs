// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BalanceEuropeService.cs
// Created : 2024/02/26 - 14:43
// Updated : 2024/02/26 - 14:43

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

namespace RecyOs.ORM.Service.hub;

public class BalanceEuropeService : BalanceService<BalanceEurope, BalanceDto>, IBalanceEuropeService
{
    private readonly IBalanceEuropeRepository _balanceEuropeRepository;
    private readonly IMapper _mapper;
    public BalanceEuropeService(
        ICurrentContextProvider contextProvider,
        IBalanceEuropeRepository balanceEuropeRepository,
        IMapper mapper
        ) : base(contextProvider, balanceEuropeRepository, mapper)
    {
        _balanceEuropeRepository = balanceEuropeRepository;
        _mapper = mapper;
    }

    public async Task<GridData<BalanceEuropeDto>> GetDataForGrid(BalanceEuropeGridFilter filter, bool includeDeleted = false)
    {
        var tuple = await _balanceEuropeRepository.GetFilteredListWithCount(filter, includeDeleted);
        var ratio = tuple.Item2 / (double)filter.PageSize;
        var begin = filter.PageNumber  * filter.PageSize;
        
        return new GridData<BalanceEuropeDto>
        {
            Items = _mapper.Map<IEnumerable<BalanceEuropeDto>>(tuple.Item1),
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
