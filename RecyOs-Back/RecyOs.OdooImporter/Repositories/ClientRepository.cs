using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.OdooImporter.Interfaces;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.OdooImporter.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly RecyOs.Helpers.DataContext _dbContext;

    public ClientRepository(IOdooImporterDbContext odooImporterDbContext)
    {
        _dbContext = odooImporterDbContext.GetContext();
    }

    public async Task<IList<long>> GetAllAsync()
    {
        var result = new List<long>();

        // Récupérer les IdOdoo des EtablissementClient (francais)
        var etablissementClientIds = await _dbContext.Set<EtablissementClient>()
            .Where(c => !c.IsDeleted && c.IdOdoo != null && c.Client && c.IdOdoo != "")
            .Select(c => long.Parse(c.IdOdoo))
            .ToListAsync();
        result.AddRange(etablissementClientIds);

        // Récupérer les IdOdoo des ClientEurope (européens)
        var clientEuropeIds = await _dbContext.Set<ClientEurope>()
            .Where(c => !c.IsDeleted && c.IdOdoo != null && c.Client && c.IdOdoo != "")
            .Select(c => long.Parse(c.IdOdoo))
            .ToListAsync();
        result.AddRange(clientEuropeIds);

        // Récupérer les IdOdoo des ClientParticulier
        var clientParticulierIds = await _dbContext.Set<ClientParticulier>()
            .Where(c => !c.IsDeleted && c.IdOdoo != null && c.IdOdoo != "")
            .Select(c => long.Parse(c.IdOdoo))
            .ToListAsync();
        result.AddRange(clientParticulierIds);

        // Retourner la liste unique et triée
        return result.Distinct().OrderBy(id => id).ToList();
    }

    public async Task<IList<EtablissementClient>> GetAllFrenchAsync()
    {

        // Récupérer les IdOdoo des EtablissementClient (francais)
        var etablissementClientIds = await _dbContext.Set<EtablissementClient>()
            .Where(c => !c.IsDeleted && c.IdOdoo != null && c.Client && c.IdOdoo != "" && c.NoBalance != true)
            .ToListAsync();

        return etablissementClientIds;
    }   

    public async Task<IList<ClientEurope>> GetAllEuropeanAsync()
    {
        // Récupérer les IdOdoo des ClientEurope (européens)
        var clientEuropeIds = await _dbContext.Set<ClientEurope>()
            .Where(c => !c.IsDeleted && c.IdOdoo != null && c.Client && c.IdOdoo != "" && c.NoBalance != true)
            .ToListAsync();

        return clientEuropeIds;
    }

    public async Task<IList<ClientParticulier>> GetAllIndividualAsync()
    {   // Récupérer les IdOdoo des ClientParticulier
        var clientParticulierIds = await _dbContext.Set<ClientParticulier>()
            .Where(c => !c.IsDeleted && c.IdOdoo != null && c.IdOdoo != "" && c.NoBalance != true)
            .ToListAsync();

        return clientParticulierIds;
    }
}