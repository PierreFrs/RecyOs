using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;

namespace RecyOs.ORM.Interfaces.IParameters;

public interface IParameterRepository<TParameter> where TParameter : Parameter
{
    Task<TParameter> GetAsync(int id, ContextSession session, bool includeDeleted = false);
    Task<(IEnumerable<Parameter>, int)> GetDataForGrid(ParameterFilter filter, ContextSession session,
        bool includeDeleted = false);
    Task<TParameter> GetByNom(string module, string nom, ContextSession session, bool includeDeleted = false);
    Task<TParameter> UpdateAsync(TParameter parameter, ContextSession session);
    Task<TParameter> CreateAsync(TParameter parameter, ContextSession session);
    Task<bool> DeleteAsync(int id, ContextSession session);
    Task<IList<string>> GetAllModulesAsync(ContextSession session);
    Task<IList<TParameter>> GetAllByModuleAsync(string module, ContextSession session);
    Task<TParameter> GetByNomAsync(string module, string nom, ContextSession session, bool includeDeleted = false);
}