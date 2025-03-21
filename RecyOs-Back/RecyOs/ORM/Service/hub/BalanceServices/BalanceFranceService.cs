// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BalanceFranceService.cs
// Created : 2024/02/26 - 12:21
// Updated : 2024/02/26 - 12:21

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

public class BalanceFranceService : BalanceService<BalanceFrance, BalanceDto>, IBalanceFranceService
{
    private readonly IBalanceFranceRepository _balanceFranceRepository;
    private readonly IMapper _mapper;
    public BalanceFranceService(
        ICurrentContextProvider contextProvider,
        IBalanceFranceRepository balanceFranceRepository,
        IMapper mapper
        ) : base(contextProvider, balanceFranceRepository, mapper)
    {
        _balanceFranceRepository = balanceFranceRepository;
        _mapper = mapper;
    }

    public async Task<GridData<BalanceFranceDto>> GetDataForGrid(BalanceFranceGridFilter filter, bool includeDeleted = false)
    {
        var tuple = await _balanceFranceRepository.GetFilteredListWithCount(filter, includeDeleted);
        var ratio = tuple.Item2 / (double)filter.PageSize;
        var begin = filter.PageNumber  * filter.PageSize;
        
        return new GridData<BalanceFranceDto>
        {
            Items = _mapper.Map<IEnumerable<BalanceFranceDto>>(tuple.Item1),
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
