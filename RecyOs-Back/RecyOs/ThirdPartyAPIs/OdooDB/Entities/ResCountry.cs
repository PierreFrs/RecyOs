// Fichier généré automatiquement par OdooJsonRpcGenerator
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace RecyOs.OdooDB.Entities;

[OdooTableName("res.country")]
[JsonConverter(typeof(OdooModelConverter))]
public class ResCountryOdooModel : IOdooModel
{

/// <summary>
/// name - char  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("name")]
public string Name { get; set; }

/// <summary>
/// code - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: The ISO country code in two chars. ; You can use this field for quick search. <br />
/// </summary>
[JsonProperty("code")]
public string Code { get; set; }

/// <summary>
/// address_format - text  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Display format to use for addresses belonging to this country.; ; You can use python-style string pattern with all the fields of the address (for example, use '%(street)s' to display the field 'street') plus; %(state_name)s: the name of the state; %(state_code)s: the code of the state; %(country_name)s: the name of the country; %(country_code)s: the code of the country <br />
/// </summary>
[JsonProperty("address_format")]
public string AddressFormat { get; set; }

/// <summary>
/// address_view_id - many2one - ir.ui.view <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Use this field if you want to replace the usual way to encode a complete address. Note that the address_format field is used to modify the way to display addresses (in reports for example), while this field is used to modify the input form for addresses. <br />
/// </summary>
[JsonProperty("address_view_id")]
public long? AddressViewId { get; set; }

/// <summary>
/// currency_id - many2one - res.currency <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("currency_id")]
public long? CurrencyId { get; set; }

/// <summary>
/// image_url - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Url of static flag image <br />
/// </summary>
[JsonProperty("image_url")]
public string ImageUrl { get; set; }

/// <summary>
/// phone_code - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("phone_code")]
public int? PhoneCode { get; set; }

/// <summary>
/// country_group_ids - many2many - res.country.group <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("country_group_ids")]
public long[] CountryGroupIds { get; set; }

/// <summary>
/// state_ids - one2many - res.country.state (country_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("state_ids")]
public long[] StateIds { get; set; }

/// <summary>
/// name_position - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Determines where the customer/company name should be placed, i.e. after or before the address. <br />
/// </summary>
[JsonProperty("name_position")]
public CustomerNamePositionResCountryOdooEnum? NamePosition { get; set; }

/// <summary>
/// vat_label - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Use this field if you want to change vat label. <br />
/// </summary>
[JsonProperty("vat_label")]
public string VatLabel { get; set; }

/// <summary>
/// state_required - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("state_required")]
public bool? StateRequired { get; set; }

/// <summary>
/// zip_required - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("zip_required")]
public bool? ZipRequired { get; set; }

/// <summary>
/// id - integer  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("id")]
public long Id { get; set; }

/// <summary>
/// __last_update - datetime  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("__last_update")]
public DateTime? LastUpdate { get; set; }

/// <summary>
/// display_name - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("display_name")]
public string DisplayName { get; set; }

/// <summary>
/// create_uid - many2one - res.users <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("create_uid")]
public long? CreateUid { get; set; }

/// <summary>
/// create_date - datetime  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("create_date")]
public DateTime? CreateDate { get; set; }

/// <summary>
/// write_uid - many2one - res.users <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("write_uid")]
public long? WriteUid { get; set; }

/// <summary>
/// write_date - datetime  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("write_date")]
public DateTime? WriteDate { get; set; }

/// <summary>
/// is_stripe_supported_country - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("is_stripe_supported_country")]
public bool? IsStripeSupportedCountry { get; set; }

/// <summary>
/// intrastat - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("intrastat")]
public bool? Intrastat { get; set; }
}


#pragma warning disable S2344
/// <summary>
/// Help: Determines where the customer/company name should be placed, i.e. after or before the address.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum CustomerNamePositionResCountryOdooEnum
{
[EnumMember(Value = "before")]
BeforeAddress = 1,

[EnumMember(Value = "after")]
AfterAddress = 2,
}
#pragma warning restore S2344
