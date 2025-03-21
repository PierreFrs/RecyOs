// DashdocRepository.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 16/09/2024
// Fichier Modifié le : 16/09/2024
// Code développé pour le projet : RecyOs

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NLog;
using RecyOs.HubSpotDB.Entities;
using RecyOs.ORM.Entities;
using RecyOs.ThirdPartyAPIs.DashdocDB.Entities;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.Repository;

public class DashdocRepository : IDashdocRepository
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDashdocSettings _dashdocSettings;
    public DashdocRepository(
        IConfiguration configuration,
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor,
        IDashdocSettings dashdocSettings
        )
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(dashdocSettings.GetUri());
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", dashdocSettings.GetToken());
        _httpContextAccessor = httpContextAccessor;
        _dashdocSettings = dashdocSettings;
    }
    
    public async Task<DashdocCompany> CreateDashdocCompanyAsync(DashdocCompany dashdocCompany)
    {
        try
        {
            var dashdocCompanyData = new
            {
                name = dashdocCompany.Name,
                phone_number = dashdocCompany.PhoneNumber ?? "",
                email = dashdocCompany.Email,
                siren = dashdocCompany.Siren,
                trade_number = dashdocCompany.TradeNumber,
                vat_number = dashdocCompany.VatNumber,
                country = dashdocCompany.Country,
                remote_id = dashdocCompany.RemoteId,
                account_code = dashdocCompany.AccountCode,
                side_account_code = dashdocCompany.SideAccountCode,
                invoicing_remote_id = dashdocCompany.InvoicingRemoteId,
                notes = dashdocCompany.Notes,
                primary_address = new
                {
                    name = dashdocCompany.DashdocPrimaryAddress.Name,
                    address = dashdocCompany.DashdocPrimaryAddress.Address,
                    city = dashdocCompany.DashdocPrimaryAddress.City,
                    postcode = dashdocCompany.DashdocPrimaryAddress.PostCode,
                    country = dashdocCompany.DashdocPrimaryAddress.Country,
                    is_shipper = dashdocCompany.DashdocPrimaryAddress.IsShipper,
                    is_carrier = dashdocCompany.DashdocPrimaryAddress.IsCarrier,
                    is_origin = dashdocCompany.DashdocPrimaryAddress.IsOrigin,
                    is_destination = dashdocCompany.DashdocPrimaryAddress.IsDestination,
                    created_by = dashdocCompany.DashdocPrimaryAddress.CreatedBy,
                    created = dashdocCompany.DashdocPrimaryAddress.Created,
                    instructions = dashdocCompany.DashdocPrimaryAddress.Instructions,
                    remote_id = dashdocCompany.DashdocPrimaryAddress.RemoteId
                }
            };
            
            // Serialize the data to JSON
            var jsonContent = JsonSerializer.Serialize(dashdocCompanyData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Set the Authorization header correctly
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_dashdocSettings.GetToken()}");

            // Make the POST request to the correct endpoint
            var response = await _httpClient.PostAsync("companies/", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseBody))
                {
                    _logger.Error("Error while creating Dashdoc company");
                    return null;
                }

                return DeserializeDashdocResponse(responseBody);
            }
            var errorResponse = await response.Content.ReadAsStringAsync();
            _logger.Error($"Failed to update company. URL: {_dashdocSettings.GetUri()}, Status Code: {response.StatusCode}, Response: {errorResponse}");
            return null;
        }
        catch (NullReferenceException e)
        {
            _logger.Error(e);
            throw;
        }
        catch (Exception e)
        {
            _logger.Error(e);
            throw;
        }
    }

    public async Task<DashdocCompany> UpdateDashdocCompanyAsync(DashdocCompany dashdocCompany)
    {
        try
        {
            var dashdocCompanyData = new
            {
                name = dashdocCompany.Name,
                phone_number = dashdocCompany.PhoneNumber ?? "",
                email = dashdocCompany.Email,
                siren = dashdocCompany.Siren,
                trade_number = dashdocCompany.TradeNumber,
                vat_number = dashdocCompany.VatNumber,
                country = dashdocCompany.Country,
                remote_id = dashdocCompany.RemoteId,
                account_code = dashdocCompany.AccountCode,
                side_account_code = dashdocCompany.SideAccountCode,
                invoicing_remote_id = dashdocCompany.InvoicingRemoteId,
                notes = dashdocCompany.Notes,
                primary_address = new
                {
                    name = dashdocCompany.DashdocPrimaryAddress.Name,
                    address = dashdocCompany.DashdocPrimaryAddress.Address,
                    city = dashdocCompany.DashdocPrimaryAddress.City,
                    postcode = dashdocCompany.DashdocPrimaryAddress.PostCode,
                    country = dashdocCompany.DashdocPrimaryAddress.Country,
                    is_shipper = dashdocCompany.DashdocPrimaryAddress.IsShipper,
                    is_carrier = dashdocCompany.DashdocPrimaryAddress.IsCarrier,
                    is_origin = dashdocCompany.DashdocPrimaryAddress.IsOrigin,
                    is_destination = dashdocCompany.DashdocPrimaryAddress.IsDestination,
                    created_by = dashdocCompany.DashdocPrimaryAddress.CreatedBy,
                    created = dashdocCompany.DashdocPrimaryAddress.Created,
                    instructions = dashdocCompany.DashdocPrimaryAddress.Instructions,
                    remote_id = dashdocCompany.DashdocPrimaryAddress.RemoteId
                }
            };
            
            // Serialize the data to JSON
            var jsonContent = JsonSerializer.Serialize(dashdocCompanyData);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            
            // Set the Authorization header correctly
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {_dashdocSettings.GetToken()}");
            
            // Make the PATCH request to the correct endpoint
            var companyId = dashdocCompany.PK;
            var requestUrl = $"companies/{companyId}/";

            var response = await _httpClient.PatchAsync($"{requestUrl}", content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(responseBody))
                {
                    _logger.Error("Error while creating Dashdoc company");
                    return null;
                }

                return DeserializeDashdocResponse(responseBody);
            }
            var errorResponse = await response.Content.ReadAsStringAsync();
            _logger.Error($"Failed to update company. URL: {_dashdocSettings.GetUri()}{requestUrl}, Status Code: {response.StatusCode}, Response: {errorResponse}");
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Gets the current user name.
    /// </summary>
    /// <returns>The current user name.</returns>
    public string GetCurrentUserName()
    {
        return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
    }
    
    private DashdocCompany DeserializeDashdocResponse(string json)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<DashdocCompany>(json, options);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to deserialize Dashdoc company response.");
            return null;
        }
    }


}