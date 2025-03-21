using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using RecyOs.OdooDB.Interfaces;
using RecyOs.OdooDB.Repository;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces;

namespace RecyOs.OdooDB.Services;

public class MoveAccountService: IMoveAccountService
{
    private readonly IMoveAccountRepository _moveAccountRepository;

    public MoveAccountService(IMoveAccountRepository moveAccountRepository)
    {
        _moveAccountRepository = moveAccountRepository;

    }

    /// <summary>
    /// Retrieves the account balance of a given partner for a specific date.
    /// </summary>
    /// <param name="partnerId">The ID of the partner.</param>
    /// <param name="companyId">The ID of the company.</param>
    /// <param name="prmDate">The specific date for which to retrieve the account balance.</param>
    /// <returns>The account balance of the partner for the specified date.</returns>
    public async Task<decimal> GetAccountBalance(long partnerId, long companyId, string prmDate, long accountId)
    {
        return await _moveAccountRepository.GetAccountBalance(partnerId, companyId, prmDate, accountId);
    }

    public async Task<IList<AccountMoveLineOdooModel>> GetAccountLines(long partnerId, string prmDate,
        long[] accountIds)
    {
        return await _moveAccountRepository.GetAccountLines(partnerId, prmDate, accountIds);
    }
}