// Created by : Pierre FRAISSE
// RecyOs => RecyOs => ExportSoumissionNDCover.cs
// Created : 2024/01/23 - 11:25
// Updated : 2024/01/23 - 11:25

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Configuration;
using RecyOs.Controllers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOs.Commands;

public class CommandExportSoumissionNDCoverService : BaseService, ICommandExportSoumissionNDCoverService
{
    private readonly IConfiguration _configuration;
    private readonly ICommandExportSoumissionNDCoverRepository<EntrepriseBase> _commandExportSoumissionNDCoverRepository;
    private readonly ITokenInfoService _tokenInfoService;
    
    public CommandExportSoumissionNDCoverService(
        IConfiguration configuration, 
        ICurrentContextProvider contextProvider, 
        ICommandExportSoumissionNDCoverRepository<EntrepriseBase> commandExportSoumissionNDCoverRepository,
        ITokenInfoService tokenInfoService)
        : base(contextProvider)
    {
        _configuration = configuration;
        _commandExportSoumissionNDCoverRepository = commandExportSoumissionNDCoverRepository;
        _tokenInfoService = tokenInfoService;
    }
    
    public async Task<XLWorkbook> ExportSoumissionNDCoverFranceAsync()
    {
        var data = await _commandExportSoumissionNDCoverRepository.ExportSoumissionNDCoverFranceRepositoryAsync(Session);

        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Data");
        
        worksheet.Cell(1,1).Value = "*Primary policy number";
        worksheet.Cell(1,2).Value = "Primary policy extension number";
        worksheet.Cell(1,3).Value = "Policy name";
        worksheet.Cell(1,4).Value = "EH ID";
        worksheet.Cell(1,5).Value = "*Identifier Type";
        worksheet.Cell(1,6).Value = "*Identifier";
        worksheet.Cell(1,7).Value = "*Country";
        worksheet.Cell(1,8).Value = "*Customer reference";
        
        int currentRow = 2;
        foreach (var entrepriseBase in data)
        {
            worksheet.Cell(currentRow,1).Value = _configuration["CoverPolicies:NDCoverFrance:PrimaryPolicyNumber"];
            worksheet.Cell(currentRow,2).Value = "";
            worksheet.Cell(currentRow,3).Value = _configuration["CoverPolicies:NDCoverFrance:PolicyName"];
            worksheet.Cell(currentRow,4).Value = "";
            worksheet.Cell(currentRow,5).Value =  _configuration["CoverPolicies:NDCoverFrance:PolicyType"];
            worksheet.Cell(currentRow,6).Value = entrepriseBase.Siren;
            worksheet.Cell(currentRow,7).Value = "FR";
            worksheet.Cell(currentRow,8).Value = _configuration["CoverPolicies:NDCoverFrance:CustomerReference"];
            currentRow++;
        }

        CreateTemporaryTable(workbook);
        
        return workbook;
    }

    private async Task CreateTemporaryTable(XLWorkbook workbook)
    {
        await _commandExportSoumissionNDCoverRepository.PurgeTableAsync();

        var worksheet = workbook.Worksheet(1);
        var entitiesToInsert = new List<TemporaryNdCoverExport>();
        int currentRow = 2;

        while (!string.IsNullOrWhiteSpace(worksheet.Cell(currentRow, 1).Value.ToString()) ||
               !string.IsNullOrWhiteSpace(worksheet.Cell(currentRow, 6).Value.ToString()))
        {
            Console.WriteLine($"Processing Row {currentRow}");

            var temporaryNdCoverExport = new TemporaryNdCoverExport()
            {
                FileRow = currentRow - 1,
                Siren = worksheet.Cell(currentRow, 6).Value.ToString(),
                CreateDate =  DateTime.Now,
                CreatedBy = _tokenInfoService.GetCurrentUserName(),
            };

            entitiesToInsert.Add(temporaryNdCoverExport);

            currentRow++;
        }

        await _commandExportSoumissionNDCoverRepository.CreateTemporaryNdCoverExportBatchAsync(entitiesToInsert);
    }
}