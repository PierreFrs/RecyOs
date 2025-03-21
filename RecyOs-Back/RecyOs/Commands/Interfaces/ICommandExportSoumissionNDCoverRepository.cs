// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IExportSoumissionNDCoverRepository.cs
// Created : 2024/01/23 - 11:41
// Updated : 2024/01/23 - 11:41

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.Controllers;

public interface ICommandExportSoumissionNDCoverRepository<TEntrepriseBase> 
    where TEntrepriseBase : EntrepriseBase, new()
{
    Task<IList<TEntrepriseBase>> ExportSoumissionNDCoverFranceRepositoryAsync(ContextSession session, bool includeDeleted = false);
    Task PurgeTableAsync();
    Task CreateTemporaryNdCoverExportBatchAsync(List<TemporaryNdCoverExport> entities);
    Task<string> GetSirenFromLineNumberAsync(int lineNumber);
}