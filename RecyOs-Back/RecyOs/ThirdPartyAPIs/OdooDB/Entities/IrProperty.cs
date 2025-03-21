// Fichier généré automatiquement par OdooJsonRpcGenerator
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;
namespace RecyOs.OdooDB.Entities;

[OdooTableName("ir.property")]
[JsonConverter(typeof(OdooModelConverter))]
public class IrPropertyOdooModel : IOdooModel
{

/// <summary>
/// name - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("name")]
public string Name { get; set; }

/// <summary>
/// res_id - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: If not set, acts as a default value for new resources <br />
/// </summary>
[JsonProperty("res_id")]
public string ResId { get; set; }

/// <summary>
/// company_id - many2one - res.company <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("company_id")]
public long? CompanyId { get; set; }

/// <summary>
/// fields_id - many2one - ir.model.fields <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("fields_id")]
public long FieldsId { get; set; }

/// <summary>
/// value_float - float  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("value_float")]
public double? ValueFloat { get; set; }

/// <summary>
/// value_integer - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("value_integer")]
public int? ValueInteger { get; set; }

/// <summary>
/// value_text - text  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("value_text")]
public string ValueText { get; set; }

/// <summary>
/// value_binary - binary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("value_binary")]
public string ValueBinary { get; set; }

/// <summary>
/// value_reference - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("value_reference")]
public string ValueReference { get; set; }

/// <summary>
/// value_datetime - datetime  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("value_datetime")]
public DateTime? ValueDatetime { get; set; }

/// <summary>
/// type - selection  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("type")]
public TypeIrPropertyOdoo Type { get; set; }

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
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TypeIrPropertyOdoo
{
[EnumMember(Value = "char")]
Char = 1,

[EnumMember(Value = "float")]
Float = 2,

[EnumMember(Value = "boolean")]
Boolean = 3,

[EnumMember(Value = "integer")]
Integer = 4,

[EnumMember(Value = "text")]
Text = 5,

[EnumMember(Value = "binary")]
Binary = 6,

[EnumMember(Value = "many2one")]
Many2one = 7,

[EnumMember(Value = "date")]
Date = 8,

[EnumMember(Value = "datetime")]
Datetime = 9,

[EnumMember(Value = "selection")]
Selection = 10,
}
