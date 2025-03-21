using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PortaCapena.OdooJsonRpcClient;
using PortaCapena.OdooJsonRpcClient.Request;
using RecyOs.Engine.Services;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;

namespace RecyOs.OdooDB.Repository;

public class AccountAccountRepository: BaseOdooRepository, IAccountAccountRepository<AccountAccountOdooModel>
{
    private readonly OdooRepository<AccountAccountOdooModel> _odooRepository;
    
    public AccountAccountRepository(IConfiguration configuration) : base(configuration)
    {
        _odooRepository = new OdooRepository<AccountAccountOdooModel>(_odooConfig);
    }
    
    public async Task<AccountAccountOdooModel> GetAccountAccountAsync(long id)
    {
        var filter = OdooFilter<AccountAccountOdooModel>.Create()
            .Or()
            .EqualTo(x => x.Id, id);
        var res = await _odooRepository.Query()
            .Where(filter)
            .FirstOrDefaultAsync();
        return res.Value;
    }
    
    public async Task<AccountAccountOdooModel[]> SearchAccountAccount(OdooFilter filter)
    {
        var res = await _odooRepository.Query()
            .Where(filter)
            .ToListAsync();
        return res.Value;
    }
    
    public async Task<AccountAccountOdooModel[]> GetAllAccountAccounts()
    {
        var res = await _odooRepository.Query()
            .ToListAsync();
        return res.Value;
    }
}