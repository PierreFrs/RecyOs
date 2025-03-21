using System.Collections.Generic;
using System.Threading.Tasks;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.OdooDB.DTO;


namespace RecyOs.OdooDB.Interfaces
{
    public interface IResCountryService
    {
        Task<ResCountryDto> GetCountryByIdAsync(long id);
        Task<IEnumerable<ResCountryDto>> SearchCountriesAsync(OdooFilter filter);
    }
}