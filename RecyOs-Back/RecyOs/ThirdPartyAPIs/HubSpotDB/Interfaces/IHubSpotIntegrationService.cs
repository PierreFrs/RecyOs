// Created by : Pierre FRAISSE
// RecyOs => RecyOs => IHubSpotIntegrationService.cs
// Created : 2024/04/19 - 14:16
// Updated : 2024/04/19 - 14:16

using System.Threading.Tasks;
using RecyOs.HubSpotDB.DTO;
using RecyOs.HubSpotDB.Entities;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.HubSpotDB.Interfaces;

public interface IHubSpotIntegrationService
{
    public Task<CompaniesDto> ConvertEtablissementClientToCompaniesDto(EtablissementClientDto etablissementClientDto);
    public Task<CompaniesDto> ConvertClientEuropeToCompaniesDto(ClientEuropeDto clientEuropeDto);
}