using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;

namespace RecyOs.ORM.Interfaces.IParameters;

public interface IParameterService
{
    Task<GridData<ParameterDto>> GetDataForGrid(ParameterFilter filter, bool includeDeleted = false);
    Task<ParameterDto> GetById(int id, bool includeDeleted = false);
    Task<ParameterDto> GetByNom(string module, string nom, bool includeDeleted = false);
    Task<ParameterDto> CreateAsync(ParameterDto dto);
    Task<ParameterDto> UpdateAsync(ParameterDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IList<string>> GetAllModulesAsync();
    Task<IList<ParameterDto>> GetAllByModuleAsync(string module);
    Task<ParameterDto> GetByNomAsync(string module, string nom, bool includeDeleted = false);
}