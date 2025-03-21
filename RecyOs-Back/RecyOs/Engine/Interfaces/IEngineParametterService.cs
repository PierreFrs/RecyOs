using System.Threading.Tasks;
using RecyOs.ORM.DTO;

namespace RecyOs.Engine.Interfaces;

public interface IEngineParametterService
{
    Task<ParameterDto> GetAsync(int id, bool includeDeleted = false);
    Task<ParameterDto> GetByNomAsync(string module, string nom, bool includeDeleted = false);
    Task<ParameterDto> CreateAsync(ParameterDto prm);
}