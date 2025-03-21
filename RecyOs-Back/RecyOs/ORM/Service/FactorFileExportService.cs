// Copyright : Pierre FRAISSE
// RecyOs>RecyOs>FactorFileExportService.cs
// Created : 2024/05/2121 - 09:05

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Mvc;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.Service;

public class FactorFileExportService : IFactorFileExportService
{
    private readonly IFactorFileExportRepository _factorFileExportRepository;
    private readonly IEtablissementClientRepository<EtablissementClient> _etablissementClientRepository;
    private readonly IClientEuropeRepository<ClientEurope> _clientEuropeRepository;
    private readonly IBusinessUnitRepository<BusinessUnit> _businessUnitRepository;

    public FactorFileExportService(
        IFactorFileExportRepository factorFileExportRepository,
        IEtablissementClientRepository<EtablissementClient> etablissementClientRepository,
        IClientEuropeRepository<ClientEurope> clientEuropeRepository,
        IBusinessUnitRepository<BusinessUnit> businessUnitRepository)
            
    {
        _factorFileExportRepository = factorFileExportRepository;
        _etablissementClientRepository = etablissementClientRepository;
        _clientEuropeRepository = clientEuropeRepository;
        _businessUnitRepository = businessUnitRepository;
    }
    
    public async Task<FileResult> ExportFactorFileAsync()
{
    // Create a list to store the BUs
    IList<BusinessUnit> buList = await _businessUnitRepository.GetListAsync(new ContextSession());
    IList<int> buIds = buList.Select(bu => bu.Id).ToList();

    // Create the files
    using (var memoryStream = new MemoryStream())
    {
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (int buId in buIds)
            {
                var factorClientList = await _factorFileExportRepository.ExportFactorFileRepositoryAsync(buId);

                var csvContent = new StringBuilder();
                csvContent.AppendLine(
                    "REFER," +
                    "NOM," +
                    "CPTBIC," +
                    "IDCPT," +
                    "CPT," +
                    "CPAYSCPT," +
                    "SIRET," +
                    "LANGUE," +
                    "PAYSRES," +
                    "ADLIB1," +
                    "ADCP," +
                    "ADLOC," +
                    "ADPAYS," +
                    "CCIV," +
                    "CNOM," +
                    "CPRENOM," +
                    "CEMAIL," +
                    "CTEL," +
                    "CFAX," +
                    "CLANGUE," +
                    "CINFO," +
                    "CPTLIB," +
                    "FLG," +
                    "CL1," +
                    "CL2," +
                    "CL3," +
                    "NOMBQE," +
                    "ADLIBBQE," +
                    "ADCPBQE," +
                    "ADVIBQE," +
                    "COMPTEMIGRE," +
                    "FORCEBIC," +
                    "SEPAMAILREF1," +
                    "SEPAMAILREF2," +
                    "TYPEDEB," +
                    "TYPESFICHIERS");

                var franceClientIds = new List<int>();
                var europeClientIds = new List<int>();

                // Write data
                foreach (var client in factorClientList)
                {
                    csvContent.AppendLine($"{client.Refer}," +
                                          $"{client.Nom}," +
                                          $"{client.Cptbic}," +
                                          $"{client.Idcpt}," +
                                          $"{client.Cpt}," +
                                          $"{client.Cpayscpt}," +
                                          $"{client.Siret}," +
                                          $"{client.Langue}," +
                                          $"{client.Paysres}," +
                                          $"{client.Adlib1}," +
                                          $"{client.Adcp}," +
                                          $"{client.Adloc}," +
                                          $"{client.Adpays}," +
                                          $"{client.Cciv}," +
                                          $"{client.Cnom}," +
                                          $"{client.Cprenom}," +
                                          $"{client.Cemail}," +
                                          $"{client.Ctel}," +
                                          $"{client.Cfax}," +
                                          $"{client.Clangue}," +
                                          $"{client.Cinfo}," +
                                          $"{client.Cptlib}," +
                                          $"{client.Flg}," +
                                          $"{client.Cl1}," +
                                          $"{client.Cl2}," +
                                          $"{client.Cl3}," +
                                          $"{client.Nombqe}," +
                                          $"{client.Adlibbqe}," +
                                          $"{client.Adcpbqe}," +
                                          $"{client.Advibqe}," +
                                          $"{client.Comptemigre}," +
                                          $"{client.Forcebic}," +
                                          $"{client.Sepamailref1}," +
                                          $"{client.Sepamailref2}," +
                                          $"{client.Typedeb}," +
                                          $"{client.Typesfichiers}");

                    if (!string.IsNullOrEmpty(client.Siret))
                    {
                        franceClientIds.Add(client.ClientId);
                    }
                    else
                    {
                        europeClientIds.Add(client.ClientId);
                    }
                    
                }

                var fileName = $"{buList.FirstOrDefault(bu => bu.Id == buId)?.Libelle}_factor_clients.csv";
                var entry = archive.CreateEntry(fileName);
                using (var entryStream = entry.Open())
                using (var streamWriter = new StreamWriter(entryStream))
                {
                    streamWriter.Write(csvContent.ToString());
                }

                if (franceClientIds.Any())
                {
                    await _factorFileExportRepository.UpdateExportDateForFranceClientsAsync(franceClientIds, buId);
                }

                if (europeClientIds.Any())
                {
                    await _factorFileExportRepository.UpdateExportDateForEuropeClientsAsync(europeClientIds, buId);
                }
            }
        }
        memoryStream.Position = 0;
        return new FileContentResult(memoryStream.ToArray(), "application/zip")
        {
            FileDownloadName = "clients_affacturage.zip"
        };
    }
}

    
}