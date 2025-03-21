using RecyOs.ORM.Models.Entities.hub.IncomeOdoo;

namespace RecyOs.OdooImporter.Interfaces
{
    public interface IAccountMoveLineRepository
    {
        Task<bool> AddAsync(AccountMoveLine accountMoveLine);
        Task<bool> AddRangeAsync(IList<AccountMoveLine> accountMoveLines);
        Task<bool> EraseAllAsync();
    }
}