using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

// Generated code
#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
#pragma warning disable S3903 // Types should be defined in named namespaces
[OdooTableName("account.move.line")]
[JsonConverter(typeof(OdooModelConverter))]
public class AccountMoveLineOdooModel : IOdooModel
{

/// <summary>
/// analytic_distribution - json  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("analytic_distribution")]
public string AnalyticDistribution { get; set; }

/// <summary>
/// analytic_distribution_search - json  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("analytic_distribution_search")]
public string AnalyticDistributionSearch { get; set; }

/// <summary>
/// analytic_precision - integer  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("analytic_precision")]
public int? AnalyticPrecision { get; set; }

/// <summary>
/// move_id - many2one - account.move <br />
/// Required: True, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("move_id")]
public long MoveId { get; set; }

/// <summary>
/// journal_id - many2one - account.journal <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("journal_id")]
public long? JournalId { get; set; }

/// <summary>
/// company_id - many2one - res.company <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("company_id")]
public long? CompanyId { get; set; }

/// <summary>
/// company_currency_id - many2one - res.currency <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("company_currency_id")]
public long? CompanyCurrencyId { get; set; }

/// <summary>
/// move_name - char  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("move_name")]
public string MoveName { get; set; }

/// <summary>
/// parent_state - selection  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("parent_state")]
public StatusAccountMoveLineOdooEnum? ParentState { get; set; }

/// <summary>
/// date - date  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("date")]
public DateTime? Date { get; set; }

/// <summary>
/// ref - char  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("ref")]
public string Ref { get; set; }

/// <summary>
/// is_storno - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Utility field to express whether the journal item is subject to storno accounting <br />
/// </summary>
[JsonProperty("is_storno")]
public bool? IsStorno { get; set; }

/// <summary>
/// sequence - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("sequence")]
public int? Sequence { get; set; }

/// <summary>
/// move_type - selection  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("move_type")]
public TypeAccountMoveLineOdooEnum? MoveType { get; set; }

/// <summary>
/// Gets or sets the account ID
/// </summary>
[JsonProperty("account_id")]
public long AccountId { get; set; }

/// <summary>
/// name - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("name")]
public string Name { get; set; }

/// <summary>
/// debit - monetary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("debit")]
public decimal? Debit { get; set; }

/// <summary>
/// credit - monetary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("credit")]
public decimal? Credit { get; set; }

/// <summary>
/// balance - monetary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("balance")]
public decimal? Balance { get; set; }

/// <summary>
/// cumulated_balance - monetary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Cumulated balance depending on the domain and the order chosen in the view. <br />
/// </summary>
[JsonProperty("cumulated_balance")]
public decimal? CumulatedBalance { get; set; }

/// <summary>
/// currency_rate - float  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Currency rate from company currency to document currency. <br />
/// </summary>
[JsonProperty("currency_rate")]
public double? CurrencyRate { get; set; }

/// <summary>
/// amount_currency - monetary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: The amount expressed in an optional other currency if it is a multi-currency entry. <br />
/// </summary>
[JsonProperty("amount_currency")]
public decimal? AmountCurrency { get; set; }

/// <summary>
/// currency_id - many2one - res.currency <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("currency_id")]
public long CurrencyId { get; set; }

/// <summary>
/// is_same_currency - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("is_same_currency")]
public bool? IsSameCurrency { get; set; }

/// <summary>
/// partner_id - many2one - res.partner <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("partner_id")]
public long? PartnerId { get; set; }

/// <summary>
/// reconcile_model_id - many2one - account.reconcile.model <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("reconcile_model_id")]
public long? ReconcileModelId { get; set; }

/// <summary>
/// payment_id - many2one - account.payment <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: The payment that created this entry <br />
/// </summary>
[JsonProperty("payment_id")]
public long? PaymentId { get; set; }

/// <summary>
/// statement_line_id - many2one - account.bank.statement.line <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: The statement line that created this entry <br />
/// </summary>
[JsonProperty("statement_line_id")]
public long? StatementLineId { get; set; }

/// <summary>
/// statement_id - many2one - account.bank.statement <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: The bank statement used for bank reconciliation <br />
/// </summary>
[JsonProperty("statement_id")]
public long? StatementId { get; set; }

/// <summary>
/// tax_ids - many2many - account.tax <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("tax_ids")]
public long[] TaxIds { get; set; }

/// <summary>
/// group_tax_id - many2one - account.tax <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("group_tax_id")]
public long? GroupTaxId { get; set; }

/// <summary>
/// tax_line_id - many2one - account.tax <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: Indicates that this journal item is a tax line <br />
/// </summary>
[JsonProperty("tax_line_id")]
public long? TaxLineId { get; set; }

/// <summary>
/// tax_group_id - many2one - account.tax.group <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("tax_group_id")]
public long? TaxGroupId { get; set; }

/// <summary>
/// tax_base_amount - monetary  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("tax_base_amount")]
public decimal? TaxBaseAmount { get; set; }

/// <summary>
/// tax_repartition_line_id - many2one - account.tax.repartition.line <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: Tax distribution line that caused the creation of this move line, if any <br />
/// </summary>
[JsonProperty("tax_repartition_line_id")]
public long? TaxRepartitionLineId { get; set; }

/// <summary>
/// tax_tag_ids - many2many - account.account.tag <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// Help: Tags assigned to this line by the tax creating it, if any. It determines its impact on financial reports. <br />
/// </summary>
[JsonProperty("tax_tag_ids")]
public long[] TaxTagIds { get; set; }

/// <summary>
/// tax_audit - char  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: Computed field, listing the tax grids impacted by this line, and the amount it applies to each of them. <br />
/// </summary>
[JsonProperty("tax_audit")]
public string TaxAudit { get; set; }

/// <summary>
/// tax_tag_invert - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("tax_tag_invert")]
public bool? TaxTagInvert { get; set; }

/// <summary>
/// amount_residual - monetary  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: The residual amount on a journal item expressed in the company currency. <br />
/// </summary>
[JsonProperty("amount_residual")]
public decimal? AmountResidual { get; set; }

/// <summary>
/// amount_residual_currency - monetary  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: The residual amount on a journal item expressed in its currency (possibly not the company currency). <br />
/// </summary>
[JsonProperty("amount_residual_currency")]
public decimal? AmountResidualCurrency { get; set; }

/// <summary>
/// reconciled - boolean  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("reconciled")]
public bool? Reconciled { get; set; }

/// <summary>
/// full_reconcile_id - many2one - account.full.reconcile <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("full_reconcile_id")]
public long? FullReconcileId { get; set; }

/// <summary>
/// matched_debit_ids - one2many - account.partial.reconcile (credit_move_id) <br />
/// Required: False, Readonly: True, Store: True, Sortable: False <br />
/// Help: Debit journal items that are matched with this journal item. <br />
/// </summary>
[JsonProperty("matched_debit_ids")]
public long[] MatchedDebitIds { get; set; }

/// <summary>
/// matched_credit_ids - one2many - account.partial.reconcile (debit_move_id) <br />
/// Required: False, Readonly: True, Store: True, Sortable: False <br />
/// Help: Credit journal items that are matched with this journal item. <br />
/// </summary>
[JsonProperty("matched_credit_ids")]
public long[] MatchedCreditIds { get; set; }

/// <summary>
/// matching_number - char  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: Matching number for this line, 'P' if it is only partially reconcile, or the name of the full reconcile if it exists. <br />
/// </summary>
[JsonProperty("matching_number")]
public string MatchingNumber { get; set; }

/// <summary>
/// is_account_reconcile - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Check this box if this account allows invoices and payments matching of journal items. <br />
/// </summary>
[JsonProperty("is_account_reconcile")]
public bool? IsAccountReconcile { get; set; }

/// <summary>
/// account_type - selection  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Account Type is used for information purpose, to generate country-specific legal reports, and set the rules to close a fiscal year and generate opening entries. <br />
/// </summary>
[JsonProperty("account_type")]
public InternalTypeAccountMoveLineOdooEnum? AccountType { get; set; }

/// <summary>
/// account_internal_group - selection  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("account_internal_group")]
public InternalGroupAccountMoveLineOdooEnum? AccountInternalGroup { get; set; }

/// <summary>
/// account_root_id - many2one - account.root <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_root_id")]
public long? AccountRootId { get; set; }

/// <summary>
/// display_type - selection  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("display_type")]
public DisplayTypeAccountMoveLineOdooEnum DisplayType { get; set; }

/// <summary>
/// product_id - many2one - product.product <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("product_id")]
public long? ProductId { get; set; }

/// <summary>
/// product_uom_id - many2one - uom.uom <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("product_uom_id")]
public long? ProductUomId { get; set; }

/// <summary>
/// product_uom_category_id - many2one - uom.category <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Conversion between Units of Measure can only occur if they belong to the same category. The conversion will be made based on the ratios. <br />
/// </summary>
[JsonProperty("product_uom_category_id")]
public long? ProductUomCategoryId { get; set; }

/// <summary>
/// quantity - float  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: The optional quantity expressed by this line, eg: number of product sold. The quantity is not a legal requirement but is very useful for some reports. <br />
/// </summary>
[JsonProperty("quantity")]
public double? Quantity { get; set; }

/// <summary>
/// date_maturity - date  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: This field is used for payable and receivable journal entries. You can put the limit date for the payment of this line. <br />
/// </summary>
[JsonProperty("date_maturity")]
public DateTime? DateMaturity { get; set; }

/// <summary>
/// price_unit - float  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("price_unit")]
public double? PriceUnit { get; set; }

/// <summary>
/// price_subtotal - monetary  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("price_subtotal")]
public decimal? PriceSubtotal { get; set; }

/// <summary>
/// price_total - monetary  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("price_total")]
public decimal? PriceTotal { get; set; }

/// <summary>
/// discount - float  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("discount")]
public double? Discount { get; set; }

/// <summary>
/// term_key - binary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("term_key")]
public string TermKey { get; set; }

/// <summary>
/// tax_key - binary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("tax_key")]
public string TaxKey { get; set; }

/// <summary>
/// compute_all_tax - binary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("compute_all_tax")]
public string ComputeAllTax { get; set; }

/// <summary>
/// compute_all_tax_dirty - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("compute_all_tax_dirty")]
public bool? ComputeAllTaxDirty { get; set; }

/// <summary>
/// epd_key - binary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("epd_key")]
public string EpdKey { get; set; }

/// <summary>
/// epd_needed - binary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("epd_needed")]
public string EpdNeeded { get; set; }

/// <summary>
/// epd_dirty - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("epd_dirty")]
public bool? EpdDirty { get; set; }

/// <summary>
/// analytic_line_ids - one2many - account.analytic.line (move_line_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("analytic_line_ids")]
public long[] AnalyticLineIds { get; set; }

/// <summary>
/// discount_date - date  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Last date at which the discounted amount must be paid in order for the Early Payment Discount to be granted <br />
/// </summary>
[JsonProperty("discount_date")]
public DateTime? DiscountDate { get; set; }

/// <summary>
/// discount_amount_currency - monetary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("discount_amount_currency")]
public decimal? DiscountAmountCurrency { get; set; }

/// <summary>
/// discount_balance - monetary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("discount_balance")]
public decimal? DiscountBalance { get; set; }

/// <summary>
/// discount_percentage - float  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("discount_percentage")]
public double? DiscountPercentage { get; set; }

/// <summary>
/// blocked - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: You can check this box to mark this journal item as a litigation with the associated partner <br />
/// </summary>
[JsonProperty("blocked")]
public bool? Blocked { get; set; }

/// <summary>
/// is_refund - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("is_refund")]
public bool? IsRefund { get; set; }

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
/// move_attachment_ids - one2many - ir.attachment <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("move_attachment_ids")]
public long[] MoveAttachmentIds { get; set; }

/// <summary>
/// vehicle_id - many2one - fleet.vehicle <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("vehicle_id")]
public long? VehicleId { get; set; }

/// <summary>
/// need_vehicle - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("need_vehicle")]
public bool? NeedVehicle { get; set; }

/// <summary>
/// expense_id - many2one - hr.expense <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("expense_id")]
public long? ExpenseId { get; set; }

/// <summary>
/// purchase_line_id - many2one - purchase.order.line <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("purchase_line_id")]
public long? PurchaseLineId { get; set; }

/// <summary>
/// purchase_order_id - many2one - purchase.order <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("purchase_order_id")]
public long? PurchaseOrderId { get; set; }

/// <summary>
/// expected_pay_date - date  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Expected payment date as manually set through the customer statement(e.g: if you had the customer on the phone and want to remember the date he promised he would pay) <br />
/// </summary>
[JsonProperty("expected_pay_date")]
public DateTime? ExpectedPayDate { get; set; }

/// <summary>
/// is_downpayment - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("is_downpayment")]
public bool? IsDownpayment { get; set; }

/// <summary>
/// sale_line_ids - many2many - sale.order.line <br />
/// Required: False, Readonly: True, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("sale_line_ids")]
public long[] SaleLineIds { get; set; }

/// <summary>
/// asset_ids - many2many - account.asset <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("asset_ids")]
public long[] AssetIds { get; set; }

/// <summary>
/// non_deductible_tax_value - monetary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("non_deductible_tax_value")]
public decimal? NonDeductibleTaxValue { get; set; }

/// <summary>
/// consolidation_journal_line_ids - many2many - consolidation.journal.line <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("consolidation_journal_line_ids")]
public long[] ConsolidationJournalLineIds { get; set; }

/// <summary>
/// followup_line_id - many2one - account_followup.followup.line <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("followup_line_id")]
public long? FollowupLineId { get; set; }

/// <summary>
/// last_followup_date - date  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("last_followup_date")]
public DateTime? LastFollowupDate { get; set; }

/// <summary>
/// next_action_date - date  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Date where the next action should be taken for a receivable item. Usually, automatically set when sending reminders through the customer statement. <br />
/// </summary>
[JsonProperty("next_action_date")]
public DateTime? NextActionDate { get; set; }

/// <summary>
/// invoice_date - date  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("invoice_date")]
public DateTime? InvoiceDate { get; set; }

/// <summary>
/// invoice_origin - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: The document(s) that generated the invoice. <br />
/// </summary>
[JsonProperty("invoice_origin")]
public string InvoiceOrigin { get; set; }

/// <summary>
/// intrastat_transaction_id - many2one - account.intrastat.code <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("intrastat_transaction_id")]
public long? IntrastatTransactionId { get; set; }

/// <summary>
/// intrastat_product_origin_country_id - many2one - res.country <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("intrastat_product_origin_country_id")]
public long? IntrastatProductOriginCountryId { get; set; }

/// <summary>
/// fec_matching_number - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Matching code that is used in FEC import for reconciliation <br />
/// </summary>
[JsonProperty("fec_matching_number")]
public string FecMatchingNumber { get; set; }

/// <summary>
/// x_studio_selection_field_sBMfS - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("x_studio_selection_field_sBMfS")]
public NewSLectionAccountMoveLineOdooEnum? XStudioSelectionFieldSbmfs { get; set; }

/// <summary>
/// x_studio_text_field_QpDBo - text  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("x_studio_text_field_QpDBo")]
public string XStudioTextFieldQpdbo { get; set; }

/// <summary>
/// x_studio_boolean_field_PMWvE - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("x_studio_boolean_field_PMWvE")]
public bool? XStudioBooleanFieldPmwve { get; set; }

/// <summary>
/// x_studio_unit - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("x_studio_unit")]
public UnitAccountMoveLineOdooEnum? XStudioUnit { get; set; }

/// <summary>
/// x_account_move_line_line_ids_1fbc3 - one2many - x_account_move_line_line_2a381 (x_account_move_line_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("x_account_move_line_line_ids_1fbc3")]
public long[] XAccountMoveLineLineIds1Fbc3 { get; set; }

/// <summary>
/// x_account_move_line_line_ids_76358 - one2many - x_account_move_line_line_1853b (x_account_move_line_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("x_account_move_line_line_ids_76358")]
public long[] XAccountMoveLineLineIds76358 { get; set; }

/// <summary>
/// x_studio_montant - monetary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("x_studio_montant")]
public decimal? XStudioMontant { get; set; }

/// <summary>
/// x_studio_monetary_field_bWuaz - monetary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("x_studio_monetary_field_bWuaz")]
public decimal? XStudioMonetaryFieldBwuaz { get; set; }
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StatusAccountMoveLineOdooEnum
{
[EnumMember(Value = "draft")]
Draft = 1,

[EnumMember(Value = "posted")]
Posted = 2,

[EnumMember(Value = "cancel")]
Cancelled = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TypeAccountMoveLineOdooEnum
{
[EnumMember(Value = "entry")]
JournalEntry = 1,

[EnumMember(Value = "out_invoice")]
CustomerInvoice = 2,

[EnumMember(Value = "out_refund")]
CustomerCreditNote = 3,

[EnumMember(Value = "in_invoice")]
VendorBill = 4,

[EnumMember(Value = "in_refund")]
VendorCreditNote = 5,

[EnumMember(Value = "out_receipt")]
SalesReceipt = 6,

[EnumMember(Value = "in_receipt")]
PurchaseReceipt = 7,
}


/// <summary>
/// Help: Account Type is used for information purpose, to generate country-specific legal reports, and set the rules to close a fiscal year and generate opening entries.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum InternalTypeAccountMoveLineOdooEnum
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
public enum InternalGroupAccountMoveLineOdooEnum
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
public enum DisplayTypeAccountMoveLineOdooEnum
{
[EnumMember(Value = "product")]
Product = 1,

[EnumMember(Value = "cogs")]
CostOfGoodsSold = 2,

[EnumMember(Value = "tax")]
Tax = 3,

[EnumMember(Value = "rounding")]
Rounding = 4,

[EnumMember(Value = "payment_term")]
PaymentTerm = 5,

[EnumMember(Value = "line_section")]
Section = 6,

[EnumMember(Value = "line_note")]
Note = 7,

[EnumMember(Value = "epd")]
EarlyPaymentDiscount = 8,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum NewSLectionAccountMoveLineOdooEnum
{
[EnumMember(Value = "Tonne(s)")]
TonneS = 1,

[EnumMember(Value = "Forfait")]
Forfait = 2,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum UnitAccountMoveLineOdooEnum
{
[EnumMember(Value = "Tonne(s)")]
TonneS = 1,

[EnumMember(Value = "Forfait")]
Forfait = 2,
}
#pragma warning restore S2344
#pragma warning restore S3903 