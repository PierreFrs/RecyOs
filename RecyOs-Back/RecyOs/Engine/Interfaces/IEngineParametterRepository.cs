using System.Threading.Tasks;
using RecyOs.ORM.Entities;

namespace RecyOs.Engine.Interfaces;

public interface IEngineParametterRepository
{
    Task<Parameter> GetAsync(int id, bool includeDeleted = false);
    Task<Parameter> GetByNomAsync(string module, string nom, bool includeDeleted = false);
    
    Task<Parameter> CreateAsync(Parameter prm);
}