// Created by : Pierre FRAISSE
// RecyOs => RecyOs => ICompaniesRepository.cs
// Created : 2024/04/16 - 14:05
// Updated : 2024/04/16 - 14:05

using System.Threading.Tasks;
using RecyOs.HubSpotDB.Entities;

namespace RecyOs.HubSpotDB.Interfaces;

public interface ICompaniesRepository<TCompanies> where TCompanies : Companies, new()
{
    Task<TCompanies> CreateCompany(TCompanies company);
    Task<TCompanies> UpdateCompany(TCompanies company);
}