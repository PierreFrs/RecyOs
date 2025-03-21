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

public class ClientEuropeBusinessUnitService<TClientEuropeBusinessUnit, TClientEuropeBusinessUnitDto, TBusinessUnit, TBusinessUnitDto> : BaseService, IClientEuropeBusinessUnitService<TClientEuropeBusinessUnitDto, TBusinessUnitDto>
    where TClientEuropeBusinessUnit : ClientEuropeBusinessUnit, new()
    where TClientEuropeBusinessUnitDto : ClientEuropeBusinessUnitDto, new()
    where TBusinessUnit : BusinessUnit, new()
    where TBusinessUnitDto : BusinessUnitDto, new()
{
    private readonly IClientEuropeBusinessUnitRepository<TClientEuropeBusinessUnit, TBusinessUnit> _clientEuropeBusinessUnitRepository;
    private readonly IClientEuropeRepository<ClientEurope> _clientEuropeRepository;
    private readonly IMapper _mapper;
    public ClientEuropeBusinessUnitService(
        ICurrentContextProvider contextProvider, 
        IClientEuropeBusinessUnitRepository<TClientEuropeBusinessUnit, TBusinessUnit> clientEuropeBusinessUnitRepository, 
        IClientEuropeRepository<ClientEurope> clientEuropeRepository,
        IMapper mapper
        ) : base(contextProvider)
    {
        _clientEuropeBusinessUnitRepository = clientEuropeBusinessUnitRepository;
        _clientEuropeRepository = clientEuropeRepository;
        _mapper = mapper;
    }

    public async Task<TClientEuropeBusinessUnitDto> CreateAsync(ClientEuropeBusinessUnitDto clientEuropeBusinessUnitDto)
    {
        var clientEuropeBusinessUnit =
            _mapper.Map<ClientEuropeBusinessUnit>(clientEuropeBusinessUnitDto);
        
        return _mapper.Map<TClientEuropeBusinessUnitDto>
            (await _clientEuropeBusinessUnitRepository.CreateAsync(clientEuropeBusinessUnit, Session));
    }

    public async Task<IList<TBusinessUnitDto>> GetByClientEuropeIdAsync(int clientEuropeId, bool includeDeleted = false)
    {
        var expectedClientEurope = await _clientEuropeRepository.GetById(clientEuropeId, Session);
        if (expectedClientEurope == null) return null;
        
        var businessUnitList = await _clientEuropeBusinessUnitRepository.GetBusinessUnitsByClientEuropeIdAsync(clientEuropeId, Session);
        return _mapper.Map<IList<TBusinessUnitDto>>(businessUnitList);
    }

    public async Task<bool> DeleteAsync(ClientEuropeBusinessUnitDto clientEuropeBusinessUnitDto)
    {
        if (clientEuropeBusinessUnitDto == null) return false;
        
        var existingClientEuropeBusinessUnit = await _clientEuropeBusinessUnitRepository.GetByClientEuropeIdAndBusinessUnitIdAsync(clientEuropeBusinessUnitDto.ClientId, clientEuropeBusinessUnitDto.BusinessUnitId, Session);
        if (existingClientEuropeBusinessUnit == null) return false;

        return await _clientEuropeBusinessUnitRepository.DeleteAsync(clientEuropeBusinessUnitDto.ClientId, clientEuropeBusinessUnitDto.BusinessUnitId, Session);
    }
}