// Created by : Pierre FRAISSE
// RecyOs => RecyOs => ICompaniesService.cs
// Created : 2024/04/16 - 14:07
// Updated : 2024/04/16 - 14:07

using System.Threading.Tasks;
using RecyOs.HubSpotDB.DTO;
using RecyOs.HubSpotDB.Entities;

namespace RecyOs.HubSpotDB.Interfaces;

public interface ICompaniesService
{
    public Task<CompaniesDto> CreateCompany(CompaniesDto company);
    public Task<CompaniesDto> UpdateCompany(CompaniesDto company);
}