// Created by : Pierre FRAISSE
// RecyOs => RecyOs => ExportSoumissionNDCoverRepository.cs
// Created : 2024/01/23 - 11:41
// Updated : 2024/01/23 - 11:41

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecyOs.Controllers;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Commands;

public class CommandExportSoumissionNDCoverRepository 
    : BaseRepository<EntrepriseBase, DataContext>, 
        ICommandExportSoumissionNDCoverRepository<EntrepriseBase>
{
    private readonly DataContext _context;
public CommandExportSoumissionNDCoverRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IList<EntrepriseBase>> ExportSoumissionNDCoverFranceRepositoryAsync(ContextSession session, bool includeDeleted = false)
    {
        return await _context.EntrepriseBase
            .AsNoTracking()
            .Where(eb => eb.EntrepriseNDCover == null)
            .Where(eb => eb.EntrepriseCessee == false || eb.EntrepriseCessee == null)
            .Where(eb => eb.NDCoverError == null)
            .ToListAsync();
    }

    public async Task PurgeTableAsync()
    {
        var temporaryNdCoverRows = await _context.TemporaryNdCoverExports
            .Where(eb => eb.Id > 0)
            .ToListAsync();

        _context.TemporaryNdCoverExports.RemoveRange(temporaryNdCoverRows);
        await _context.SaveChangesAsync();
    }

    public async Task CreateTemporaryNdCoverExportBatchAsync(List<TemporaryNdCoverExport> entities)
    {
        await _context.TemporaryNdCoverExports.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task<string> GetSirenFromLineNumberAsync(int lineNumber)
    {
        var temporaryNdCoverExport = await _context.TemporaryNdCoverExports
            .AsNoTracking()
            .FirstOrDefaultAsync(eb => eb.FileRow == lineNumber);

        return temporaryNdCoverExport?.Siren;
    }
}