// DashdocCompanyDto.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 16/09/2024
// Fichier Modifié le : 16/09/2024
// Code développé pour le projet : RecyOs

using System;
using RecyOs.HubSpotDB.Entities;
using RecyOs.ThirdPartyAPIs.DashdocDB.Entities;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.DTO;

public class DashdocCompanyDto
{
    public int? PK { get; set; }
    
    public string Name { get; set; } = String.Empty;
    
    public string Email { get; set; } = String.Empty;
    
    public string PhoneNumber { get; set; } = String.Empty;
    
    public string Siren { get; set; } = String.Empty;
    
    public string TradeNumber { get; set; } = String.Empty;
    
    public string VatNumber { get; set; } = String.Empty;
    
    public string Country { get; set; } = String.Empty;
    
    public string RemoteId { get; set; } = String.Empty;
    
    public string AccountCode { get; set; } = String.Empty;
    
    public string SideAccountCode { get; set; } = String.Empty;
    
    public string InvoicingRemoteId { get; set; } = String.Empty;
    
    public string Notes { get; set; } = String.Empty;
    
    public DashdocPrimaryAddress DashdocPrimaryAddress { get; set; } = new DashdocPrimaryAddress();
}