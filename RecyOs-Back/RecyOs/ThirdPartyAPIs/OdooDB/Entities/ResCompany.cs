// Fichier généré automatiquement par OdooJsonRpcGenerator
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;
#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
namespace RecyOs.OdooDB.Entities;

[OdooTableName("res.company")]
[JsonConverter(typeof(OdooModelConverter))]
public class ResCompanyOdooModel : IOdooModel
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
/// active - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("active")]
public bool? Active { get; set; }

/// <summary>
/// sequence - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Used to order Companies in the company switcher <br />
/// </summary>
[JsonProperty("sequence")]
public int? Sequence { get; set; }

/// <summary>
/// parent_id - many2one - res.company <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("parent_id")]
public long? ParentId { get; set; }

/// <summary>
/// child_ids - one2many - res.company (parent_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("child_ids")]
public long[] ChildIds { get; set; }

/// <summary>
/// partner_id - many2one - res.partner <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("partner_id")]
public long PartnerId { get; set; }

/// <summary>
/// report_header - html  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Appears by default on the top right corner of your printed documents (report header). <br />
/// </summary>
[JsonProperty("report_header")]
public string ReportHeader { get; set; }

/// <summary>
/// report_footer - html  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Footer text displayed at the bottom of all reports. <br />
/// </summary>
[JsonProperty("report_footer")]
public string ReportFooter { get; set; }

/// <summary>
/// company_details - html  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Header text displayed at the top of all reports. <br />
/// </summary>
[JsonProperty("company_details")]
public string CompanyDetails { get; set; }

/// <summary>
/// is_company_details_empty - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("is_company_details_empty")]
public bool? IsCompanyDetailsEmpty { get; set; }

/// <summary>
/// logo - binary  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("logo")]
public string Logo { get; set; }

/// <summary>
/// logo_web - binary  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("logo_web")]
public string LogoWeb { get; set; }

/// <summary>
/// currency_id - many2one - res.currency <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("currency_id")]
public long CurrencyId { get; set; }

/// <summary>
/// user_ids - many2many - res.users <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("user_ids")]
public long[] UserIds { get; set; }

/// <summary>
/// street - char  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("street")]
public string Street { get; set; }

/// <summary>
/// street2 - char  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("street2")]
public string Street2 { get; set; }

/// <summary>
/// zip - char  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("zip")]
public string Zip { get; set; }

/// <summary>
/// city - char  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("city")]
public string City { get; set; }

/// <summary>
/// state_id - many2one - res.country.state <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("state_id")]
public long? StateId { get; set; }

/// <summary>
/// bank_ids - one2many - res.partner.bank <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("bank_ids")]
public long[] BankIds { get; set; }

/// <summary>
/// country_id - many2one - res.country <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("country_id")]
public long? CountryId { get; set; }

/// <summary>
/// email - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("email")]
public string Email { get; set; }

/// <summary>
/// phone - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("phone")]
public string Phone { get; set; }

/// <summary>
/// mobile - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("mobile")]
public string Mobile { get; set; }

/// <summary>
/// website - char  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("website")]
public string Website { get; set; }

/// <summary>
/// vat - char  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: The Tax Identification Number. Values here will be validated based on the country format. You can use '/' to indicate that the partner is not subject to tax. <br />
/// </summary>
[JsonProperty("vat")]
public string Vat { get; set; }

/// <summary>
/// company_registry - char  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: The registry number of the company. Use it if it is different from the Tax ID. It must be unique across all partners of a same country <br />
/// </summary>
[JsonProperty("company_registry")]
public string CompanyRegistry { get; set; }

/// <summary>
/// paperformat_id - many2one - report.paperformat <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("paperformat_id")]
public long? PaperformatId { get; set; }

/// <summary>
/// external_report_layout_id - many2one - ir.ui.view <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("external_report_layout_id")]
public long? ExternalReportLayoutId { get; set; }

/// <summary>
/// base_onboarding_company_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("base_onboarding_company_state")]
public StateOfTheOnboardingCompanyStepResCompanyOdooEnum? BaseOnboardingCompanyState { get; set; }

/// <summary>
/// favicon - binary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// Help: This field holds the image used to display a favicon for a given company. <br />
/// </summary>
[JsonProperty("favicon")]
public string Favicon { get; set; }

/// <summary>
/// font - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("font")]
public FontResCompanyOdooEnum? Font { get; set; }

/// <summary>
/// primary_color - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("primary_color")]
public string PrimaryColor { get; set; }

/// <summary>
/// secondary_color - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("secondary_color")]
public string SecondaryColor { get; set; }

/// <summary>
/// layout_background - selection  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("layout_background")]
public LayoutBackgroundResCompanyOdooEnum LayoutBackground { get; set; }

/// <summary>
/// layout_background_image - binary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("layout_background_image")]
public string LayoutBackgroundImage { get; set; }

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
/// social_twitter - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("social_twitter")]
public string SocialTwitter { get; set; }

/// <summary>
/// social_facebook - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("social_facebook")]
public string SocialFacebook { get; set; }

/// <summary>
/// social_github - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("social_github")]
public string SocialGithub { get; set; }

/// <summary>
/// social_linkedin - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("social_linkedin")]
public string SocialLinkedin { get; set; }

/// <summary>
/// social_youtube - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("social_youtube")]
public string SocialYoutube { get; set; }

/// <summary>
/// social_instagram - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("social_instagram")]
public string SocialInstagram { get; set; }

/// <summary>
/// resource_calendar_ids - one2many - resource.calendar (company_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("resource_calendar_ids")]
public long[] ResourceCalendarIds { get; set; }

/// <summary>
/// resource_calendar_id - many2one - resource.calendar <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("resource_calendar_id")]
public long? ResourceCalendarId { get; set; }

/// <summary>
/// catchall_email - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("catchall_email")]
public string CatchallEmail { get; set; }

/// <summary>
/// catchall_formatted - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("catchall_formatted")]
public string CatchallFormatted { get; set; }

/// <summary>
/// email_formatted - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("email_formatted")]
public string EmailFormatted { get; set; }

/// <summary>
/// hr_presence_control_email_amount - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("hr_presence_control_email_amount")]
public int? HrPresenceControlEmailAmount { get; set; }

/// <summary>
/// hr_presence_control_ip_list - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("hr_presence_control_ip_list")]
public string HrPresenceControlIpList { get; set; }

/// <summary>
/// partner_gid - integer  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("partner_gid")]
public int? PartnerGid { get; set; }

/// <summary>
/// iap_enrich_auto_done - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("iap_enrich_auto_done")]
public bool? IapEnrichAutoDone { get; set; }

/// <summary>
/// snailmail_color - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("snailmail_color")]
public bool? SnailmailColor { get; set; }

/// <summary>
/// snailmail_cover - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("snailmail_cover")]
public bool? SnailmailCover { get; set; }

/// <summary>
/// snailmail_duplex - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("snailmail_duplex")]
public bool? SnailmailDuplex { get; set; }

/// <summary>
/// payment_provider_onboarding_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("payment_provider_onboarding_state")]
public StateOfTheOnboardingPaymentProviderStepResCompanyOdooEnum? PaymentProviderOnboardingState { get; set; }

/// <summary>
/// payment_onboarding_payment_method - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("payment_onboarding_payment_method")]
public SelectedOnboardingPaymentMethodResCompanyOdooEnum? PaymentOnboardingPaymentMethod { get; set; }

/// <summary>
/// background_image - binary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("background_image")]
public string BackgroundImage { get; set; }

/// <summary>
/// fiscalyear_last_day - integer  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("fiscalyear_last_day")]
public int FiscalyearLastDay { get; set; }

/// <summary>
/// fiscalyear_last_month - selection  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("fiscalyear_last_month")]
public FiscalyearLastMonthResCompanyOdooEnum FiscalyearLastMonth { get; set; }

/// <summary>
/// period_lock_date - date  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Only users with the 'Adviser' role can edit accounts prior to and inclusive of this date. Use it for period locking inside an open fiscal year, for example. <br />
/// </summary>
[JsonProperty("period_lock_date")]
public DateTime? PeriodLockDate { get; set; }

/// <summary>
/// fiscalyear_lock_date - date  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: No users, including Advisers, can edit accounts prior to and inclusive of this date. Use it for fiscal year locking for example. <br />
/// </summary>
[JsonProperty("fiscalyear_lock_date")]
public DateTime? FiscalyearLockDate { get; set; }

/// <summary>
/// tax_lock_date - date  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: No users can edit journal entries related to a tax prior and inclusive of this date. <br />
/// </summary>
[JsonProperty("tax_lock_date")]
public DateTime? TaxLockDate { get; set; }

/// <summary>
/// transfer_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Intermediary account used when moving money from a liqity account to another <br />
/// </summary>
[JsonProperty("transfer_account_id")]
public long? TransferAccountId { get; set; }

/// <summary>
/// expects_chart_of_accounts - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("expects_chart_of_accounts")]
public bool? ExpectsChartOfAccounts { get; set; }

/// <summary>
/// chart_template_id - many2one - account.chart.template <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: The chart template for the company (if any) <br />
/// </summary>
[JsonProperty("chart_template_id")]
public long? ChartTemplateId { get; set; }

/// <summary>
/// bank_account_code_prefix - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("bank_account_code_prefix")]
public string BankAccountCodePrefix { get; set; }

/// <summary>
/// cash_account_code_prefix - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("cash_account_code_prefix")]
public string CashAccountCodePrefix { get; set; }

/// <summary>
/// default_cash_difference_income_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("default_cash_difference_income_account_id")]
public long? DefaultCashDifferenceIncomeAccountId { get; set; }

/// <summary>
/// default_cash_difference_expense_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("default_cash_difference_expense_account_id")]
public long? DefaultCashDifferenceExpenseAccountId { get; set; }

/// <summary>
/// account_journal_suspense_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_journal_suspense_account_id")]
public long? AccountJournalSuspenseAccountId { get; set; }

/// <summary>
/// account_journal_payment_debit_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_journal_payment_debit_account_id")]
public long? AccountJournalPaymentDebitAccountId { get; set; }

/// <summary>
/// account_journal_payment_credit_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_journal_payment_credit_account_id")]
public long? AccountJournalPaymentCreditAccountId { get; set; }

/// <summary>
/// account_journal_early_pay_discount_gain_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_journal_early_pay_discount_gain_account_id")]
public long? AccountJournalEarlyPayDiscountGainAccountId { get; set; }

/// <summary>
/// account_journal_early_pay_discount_loss_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_journal_early_pay_discount_loss_account_id")]
public long? AccountJournalEarlyPayDiscountLossAccountId { get; set; }

/// <summary>
/// early_pay_discount_computation - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("early_pay_discount_computation")]
public CashDiscountTaxReductionResCompanyOdooEnum? EarlyPayDiscountComputation { get; set; }

/// <summary>
/// transfer_account_code_prefix - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("transfer_account_code_prefix")]
public string TransferAccountCodePrefix { get; set; }

/// <summary>
/// account_sale_tax_id - many2one - account.tax <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_sale_tax_id")]
public long? AccountSaleTaxId { get; set; }

/// <summary>
/// account_purchase_tax_id - many2one - account.tax <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_purchase_tax_id")]
public long? AccountPurchaseTaxId { get; set; }

/// <summary>
/// tax_calculation_rounding_method - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("tax_calculation_rounding_method")]
public TaxCalculationRoundingMethodResCompanyOdooEnum? TaxCalculationRoundingMethod { get; set; }

/// <summary>
/// currency_exchange_journal_id - many2one - account.journal <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("currency_exchange_journal_id")]
public long? CurrencyExchangeJournalId { get; set; }

/// <summary>
/// income_currency_exchange_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("income_currency_exchange_account_id")]
public long? IncomeCurrencyExchangeAccountId { get; set; }

/// <summary>
/// expense_currency_exchange_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("expense_currency_exchange_account_id")]
public long? ExpenseCurrencyExchangeAccountId { get; set; }

/// <summary>
/// anglo_saxon_accounting - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("anglo_saxon_accounting")]
public bool? AngloSaxonAccounting { get; set; }

/// <summary>
/// property_stock_account_input_categ_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("property_stock_account_input_categ_id")]
public long? PropertyStockAccountInputCategId { get; set; }

/// <summary>
/// property_stock_account_output_categ_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("property_stock_account_output_categ_id")]
public long? PropertyStockAccountOutputCategId { get; set; }

/// <summary>
/// property_stock_valuation_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("property_stock_valuation_account_id")]
public long? PropertyStockValuationAccountId { get; set; }

/// <summary>
/// bank_journal_ids - one2many - account.journal (company_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("bank_journal_ids")]
public long[] BankJournalIds { get; set; }

/// <summary>
/// incoterm_id - many2one - account.incoterms <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: International Commercial Terms are a series of predefined commercial terms used in international transactions. <br />
/// </summary>
[JsonProperty("incoterm_id")]
public long? IncotermId { get; set; }

/// <summary>
/// qr_code - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("qr_code")]
public bool? QrCode { get; set; }

/// <summary>
/// invoice_is_email - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("invoice_is_email")]
public bool? InvoiceIsEmail { get; set; }

/// <summary>
/// invoice_is_print - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("invoice_is_print")]
public bool? InvoiceIsPrint { get; set; }

/// <summary>
/// account_use_credit_limit - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Enable the use of credit limit on partners. <br />
/// </summary>
[JsonProperty("account_use_credit_limit")]
public bool? AccountUseCreditLimit { get; set; }

/// <summary>
/// account_opening_move_id - many2one - account.move <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: The journal entry containing the initial balance of all this company's accounts. <br />
/// </summary>
[JsonProperty("account_opening_move_id")]
public long? AccountOpeningMoveId { get; set; }

/// <summary>
/// account_opening_journal_id - many2one - account.journal <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: Journal where the opening entry of this company's accounting has been posted. <br />
/// </summary>
[JsonProperty("account_opening_journal_id")]
public long? AccountOpeningJournalId { get; set; }

/// <summary>
/// account_opening_date - date  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// Help: That is the date of the opening entry. <br />
/// </summary>
[JsonProperty("account_opening_date")]
public DateTime AccountOpeningDate { get; set; }

/// <summary>
/// account_setup_bank_data_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_setup_bank_data_state")]
public StateOfTheOnboardingBankDataStepResCompanyOdooEnum? AccountSetupBankDataState { get; set; }

/// <summary>
/// account_setup_fy_data_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_setup_fy_data_state")]
public StateOfTheOnboardingFiscalYearStepResCompanyOdooEnum? AccountSetupFyDataState { get; set; }

/// <summary>
/// account_setup_coa_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_setup_coa_state")]
public StateOfTheOnboardingChartsOfAccountStepResCompanyOdooEnum? AccountSetupCoaState { get; set; }

/// <summary>
/// account_setup_taxes_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_setup_taxes_state")]
public StateOfTheOnboardingTaxesStepResCompanyOdooEnum? AccountSetupTaxesState { get; set; }

/// <summary>
/// account_onboarding_invoice_layout_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_onboarding_invoice_layout_state")]
public StateOfTheOnboardingInvoiceLayoutStepResCompanyOdooEnum? AccountOnboardingInvoiceLayoutState { get; set; }

/// <summary>
/// account_onboarding_create_invoice_state - selection  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("account_onboarding_create_invoice_state")]
public StateOfTheOnboardingCreateInvoiceStepResCompanyOdooEnum? AccountOnboardingCreateInvoiceState { get; set; }

/// <summary>
/// account_onboarding_create_invoice_state_flag - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_onboarding_create_invoice_state_flag")]
public bool? AccountOnboardingCreateInvoiceStateFlag { get; set; }

/// <summary>
/// account_onboarding_sale_tax_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_onboarding_sale_tax_state")]
public StateOfTheOnboardingSaleTaxStepResCompanyOdooEnum? AccountOnboardingSaleTaxState { get; set; }

/// <summary>
/// account_invoice_onboarding_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_invoice_onboarding_state")]
public StateOfTheAccountInvoiceOnboardingPanelResCompanyOdooEnum? AccountInvoiceOnboardingState { get; set; }

/// <summary>
/// account_dashboard_onboarding_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_dashboard_onboarding_state")]
public StateOfTheAccountDashboardOnboardingPanelResCompanyOdooEnum? AccountDashboardOnboardingState { get; set; }

/// <summary>
/// invoice_terms - html  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("invoice_terms")]
public string InvoiceTerms { get; set; }

/// <summary>
/// terms_type - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("terms_type")]
public TermsConditionsFormatResCompanyOdooEnum? TermsType { get; set; }

/// <summary>
/// invoice_terms_html - html  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("invoice_terms_html")]
public string InvoiceTermsHtml { get; set; }

/// <summary>
/// account_setup_bill_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_setup_bill_state")]
public StateOfTheOnboardingBillStepResCompanyOdooEnum? AccountSetupBillState { get; set; }

/// <summary>
/// account_default_pos_receivable_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_default_pos_receivable_account_id")]
public long? AccountDefaultPosReceivableAccountId { get; set; }

/// <summary>
/// expense_accrual_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Account used to move the period of an expense <br />
/// </summary>
[JsonProperty("expense_accrual_account_id")]
public long? ExpenseAccrualAccountId { get; set; }

/// <summary>
/// revenue_accrual_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Account used to move the period of a revenue <br />
/// </summary>
[JsonProperty("revenue_accrual_account_id")]
public long? RevenueAccrualAccountId { get; set; }

/// <summary>
/// automatic_entry_default_journal_id - many2one - account.journal <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Journal used by default for moving the period of an entry <br />
/// </summary>
[JsonProperty("automatic_entry_default_journal_id")]
public long? AutomaticEntryDefaultJournalId { get; set; }

/// <summary>
/// country_code - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: The ISO country code in two chars. ; You can use this field for quick search. <br />
/// </summary>
[JsonProperty("country_code")]
public string CountryCode { get; set; }

/// <summary>
/// account_fiscal_country_id - many2one - res.country <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: The country to use the tax reports from for this company <br />
/// </summary>
[JsonProperty("account_fiscal_country_id")]
public long? AccountFiscalCountryId { get; set; }

/// <summary>
/// account_enabled_tax_country_ids - many2many - res.country <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Technical field containing the countries for which this company is using tax-related features(hence the ones for which l10n modules need to show tax-related fields). <br />
/// </summary>
[JsonProperty("account_enabled_tax_country_ids")]
public long[] AccountEnabledTaxCountryIds { get; set; }

/// <summary>
/// tax_exigibility - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("tax_exigibility")]
public bool? TaxExigibility { get; set; }

/// <summary>
/// tax_cash_basis_journal_id - many2one - account.journal <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("tax_cash_basis_journal_id")]
public long? TaxCashBasisJournalId { get; set; }

/// <summary>
/// account_cash_basis_base_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Account that will be set on lines created in cash basis journal entry and used to keep track of the tax base amount. <br />
/// </summary>
[JsonProperty("account_cash_basis_base_account_id")]
public long? AccountCashBasisBaseAccountId { get; set; }

/// <summary>
/// account_storno - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_storno")]
public bool? AccountStorno { get; set; }

/// <summary>
/// fiscal_position_ids - one2many - account.fiscal.position (company_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("fiscal_position_ids")]
public long[] FiscalPositionIds { get; set; }

/// <summary>
/// multi_vat_foreign_country_ids - many2many - res.country <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Countries for which the company has a VAT number <br />
/// </summary>
[JsonProperty("multi_vat_foreign_country_ids")]
public long[] MultiVatForeignCountryIds { get; set; }

/// <summary>
/// quick_edit_mode - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("quick_edit_mode")]
public QuickEncodingResCompanyOdooEnum? QuickEditMode { get; set; }

/// <summary>
/// website_id - many2one - website <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("website_id")]
public long? WebsiteId { get; set; }

/// <summary>
/// invoicing_switch_threshold - date  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Every payment and invoice before this date will receive the 'From Invoicing' status, hiding all the accounting entries related to it. Use this option after installing Accounting if you were using only Invoicing before, before importing all your actual accounting data in to Odoo. <br />
/// </summary>
[JsonProperty("invoicing_switch_threshold")]
public DateTime? InvoicingSwitchThreshold { get; set; }

/// <summary>
/// account_check_printing_layout - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Select the format corresponding to the check paper you will be printing your checks on.; In order to disable the printing feature, select 'None'. <br />
/// </summary>
[JsonProperty("account_check_printing_layout")]
public CheckLayoutResCompanyOdooEnum? AccountCheckPrintingLayout { get; set; }

/// <summary>
/// account_check_printing_date_label - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: This option allows you to print the date label on the check as per CPA.; Disable this if your pre-printed check includes the date label. <br />
/// </summary>
[JsonProperty("account_check_printing_date_label")]
public bool? AccountCheckPrintingDateLabel { get; set; }

/// <summary>
/// account_check_printing_multi_stub - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: This option allows you to print check details (stub) on multiple pages if they don't fit on a single page. <br />
/// </summary>
[JsonProperty("account_check_printing_multi_stub")]
public bool? AccountCheckPrintingMultiStub { get; set; }

/// <summary>
/// account_check_printing_margin_top - float  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Adjust the margins of generated checks to make it fit your printer's settings. <br />
/// </summary>
[JsonProperty("account_check_printing_margin_top")]
public double? AccountCheckPrintingMarginTop { get; set; }

/// <summary>
/// account_check_printing_margin_left - float  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Adjust the margins of generated checks to make it fit your printer's settings. <br />
/// </summary>
[JsonProperty("account_check_printing_margin_left")]
public double? AccountCheckPrintingMarginLeft { get; set; }

/// <summary>
/// account_check_printing_margin_right - float  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Adjust the margins of generated checks to make it fit your printer's settings. <br />
/// </summary>
[JsonProperty("account_check_printing_margin_right")]
public double? AccountCheckPrintingMarginRight { get; set; }

/// <summary>
/// rule_type - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Select the type to setup inter company rules in selected company. <br />
/// </summary>
[JsonProperty("rule_type")]
public RuleResCompanyOdooEnum? RuleType { get; set; }

/// <summary>
/// intercompany_user_id - many2one - res.users <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Responsible user for creation of documents triggered by intercompany rules. <br />
/// </summary>
[JsonProperty("intercompany_user_id")]
public long? IntercompanyUserId { get; set; }

/// <summary>
/// intercompany_transaction_message - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("intercompany_transaction_message")]
public string IntercompanyTransactionMessage { get; set; }

/// <summary>
/// extract_in_invoice_digitalization_mode - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("extract_in_invoice_digitalization_mode")]
public DigitizationModeOnVendorBillsResCompanyOdooEnum? ExtractInInvoiceDigitalizationMode { get; set; }

/// <summary>
/// extract_out_invoice_digitalization_mode - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("extract_out_invoice_digitalization_mode")]
public DigitizationModeOnCustomerInvoicesResCompanyOdooEnum? ExtractOutInvoiceDigitalizationMode { get; set; }

/// <summary>
/// extract_single_line_per_tax - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("extract_single_line_per_tax")]
public bool? ExtractSingleLinePerTax { get; set; }

/// <summary>
/// vat_check_vies - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("vat_check_vies")]
public bool? VatCheckVies { get; set; }

/// <summary>
/// currency_interval_unit - selection  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("currency_interval_unit")]
public IntervalUnitResCompanyOdooEnum CurrencyIntervalUnit { get; set; }

/// <summary>
/// currency_next_execution_date - date  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("currency_next_execution_date")]
public DateTime? CurrencyNextExecutionDate { get; set; }

/// <summary>
/// currency_provider - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("currency_provider")]
public ServiceProviderResCompanyOdooEnum? CurrencyProvider { get; set; }

/// <summary>
/// documents_hr_settings - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("documents_hr_settings")]
public bool? DocumentsHrSettings { get; set; }

/// <summary>
/// documents_hr_folder - many2one - documents.folder <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("documents_hr_folder")]
public long? DocumentsHrFolder { get; set; }

/// <summary>
/// documents_product_settings - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("documents_product_settings")]
public bool? DocumentsProductSettings { get; set; }

/// <summary>
/// product_folder - many2one - documents.folder <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("product_folder")]
public long? ProductFolder { get; set; }

/// <summary>
/// product_tags - many2many - documents.tag <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("product_tags")]
public long[] ProductTags { get; set; }

/// <summary>
/// documents_spreadsheet_folder_id - many2one - documents.folder <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("documents_spreadsheet_folder_id")]
public long? DocumentsSpreadsheetFolderId { get; set; }

/// <summary>
/// expense_journal_id - many2one - account.journal <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: The company's default journal used when an employee expense is created. <br />
/// </summary>
[JsonProperty("expense_journal_id")]
public long? ExpenseJournalId { get; set; }

/// <summary>
/// company_expense_journal_id - many2one - account.journal <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: The company's default journal used when a company expense is created. <br />
/// </summary>
[JsonProperty("company_expense_journal_id")]
public long? CompanyExpenseJournalId { get; set; }

/// <summary>
/// po_lead - float  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// Help: Margin of error for vendor lead times. When the system generates Purchase Orders for procuring products, they will be scheduled that many days earlier to cope with unexpected vendor delays. <br />
/// </summary>
[JsonProperty("po_lead")]
public double PoLead { get; set; }

/// <summary>
/// po_lock - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Purchase Order Modification used when you want to purchase order editable after confirm <br />
/// </summary>
[JsonProperty("po_lock")]
public PurchaseOrderModificationResCompanyOdooEnum? PoLock { get; set; }

/// <summary>
/// po_double_validation - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Provide a double validation mechanism for purchases <br />
/// </summary>
[JsonProperty("po_double_validation")]
public LevelsOfApprovalsResCompanyOdooEnum? PoDoubleValidation { get; set; }

/// <summary>
/// po_double_validation_amount - monetary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Minimum amount for which a double validation is required <br />
/// </summary>
[JsonProperty("po_double_validation_amount")]
public decimal? PoDoubleValidationAmount { get; set; }

/// <summary>
/// invoice_is_snailmail - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("invoice_is_snailmail")]
public bool? InvoiceIsSnailmail { get; set; }

/// <summary>
/// totals_below_sections - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: When ticked, totals and subtotals appear below the sections of the report. <br />
/// </summary>
[JsonProperty("totals_below_sections")]
public bool? TotalsBelowSections { get; set; }

/// <summary>
/// account_tax_periodicity - selection  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// Help: Periodicity <br />
/// </summary>
[JsonProperty("account_tax_periodicity")]
public DelayUnitsResCompanyOdooEnum AccountTaxPeriodicity { get; set; }

/// <summary>
/// account_tax_periodicity_reminder_day - integer  <br />
/// Required: True, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_tax_periodicity_reminder_day")]
public int AccountTaxPeriodicityReminderDay { get; set; }

/// <summary>
/// account_tax_periodicity_journal_id - many2one - account.journal <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_tax_periodicity_journal_id")]
public long? AccountTaxPeriodicityJournalId { get; set; }

/// <summary>
/// account_revaluation_journal_id - many2one - account.journal <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_revaluation_journal_id")]
public long? AccountRevaluationJournalId { get; set; }

/// <summary>
/// account_revaluation_expense_provision_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_revaluation_expense_provision_account_id")]
public long? AccountRevaluationExpenseProvisionAccountId { get; set; }

/// <summary>
/// account_revaluation_income_provision_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_revaluation_income_provision_account_id")]
public long? AccountRevaluationIncomeProvisionAccountId { get; set; }

/// <summary>
/// account_tax_unit_ids - many2many - account.tax.unit <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// Help: The tax units this company belongs to. <br />
/// </summary>
[JsonProperty("account_tax_unit_ids")]
public long[] AccountTaxUnitIds { get; set; }

/// <summary>
/// account_representative_id - many2one - res.partner <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Specify an Accounting Firm that will act as a representative when exporting reports. <br />
/// </summary>
[JsonProperty("account_representative_id")]
public long? AccountRepresentativeId { get; set; }

/// <summary>
/// account_display_representative_field - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("account_display_representative_field")]
public bool? AccountDisplayRepresentativeField { get; set; }

/// <summary>
/// sepa_orgid_id - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Identification assigned by an institution (eg. VAT number). <br />
/// </summary>
[JsonProperty("sepa_orgid_id")]
public string SepaOrgidId { get; set; }

/// <summary>
/// sepa_orgid_issr - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Entity that assigns the identification (eg. KBE-BCO or Finanzamt Muenchen IV). <br />
/// </summary>
[JsonProperty("sepa_orgid_issr")]
public string SepaOrgidIssr { get; set; }

/// <summary>
/// sepa_initiating_party_name - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Will appear in SEPA payments as the name of the party initiating the payment. Limited to 70 characters. <br />
/// </summary>
[JsonProperty("sepa_initiating_party_name")]
public string SepaInitiatingPartyName { get; set; }

/// <summary>
/// documents_hr_contracts_tags - many2many - documents.tag <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("documents_hr_contracts_tags")]
public long[] DocumentsHrContractsTags { get; set; }

/// <summary>
/// l10n_fr_closing_sequence_id - many2one - ir.sequence <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("l10n_fr_closing_sequence_id")]
public long? L10nFrClosingSequenceId { get; set; }

/// <summary>
/// siret - char  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("siret")]
public string Siret { get; set; }

/// <summary>
/// ape - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("ape")]
public string Ape { get; set; }

/// <summary>
/// portal_confirmation_sign - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("portal_confirmation_sign")]
public bool? PortalConfirmationSign { get; set; }

/// <summary>
/// portal_confirmation_pay - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("portal_confirmation_pay")]
public bool? PortalConfirmationPay { get; set; }

/// <summary>
/// quotation_validity_days - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("quotation_validity_days")]
public int? QuotationValidityDays { get; set; }

/// <summary>
/// sale_quotation_onboarding_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("sale_quotation_onboarding_state")]
public StateOfTheSaleOnboardingPanelResCompanyOdooEnum? SaleQuotationOnboardingState { get; set; }

/// <summary>
/// sale_onboarding_order_confirmation_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("sale_onboarding_order_confirmation_state")]
public StateOfTheOnboardingConfirmationOrderStepResCompanyOdooEnum? SaleOnboardingOrderConfirmationState { get; set; }

/// <summary>
/// sale_onboarding_sample_quotation_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("sale_onboarding_sample_quotation_state")]
public StateOfTheOnboardingSampleQuotationStepResCompanyOdooEnum? SaleOnboardingSampleQuotationState { get; set; }

/// <summary>
/// sale_onboarding_payment_method - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("sale_onboarding_payment_method")]
public SaleOnboardingSelectedPaymentMethodResCompanyOdooEnum? SaleOnboardingPaymentMethod { get; set; }

/// <summary>
/// gain_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Account used to write the journal item in case of gain while selling an asset <br />
/// </summary>
[JsonProperty("gain_account_id")]
public long? GainAccountId { get; set; }

/// <summary>
/// loss_account_id - many2one - account.account <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Account used to write the journal item in case of loss while selling an asset <br />
/// </summary>
[JsonProperty("loss_account_id")]
public long? LossAccountId { get; set; }

/// <summary>
/// consolidation_color - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("consolidation_color")]
public int? ConsolidationColor { get; set; }

/// <summary>
/// account_consolidation_currency_is_different - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("account_consolidation_currency_is_different")]
public bool? AccountConsolidationCurrencyIsDifferent { get; set; }

/// <summary>
/// consolidation_dashboard_onboarding_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("consolidation_dashboard_onboarding_state")]
public StateOfTheAccountConsolidationDashboardOnboardingPanelResCompanyOdooEnum? ConsolidationDashboardOnboardingState { get; set; }

/// <summary>
/// consolidation_setup_consolidation_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("consolidation_setup_consolidation_state")]
public StateOfTheOnboardingConsolidationStepResCompanyOdooEnum? ConsolidationSetupConsolidationState { get; set; }

/// <summary>
/// consolidation_setup_ccoa_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("consolidation_setup_ccoa_state")]
public StateOfTheOnboardingConsolidatedChartOfAccountStepResCompanyOdooEnum? ConsolidationSetupCcoaState { get; set; }

/// <summary>
/// consolidation_create_period_state - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("consolidation_create_period_state")]
public StateOfTheOnboardingCreatePeriodStepResCompanyOdooEnum? ConsolidationCreatePeriodState { get; set; }

/// <summary>
/// intrastat_region_id - many2one - account.intrastat.code <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("intrastat_region_id")]
public long? IntrastatRegionId { get; set; }

/// <summary>
/// intrastat_transport_mode_id - many2one - account.intrastat.code <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("intrastat_transport_mode_id")]
public long? IntrastatTransportModeId { get; set; }

/// <summary>
/// intrastat_default_invoice_transaction_code_id - many2one - account.intrastat.code <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("intrastat_default_invoice_transaction_code_id")]
public long? IntrastatDefaultInvoiceTransactionCodeId { get; set; }

/// <summary>
/// intrastat_default_refund_transaction_code_id - many2one - account.intrastat.code <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("intrastat_default_refund_transaction_code_id")]
public long? IntrastatDefaultRefundTransactionCodeId { get; set; }

/// <summary>
/// documents_account_settings - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("documents_account_settings")]
public bool? DocumentsAccountSettings { get; set; }

/// <summary>
/// account_folder - many2one - documents.folder <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("account_folder")]
public long? AccountFolder { get; set; }

/// <summary>
/// expense_extract_show_ocr_option_selection - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("expense_extract_show_ocr_option_selection")]
public SendModeOnExpenseAttachmentsResCompanyOdooEnum? ExpenseExtractShowOcrOptionSelection { get; set; }

/// <summary>
/// sale_order_template_id - many2one - sale.order.template <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("sale_order_template_id")]
public long? SaleOrderTemplateId { get; set; }

/// <summary>
/// extra_hour - float  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("extra_hour")]
public double? ExtraHour { get; set; }

/// <summary>
/// extra_day - float  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("extra_day")]
public double? ExtraDay { get; set; }

/// <summary>
/// min_extra_hour - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("min_extra_hour")]
public int? MinExtraHour { get; set; }

/// <summary>
/// extra_product - many2one - product.product <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: The product is used to add the cost to the sales order <br />
/// </summary>
[JsonProperty("extra_product")]
public long? ExtraProduct { get; set; }
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingCompanyStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum FontResCompanyOdooEnum
{
[EnumMember(Value = "Lato")]
Lato = 1,

[EnumMember(Value = "Roboto")]
Roboto = 2,

[EnumMember(Value = "Open_Sans")]
OpenSans = 3,

[EnumMember(Value = "Montserrat")]
Montserrat = 4,

[EnumMember(Value = "Oswald")]
Oswald = 5,

[EnumMember(Value = "Raleway")]
Raleway = 6,

[EnumMember(Value = "Tajawal")]
Tajawal = 7,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum LayoutBackgroundResCompanyOdooEnum
{
[EnumMember(Value = "Blank")]
Blank = 1,

[EnumMember(Value = "Geometric")]
Geometric = 2,

[EnumMember(Value = "Custom")]
Custom = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingPaymentProviderStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum SelectedOnboardingPaymentMethodResCompanyOdooEnum
{
[EnumMember(Value = "paypal")]
Paypal = 1,

[EnumMember(Value = "stripe")]
Stripe = 2,

[EnumMember(Value = "manual")]
Manual = 3,

[EnumMember(Value = "other")]
Other = 4,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum FiscalyearLastMonthResCompanyOdooEnum
{
[EnumMember(Value = "1")]
January = 1,

[EnumMember(Value = "2")]
February = 2,

[EnumMember(Value = "3")]
March = 3,

[EnumMember(Value = "4")]
April = 4,

[EnumMember(Value = "5")]
May = 5,

[EnumMember(Value = "6")]
June = 6,

[EnumMember(Value = "7")]
July = 7,

[EnumMember(Value = "8")]
August = 8,

[EnumMember(Value = "9")]
September = 9,

[EnumMember(Value = "10")]
October = 10,

[EnumMember(Value = "11")]
November = 11,

[EnumMember(Value = "12")]
December = 12,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum CashDiscountTaxReductionResCompanyOdooEnum
{
[EnumMember(Value = "included")]
OnEarlyPayment = 1,

[EnumMember(Value = "excluded")]
Never = 2,

[EnumMember(Value = "mixed")]
AlwaysUponInvoice = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TaxCalculationRoundingMethodResCompanyOdooEnum
{
[EnumMember(Value = "round_per_line")]
RoundPerLine = 1,

[EnumMember(Value = "round_globally")]
RoundGlobally = 2,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingBankDataStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingFiscalYearStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingChartsOfAccountStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingTaxesStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingInvoiceLayoutStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingCreateInvoiceStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingSaleTaxStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheAccountInvoiceOnboardingPanelResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,

[EnumMember(Value = "closed")]
Closed = 4,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheAccountDashboardOnboardingPanelResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,

[EnumMember(Value = "closed")]
Closed = 4,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TermsConditionsFormatResCompanyOdooEnum
{
[EnumMember(Value = "plain")]
AddANote = 1,

[EnumMember(Value = "html")]
AddALinkToAWebPage = 2,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingBillStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum QuickEncodingResCompanyOdooEnum
{
[EnumMember(Value = "out_invoices")]
CustomerInvoices = 1,

[EnumMember(Value = "in_invoices")]
VendorBills = 2,

[EnumMember(Value = "out_and_in_invoices")]
CustomerInvoicesAndVendorBills = 3,
}


/// <summary>
/// Help: Select the format corresponding to the check paper you will be printing your checks on. <br />
 /// In order to disable the printing feature, select 'None'.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum CheckLayoutResCompanyOdooEnum
{
[EnumMember(Value = "disabled")]
None = 1,
}


/// <summary>
/// Help: Select the type to setup inter company rules in selected company.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum RuleResCompanyOdooEnum
{
[EnumMember(Value = "not_synchronize")]
DoNotSynchronize = 1,

[EnumMember(Value = "invoice_and_refund")]
SynchronizeInvoicesBills = 2,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum DigitizationModeOnVendorBillsResCompanyOdooEnum
{
[EnumMember(Value = "no_send")]
DoNotDigitize = 1,

[EnumMember(Value = "manual_send")]
DigitizeOnDemandOnly = 2,

[EnumMember(Value = "auto_send")]
DigitizeAutomatically = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum DigitizationModeOnCustomerInvoicesResCompanyOdooEnum
{
[EnumMember(Value = "no_send")]
DoNotDigitize = 1,

[EnumMember(Value = "manual_send")]
DigitizeOnDemandOnly = 2,

[EnumMember(Value = "auto_send")]
DigitizeAutomatically = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum IntervalUnitResCompanyOdooEnum
{
[EnumMember(Value = "manually")]
Manually = 1,

[EnumMember(Value = "daily")]
Daily = 2,

[EnumMember(Value = "weekly")]
Weekly = 3,

[EnumMember(Value = "monthly")]
Monthly = 4,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum ServiceProviderResCompanyOdooEnum
{
[EnumMember(Value = "ecb")]
EuropeanCentralBank = 1,

[EnumMember(Value = "xe_com")]
XeCom = 2,

[EnumMember(Value = "cbuae")]
UAECentralBank = 3,

[EnumMember(Value = "boc")]
BankOfCanada = 4,

[EnumMember(Value = "fta")]
FederalTaxAdministrationSwitzerland = 5,

[EnumMember(Value = "mindicador")]
ChileanMindicadorCl = 6,

[EnumMember(Value = "cbegy")]
CentralBankOfEgypt = 7,

[EnumMember(Value = "banxico")]
MexicanBank = 8,

[EnumMember(Value = "bcrp")]
SUNATReplacesBankOfPeru = 9,

[EnumMember(Value = "bnr")]
NationalBankOfRomania = 10,

[EnumMember(Value = "tcmb")]
TurkeyRepublicCentralBank = 11,

[EnumMember(Value = "nbp")]
NationalBankOfPoland = 12,

[EnumMember(Value = "bbr")]
CentralBankOfBrazil = 13,
}


/// <summary>
/// Help: Purchase Order Modification used when you want to purchase order editable after confirm
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum PurchaseOrderModificationResCompanyOdooEnum
{
[EnumMember(Value = "edit")]
AllowToEditPurchaseOrders = 1,

[EnumMember(Value = "lock")]
ConfirmedPurchaseOrdersAreNotEditable = 2,
}


/// <summary>
/// Help: Provide a double validation mechanism for purchases
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum LevelsOfApprovalsResCompanyOdooEnum
{
[EnumMember(Value = "one_step")]
ConfirmPurchaseOrdersInOneStep = 1,

[EnumMember(Value = "two_step")]
Get2LevelsOfApprovalsToConfirmAPurchaseOrder = 2,
}


/// <summary>
/// Help: Periodicity
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum DelayUnitsResCompanyOdooEnum
{
[EnumMember(Value = "year")]
Annually = 1,

[EnumMember(Value = "semester")]
SemiAnnually = 2,

[EnumMember(Value = "4_months")]
Every4Months = 3,

[EnumMember(Value = "trimester")]
Quarterly = 4,

[EnumMember(Value = "2_months")]
Every2Months = 5,

[EnumMember(Value = "monthly")]
Monthly = 6,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheSaleOnboardingPanelResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,

[EnumMember(Value = "closed")]
Closed = 4,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingConfirmationOrderStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingSampleQuotationStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum SaleOnboardingSelectedPaymentMethodResCompanyOdooEnum
{
[EnumMember(Value = "digital_signature")]
SignOnline = 1,

[EnumMember(Value = "paypal")]
Paypal = 2,

[EnumMember(Value = "stripe")]
Stripe = 3,

[EnumMember(Value = "other")]
PayWithAnotherPaymentProvider = 4,

[EnumMember(Value = "manual")]
ManualPayment = 5,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheAccountConsolidationDashboardOnboardingPanelResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,

[EnumMember(Value = "closed")]
Closed = 4,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingConsolidationStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingConsolidatedChartOfAccountStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum StateOfTheOnboardingCreatePeriodStepResCompanyOdooEnum
{
[EnumMember(Value = "not_done")]
NotDone = 1,

[EnumMember(Value = "just_done")]
JustDone = 2,

[EnumMember(Value = "done")]
Done = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum SendModeOnExpenseAttachmentsResCompanyOdooEnum
{
[EnumMember(Value = "no_send")]
DoNotDigitize = 1,

[EnumMember(Value = "manual_send")]
DigitizeOnDemandOnly = 2,

[EnumMember(Value = "auto_send")]
DigitizeAutomatically = 3,
}
#pragma warning restore S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes