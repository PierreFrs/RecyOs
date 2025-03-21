using System.Collections.Generic;
using AutoMapper;
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Models.DTO.hub;

namespace RecyOs.Engine.Services;

public class EngineClientParticulierService : BaseHubService<ClientParticulierDto>, IEngineClientParticulierService
{
    readonly IEngineClientParticulierRepository _clientParticulierRepository;
    private readonly IMapper _mapper;
    
    public EngineClientParticulierService(IEngineClientParticulierRepository clientParticulierRepository,
        IMapper mapper) : base()
    {
        _clientParticulierRepository = clientParticulierRepository;
        _mapper = mapper;
    }
    
    public override IList<ClientParticulierDto> GetCreatedItems(string prmModuleName)
    {
        return _mapper.Map<IList<ClientParticulierDto>>(_clientParticulierRepository.GetCreatedEntities(prmModuleName));
    }
    
    public override IList<ClientParticulierDto> GetUpdatedItems(string prmModuleName)
    {
        return _mapper.Map<IList<ClientParticulierDto>>(_clientParticulierRepository.GetUpdatedEntities(prmModuleName));
    }
    
    public override IList<ClientParticulierDto> CallBackDestIdCreation(string prmModuleName, IList<ClientParticulierDto> prmItems)
    {
        return _mapper.Map<IList<ClientParticulierDto>>(_clientParticulierRepository.CallBackDestIdCreation(prmModuleName, _mapper.Map<IList<ClientParticulier>>(prmItems)));
    }
}