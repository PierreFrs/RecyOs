using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.Engine.Modules.Odoo;
using RecyOs.OdooDB.DTO;
using RecyOs.OdooDB.Interfaces;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Models.DTO.hub;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("partner_odoo")]
public class PartnerOdooController : BaseApiController
{
    private readonly IEtablissementClientService _etablissementClientService;
    private readonly IClientEuropeService _clientEuropeService;
    private readonly IClientParticulierService _clientParticulierService;
    private readonly IResPartnerService _resPartnerService;
    private readonly IMapper _mapper;
    private readonly IEtablissementFournisseurService _etablissementFournisseurService;
    private readonly IFournisseurEuropeService _fournisseurEuropeService;
    
    public PartnerOdooController(
        IEtablissementClientService etablissementClientService, 
        IResPartnerService odooClientService, 
        IMapper mapper,
        IClientEuropeService clientEuropeService,
        IClientParticulierService clientParticulierService,
        IEtablissementFournisseurService etablissementFournisseurService,
        IFournisseurEuropeService fournisseurEuropeService)
    {
        _etablissementClientService = etablissementClientService;
        _resPartnerService = odooClientService;
        _mapper = mapper;
        _clientEuropeService = clientEuropeService;
        _clientParticulierService = clientParticulierService;
        _etablissementFournisseurService = etablissementFournisseurService;
        _fournisseurEuropeService = fournisseurEuropeService;
    }

    /// <summary>
    /// Permet de creér un client dans odoo à partir d'un client français. si idOdoo existe on ne fait rien
    /// </summary>
    /// <param name="id"></param>
    /// <returns>La fiche client mise a jour</returns>
    [SwaggerResponse(200, "EtablissementClient crée dans odoo", typeof(EtablissementClientDto))]
    [SwaggerResponse(400, "EtablissementClient non crée dans odoo", typeof(EtablissementClientDto))]
    [SwaggerResponse(404, "EtablissementClient non trouvé", typeof(EtablissementClientDto))]
    [HttpPost("create_french_client/{id}")]
    [Authorize(Policy = "CreationOdoo")]
    public async Task<IActionResult> createFrenchCli(int id)
    {

        EtablissementClientDto etablissement = await _etablissementClientService.GetById(id); 
        if(etablissement == null)
        {
            return NotFound();  // Client not found
        }
        string idOdoo = etablissement.IdOdoo; // get the odoo id of the client
        
        // if no id existed, create a new client
        if (string.IsNullOrWhiteSpace(idOdoo))
        {
            ResPartnerDto resPartnerDto = _mapper.Map<ResPartnerDto>(etablissement);
            var ret = await _resPartnerService.InsertPartnerAsync(resPartnerDto);
            
            if (ret != null)
            {
                // update the idOdoo of the client
                etablissement.IdOdoo = ret.Id.ToString();
                await _etablissementClientService.Edit(etablissement);
                return Ok(etablissement); // success
            }
            else
            {
                return BadRequest(); // do nothing
            }
        }else
        {
            return BadRequest(); // do nothing
        }
    }

    /// <summary>
    /// Permet de creér un client dans odoo à partir d'un client europe. si idOdoo existe on ne fait rien
    /// </summary>
    /// <param name="id"></param>
    /// <returns>La fiche client mise a jour</returns>
    [SwaggerResponse(200, "EtablissementClient crée dans odoo", typeof(EtablissementClientDto))]
    [SwaggerResponse(400, "EtablissementClient non crée dans odoo", typeof(EtablissementClientDto))]
    [SwaggerResponse(404, "EtablissementClient non trouvé", typeof(EtablissementClientDto))]
    [HttpPost("create_europe_client/{id}")]
    [Authorize(Policy = "CreationOdoo")]
    public async Task<IActionResult> CreateEuropeCli(int id)
    {
        ClientEuropeDto clientEurope = await _clientEuropeService.GetById(id);
        if (clientEurope == null)
        {
            return NotFound(); // Client not found
        }

        string idOdoo = clientEurope.IdOdoo; // get the odoo id of the client

        // if no id existed, create a new client
        if (string.IsNullOrWhiteSpace(idOdoo))
        {
            ResPartnerDto resPartnerDto = _mapper.Map<ResPartnerDto>(clientEurope);
            var ret = await _resPartnerService.InsertPartnerAsync(resPartnerDto);

            if (ret != null)
            {
                // update the idOdoo of the client
                clientEurope.IdOdoo = ret.Id.ToString();
                await _clientEuropeService.Update(clientEurope);
                return Ok(clientEurope); // success
            }
            else
            {
                return BadRequest(); // do nothing
            }
        }
        else
        {
            return BadRequest(); // do nothing
        }
    }

    /// <summary>
    /// Permet de creér un client dans odoo à partir d'un client particulier. si idOdoo existe on ne fait rien
    /// </summary>
    /// <param name="id"></param>
    /// <returns>La fiche client mise a jour</returns>
    [SwaggerResponse(200, "ClientParticulier crée dans odoo", typeof(ClientParticulierDto))]
    [SwaggerResponse(400, "ClientParticulier non crée dans odoo", typeof(ClientParticulierDto))]
    [SwaggerResponse(404, "ClientParticulier non trouvé", typeof(ClientParticulierDto))]
    [HttpPost("create_particulier_client/{id}")]
    [Authorize(Policy = "UpdateClient")]
    public async Task<IActionResult> CreateParticulierCli(int id)
    {

        ClientParticulierDto client = await _clientParticulierService.GetClientParticulierByIdAsync(id); 
        if(client == null)
        {
            return NotFound();  // Client not found
        }
        string idOdoo = client.IdOdoo; // get the odoo id of the client
        
        // if no id existed, create a new client
        if (string.IsNullOrWhiteSpace(idOdoo))
        {
            ResPartnerDto resPartnerDto = _mapper.Map<ResPartnerDto>(client);
            var ret = await _resPartnerService.InsertPartnerAsync(resPartnerDto);
            
            if (ret != null)
            {
                // update the idOdoo of the client
                client.IdOdoo = ret.Id.ToString();
                await _clientParticulierService.UpdateClientParticulierAsync(client.Id, client);
                return Ok(client); // success
            }
            else
            {
                return BadRequest(); // do nothing
            }
        }else
        {
            return BadRequest(); // do nothing
        }
    }
    
    /// <summary>
    /// Creates a French supplier in Odoo.
    /// </summary>
    /// <param name="id">The ID of the supplier.</param>
    /// <returns>
    /// - 200 if the supplier is successfully created in Odoo.
    /// - 400 if the supplier is not created in Odoo.
    /// - 404 if the supplier is not found.
    /// </returns>
    [SwaggerResponse(200, "EtablissementClient crée dans odoo", typeof(EtablissementClientDto))]
    [SwaggerResponse(400, "EtablissementClient non crée dans odoo", typeof(EtablissementClientDto))]
    [SwaggerResponse(404, "EtablissementClient non trouvé", typeof(EtablissementClientDto))]
    [HttpPost("create_french_fournisseur/{id}")]
    [Authorize(Policy = "UpdateFournisseur")]
    public async Task<IActionResult> CreateFrenchFrn(int id)
    {
        EtablissementClientDto etablissement = await _etablissementFournisseurService.GetById(id); 
        if(etablissement == null)
        {
            return NotFound();  // Client not found
        }
        string idOdoo = etablissement.IdOdoo; // get the odoo id of the client
        
        // if no id existed, create a new client
        if (string.IsNullOrWhiteSpace(idOdoo))
        {
            ResPartnerDto resPartnerDto = _mapper.Map<ResPartnerDto>(etablissement);
            var ret = await _resPartnerService.InsertPartnerAsync(resPartnerDto);
            
            if (ret != null)
            {
                // update the idOdoo of the client
                etablissement.IdOdoo = ret.Id.ToString();
                await _etablissementFournisseurService.Edit(etablissement);
                return Ok(etablissement); // success
            }
            else
            {
                return BadRequest(); // do nothing
            }
        }else
        {
            return BadRequest(); // do nothing
        }
    }

    /// <summary>
    /// Creates a European supplier in Odoo.
    /// </summary>
    /// <param name="id">The ID of the client.</param>
    /// <returns>Returns the created EtablissementClientDto if successful, returns BadRequest if the client already exists or an error occurred.</returns>
    [SwaggerResponse(200, "EtablissementClient crée dans odoo", typeof(EtablissementClientDto))]
    [SwaggerResponse(400, "EtablissementClient non crée dans odoo", typeof(EtablissementClientDto))]
    [SwaggerResponse(404, "EtablissementClient non trouvé", typeof(EtablissementClientDto))]
    [HttpPost("create_europe_fournisseur/{id}")]
    [Authorize(Policy = "UpdateFournisseur")]
    public async Task<IActionResult> CreateEuropeFrn(int id)
    {
        ClientEuropeDto clientEurope = await _fournisseurEuropeService.GetById(id);
        if (clientEurope == null)
        {
            return NotFound(); // Client not found
        }

        string idOdoo = clientEurope.IdOdoo; // get the odoo id of the client

        // if no id existed, create a new client
        if (string.IsNullOrWhiteSpace(idOdoo))
        {
            ResPartnerDto resPartnerDto = _mapper.Map<ResPartnerDto>(clientEurope);
            var ret = await _resPartnerService.InsertPartnerAsync(resPartnerDto);

            if (ret != null)
            {
                // update the idOdoo of the client
                clientEurope.IdOdoo = ret.Id.ToString();
                await _fournisseurEuropeService.Update(clientEurope);
                return Ok(clientEurope); // success
            }
            else
            {
                return BadRequest(); // do nothing
            }
        }
        else
        {
            return BadRequest(); // do nothing
        }
    }
}