using System.Threading.Tasks;
using RecyOs.OdooDB.Interfaces;

namespace RecyOs.OdooDB.Services;

public class BaseOdooService : IBaseOdooService
{
    protected readonly IBaseOdooRepository _baseOdooRepository;
    
    public BaseOdooService(IBaseOdooRepository baseOdooRepository)
    {
        _baseOdooRepository = baseOdooRepository;
    }
    
    public async Task<string> GetModelAsync(string modelName)
    {
        return await _baseOdooRepository.GetModelAsync(modelName);
    }
}