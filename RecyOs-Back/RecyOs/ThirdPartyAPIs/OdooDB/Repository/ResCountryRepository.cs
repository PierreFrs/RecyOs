using System.Threading.Tasks;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;
using Microsoft.Extensions.Configuration;
using PortaCapena.OdooJsonRpcClient;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.Engine.Services;

namespace RecyOs.OdooDB.Repository
{
    public class ResCountryRepository : BaseOdooRepository, IResCountryRepository<ResCountryOdooModel>
    {
        private readonly OdooRepository<ResCountryOdooModel> _odooRepository;

        public ResCountryRepository(IConfiguration configuration) : base(configuration)
        {
            _odooRepository = new OdooRepository<ResCountryOdooModel>(_odooConfig);
        }

        public async Task<ResCountryOdooModel> GetCountryAsync(long id)
        {
            var filter = OdooFilter<ResCountryOdooModel>.Create()
                .Or()
                .EqualTo(x => x.Id, id);
            var res = await _odooRepository.Query()
                .Where(filter)
                .FirstOrDefaultAsync();
            return res.Value;
        }

        public async Task<ResCountryOdooModel[]> SearchCountry(OdooFilter filter)
        {
            var res = await _odooRepository.Query()
                .Where(filter)
                .ToListAsync();
            return res.Value;
        }
        
        public async Task<ResCountryOdooModel[]> GetAllCountries()
        {
            var res = await _odooRepository.Query()
                .ToListAsync();
            return res.Value;
        }
        
        
    }
}