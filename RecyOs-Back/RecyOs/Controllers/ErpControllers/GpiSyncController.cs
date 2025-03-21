using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCExport;
using RecyOs.Helpers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Hub.DTO;
using RecyOs.ORM.Interfaces.gpiSync;
using RecyOs.ORM.Interfaces.hub;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("gpi_sync")]
public class GpiSyncController : BaseApiController
{
    private readonly IGpiSyncService _gpiSyncService;
    private readonly IEtablissementClientService _etablissementClientService;
    private readonly IClientEuropeService _clientEuropeService;
    private readonly ICounterService _counterService;
    private readonly IEtablissementFournisseurService _etablissementFournisseurService;
    private readonly IFournisseurEuropeService _fournisseurEuropeService;
    
    public GpiSyncController(IGpiSyncService gpiSyncService, IEtablissementClientService etablissementClientService, 
        IClientEuropeService clientEuropeService, ICounterService counterService, 
        IEtablissementFournisseurService etablissementFournisseurService, IFournisseurEuropeService fournisseurEuropeService)
    {
        _gpiSyncService = gpiSyncService;
        _etablissementClientService = etablissementClientService;
        _clientEuropeService = clientEuropeService;
        _counterService = counterService;
        _etablissementFournisseurService = etablissementFournisseurService;
        _fournisseurEuropeService = fournisseurEuropeService;
    }

    /// <summary>
    /// Retrieves the changes made to the client data.
    /// </summary>
    /// <returns>A file result representing the client changes in CSV format.</returns>
    /// <response code="401">Unauthorized access.</response>
    /// <response code="403">Insufficient rights to access the resource.</response>
    /// <response code="404">No client changes found.</response>
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(403, "Vous n'avez pas les droits")]
    [SwaggerResponse(404, "Aucun changement de client")]
    [HttpGet]
    [Route("get-client-changes")]
    [Authorize(Policy = "GpiSync")]
    public async Task<IActionResult> getClientChanges()
    {
        IEnumerable<ClientGpiDto> _clientGpi = await _gpiSyncService.GetChangedCustomers();
        if (!_clientGpi.Any())
        {
            return NotFound(); // No client changes
        }
        var result = new CsvFileResult<ClientGpiDto>(_clientGpi, "clients.csv")
        {
            Delimiter = "|",  // Set the delimiter to pipe
        };
        return result;
    }

    /// <summary>
    /// Creates a French client in Gpi.
    /// </summary>
    /// <param name="id">The ID of the client.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> representing the asynchronous operation.
    /// The action returns one of the following HTTP status codes:
    /// 200 (OK) - The EtablissementClient is created in Odoo. The response body contains the created EtablissementClientDto.
    /// 400 (Bad Request) - The EtablissementClient is not created in Gpi.
    /// 401 (Unauthorized) - The user is not authorized.
    /// 404 (Not Found) - The EtablissementClient is not found.
    /// </returns>
    [SwaggerResponse(200, "EtablissementClient crée dans Gpi", typeof(EtablissementClientDto))]
    [SwaggerResponse(400, "EtablissementClient non crée dans Gpi")]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EtablissementClient non trouvé")]
    [HttpPost("create_french_client/{id}")]
    [Authorize(Policy = "CreationGpi")]
    public async Task<IActionResult> createFrenchCli(int id)
    {
        EtablissementClientDto etablissement = await _etablissementClientService.GetById(id); 
        if(etablissement == null)
        {
            return NotFound();  // Client not found
        }
        string codeGpi = etablissement.CodeGpi; // get the gpi code of the client
        
        // if no id existed, create a new client
        if (string.IsNullOrWhiteSpace(codeGpi))
        {
            CounterDto counter = await _counterService.IncrementCounterByName("Client_gpi");
            
            if (counter == null)
            {
                return BadRequest(); // Counter not found
            }
            etablissement.CodeGpi = "C" + counter.Value.ToString("D4");
            await _etablissementClientService.Edit(etablissement);
            return Ok(etablissement); // success
        }else
        {
            return BadRequest(); // do nothing
        }
    }

    /// <summary>
    /// Creates a new client in the Europe system.
    /// </summary>
    /// <param name="id">The ID of the client.</param>
    /// <returns>An ActionResult with the created EtablissementClientDto if the creation was successful,
    /// otherwise a BadRequestResult or NotFoundResult.</returns>
    [SwaggerResponse(200, "Client crée dans gpi", typeof(ClientEuropeDto))]
    [SwaggerResponse(400, "Client non crée dans gpi", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "EtablissementClient non trouvé", typeof(ClientEuropeDto))]
    [HttpPost("create_europe_client/{id}")]
    [Authorize(Policy = "CreationGpi")]
    public async Task<IActionResult> CreateEuropeCli(int id)
    {
        ClientEuropeDto clientEurope = await _clientEuropeService.GetById(id);
        if (clientEurope == null)
        {
            return NotFound(); // Client not found
        }

        string idGpi = clientEurope.CodeGpi; // get the gpi id of the client
        if (string.IsNullOrWhiteSpace(idGpi))
        {
            CounterDto counter = await _counterService.IncrementCounterByName("Client_gpi");
            if (counter == null)
            {
                return BadRequest(); // Counter not found
            }
            clientEurope.CodeGpi = "C" + counter.Value.ToString("D4");
            await _clientEuropeService.Update(clientEurope);
            return Ok(clientEurope); // success
        }else
        {
            return BadRequest(); // do nothing
        }
    }

    /// <summary>
    /// Creates a French fournisseur (supplier) in Gpi for the given EtablissementClient ID.
    /// </summary>
    /// <param name="id">The ID of the EtablissementClient.</param>
    /// <returns>The created EtablissementClient in Gpi if successful, an error response otherwise.</returns>
    [SwaggerResponse(200, "Fourniseur crée dans gpi", typeof(EtablissementClientDto))]
    [SwaggerResponse(400, "Fournisseur non crée dans gpi", typeof(EtablissementClientDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Etablissement non trouvé", typeof(EtablissementClientDto))]
    [HttpPost("create_french_fournisseur/{id}")]
    [Authorize(Policy = "UpdateFournisseur")]
    public async Task<IActionResult> createFrenchFrn(int id)
    {
        EtablissementClientDto etablissement = await _etablissementFournisseurService.GetById(id); 
        if(etablissement == null)
        {
            return NotFound();  // Client not found
        }
        string codeGpi = etablissement.FrnCodeGpi; // get the gpi code of the fournisseur
        
        // if no id existed, create a new client
        if (string.IsNullOrWhiteSpace(codeGpi))
        {
            CounterDto counter = await _counterService.IncrementCounterByName("Fourniseurs_gpi");
            
            if (counter == null)
            {
                return BadRequest(); // Counter not found
            }
            etablissement.FrnCodeGpi = "A" + counter.Value.ToString("D4");
            await _etablissementFournisseurService.Edit(etablissement);
            return Ok(etablissement); // success
        }else
        {
            return BadRequest(); // do nothing
        }
    }

    /// <summary>
    /// Creates a European Fournisseur in GPI.
    /// </summary>
    /// <param name="id">The ID of the Fournisseur to create in GPI.</param>
    /// <returns>
    /// Returns an IActionResult representing the HTTP response:
    /// - 200 (OK) if the Fournisseur was successfully created in GPI. The response body contains the created Fournisseur.
    /// - 400 (BadRequest) if the Fournisseur was not created in GPI. This can happen if the Fournisseur already has a GPI ID.
    /// - 401 (Unauthorized) if the user is not authorized to perform this operation.
    /// - 404 (Not Found) if the Fournisseur with the specified ID was not found.
    /// </returns>
    [SwaggerResponse(200, "Fournisseur crée dans gpi", typeof(ClientEuropeDto))]
    [SwaggerResponse(400, "Fournisseur non crée dans gpi", typeof(ClientEuropeDto))]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Etablissement non trouvé", typeof(ClientEuropeDto))]
    [HttpPost("create_europe_fournisseur/{id}")]
    [Authorize(Policy = "UpdateFournisseur")]
    public async Task<IActionResult> CreateEuropeFrn(int id)
    {
        ClientEuropeDto clientEurope = await _fournisseurEuropeService.GetById(id);
        if (clientEurope == null)
        {
            return NotFound(); // Client not found
        }

        string idGpi = clientEurope.FrnCodeGpi; // get the gpi id of the client
        if (string.IsNullOrWhiteSpace(idGpi))
        {
            CounterDto counter = await _counterService.IncrementCounterByName("Fourniseurs_gpi");
            if (counter == null)
            {
                return BadRequest(); // Counter not found
            }
            clientEurope.FrnCodeGpi = "A" + counter.Value.ToString("D4");
            await _fournisseurEuropeService.Update(clientEurope);
            return Ok(clientEurope); // success
        }else
        {
            return BadRequest(); // do nothing
        }
    }
}