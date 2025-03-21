using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IEntrepriseNDCoverService
{
    Task<GridData<EntrepriseNDCoverDto>> GetDataForGrid(EntrepriseNDCoverGridFilter filter);
    Task<EntrepriseNDCoverDto> GetById(int id);
    Task<EntrepriseNDCoverDto> GetBySiren(string siren);
    Task<EntrepriseNDCoverDto> Edit(EntrepriseNDCoverDto dto);
    Task<EntrepriseNDCoverDto> Create(EntrepriseNDCoverDto dto);
}