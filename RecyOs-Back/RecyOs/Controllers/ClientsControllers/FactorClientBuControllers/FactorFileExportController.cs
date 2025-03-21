// Copyright : Pierre FRAISSE
// RecyOs>RecyOs>FactorFileExport.cs
// Created : 2024/05/2121 - 08:05

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.Interfaces;

namespace RecyOs.Controllers;

[Route("factor")]

public class FactorFileExportController  : BaseApiController
{
    private readonly IFactorFileExportService _factorFileExportService;

    public FactorFileExportController(IFactorFileExportService factorFileExportService)
    {
        _factorFileExportService = factorFileExportService;
    }
    
    /// <summary>
    /// Exporte le fichier d'affacturage depuis la base de données.
    /// </summary>
    /// <param>Fichier CSV à envoyer à la société d'affacturage</param>
    /// <returns>Document CSV d'affacturage</returns>
    [HttpGet]
    [Route("export-factor-file")]
    [Authorize(Policy = "OperatorOnly")]
    public async Task<IActionResult> ExportFactorFile()
    {
        var fileResult = await _factorFileExportService.ExportFactorFileAsync();
        return fileResult;
    }
}