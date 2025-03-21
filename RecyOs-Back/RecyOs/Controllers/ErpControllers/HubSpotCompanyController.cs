// Created by : Pierre FRAISSE
// RecyOs => RecyOs => HubSpotCompanyController.cs
// Created : 2024/04/18 - 09:13
// Updated : 2024/04/18 - 09:13

using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.Engine.Modules.HubSpot.Interfaces;
using RecyOs.HubSpotDB.DTO;
using RecyOs.HubSpotDB.Interfaces;
using RecyOs.OdooDB.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces.hub;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("hubspot_company")]
public class HubSpotCompanyController : BaseApiController
{
    private readonly IEtablissementClientService _etablissementClientService;
    private readonly IClientEuropeService _clientEuropeService;
    private readonly ICompaniesService _companiesService;
    private readonly IMapper _mapper;
    
    public HubSpotCompanyController(
        IEtablissementClientService etablissementClientService, 
        IClientEuropeService clientEuropeService, 
        ICompaniesService companiesService, 
        IMapper mapper
        )
    {
        _etablissementClientService = etablissementClientService;
        _clientEuropeService = clientEuropeService;
        _companiesService = companiesService;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Permet de creér un client dans HubSpot à partir d'un client français. si idHubSpot existe on ne fait rien
    /// </summary>
    /// <param name="id"></param>
    /// <returns>La fiche client mise a jour</returns>
    [SwaggerResponse(200, "EtablissementClient crée dans HubSpot", typeof(EtablissementClientDto))]
    [SwaggerResponse(400, "EtablissementClient non crée dans HubSpot", typeof(EtablissementClientDto))]
    [SwaggerResponse(404, "EtablissementClient non trouvé", typeof(EtablissementClientDto))]
    [HttpPost("create_french_company/{id}")]
    [Authorize(Policy = "UpdateClient")]
    public async Task<IActionResult> CreateFrenchCompany(int id)
    {
        EtablissementClientDto etablissement = await _etablissementClientService.GetById(id); 
        if(etablissement == null)
        {
            return NotFound();
        }
        
        string idHubSpot = etablissement.IdHubspot;
        
        if (string.IsNullOrWhiteSpace(idHubSpot))
        {
            CompaniesDto companiesDto = _mapper.Map<CompaniesDto>(etablissement);
            var ret = await _companiesService.CreateCompany(companiesDto);
            
            if (ret?.Id != null)
            {
                etablissement.IdHubspot = ret.Id.ToString();
                await _etablissementClientService.Edit(etablissement);
                return Ok(etablissement);
            }
            else
            {
                return BadRequest();
            }
        }
        else
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// Permet de creér un client dans HubSpot à partir d'un client europe. si idHubSpot existe on ne fait rien
    /// </summary>
    /// <param name="id"></param>
    /// <returns>La fiche client mise a jour</returns>
    [SwaggerResponse(200, "EtablissementClient crée dans HubSpot", typeof(EtablissementClientDto))]
    [SwaggerResponse(400, "EtablissementClient non crée dans HubSpot", typeof(EtablissementClientDto))]
    [SwaggerResponse(404, "EtablissementClient non trouvé", typeof(EtablissementClientDto))]
    [HttpPost("create_europe_company/{id}")]
    [Authorize(Policy = "UpdateClient")]
    public async Task<IActionResult> CreateEuropeCompany(int id)
    {
        ClientEuropeDto clientEurope = await _clientEuropeService.GetById(id);
        if (clientEurope == null)
        {
            return NotFound();
        }

        string idHubSpot = clientEurope.IdHubspot;

        if (string.IsNullOrWhiteSpace(idHubSpot))
        {
            CompaniesDto companiesDto = _mapper.Map<CompaniesDto>(clientEurope);
            var ret = await _companiesService.CreateCompany(companiesDto);

            if (ret?.Id != null)
            {
                clientEurope.IdHubspot = ret.Id.ToString();
                await _clientEuropeService.Update(clientEurope);
                return Ok(clientEurope);
            }
            else
            {
                return BadRequest();
            }
        }
        else
        {
            return BadRequest();
        }
    }
}