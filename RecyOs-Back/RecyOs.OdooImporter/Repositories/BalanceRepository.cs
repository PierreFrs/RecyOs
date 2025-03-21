using Microsoft.EntityFrameworkCore;
using  RecyOs.OdooImporter.Interfaces;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using NLog;

namespace RecyOs.OdooImporter.Services;

public abstract class BalanceRepository<TBalance> : RecyOs.OdooImporter.Interfaces.IBalanceRepository<TBalance> 
    where TBalance : Balance, IClientBalance, new()
{
    protected readonly RecyOs.Helpers.DataContext _dbContext;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    protected BalanceRepository(IOdooImporterDbContext odooImporterDbContext)
    {
        _dbContext = odooImporterDbContext.GetContext();
    }

    public async Task<TBalance> SaveBalance(int clientId, int societeId, decimal balance)
    {
        try
        {
            var entityExists = await _dbContext.Set<TBalance>()
                .Where(q => q.ClientId == clientId && q.SocieteId == societeId)
                .FirstOrDefaultAsync();
            if (entityExists != null)
            {
                if (HasChanges(entityExists, balance))
                {
                    entityExists.Montant = balance;
                    entityExists.UpdatedBy = "RecyOs.OdooImporter";
                    entityExists.UpdatedAt = DateTime.Now;
                    entityExists.IsDeleted = false;
                    entityExists.DateRecuperationBalance = DateTime.Now;
                    await _dbContext.SaveChangesAsync();
                }
                return entityExists;
            }
            var newBalance = new TBalance { ClientId = clientId, SocieteId = societeId, Montant = balance };
            newBalance.CreatedBy = "RecyOs.OdooImporter";
            newBalance.CreateDate = DateTime.Now;
            newBalance.IsDeleted = false;
            newBalance.DateRecuperationBalance = DateTime.Now;
            await _dbContext.Set<TBalance>().AddAsync(newBalance);
            await _dbContext.SaveChangesAsync();
            return newBalance;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error saving balance");
            throw;
        }
    }

    private bool HasChanges(TBalance existingBalance, decimal newBalance)
    {
        return existingBalance.Montant != newBalance || existingBalance.IsDeleted;
    }
}
