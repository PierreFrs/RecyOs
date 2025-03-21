// Created by : Pierre FRAISSE
// RecyOs => RecyOs => ICommercialService.cs
// Created : 2024/03/26 - 15:05
// Updated : 2024/03/26 - 15:05

using System.Threading.Tasks;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;

namespace RecyOs.ORM.Interfaces;

public interface ICommercialBaseService : IBaseService<CommercialDto>
{
    Task<GridData<CommercialDto>> GetFilteredListAsync(CommercialFilter filter);
    Task<GridData<object>> GetClientsByCommercialIdAsync(int commercialId, ClientByCommercialFilter filter);
    
}