using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using RecyOs.ORM.DTO;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class BusinessUnitService<TBusinessUnit, TBusinessUnitDto> : BaseService, IBusinessUnitService<TBusinessUnitDto>
    where TBusinessUnit : BusinessUnit, new()
    where TBusinessUnitDto : BusinessUnitDto, new()
{
    private readonly IBusinessUnitRepository<TBusinessUnit> _businessunitRepository;
    private readonly IMapper _mapper;
    public BusinessUnitService(ICurrentContextProvider contextProvider, IBusinessUnitRepository<TBusinessUnit> businessunitRepository, IMapper mapper) : base(contextProvider)
    {
        _businessunitRepository = businessunitRepository;
        _mapper = mapper;
    }

    public async Task<List<TBusinessUnitDto>> GetListAsync()
    {
        var businessunits = await _businessunitRepository.GetListAsync(Session);
        return _mapper.Map<List<TBusinessUnitDto>>(businessunits);
    }

    public async Task<TBusinessUnitDto> GetByIdAsync(int id)
    {
        var businessunit = await _businessunitRepository.GetByIdAsync(id, Session);
        return _mapper.Map<TBusinessUnitDto>(businessunit);
    }
}