
using System.Collections.Generic;
using RecyOs.OdooDB.Entities;

namespace RecyOs.OdooDB.Interfaces;

public interface IAcountAccountService
{
    public AccountAccountOdooModel GetAccountAccount(long id);
    public AccountAccountOdooModel[] GetAllAccountAccounts();
    public IList<AccountAccountOdooModel> GetAccountByCode(string prmCode);
    public IReadOnlyList<AccountAccountOdooModel> GetCustomerAccountList(long societeId);
}