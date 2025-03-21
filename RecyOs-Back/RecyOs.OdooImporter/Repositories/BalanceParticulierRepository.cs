using RecyOs.OdooImporter.Interfaces;
using RecyOs.OdooImporter.Services;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.OdooImporter.Repositories;

public class BalanceParticulierRepository : BalanceRepository<BalanceParticulier>, IBalanceParticulierRepository
{
    public BalanceParticulierRepository(IOdooImporterDbContext odooImporterDbContext) : base(odooImporterDbContext)
    {
    }
}   