// DashdocService.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 16/09/2024
// Fichier Modifié le : 16/09/2024
// Code développé pour le projet : RecyOs

using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;
using RecyOs.ThirdPartyAPIs.DashdocDB.Entities;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.Service;

public class DashdocService : IDashdocService
{
    private readonly IDashdocRepository _dashdocRepository;
    private readonly IMapper _mapper;
    
    public DashdocService(
        IDashdocRepository dashdocRepository, 
        IMapper mapper)
    {
        _dashdocRepository = dashdocRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Crée un nouvel établissement dans DashDoc
    /// </summary>
    /// <param name="dashdocCompanyDto"></param>
    /// <returns></returns>
    public async Task<DashdocCompanyDto> CreateDashdocCompanyAsync(DashdocCompanyDto dashdocCompanyDto)
    {
        var dashdocCompany= _mapper.Map<DashdocCompany>(dashdocCompanyDto);
        var dashdocEntity = await _dashdocRepository.CreateDashdocCompanyAsync(dashdocCompany);
        var mappedEntity = _mapper.Map<DashdocCompanyDto>(dashdocEntity);
        return mappedEntity;
    }

    /// <summary>
    /// Met à jour une entreprise Dashdoc
    /// </summary>
    /// <param name="dashdocCompanyDto">Les données de l'entreprise à mettre à jour</param>
    /// <returns>L'entreprise mise à jour</returns>
    public async Task<DashdocCompanyDto> UpdateDashdocCompanyAsync(DashdocCompanyDto dashdocCompanyDto)
    {
        var dashdocCompany = _mapper.Map<DashdocCompany>(dashdocCompanyDto);
        return _mapper.Map<DashdocCompanyDto>(await _dashdocRepository.UpdateDashdocCompanyAsync(dashdocCompany));
    }
}