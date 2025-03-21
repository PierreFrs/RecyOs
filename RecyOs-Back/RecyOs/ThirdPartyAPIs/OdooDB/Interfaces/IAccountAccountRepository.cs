using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.OdooDB.Entities;

namespace RecyOs.OdooDB.Interfaces;

public interface IAccountAccountRepository<TAccountAcount> where TAccountAcount : AccountAccountOdooModel, new()
{
    Task<TAccountAcount> GetAccountAccountAsync(long id);
    Task<TAccountAcount[]> SearchAccountAccount(OdooFilter filter);
    Task<TAccountAcount[]> GetAllAccountAccounts();  
}