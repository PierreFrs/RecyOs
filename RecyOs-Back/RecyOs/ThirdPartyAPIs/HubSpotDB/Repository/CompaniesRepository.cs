// Created by : Pierre FRAISSE
// RecyOs => RecyOs => CompaniesRepository.cs
// Created : 2024/04/16 - 14:11
// Updated : 2024/04/16 - 14:11

using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NLog;
using RecyOs.Engine.Interfaces;
using RecyOs.HubSpotDB.Entities;
using RecyOs.HubSpotDB.Interfaces;
using RecyOs.ORM.Interfaces;

namespace RecyOs.HubSpotDB.Repository;

public class CompaniesRepository: BaseHubSpotRepository, ICompaniesRepository<Companies>
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private readonly HttpClient _httpClient;
    private readonly IEmailDomainService _emailDomainService;
    private readonly IDateFormater _dateFormater;
    private readonly IEngineEtablissementClientRepository _engineEtablissementClientRepository;
    public CompaniesRepository(
        IConfiguration configuration, 
        HttpClient httpClient,
        IEmailDomainService emailDomainService,
        IDateFormater dateFormater,
        IEngineEtablissementClientRepository engineEtablissementClientRepository
        ) : base(configuration)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_endPoint);
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        _emailDomainService = emailDomainService;
        _dateFormater = dateFormater;
        _engineEtablissementClientRepository = engineEtablissementClientRepository;
    }
    
    public async Task<Companies> CreateCompany(Companies company)
    {
        if (company == null) return null;

        var companyData = new
        {
            properties = new
            {
                name = company.Name,
                type = company.Type,
                phone = company.Phone,
                address = company.Address,
                address2 = company.Address2,
                city = company.City,
                zip = company.Zip,
                founded_year = _dateFormater.FormatToYear(company.FoundedYear),
                country = company.Country,
                description = company.Description,
                domain = _emailDomainService.GetEmailDomain(company.Domain),
                date_creation_fiche_recyos = DateTime.Parse(company.DateCreationFicheRecyOs).ToString("dd/MM/yyyy"),
                code_mkgt = company.CodeMkgt,
                couverture_client = await _engineEtablissementClientRepository.GetMontantGarantieForEntreprise(company.Siren)
            }
        };

        var jsonContent = JsonSerializer.Serialize(companyData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(_endPoint, content);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseBody))
            {
                Logger.Error("Received empty response body despite successful status code.");
                return null;
            }
            
            return DeserializeHubSpotResponse(responseBody);
        }
        else
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            Logger.Error($"Failed to update company. URL: {_endPoint}/{company.Id}, Status Code: {response.StatusCode}, Response: {errorResponse}");
            return null;
        }
    }
    
    public async Task<Companies> UpdateCompany(Companies company)
    {
        if (company == null) return null;
        
        var companyData = new
        {
            properties = new 
            {
                name = company.Name,
                type = company.Type,
                phone = company.Phone,
                address = company.Address,
                city = company.City,
                zip = company.Zip,
                hubspot_owner_id = company.HubSpotOwnerId,
                address2 = company.Address2,
                country = company.Country,
                founded_year = _dateFormater.FormatToYear(company.FoundedYear),
                description = company.Description,
                domain = _emailDomainService.GetEmailDomain(company.Domain),
                date_creation_fiche_recyos = DateTime.Parse(company.DateCreationFicheRecyOs).ToString("dd/MM/yyyy"),
                code_mkgt = company.CodeMkgt,
                couverture_client = await _engineEtablissementClientRepository.GetMontantGarantieForEntreprise(company.Siren)
            }
        };

        var jsonContent = JsonSerializer.Serialize(companyData);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _httpClient.PatchAsync($"{_endPoint}/{company.Id}", content);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseBody))
            {
                Logger.Error("Received empty response body despite successful status code.");
                return null;
            }
        
            return DeserializeHubSpotResponse(responseBody);
        }
        else
        {
            Logger.Error($"Failed to update company. Status Code: {response.StatusCode}");
            return null;
        }
    }
    
    private Companies DeserializeHubSpotResponse(string json)
    {
        using (JsonDocument doc = JsonDocument.Parse(json))
        {
            var root = doc.RootElement;
            var properties = root.GetProperty("properties");

            return new Companies
            {
                Id = root.GetProperty("id").GetString(),
                Name = properties.TryGetProperty("name", out var name) ? name.GetString() : null,
                Type = properties.TryGetProperty("type", out var type) ? type.GetString() : null,
                Phone = properties.TryGetProperty("phone", out var phone) ? phone.GetString() : null,
                Address = properties.TryGetProperty("address", out var address) ? address.GetString() : null,
                Address2 = properties.TryGetProperty("address2", out var address2) ? address2.GetString() : null,
                City = properties.TryGetProperty("city", out var city) ? city.GetString() : null,
                Zip = properties.TryGetProperty("zip", out var zip) ? zip.GetString() : null,
                HubSpotOwnerId = properties.TryGetProperty("hubspot_owner_id", out var hubSpotOwnerId) ? hubSpotOwnerId.GetString() : null,
                FoundedYear = properties.TryGetProperty("founded_year", out var foundedYear) ? foundedYear.GetString() : null,
                Country = properties.TryGetProperty("country", out var country) ? country.GetString() : null,
                Description = properties.TryGetProperty("description", out var description) ? description.GetString() : null,
                Domain = properties.TryGetProperty("domain", out var domain) ? domain.GetString() : null,
                DateCreationFicheRecyOs = properties.TryGetProperty("date_creation_fiche_recyos", out var dateCreationFicheRecyOs) ? dateCreationFicheRecyOs.GetString() : null,
                CodeMkgt = properties.TryGetProperty("code_mkgt", out var codeMkgt) ? codeMkgt.GetString() : null,
                CouvertureClient = properties.TryGetProperty("couverture_client", out var couvertureClient) ? couvertureClient.GetString() : null
            };
        }
    }
}