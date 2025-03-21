using System.Threading.Tasks;

namespace RecyOs.OdooDB.Interfaces;

public interface IBaseOdooRepository
{
    public Task<string> GetModelAsync(string modelName);
}