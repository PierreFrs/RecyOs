// Copyright : Pierre FRAISSE
// RecyOs>RecyOs>FactorFileExportRepository.cs
// Created : 2024/05/2121 - 10:05

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository;

public class FactorFileExportRepository : IFactorFileExportRepository
{
    private readonly DataContext _context;

    public FactorFileExportRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FactorClientDto>> ExportFactorFileRepositoryAsync(int buId)
{
    List<FactorClientDto> combinedEntities = new List<FactorClientDto>();

    // Fetch unexported FactorClientBuFrance records
    var factorClientBuFranceList = await _context.FactorClientFranceBus
        .AsNoTracking()
        .Where(fcb => fcb.ExportDate == null && fcb.IdBu == buId && fcb.IsDeleted == false)
        .Include(fcb => fcb.EtablissementClient)
        .ToListAsync();

    foreach (var factorClientBu in factorClientBuFranceList)
    {
        var ec = factorClientBu.EtablissementClient;
        if (ec != null)
        {
            var dto = new FactorClientDto
            {
                ClientId = ec.Id,
                Refer = Guid.NewGuid(),
                Nom = ec.Nom,
                Cptbic = string.Empty,
                Idcpt = "1",
                Cpt = "FR7611449000021111111111125",
                Cpayscpt = string.Empty,
                Siret = ec.Siret,
                Langue = "FR",
                Paysres = "FR",
                Adlib1 = ec.AdresseFacturation1,
                Adcp = ec.CodePostalFacturation,
                Adloc = ec.VilleFacturation,
                Adpays = CountryCodeHelper.GetCountryCode(ec.PaysFacturation),
                Cciv = string.Empty,
                Cnom = string.Empty,
                Cprenom = string.Empty,
                Cemail = ec.EmailFacturation,
                Ctel = ec.TelephoneFacturation,
                Cfax = string.Empty,
                Clangue = string.Empty,
                Cinfo = string.Empty,
                Cptlib = string.Empty,
                Flg = string.Empty,
                Cl1 = string.Empty,
                Cl2 = string.Empty,
                Cl3 = string.Empty,
                Nombqe = string.Empty,
                Adlibbqe = string.Empty,
                Adcpbqe = string.Empty,
                Advibqe = string.Empty,
                Comptemigre = string.Empty,
                Forcebic = string.Empty,
                Sepamailref1 = string.Empty,
                Sepamailref2 = string.Empty,
                Typedeb = string.Empty,
                Typesfichiers = string.Empty
            };

            combinedEntities.Add(dto);
        }
    }

    // Fetch unexported FactorClientBuEurope records
    var factorClientBuEuropeList = await _context.FactorClientEuropeBus
        .AsNoTracking()
        .Where(fcb => fcb.ExportDate == null && fcb.IdBu == buId && fcb.IsDeleted == false)
        .Include(fcb => fcb.ClientEurope)
        .ToListAsync();

    foreach (var factorClientBu in factorClientBuEuropeList)
    {
        var ce = factorClientBu.ClientEurope;
        if (ce != null)
        {
            var dto = new FactorClientDto
            {
                ClientId = ce.Id,
                Refer = Guid.NewGuid(),
                Nom = ce.Nom,
                Cptbic = string.Empty,
                Idcpt = "1",
                Cpt = "FR7611449000021111111111125",
                Cpayscpt = string.Empty,
                Siret = string.Empty,
                Langue = "FR",
                Paysres = CountryCodeHelper.GetCountryCode(ce.PaysFacturation),
                Adlib1 = ce.AdresseFacturation1,
                Adcp = ce.CodePostalFacturation,
                Adloc = ce.VilleFacturation,
                Adpays = CountryCodeHelper.GetCountryCode(ce.PaysFacturation),
                Cciv = string.Empty,
                Cnom = string.Empty,
                Cprenom = string.Empty,
                Cemail = ce.EmailFacturation,
                Ctel = ce.TelephoneFacturation,
                Cfax = string.Empty,
                Clangue = string.Empty,
                Cinfo = string.Empty,
                Cptlib = string.Empty,
                Flg = string.Empty,
                Cl1 = string.Empty,
                Cl2 = string.Empty,
                Cl3 = string.Empty,
                Nombqe = string.Empty,
                Adlibbqe = string.Empty,
                Adcpbqe = string.Empty,
                Advibqe = string.Empty,
                Comptemigre = string.Empty,
                Forcebic = string.Empty,
                Sepamailref1 = string.Empty,
                Sepamailref2 = string.Empty,
                Typedeb = string.Empty,
                Typesfichiers = string.Empty
            };
            combinedEntities.Add(dto);
        }
    }

    return combinedEntities;
}

    public async Task UpdateExportDateForFranceClientsAsync(IEnumerable<int> clientIds, int buId)
    {
        var clientsToUpdate = await _context.FactorClientFranceBus
            .Where(fcb => clientIds.Contains(fcb.IdClient) && fcb.IdBu == buId && fcb.ExportDate == null)
            .ToListAsync();

        foreach (var client in clientsToUpdate)
        {
            client.ExportDate = DateTime.UtcNow;
        }

        _context.FactorClientFranceBus.UpdateRange(clientsToUpdate);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateExportDateForEuropeClientsAsync(IEnumerable<int> clientIds, int buId)
    {
        var clientsToUpdate = await _context.FactorClientEuropeBus
            .Where(fcb => clientIds.Contains(fcb.IdClient) && fcb.IdBu == buId && fcb.ExportDate == null)
            .ToListAsync();

        foreach (var client in clientsToUpdate)
        {
            client.ExportDate = DateTime.UtcNow;
        }

        _context.FactorClientEuropeBus.UpdateRange(clientsToUpdate);
        await _context.SaveChangesAsync();
    }

}