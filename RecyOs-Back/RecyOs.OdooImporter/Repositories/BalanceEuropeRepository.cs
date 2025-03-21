using RecyOs.OdooImporter.Interfaces;
using RecyOs.OdooImporter.Services;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.OdooImporter.Repositories;

public class BalanceEuropeRepository : BalanceRepository<BalanceEurope>, IBalanceEuropeRepository
{
    public BalanceEuropeRepository(IOdooImporterDbContext odooImporterDbContext) : base(odooImporterDbContext)
    {
    }
}