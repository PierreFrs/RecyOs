//  InstallatorControler.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 12/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyOs.Cron;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using RecyOs.ORM.Interfaces.ICron;
using Swashbuckle.AspNetCore.Annotations;

namespace RecyOs.Controllers;

[Route("installator")]
public class InstallatorControler : BaseApiController
{
    private readonly ICommandImportFcli _commandImportFcli;
    private readonly ICommandImportCouverture _commandImportCouverture;
    private readonly ICommandImportNDCover _commandImportNdCover;
    private readonly ICommandExportSoumissionNDCoverService _commandExportSoumissionNdCoverService;
    private readonly ISyncBalanceCron _syncBalanceCron;
    
    public InstallatorControler(
        ICommandImportFcli commandImportFcli, 
        ICommandImportCouverture commandImportCouverture, 
        ICommandImportNDCover commandImportNdCover,
        ICommandExportSoumissionNDCoverService commandExportSoumissionNdCoverService,
        ISyncBalanceCron syncBalanceCron)
    {
        _commandImportFcli = commandImportFcli;
        _commandImportCouverture = commandImportCouverture;
        _commandImportNdCover = commandImportNdCover;
        _commandExportSoumissionNdCoverService = commandExportSoumissionNdCoverService;
        _syncBalanceCron = syncBalanceCron;
    }

    /// <summary>
    /// Importe le fichier client de MKGT dans la base de données. Seul les fiches clients ayant un numéro de SIRET valide sont importées.
    /// </summary>
    /// <returns></returns>
    [HttpGet("import-fcli")]
    [Authorize(Policy = "InstallatorOnly")]
    public async Task<IActionResult> ImportFcli()
    {
        await _commandImportFcli.Import();
        return Ok();
    }

    /// <summary>
    /// Importe le fichier de couverture Euler Hermes dans la base de données.
    /// </summary>
    /// <param name="file">Fichier excel envoyé par le courtier</param>
    /// <returns></returns>
    [HttpPost]
    [Route("import-couverture")]
    [Authorize(Policy = "OperatorOnly")]
    public async Task<IActionResult> ImportCouverture(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Content("file not selected");
        
        await _commandImportCouverture.Import(file);
        
        return Ok();
    }
    
    /// <summary>
    /// Vérifie si le fichier de couverture Euler Hermes est valide.
    /// </summary>
    /// <param name="file">Fichier excel envoyé par le courtier</param>
    /// <returns>true si le fichier est valide</returns>
    [HttpPost]
    [Route("check-couverture")]
    [Authorize(Policy = "OperatorOnly")]
    public async Task<IActionResult> CheckCouverture(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Content("file not selected");
        var check = await _commandImportCouverture.CheckFormat(file);
        return Ok(check);
    }
    
    /// <summary>
    /// Importe le fichier de couverture NDCover dans la base de données.
    /// </summary>
    /// <param name="file">Fichier excel envoyé par le courtier</param>
    /// <returns></returns>
    [HttpPost]
    [Route("import-NDCover")]
    [Authorize(Policy = "OperatorOnly")]
    public async Task<IActionResult> ImportNdCoverFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Content("file not selected");
        
        await _commandImportNdCover.Import(file);
        
        return Ok();
    }
    
    /// <summary>
    /// Vérifie si le fichier de couverture ND Cover est valide.
    /// </summary>
    /// <param name="file">Fichier excel envoyé par le courtier</param>
    /// <returns>true si le fichier est valide</returns>
    [HttpPost]
    [Route("check-NDCover")]
    [Authorize(Policy = "OperatorOnly")]
    public async Task<IActionResult> CheckNdCoverFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Content("file not selected");
        var check = await _commandImportNdCover.CheckFormat(file);
        return Ok(check);
    }
    
    /// <summary>
    /// Exporte le fichier de soumission de couverture NDCover depuis la base de données.
    /// </summary>
    /// <param>Fichier excel à envoyer au courtier</param>
    /// <returns>Document excel soumission NDCover</returns>
    [HttpGet]
    [Route("export-soumission-ndcover-france")]
    [Authorize(Policy = "OperatorOnly")]
    public async Task<IActionResult> ExportNdCoverFileFrance()
    {
        
        var workbook = await _commandExportSoumissionNdCoverService.ExportSoumissionNDCoverFranceAsync();

        using (var stream = new MemoryStream())
        {
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NDCover_France_Soumission_Export.xlsx");
        }
    }
    
    /// <summary>
    /// importe un client par son code depuis MKGT
    /// </summary>
    /// <param name="code">code du client à importer</param>
    [SwaggerResponse(200, "Client importé")]
    [SwaggerResponse(401, "Non autorisé")]
    [SwaggerResponse(404, "Client non trouvé ou la fiche ne respecte pas les prérequis")]
    [HttpPost]
    [Route("import-mkgt-client")]
    [Authorize(Policy = "InstallatorOnly")]
    public async Task<IActionResult> ImportMkgtClient(string code)
    {
        var result = await _commandImportFcli.Import(code);
        if (!result ) return NotFound();
        return Ok();
    }
    
    [HttpGet]
    [Route("sync-balance")]
    [AllowAnonymous]
    public async Task SyncBalance()
    {
        await _syncBalanceCron.ExecuteAsync();
    }

    /// <summary>
    /// Met à jour les clients par rapports aux erreurs retournées par Allianz.
    /// </summary>
    /// <param name="file">Fichier excel envoyé par le courtier</param>
    /// <returns>true si le fichier est valide</returns>
    [HttpPost]
    [Route("import-ndcover-error")]
    [Authorize(Policy = "OperatorOnly")]
    public async Task<IActionResult> ImportNdCoverError(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return Content("file not selected");
        await _commandImportNdCover.ImportNdCoverErrorAsync(file);
        return Ok();
    }
}