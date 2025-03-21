// Fichier généré automatiquement par OdooJsonRpcGenerator
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;
namespace RecyOs.OdooDB.Entities;
#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes

[OdooTableName("account.account")]
[JsonConverter(typeof(OdooModelConverter))]
public class AccountAccountOdooModel : IOdooModel
{

/// <summary>
/// message_is_follower - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("message_is_follower")]
public bool? MessageIsFollower { get; set; }

/// <summary>
/// message_follower_ids - one2many - mail.followers (res_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("message_follower_ids")]
public long[] MessageFollowerIds { get; set; }

/// <summary>
/// message_partner_ids - many2many - res.partner <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("message_partner_ids")]
public long[] MessagePartnerIds { get; set; }

/// <summary>
/// message_ids - one2many - mail.message (res_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("message_ids")]
public long[] MessageIds { get; set; }

/// <summary>
/// has_message - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("has_message")]
public bool? HasMessage { get; set; }

/// <summary>
/// message_needaction - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: If checked, new messages require your attention. <br />
/// </summary>
[JsonProperty("message_needaction")]
public bool? MessageNeedaction { get; set; }

/// <summary>
/// message_needaction_counter - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Number of messages requiring action <br />
/// </summary>
[JsonProperty("message_needaction_counter")]
public int? MessageNeedactionCounter { get; set; }

/// <summary>
/// message_has_error - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: If checked, some messages have a delivery error. <br />
/// </summary>
[JsonProperty("message_has_error")]
public bool? MessageHasError { get; set; }

/// <summary>
/// message_has_error_counter - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Number of messages with delivery error <br />
/// </summary>
[JsonProperty("message_has_error_counter")]
public int? MessageHasErrorCounter { get; set; }

/// <summary>
/// message_attachment_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("message_attachment_count")]
public int? MessageAttachmentCount { get; set; }

/// <summary>
/// message_main_attachment_id - many2one - ir.attachment <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("message_main_attachment_id")]
public long? MessageMainAttachmentId { get; set; }

/// <summary>
/// website_message_ids - one2many - mail.message (res_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// Help: Website communication history <br />
/// </summary>
[JsonProperty("website_message_ids")]
public long[] WebsiteMessageIds { get; set; }

/// <summary>
/// message_has_sms_error - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: If checked, some messages have a delivery error. <br />
/// </summary>
[JsonProperty("message_has_sms_error")]
public bool? MessageHasSmsError { get; set; }

/// <summary>
/// name - char  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("name")]
public string Name { get; set; }

/// <summary>
/// currency_id - many2one - res.currency <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Forces all journal items in this account to have a specific currency (i.e. bank journals). If no currency is set, entries can use any currency. <br />
/// </summary>
[JsonProperty("currency_id")]
public long? CurrencyId { get; set; }

/// <summary>
/// code - char  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("code")]
public string Code { get; set; }

/// <summary>
/// deprecated - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("deprecated")]
public bool? Deprecated { get; set; }

/// <summary>
/// used - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("used")]
public bool? Used { get; set; }

/// <summary>
/// account_type - selection  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// Help: Account Type is used for information purpose, to generate country-specific legal reports, and set the rules to close a fiscal year and generate opening entries. <br />
/// </summary>
[JsonProperty("account_type")]
public TypeAccountAccountOdooEnum AccountType { get; set; }

/// <summary>
/// include_initial_balance - boolean  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: Used in reports to know if we should consider journal items from the beginning of time instead of from the fiscal year only. Account types that should be reset to zero at each new fiscal year (like expenses, revenue..) should not have this option set. <br />
/// </summary>
[JsonProperty("include_initial_balance")]
public bool? IncludeInitialBalance { get; set; }

/// <summary>
/// internal_group - selection  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("internal_group")]
public InternalGroupAccountAccountOdooEnum? InternalGroup { get; set; }

/// <summary>
/// reconcile - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Check this box if this account allows invoices payments matching of journal items. <br />
/// </summary>
[JsonProperty("reconcile")]
public bool? Reconcile { get; set; }

/// <summary>
/// tax_ids - many2many - account.tax <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("tax_ids")]
public long[] TaxIds { get; set; }

/// <summary>
/// note - text  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("note")]
public string Note { get; set; }

/// <summary>
/// company_id - many2one - res.company <br />
/// Required: True, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("company_id")]
public long CompanyId { get; set; }

/// <summary>
/// tag_ids - many2many - account.account.tag <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// Help: Optional tags you may want to assign for custom reporting <br />
/// </summary>
[JsonProperty("tag_ids")]
public long[] TagIds { get; set; }

/// <summary>
/// group_id - many2one - account.group <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: Account prefixes can determine account groups. <br />
/// </summary>
[JsonProperty("group_id")]
public long? GroupId { get; set; }

/// <summary>
/// root_id - many2one - account.root <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("root_id")]
public long? RootId { get; set; }

/// <summary>
/// allowed_journal_ids - many2many - account.journal <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// Help: Define in which journals this account can be used. If empty, can be used in all journals. <br />
/// </summary>
[JsonProperty("allowed_journal_ids")]
public long[] AllowedJournalIds { get; set; }

/// <summary>
/// opening_debit - monetary  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("opening_debit")]
public decimal? OpeningDebit { get; set; }

/// <summary>
/// opening_credit - monetary  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("opening_credit")]
public decimal? OpeningCredit { get; set; }

/// <summary>
/// opening_balance - monetary  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("opening_balance")]
public decimal? OpeningBalance { get; set; }

/// <summary>
/// is_off_balance - boolean  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("is_off_balance")]
public bool? IsOffBalance { get; set; }

/// <summary>
/// current_balance - float  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("current_balance")]
public double? CurrentBalance { get; set; }

/// <summary>
/// related_taxes_amount - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("related_taxes_amount")]
public int? RelatedTaxesAmount { get; set; }

/// <summary>
/// non_trade - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: If set, this account will belong to Non Trade Receivable/Payable in reports and filters.; If not, this account will belong to Trade Receivable/Payable in reports and filters. <br />
/// </summary>
[JsonProperty("non_trade")]
public bool? NonTrade { get; set; }

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
/// exclude_provision_currency_ids - many2many - res.currency <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// Help: Whether or not we have to make provisions for the selected foreign currencies. <br />
/// </summary>
[JsonProperty("exclude_provision_currency_ids")]
public long[] ExcludeProvisionCurrencyIds { get; set; }

/// <summary>
/// asset_model - many2one - account.asset <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: If this is selected, an expense/revenue will be created automatically when Journal Items on this account are posted. <br />
/// </summary>
[JsonProperty("asset_model")]
public long? AssetModel { get; set; }

/// <summary>
/// create_asset - selection  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("create_asset")]
public CreateAssetAccountAccountOdooEnum CreateAsset { get; set; }

/// <summary>
/// can_create_asset - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("can_create_asset")]
public bool? CanCreateAsset { get; set; }

/// <summary>
/// form_view_ref - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("form_view_ref")]
public string FormViewRef { get; set; }

/// <summary>
/// asset_type - selection  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("asset_type")]
public AssetTypeAccountAccountOdooEnum? AssetType { get; set; }

/// <summary>
/// multiple_assets_per_line - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Multiple asset items will be generated depending on the bill line quantity instead of 1 global asset. <br />
/// </summary>
[JsonProperty("multiple_assets_per_line")]
public bool? MultipleAssetsPerLine { get; set; }

/// <summary>
/// consolidation_account_ids - many2many - consolidation.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("consolidation_account_ids")]
public long[] ConsolidationAccountIds { get; set; }

/// <summary>
/// consolidation_account_chart_filtered_ids - many2many - consolidation.account <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("consolidation_account_chart_filtered_ids")]
public long[] ConsolidationAccountChartFilteredIds { get; set; }

/// <summary>
/// consolidation_color - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("consolidation_color")]
public int? ConsolidationColor { get; set; }
}


/// <summary>
/// Help: Account Type is used for information purpose, to generate country-specific legal reports, and set the rules to close a fiscal year and generate opening entries.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TypeAccountAccountOdooEnum
{
[EnumMember(Value = "asset_receivable")]
Receivable = 1,

[EnumMember(Value = "asset_cash")]
BankAndCash = 2,

[EnumMember(Value = "asset_current")]
CurrentAssets = 3,

[EnumMember(Value = "asset_non_current")]
NonCurrentAssets = 4,

[EnumMember(Value = "asset_prepayments")]
Prepayments = 5,

[EnumMember(Value = "asset_fixed")]
FixedAssets = 6,

[EnumMember(Value = "liability_payable")]
Payable = 7,

[EnumMember(Value = "liability_credit_card")]
CreditCard = 8,

[EnumMember(Value = "liability_current")]
CurrentLiabilities = 9,

[EnumMember(Value = "liability_non_current")]
NonCurrentLiabilities = 10,

[EnumMember(Value = "equity")]
Equity = 11,

[EnumMember(Value = "equity_unaffected")]
CurrentYearEarnings = 12,

[EnumMember(Value = "income")]
Income = 13,

[EnumMember(Value = "income_other")]
OtherIncome = 14,

[EnumMember(Value = "expense")]
Expenses = 15,

[EnumMember(Value = "expense_depreciation")]
Depreciation = 16,

[EnumMember(Value = "expense_direct_cost")]
CostOfRevenue = 17,

[EnumMember(Value = "off_balance")]
OffBalanceSheet = 18,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum InternalGroupAccountAccountOdooEnum
{
[EnumMember(Value = "equity")]
Equity = 1,

[EnumMember(Value = "asset")]
Asset = 2,

[EnumMember(Value = "liability")]
Liability = 3,

[EnumMember(Value = "income")]
Income = 4,

[EnumMember(Value = "expense")]
Expense = 5,

[EnumMember(Value = "off_balance")]
OffBalance = 6,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum CreateAssetAccountAccountOdooEnum
{
[EnumMember(Value = "no")]
No = 1,

[EnumMember(Value = "draft")]
CreateInDraft = 2,

[EnumMember(Value = "validate")]
CreateAndValidate = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum AssetTypeAccountAccountOdooEnum
{
[EnumMember(Value = "sale")]
DeferredRevenue = 1,

[EnumMember(Value = "expense")]
DeferredExpense = 2,

[EnumMember(Value = "purchase")]
Asset = 3,
}
#pragma warning restore S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
