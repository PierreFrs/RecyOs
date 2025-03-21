// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IService.cs
// Created : 2024/02/23 - 15:39
// Updated : 2024/02/23 - 15:39

using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecyOs.ORM.Interfaces;

public interface IBaseService<TDto>
{
    Task<TDto> CreateAsync(TDto createDto);
    
    Task<IReadOnlyList<TDto>> GetListAsync();
    
    Task<TDto> GetByIdAsync(int id);
    
    Task<TDto> UpdateAsync(int id, TDto dtoUpdate);
    
    Task<bool> DeleteAsync(int id);
}