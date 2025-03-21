using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.OdooDB.Entities;

namespace RecyOs.OdooDB.Interfaces
{
    public interface IResCountryRepository<TResCountry> where TResCountry : ResCountryOdooModel, new()
    {
        Task<TResCountry> GetCountryAsync(long id);
        Task<TResCountry[]> SearchCountry(OdooFilter filter);
        Task<ResCountryOdooModel[]> GetAllCountries();
    }
}