using System;

namespace RecyOs.OdooDB.DTO;

public class ResCountryDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string AddressFormat { get; set; }
    public long? AddressViewId { get; set; }
    public long? CurrencyId { get; set; }
    public string ImageUrl { get; set; }
    public int? PhoneCode { get; set; }
    public long[] CountryGroupIds { get; set; }
    public long[] StateIds { get; set; }
    public string NamePosition { get; set; }
    public string VatLabel { get; set; }
    public bool? StateRequired { get; set; }
    public bool? ZipRequired { get; set; }
    public DateTime? LastUpdate { get; set; }
    public string DisplayName { get; set; }
    public long? CreateUid { get; set; }
    public DateTime? CreateDate { get; set; }
    public long? WriteUid { get; set; }
    public DateTime? WriteDate { get; set; }
    public bool? IsStripeSupportedCountry { get; set; }
    public bool? Intrastat { get; set; }
}