using RecyOs.OdooImporter.Interfaces;
using RecyOs.OdooImporter.Services;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.OdooImporter.Repositories;

public class BalanceFranceRepository : BalanceRepository<BalanceFrance>, IBalanceFranceRepository
{
    public BalanceFranceRepository(IOdooImporterDbContext odooImporterDbContext) : base(odooImporterDbContext)
    {
    }
}