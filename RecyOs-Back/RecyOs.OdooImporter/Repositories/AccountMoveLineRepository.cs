using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.OdooImporter.Interfaces;
using RecyOs.ORM.Models.Entities.hub.IncomeOdoo;
using NLog;
using Microsoft.Data.SqlClient;

namespace RecyOs.OdooImporter.Repositories;

public class AccountMoveLineRepository : IAccountMoveLineRepository
{
    private readonly RecyOs.Helpers.DataContext _dbContext;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public AccountMoveLineRepository(IOdooImporterDbContext odooImporterDbContext)
    {
        _dbContext = odooImporterDbContext.GetContext();
    }

    public async Task<bool> AddAsync(AccountMoveLine accountMoveLine)
    {
        try
        {
            _dbContext.ChangeTracker.Clear();
            _logger.Trace($"Tentative de sauvegarde de la ligne {accountMoveLine.Id}");
            await _dbContext.Set<AccountMoveLine>().AddAsync(accountMoveLine);
            var result = await _dbContext.SaveChangesAsync();
            _logger.Trace($"Résultat de la sauvegarde: {result} lignes affectées");
            // Untrack all the entity
            _dbContext.ChangeTracker.Clear();
            return result > 0;
        }
                catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
        {
            _logger.Warn($"Attention, la ligne {accountMoveLine.Id} existe déjà dans la base de données doublon d'importation");
            return false;
        }
        catch (Exception ex)
        {
            _logger.Error($"Erreur lors de la sauvegarde: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> AddRangeAsync(IList<AccountMoveLine> accountMoveLines)
    {
        try
        {
            _dbContext.ChangeTracker.Clear();
            await _dbContext.Set<AccountMoveLine>().AddRangeAsync(accountMoveLines);
            var result = await _dbContext.SaveChangesAsync();
            _logger.Trace($"Résultat de la sauvegarde: {result} lignes affectées");
            // Untrack all the entities
            _dbContext.ChangeTracker.Clear();
            return result > 0;
        }
        // Duplicate key error
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException && sqlException.Number == 2627)
        {
            _logger.Warn($"Attention, la ligne { accountMoveLines } existe déjà dans la base de données doublon d'importation");
            return false;
        }
        catch (Exception ex)
        {
            _logger.Error($"Erreur lors de la sauvegarde: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> EraseAllAsync()
    {
        try
        {
            var allLines = await _dbContext.Set<AccountMoveLine>().ToListAsync();
            _dbContext.Set<AccountMoveLine>().RemoveRange(allLines);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"Erreur lors de la suppression: {ex.Message}");
            throw;
        }
    }
}
