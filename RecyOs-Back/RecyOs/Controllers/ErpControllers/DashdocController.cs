// DashdocController.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 16/09/2024
// Fichier Modifié le : 16/09/2024
// Code développé pour le projet : RecyOs

using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Models.DTO.hub;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

public class DashdocController : BaseApiController
{
    private readonly IEtablissementClientService _etablissementClientService;
    private readonly IClientEuropeService _clientEuropeService;
    private readonly ITransportDashdocService _dashdocService;
    private readonly IMapper _mapper;
    
    public DashdocController(
        IEtablissementClientService etablissementClientService,
        IClientEuropeService clientEuropeService,
        ITransportDashdocService dashdocService, 
        IMapper mapper
        )
    {
        _etablissementClientService = etablissementClientService;
        _clientEuropeService = clientEuropeService;
        _dashdocService = dashdocService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Permet de creér un client dans DashDoc à partir d'un client français. si idDashDoc existe on ne fait rien
    /// </summary>
    /// <param name="etabId">ID de l'étabissement</param>
    /// <returns>La fiche client mise a jour</returns>
    [SwaggerResponse(200, "EtablissementClient crée dans DashDoc", typeof(EtablissementClientDto))]
    [SwaggerResponse(400, "EtablissementClient non crée dans DashDoc", typeof(EtablissementClientDto))]
    [SwaggerResponse(404, "EtablissementClient non trouvé", typeof(EtablissementClientDto))]
    [HttpPost("create_french_dashdoc_company/{etabId}")]
    [Authorize(Policy = "CreationDashdoc")]
    public async Task<IActionResult> CreateFrenchDashdocCompany(int etabId)
    {
        EtablissementClientDto etablissement = await _etablissementClientService.GetById(etabId); 
        if(etablissement == null)
        {
            return NotFound();
        }
        
        var idDashDoc = etablissement.IdDashdoc;
        
        if (idDashDoc == null)
        {
            var dashdocCompanyDto = _mapper.Map<DashdocCompanyDto>(etablissement);
            var dashdocCompany =  await _dashdocService.CreateDashdocCompanyAsync(dashdocCompanyDto);
            if (dashdocCompany == null)
            {
                return BadRequest();
            }
            etablissement.IdDashdoc = dashdocCompany.PK;
            etablissement.DateCreDashdoc = DateTime.UtcNow;
            await _etablissementClientService.Edit(etablissement);
            return Ok(etablissement);
        }
        
        return BadRequest();
    }
    
    /// <summary>
    /// Permet de creér un client dans DashDoc à partir d'un client européen. si idDashDoc existe on ne fait rien
    /// </summary>
    /// <param name="etabId">ID de l'étabissement</param>
    /// <returns>La fiche client mise a jour</returns>
    [SwaggerResponse(200, "ClientEurope crée dans DashDoc", typeof(ClientEuropeDto))]
    [SwaggerResponse(400, "ClientEurope non crée dans DashDoc", typeof(ClientEuropeDto))]
    [SwaggerResponse(404, "ClientEurope non trouvé", typeof(ClientEuropeDto))]
    [HttpPost("create_europe_dashdoc_company/{etabId}")]
    [Authorize(Policy = "CreationDashdoc")]
    public async Task<IActionResult> CreateEuropeDashdocCompany(int etabId)
    {
        ClientEuropeDto clientEurope = await _clientEuropeService.GetById(etabId); 
        if(clientEurope == null)
        {
            return NotFound();
        }
        
        var idDashDoc = clientEurope.IdDashdoc;
        
        if (idDashDoc == null)
        {
            var dashdocCompanyDto = _mapper.Map<DashdocCompanyDto>(clientEurope);
            var dashdocCompany = await _dashdocService.CreateDashdocCompanyAsync(dashdocCompanyDto);
            if (dashdocCompany == null)
            {
                return BadRequest();
            }
            clientEurope.IdDashdoc = dashdocCompany.PK;
            clientEurope.DateCreDashdoc = DateTime.UtcNow;
            await _clientEuropeService.Update(clientEurope);
            return Ok(clientEurope);
        }
        
        return BadRequest();
    }
    
    /// <summary>
    /// Met à jour un client français dans DashDoc
    /// </summary>
    /// <param name="etabId">ID de l'étabissement</param>
    /// <returns>La fiche client mise a jour</returns>
    [SwaggerResponse(200, "EtablissementClient mis à jour dans DashDoc", typeof(EtablissementClientDto))]
    [SwaggerResponse(400, "EtablissementClient non mis à jour dans DashDoc", typeof(EtablissementClientDto))]
    [SwaggerResponse(404, "EtablissementClient non trouvé", typeof(EtablissementClientDto))]
    [HttpPut("update_french_dashdoc_company/{etabId}")]
    [Authorize (Policy = "UpdateClient")]
    public async Task<IActionResult> UpdateFrenchDashdocCompany(int etabId)
    {
        EtablissementClientDto etablissement = await _etablissementClientService.GetById(etabId); 
        if(etablissement == null)
        {
            return NotFound();
        }
        
        var idDashDoc = etablissement.IdDashdoc;
        
        if (idDashDoc != null)
        {
            var dashdocCompanyDto = _mapper.Map<DashdocCompanyDto>(etablissement);
            dashdocCompanyDto.PK = idDashDoc;
            var dashdocCompany = await _dashdocService.UpdateDashdocCompanyAsync(dashdocCompanyDto);
            if (dashdocCompany == null)
            {
                return BadRequest();
            }
            await _etablissementClientService.Edit(etablissement);
            return Ok(etablissement);
        }
        
        return BadRequest();
    }
    
    /// <summary>
    /// Met à jour un client européen dans DashDoc
    /// </summary>
    /// <param name="etabId">ID de l'étabissement</param>
    /// <returns>La fiche client mise a jour</returns>
    [SwaggerResponse(200, "ClientEurope mis à jour dans DashDoc", typeof(ClientEuropeDto))]
    [SwaggerResponse(400, "ClientEurope non mis à jour dans DashDoc", typeof(ClientEuropeDto))]
    [SwaggerResponse(404, "ClientEurope non trouvé", typeof(ClientEuropeDto))]
    [HttpPut("update_europe_dashdoc_company/{etabId}")]
    [Authorize (Policy = "UpdateClient")]
    public async Task<IActionResult> UpdateEuropeDashdocCompany(int etabId)
    {
        ClientEuropeDto clientEurope = await _clientEuropeService.GetById(etabId); 
        if(clientEurope == null)
        {
            return NotFound();
        }
        
        var idDashDoc = clientEurope.IdDashdoc;
        
        if (idDashDoc != null)
        {
            var dashdocCompanyDto = _mapper.Map<DashdocCompanyDto>(clientEurope);
            dashdocCompanyDto.PK = idDashDoc;
            var dashdocCompany = await _dashdocService.UpdateDashdocCompanyAsync(dashdocCompanyDto);
            if (dashdocCompany == null)
            {
                return BadRequest();
            }
            await _clientEuropeService.Update(clientEurope);
            return Ok(clientEurope);
        }
        
        return BadRequest();
    }
}