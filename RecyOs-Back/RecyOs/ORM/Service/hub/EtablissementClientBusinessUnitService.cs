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
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.Service;

public class EtablissementClientBusinessUnitService<TEtablissementClientBusinessUnit, TEtablissementClientBusinessUnitDto, TBusinessUnit, TBusinessUnitDto> : BaseService, IEtablissementClientBusinessUnitService<TEtablissementClientBusinessUnitDto, TBusinessUnitDto>
    where TEtablissementClientBusinessUnit : EtablissementClientBusinessUnit, new()
    where TEtablissementClientBusinessUnitDto : EtablissementClientBusinessUnitDto, new()
    where TBusinessUnit : BusinessUnit, new()
    where TBusinessUnitDto : BusinessUnitDto, new()
{
    private readonly IEtablissementClientBusinessUnitRepository<TEtablissementClientBusinessUnit, TBusinessUnit> _etablissementClientBusinessUnitRepository;
    private readonly IEtablissementClientRepository<EtablissementClient> _etablissementClientRepository;
    private readonly IMapper _mapper;
    public EtablissementClientBusinessUnitService(
        ICurrentContextProvider contextProvider, 
        IEtablissementClientBusinessUnitRepository<TEtablissementClientBusinessUnit, TBusinessUnit> etablissementClientBusinessUnitRepository, 
        IEtablissementClientRepository<EtablissementClient> etablissementClientRepository,
        IMapper mapper
        ) : base(contextProvider)
    {
        _etablissementClientBusinessUnitRepository = etablissementClientBusinessUnitRepository;
        _etablissementClientRepository = etablissementClientRepository;
        _mapper = mapper;
    }

    public async Task<TEtablissementClientBusinessUnitDto> CreateAsync(EtablissementClientBusinessUnitDto etablissementClientBusinessUnitDto)
    {
        var etablissementClientBusinessUnit =
            _mapper.Map<EtablissementClientBusinessUnit>(etablissementClientBusinessUnitDto);
        
        return _mapper.Map<TEtablissementClientBusinessUnitDto>
            (await _etablissementClientBusinessUnitRepository.CreateAsync(etablissementClientBusinessUnit, Session));
    }

    public async Task<IList<TBusinessUnitDto>> GetByEtablissementClientIdAsync(int etablissementClientId, bool includeDeleted = false)
    {
        var expectedEtablissementClient = await _etablissementClientRepository.GetById(etablissementClientId, Session);
        if (expectedEtablissementClient == null) return null;
        
        var businessUnitList = await _etablissementClientBusinessUnitRepository.GetBusinessUnitsByEtablissementClientIdAsync(etablissementClientId, Session);
        return _mapper.Map<IList<TBusinessUnitDto>>(businessUnitList);
    }

    public async Task<bool> DeleteAsync(EtablissementClientBusinessUnitDto etablissementClientBusinessUnitDto)
    {
        if (etablissementClientBusinessUnitDto == null) return false;
        
        var existingEtablissementClientBusinessUnit = await _etablissementClientBusinessUnitRepository.GetByEtablissementClientIdAndBusinessUnitIdAsync(etablissementClientBusinessUnitDto.ClientId, etablissementClientBusinessUnitDto.BusinessUnitId, Session);
        if (existingEtablissementClientBusinessUnit == null) return false;

        return await _etablissementClientBusinessUnitRepository.DeleteAsync(existingEtablissementClientBusinessUnit.ClientId, existingEtablissementClientBusinessUnit.BusinessUnitId, Session);
    }
}