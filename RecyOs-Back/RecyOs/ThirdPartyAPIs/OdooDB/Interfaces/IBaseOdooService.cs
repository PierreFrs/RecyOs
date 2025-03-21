using System.Threading.Tasks;

namespace RecyOs.OdooDB.Interfaces;

public interface IBaseOdooService
{
    public Task<string> GetModelAsync(string modelName);
}