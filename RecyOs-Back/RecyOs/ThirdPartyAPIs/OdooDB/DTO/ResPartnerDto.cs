// /** ResPartnerDto.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 17/05/2023
//  * Fichier Modifié le : 16/07/2023
//  * Code développé pour le projet : RecyOs
//  */
namespace RecyOs.OdooDB.DTO;

public class ResPartnerDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Vat { get; set; }
    public string Siret { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string Street2 { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Mobile { get; set; }
    public bool IsCompany { get; set; }
    public long CountryId { get; set; }
    public string CodeMkgt { get; set; }
#nullable enable
    public int? CustomerPaymentTermId { get; set; }
    public int? SupplierPaymentTermId { get; set; }
    public string? SellAccount { get; set; }
    public string? BuyAccount { get; set; }
    public string? Origine { get; set; }
#nullable disable
}