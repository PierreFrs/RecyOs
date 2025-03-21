// BalanceParticulierRepository.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 30/01/2025
// Fichier Modifié le : 30/01/2025
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.Extensions.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

namespace RecyOs.ORM.EFCore.Repository.hub;

public class BalanceParticulierRepository : BalanceRepository<BalanceParticulier>, IBalanceParticulierRepository
{
    public BalanceParticulierRepository(
        DataContext context,
        ITokenInfoService tokenInfoService
        ) : base(context, tokenInfoService)
    {
    }

    public async Task<(IEnumerable<BalanceParticulier>,int, decimal)> GetFilteredListWithCount(BalanceParticulierGridFilter filter, bool includeDeleted = false)
    {
        var query = _dbSet.ApplyFilter<BalanceParticulier>(filter).Include(b => b.ClientParticuliers).Include(b => b.Societe);
        return (
            await query.Skip(filter.PageSize * (filter.PageNumber - 1)).Take(filter.PageSize).ToArrayAsync(),
            await query.CountAsync(),
            await query.SumAsync(b => b.Montant)
        );
    }
}