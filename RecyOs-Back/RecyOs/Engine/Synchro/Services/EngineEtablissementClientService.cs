//  EtablissementClientService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Engine.Services;

public class EngineEtablissementClientService : BaseHubService<EtablissementClientDto>, IEngineEtablissementClientService
{
    readonly IEngineEtablissementClientRepository _etablissementClientRepository;
    private readonly IMapper _mapper;

    public EngineEtablissementClientService(IEngineEtablissementClientRepository etablissementClientRepository,
        IMapper mapper) : base()
    {
        _etablissementClientRepository = etablissementClientRepository;
        _mapper = mapper;
    }

    public override IList<EtablissementClientDto> GetCreatedItems(string prmModuleName)
    {
        return _mapper.Map<IList<EtablissementClientDto>>(_etablissementClientRepository.GetCreatedEntities(prmModuleName));
    }

    public override IList<EtablissementClientDto> GetUpdatedItems(string prmModuleName)
    {
        return _mapper.Map<IList<EtablissementClientDto>>(_etablissementClientRepository.GetUpdatedEntities(prmModuleName));
    }
    
    public override IList<EtablissementClientDto> CallBackDestIdCreation(string prmModuleName, IList<EtablissementClientDto> prmItems)
    {
        return _mapper.Map<IList<EtablissementClientDto>>(_etablissementClientRepository.CallBackDestIdCreation(prmModuleName, _mapper.Map<IList<EtablissementClient>>(prmItems)));
    }
    
}