using System.Collections.Generic;
using AutoMapper;
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Engine.Services;

/// <summary>
/// Represents a service for managing client Europe DTOs specific to the Engine module.
/// </summary>
public class EngineEuropeClientService: BaseHubService<ClientEuropeDto>, IEngineEuropeClientService
{
    readonly IEngineEuropeClientRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the EngineEuropeClientService class.
    /// </summary>
    /// <param name="repository">The repository interface for accessing EngineEuropeClient data.</param>
    /// <param name="mapper">The AutoMapper interface for object mapping.</param>
    public EngineEuropeClientService(IEngineEuropeClientRepository repository, IMapper mapper) : base()
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a list of created client Europe DTO items based on the provided module name.
    /// </summary>
    /// <param name="prmModuleName">The name of the module.</param>
    /// <returns>A list of ClientEuropeDto objects.</returns>
    public override IList<ClientEuropeDto> GetCreatedItems(string prmModuleName)
    {
        return _mapper.Map<IList<ClientEuropeDto>>(_repository.GetCreatedEntities(prmModuleName));
    }

    /// <summary>
    /// Retrieves the updated items for a given module from the repository.
    /// </summary>
    /// <param name="prmModuleName">The name of the module to retrieve updated items for.</param>
    /// <returns>
    /// The updated items as a list of <see cref="ClientEuropeDto"/>.
    /// </returns>
    public override IList<ClientEuropeDto> GetUpdatedItems(string prmModuleName)
    {
        return _mapper.Map<IList<ClientEuropeDto>>(_repository.GetUpdatedEntities(prmModuleName));
    }
    
    /// <summary>
    /// Callback method for creating destination IDs for client Europe DTOs.
    /// </summary>
    /// <param name="prmModuleName">The name of the module.</param>
    /// <param name="prmItems">The list of client Europe DTO items.</param>
    /// <returns>A list of client Europe DTO items with destination IDs.</returns>
    public override IList<ClientEuropeDto> CallBackDestIdCreation(string prmModuleName, IList<ClientEuropeDto> prmItems)
    {
        return _mapper.Map<IList<ClientEuropeDto>>(_repository.CallBackDestIdCreation(prmModuleName, _mapper.Map<IList<ClientEurope>>(prmItems)));
    }
}