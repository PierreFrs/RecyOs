// Copyright : Pierre FRAISSE
// RecyOs>RecyOs>IFactorFileExportRepository.cs
// Created : 2024/05/2121 - 10:05

using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.ORM.Interfaces;

public interface IFactorFileExportRepository
{
    Task<IEnumerable<FactorClientDto>> ExportFactorFileRepositoryAsync(int buId);
    
    Task UpdateExportDateForFranceClientsAsync(IEnumerable<int> factorClientIds, int buId);
        
    Task UpdateExportDateForEuropeClientsAsync(IEnumerable<int> factorClientIds, int buId);
}