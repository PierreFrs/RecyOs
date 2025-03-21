using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.MKGT_DB.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.MKGT_DB.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.MKGT_DB.Services;

public class FCliService<TFCli> :  IFCliService where TFCli : Fcli, new()
{
    private readonly IFCliRepository<TFCli> _fCliRepository;
    private readonly IMapper _mapper;
    
    public FCliService(IFCliRepository<TFCli> fCliRepository, IMapper mapper)
    {
        _fCliRepository = fCliRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Insère un nouvel établissement client dans la base de données.
    /// </summary>
    /// <param name="etablissementClient">Le DTO représentant l'établissement client à insérer.</param>
    /// <returns>
    /// Un DTO représentant l'établissement client inséré.
    /// </returns>
    public async Task<EtablissementMkgtDto> InsertEtablissementClient(EtablissementMkgtDto etablissementClient)
    {
        var etablissementClientFcli = _mapper.Map<TFCli>(etablissementClient);
        Fcli result = await _fCliRepository.CreFac(etablissementClientFcli);
        return _mapper.Map<EtablissementMkgtDto>(result);
    }
    
    /// <summary>
    /// Met à jour un établissement client existant dans la base de données.
    /// </summary>
    /// <param name="etablissementMkgtDto">Le DTO représentant l'établissement client à mettre à jour.</param>
    /// <returns>
    /// Un DTO représentant l'établissement client mis à jour.
    /// </returns>
    public async Task<EtablissementMkgtDto> UpdateEtablissementClient(EtablissementMkgtDto etablissementMkgtDto)
    {
        var etablissementClient = _mapper.Map<TFCli>(etablissementMkgtDto);
        var result = await _fCliRepository.UpFac(etablissementClient);
        return _mapper.Map<EtablissementMkgtDto>(result);
    }

    /// <summary>
    /// Récupère une liste de clients valides depuis la base de données.
    /// </summary>
    /// <returns>
    /// Une liste de DTOs représentant les clients valides.
    /// </returns>
    public async Task<List<EtablissementMkgtDto>> GetValidsClients()
    {
        var result = await _fCliRepository.GetValidsFcli();
        return _mapper.Map<List<EtablissementMkgtDto>>(result);
    }
    
    /// <summary>
    /// Récupère un client depuis la base de données.
    /// </summary>
    /// <param name="code">Code du client à récupérer.</param>
    /// <returns>
    /// Un DTO représentant le client récupéré.
    /// </returns>
    public async Task<EtablissementMkgtDto> GetClient(string code)
    {
        var result = await _fCliRepository.GetByCode(code);
        return _mapper.Map<EtablissementMkgtDto>(result);
    }


}