using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecyOs.OdooDB.Interfaces;

public interface IMoveAccountService
{
    Task<decimal> GetAccountBalance(long partnerId, long companyId, string prmDate, long accountId);

    Task<IList<AccountMoveLineOdooModel>> GetAccountLines(long partnerId, string prmDate,
        long[] accountIds);
}