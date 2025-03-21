using System;
using System.Text.Json.Serialization;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.Entities;

public class DashdocPrimaryAddress
{
    [JsonPropertyName("pk")]
    public int? PK { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = String.Empty;

    [JsonPropertyName("address")]
    public string Address { get; set; } = String.Empty;

    [JsonPropertyName("city")]
    public string City { get; set; } = String.Empty;

    [JsonPropertyName("postcode")]
    public string PostCode { get; set; } = String.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = String.Empty;

    [JsonPropertyName("is_shipper")]
    public bool IsShipper { get; set; } = true;
    
    [JsonPropertyName("is_carrier")]
    public bool IsCarrier { get; set; } = false;
    
    [JsonPropertyName("is_origin")]
    public bool IsOrigin { get; set; } = false;
    
    [JsonPropertyName("is_destination")]
    public bool IsDestination { get; set; } = false;

    [JsonPropertyName("created_by")]
    public int CreatedBy { get; set; } = 0;

    [JsonPropertyName("created")]
    public DateTime Created { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("instructions")]
    public string Instructions { get; set; } = String.Empty;

    [JsonPropertyName("remote_id")]
    public string RemoteId { get; set; } = String.Empty;
}