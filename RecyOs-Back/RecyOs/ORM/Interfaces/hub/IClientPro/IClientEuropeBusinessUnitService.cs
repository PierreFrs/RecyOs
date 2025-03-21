using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.ORM.Interfaces;

public interface IClientEuropeBusinessUnitService<TClientEuropeBusinessUnitDto, TBusinessUnitDto>
{
    Task<TClientEuropeBusinessUnitDto> CreateAsync(ClientEuropeBusinessUnitDto clientEuropeBusinessUnitDto);
    Task<IList<TBusinessUnitDto>> GetByClientEuropeIdAsync(int clientEuropeId, bool includeDeleted = false);
    Task<bool> DeleteAsync(ClientEuropeBusinessUnitDto clientEuropeBusinessUnitDto);
}