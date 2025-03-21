using System;
using System.Text.Json.Serialization;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.Entities;

public class DashdocCompany
{
    [JsonPropertyName("pk")]
    public int? PK { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = String.Empty;

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; } = String.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = String.Empty;

    [JsonPropertyName("siren")]
    public string Siren { get; set; } = String.Empty;

    [JsonPropertyName("trade_number")]
    public string TradeNumber { get; set; } = String.Empty;

    [JsonPropertyName("vat_number")]
    public string VatNumber { get; set; } = String.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = String.Empty;

    [JsonPropertyName("remote_id")]
    public string RemoteId { get; set; } = String.Empty;
    
    [JsonPropertyName("account_code")]
    public string AccountCode { get; set; } = String.Empty;
    
    [JsonPropertyName("side_account_code")]
    public string SideAccountCode { get; set; } = String.Empty;

    [JsonPropertyName("invoicing_remote_id")]
    public string InvoicingRemoteId { get; set; } = String.Empty;

    [JsonPropertyName("notes")]
    public string Notes { get; set; } = String.Empty;

    [JsonPropertyName("primary_address")]
    public DashdocPrimaryAddress DashdocPrimaryAddress { get; set; } = new DashdocPrimaryAddress();
}