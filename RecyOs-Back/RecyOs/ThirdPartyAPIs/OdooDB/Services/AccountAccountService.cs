/** AccountAccountService.cs
 * 
 */
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;
using RecyOs.OdooDB.Repository;

namespace RecyOs.OdooDB.Services;

public class AccountAccountService<TAccountAccount> : IAcountAccountService where TAccountAccount : AccountAccountOdooModel, new()
{
    private readonly IList<AccountAccountOdooModel> _accountAccounts;
    private readonly IAccountAccountRepository<TAccountAccount> _accountAccountRepository;
    private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    
    public AccountAccountService(IAccountAccountRepository<TAccountAccount> accountAccountRepository)
    {
        _logger.Trace("Mise en cache des comptes comptables");
        _accountAccountRepository = accountAccountRepository;
        _accountAccounts = _accountAccountRepository.GetAllAccountAccounts().Result;
        _logger.Trace("Fin de mise en cache des comptes comptables. Comptes en cache : " + _accountAccounts.Count);
    }
    
    public AccountAccountOdooModel GetAccountAccount(long id)
    {
        return _accountAccounts.FirstOrDefault(x => x.Id == id);
    }
    
    public AccountAccountOdooModel[] GetAllAccountAccounts()
    {
        return _accountAccounts.ToArray();
    }
    
    public IList<AccountAccountOdooModel> GetAccountByCode(string prmCode)
    {
        return _accountAccounts.Where(x => x.Code == prmCode).ToList();
    }

    public IReadOnlyList<AccountAccountOdooModel> GetCustomerAccountList(long societeId)
    {
        return _accountAccounts
            .Where(x => x.Code.StartsWith("411") && x.CompanyId == societeId)
            .ToList();
    }
}