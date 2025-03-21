// DashdocPrimaryAddressDto.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 16/09/2024
// Fichier Modifié le : 16/09/2024
// Code développé pour le projet : RecyOs

using System;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.DTO;

public class DashdocPrimaryAddressDto
{
    public int? PK { get; set; }
    
    public string Name { get; set; } = String.Empty;
    
    public string Address { get; set; } = String.Empty;
    
    public string City { get; set; } = String.Empty;
    
    public string PostCode { get; set; } = String.Empty;
    
    public string Country { get; set; } = String.Empty;
    
    public bool IsShipper { get; set; } = true;
    
    public bool IsCarrier { get; set; } = false;
    
    public bool IsOrigin { get; set; } = false;
    
    public bool IsDestination { get; set; } = false;
    
    public int CreatedBy { get; set; } = 0;
    
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public string Instructions { get; set; } = String.Empty;
    
    public string RemoteId { get; set; } = String.Empty;
}