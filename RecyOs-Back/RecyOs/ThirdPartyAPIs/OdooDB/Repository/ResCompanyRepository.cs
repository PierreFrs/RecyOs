using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PortaCapena.OdooJsonRpcClient;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.Engine.Services;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;

namespace RecyOs.OdooDB.Repository;

public class ResCompanyRepository : BaseOdooRepository, IResCompanyRepository<ResCompanyOdooModel>
{
    private readonly OdooRepository<ResCompanyOdooModel> _odooRepository;
    
    public ResCompanyRepository(IConfiguration configuration) : base(configuration)
    {
        _odooRepository = new OdooRepository<ResCompanyOdooModel>(_odooConfig);
    }
    
    public async Task<ResCompanyOdooModel> GetCompanyAsync(long id)
    {
        var filter = OdooFilter<ResCompanyOdooModel>.Create()
            .Or()
            .EqualTo(x => x.Id, id);
        var res = await _odooRepository.Query()
            .Where(filter)
            .FirstOrDefaultAsync();
        return res.Value;
    }
    
    public async Task<ResCompanyOdooModel[]> SearchCompany(OdooFilter filter)
    {
        var res = await _odooRepository.Query()
            .Where(filter)
            .ToListAsync();
        return res.Value;
    }
    
    public async Task<ResCompanyOdooModel[]> GetAllCompanies()
    {
        var res = await _odooRepository.Query()
            .ToListAsync();
        return res.Value;
    }
}