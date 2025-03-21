using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.OdooDB.Entities;

namespace RecyOs.OdooDB.Interfaces;

public interface IResCompanyRepository<TResCompany> where TResCompany : ResCompanyOdooModel, new()
{
    Task<TResCompany> GetCompanyAsync(long id);
    Task<TResCompany[]> SearchCompany(OdooFilter filter);
    Task<TResCompany[]> GetAllCompanies();
}