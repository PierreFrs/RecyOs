using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.OdooDB.Entities;

namespace RecyOs.OdooDB.Interfaces;

public interface IMoveAccountRepository
{ 
    Task<decimal> GetAccountBalance(long partnerId, long companyId, string prmDate, long accountId);
    Task<IList<AccountMoveLineOdooModel>> GetAccountMoveLines(string prmDate, long accountId);
    Task<IList<AccountMoveLineOdooModel>> GetAccountLines(long partnerId, string prmDate, long[] accountIds);
}