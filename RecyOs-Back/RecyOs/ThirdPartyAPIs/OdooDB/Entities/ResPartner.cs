using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;
#pragma warning disable S2344 // Enumeration type names should not have "Flags" or "Enum" suffixes
#pragma warning disable S3903 // Types should be defined in named namespaces
[OdooTableName("res.partner")]
[JsonConverter(typeof(OdooModelConverter))]
public class ResPartnerOdooModel : IOdooModel
{

/// <summary>
/// is_seo_optimized - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("is_seo_optimized")]
public bool? IsSeoOptimized { get; set; }  

/// <summary>
/// website_meta_title - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("website_meta_title")]
public string WebsiteMetaTitle { get; set; }

/// <summary>
/// website_meta_description - text  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("website_meta_description")]
public string WebsiteMetaDescription { get; set; }

/// <summary>
/// website_meta_keywords - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("website_meta_keywords")]
public string WebsiteMetaKeywords { get; set; }

/// <summary>
/// website_meta_og_img - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("website_meta_og_img")]
public string WebsiteMetaOgImg { get; set; }

/// <summary>
/// seo_name - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("seo_name")]
public string SeoName { get; set; }

/// <summary>
/// website_id - many2one - website <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Restrict publishing to this website. <br />
/// </summary>
[JsonProperty("website_id")]
public long? WebsiteId { get; set; }

/// <summary>
/// website_published - boolean  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("website_published")]
public bool? WebsitePublished { get; set; }

/// <summary>
/// is_published - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("is_published")]
public bool? IsPublished { get; set; }

/// <summary>
/// can_publish - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("can_publish")]
public bool? CanPublish { get; set; }

/// <summary>
/// website_url - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: The full URL to access the document through the website. <br />
/// </summary>
[JsonProperty("website_url")]
public string WebsiteUrl { get; set; }

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
/// email_normalized - char  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: This field is used to search on email address as the primary email field can contain more than strictly an email address. <br />
/// </summary>
[JsonProperty("email_normalized")]
public string EmailNormalized { get; set; }

/// <summary>
/// is_blacklisted - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: If the email address is on the blacklist, the contact won't receive mass mailing anymore, from any list <br />
/// </summary>
[JsonProperty("is_blacklisted")]
public bool? IsBlacklisted { get; set; }

/// <summary>
/// message_bounce - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Counter of the number of bounced emails for this contact <br />
/// </summary>
[JsonProperty("message_bounce")]
public int? MessageBounce { get; set; }

/// <summary>
/// activity_ids - one2many - mail.activity (res_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("activity_ids")]
public long[] ActivityIds { get; set; }

/// <summary>
/// activity_state - selection  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Status based on activities; Overdue: Due date is already passed; Today: Activity date is today; Planned: Future activities. <br />
/// </summary>
[JsonProperty("activity_state")]
public ActivityStateResPartnerOdooEnum? ActivityState { get; set; }

/// <summary>
/// activity_user_id - many2one - res.users <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("activity_user_id")]
public long? ActivityUserId { get; set; }

/// <summary>
/// activity_type_id - many2one - mail.activity.type <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("activity_type_id")]
public long? ActivityTypeId { get; set; }

/// <summary>
/// activity_type_icon - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Font awesome icon e.g. fa-tasks <br />
/// </summary>
[JsonProperty("activity_type_icon")]
public string ActivityTypeIcon { get; set; }

/// <summary>
/// activity_date_deadline - date  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("activity_date_deadline")]
public DateTime? ActivityDateDeadline { get; set; }

/// <summary>
/// my_activity_date_deadline - date  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("my_activity_date_deadline")]
public DateTime? MyActivityDateDeadline { get; set; }

/// <summary>
/// activity_summary - char  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("activity_summary")]
public string ActivitySummary { get; set; }

/// <summary>
/// activity_exception_decoration - selection  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Type of the exception activity on record. <br />
/// </summary>
[JsonProperty("activity_exception_decoration")]
public ActivityExceptionDecorationResPartnerOdooEnum? ActivityExceptionDecoration { get; set; }

/// <summary>
/// activity_exception_icon - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Icon to indicate an exception activity. <br />
/// </summary>
[JsonProperty("activity_exception_icon")]
public string ActivityExceptionIcon { get; set; }

/// <summary>
/// image_1920 - binary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("image_1920")]
public string Image1920 { get; set; }

/// <summary>
/// image_1024 - binary  <br />
/// Required: False, Readonly: True, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("image_1024")]
public string Image1024 { get; set; }

/// <summary>
/// image_512 - binary  <br />
/// Required: False, Readonly: True, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("image_512")]
public string Image512 { get; set; }

/// <summary>
/// image_256 - binary  <br />
/// Required: False, Readonly: True, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("image_256")]
public string Image256 { get; set; }

/// <summary>
/// image_128 - binary  <br />
/// Required: False, Readonly: True, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("image_128")]
public string Image128 { get; set; }

/// <summary>
/// avatar_1920 - binary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("avatar_1920")]
public string Avatar1920 { get; set; }

/// <summary>
/// avatar_1024 - binary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("avatar_1024")]
public string Avatar1024 { get; set; }

/// <summary>
/// avatar_512 - binary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("avatar_512")]
public string Avatar512 { get; set; }

/// <summary>
/// avatar_256 - binary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("avatar_256")]
public string Avatar256 { get; set; }

/// <summary>
/// avatar_128 - binary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("avatar_128")]
public string Avatar128 { get; set; }

/// <summary>
/// name - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("name")]
public string Name { get; set; }

/// <summary>
/// display_name - char  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("display_name")]
public string DisplayName { get; set; }

/// <summary>
/// date - date  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("date")]
public DateTime? Date { get; set; }

/// <summary>
/// title - many2one - res.partner.title <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("title")]
public long? Title { get; set; }

/// <summary>
/// parent_id - many2one - res.partner <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("parent_id")]
public long? ParentId { get; set; }

/// <summary>
/// parent_name - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("parent_name")]
public string ParentName { get; set; }

/// <summary>
/// child_ids - one2many - res.partner (parent_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("child_ids")]
public long[] ChildIds { get; set; }

/// <summary>
/// ref - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("ref")]
public string Ref { get; set; }

/// <summary>
/// lang - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: All the emails and documents sent to this contact will be translated in this language. <br />
/// </summary>
[JsonProperty("lang")]
public LanguageResPartnerOdooEnum? Lang { get; set; }

/// <summary>
/// active_lang_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("active_lang_count")]
public int? ActiveLangCount { get; set; }

/// <summary>
/// tz - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: When printing documents and exporting/importing data, time values are computed according to this timezone.; If the timezone is not set, UTC (Coordinated Universal Time) is used.; Anywhere else, time values are computed according to the time offset of your web client. <br />
/// </summary>
[JsonProperty("tz")]
public TimezoneResPartnerOdooEnum? Tz { get; set; }

/// <summary>
/// tz_offset - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("tz_offset")]
public string TzOffset { get; set; }

/// <summary>
/// user_id - many2one - res.users <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: The internal user in charge of this contact. <br />
/// </summary>
[JsonProperty("user_id")]
public long? UserId { get; set; }

/// <summary>
/// vat - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: The Tax Identification Number. Values here will be validated based on the country format. You can use '/' to indicate that the partner is not subject to tax. <br />
/// </summary>
[JsonProperty("vat")]
public string Vat { get; set; }

/// <summary>
/// same_vat_partner_id - many2one - res.partner <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("same_vat_partner_id")]
public long? SameVatPartnerId { get; set; }

/// <summary>
/// same_company_registry_partner_id - many2one - res.partner <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("same_company_registry_partner_id")]
public long? SameCompanyRegistryPartnerId { get; set; }

/// <summary>
/// company_registry - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: The registry number of the company. Use it if it is different from the Tax ID. It must be unique across all partners of a same country <br />
/// </summary>
[JsonProperty("company_registry")]
public string CompanyRegistry { get; set; }

/// <summary>
/// bank_ids - one2many - res.partner.bank (partner_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("bank_ids")]
public long[] BankIds { get; set; }

/// <summary>
/// website - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("website")]
public string Website { get; set; }

/// <summary>
/// comment - html  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("comment")]
public string Comment { get; set; }

/// <summary>
/// category_id - many2many - res.partner.category <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("category_id")]
public long[] CategoryId { get; set; }

/// <summary>
/// active - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("active")]
public bool? Active { get; set; }

/// <summary>
/// employee - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Check this box if this contact is an Employee. <br />
/// </summary>
[JsonProperty("employee")]
public bool? Employee { get; set; }

/// <summary>
/// function - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("function")]
public string Function { get; set; }

/// <summary>
/// type - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: - Contact: Use this to organize the contact details of employees of a given company (e.g. CEO, CFO, ...).; - Invoice Address : Preferred address for all invoices. Selected by default when you invoice an order that belongs to this company.; - Delivery Address : Preferred address for all deliveries. Selected by default when you deliver an order that belongs to this company.; - Private: Private addresses are only visible by authorized users and contain sensitive data (employee home addresses, ...).; - Other: Other address for the company (e.g. subsidiary, ...) <br />
/// </summary>
[JsonProperty("type")]
public AddressTypeResPartnerOdooEnum? Type { get; set; }

/// <summary>
/// street - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("street")]
public string Street { get; set; }

/// <summary>
/// street2 - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("street2")]
public string Street2 { get; set; }

/// <summary>
/// zip - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("zip")]
public string Zip { get; set; }

/// <summary>
/// city - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("city")]
public string City { get; set; }

/// <summary>
/// state_id - many2one - res.country.state <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("state_id")]
public long? StateId { get; set; }

/// <summary>
/// country_id - many2one - res.country <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("country_id")]
public long? CountryId { get; set; }

/// <summary>
/// country_code - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: The ISO country code in two chars. ; You can use this field for quick search. <br />
/// </summary>
[JsonProperty("country_code")]
public string CountryCode { get; set; }

/// <summary>
/// partner_latitude - float  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("partner_latitude")]
public double? PartnerLatitude { get; set; }

/// <summary>
/// partner_longitude - float  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("partner_longitude")]
public double? PartnerLongitude { get; set; }

/// <summary>
/// email - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("email")]
public string Email { get; set; }

/// <summary>
/// email_formatted - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Format email address "Name" <br />
/// </summary>
[JsonProperty("email_formatted")]
public string EmailFormatted { get; set; }

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
/// is_company - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Check if the contact is a company, otherwise it is a person <br />
/// </summary>
[JsonProperty("is_company")]
public bool? IsCompany { get; set; }

/// <summary>
/// is_public - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("is_public")]
public bool? IsPublic { get; set; }

/// <summary>
/// industry_id - many2one - res.partner.industry <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("industry_id")]
public long? IndustryId { get; set; }

/// <summary>
/// company_type - selection  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("company_type")]
public CompanyTypeResPartnerOdooEnum? CompanyType { get; set; }

/// <summary>
/// company_id - many2one - res.company <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("company_id")]
public long? CompanyId { get; set; }

/// <summary>
/// color - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("color")]
public int? Color { get; set; }

/// <summary>
/// user_ids - one2many - res.users (partner_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("user_ids")]
public long[] UserIds { get; set; }

/// <summary>
/// partner_share - boolean  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: Either customer (not a user), either shared user. Indicated the current partner is a customer without access or with a limited access created for sharing data. <br />
/// </summary>
[JsonProperty("partner_share")]
public bool? PartnerShare { get; set; }

/// <summary>
/// contact_address - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("contact_address")]
public string ContactAddress { get; set; }

/// <summary>
/// commercial_partner_id - many2one - res.partner <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("commercial_partner_id")]
public long? CommercialPartnerId { get; set; }

/// <summary>
/// commercial_company_name - char  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("commercial_company_name")]
public string CommercialCompanyName { get; set; }

/// <summary>
/// company_name - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("company_name")]
public string CompanyName { get; set; }

/// <summary>
/// barcode - char  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: Use a barcode to identify this contact. <br />
/// </summary>
[JsonProperty("barcode")]
public string Barcode { get; set; }

/// <summary>
/// self - many2one - res.partner <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("self")]
public long? Self { get; set; }

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
/// im_status - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("im_status")]
public string ImStatus { get; set; }

/// <summary>
/// channel_ids - many2many - mail.channel <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("channel_ids")]
public long[] ChannelIds { get; set; }

/// <summary>
/// contact_address_complete - char  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("contact_address_complete")]
public string ContactAddressComplete { get; set; }

/// <summary>
/// image_medium - binary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("image_medium")]
public string ImageMedium { get; set; }

/// <summary>
/// signup_token - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("signup_token")]
public string SignupToken { get; set; }

/// <summary>
/// signup_type - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("signup_type")]
public string SignupType { get; set; }

/// <summary>
/// signup_expiration - datetime  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("signup_expiration")]
public DateTime? SignupExpiration { get; set; }

/// <summary>
/// signup_valid - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("signup_valid")]
public bool? SignupValid { get; set; }

/// <summary>
/// signup_url - char  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("signup_url")]
public string SignupUrl { get; set; }

/// <summary>
/// plan_to_change_car - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("plan_to_change_car")]
public bool? PlanToChangeCar { get; set; }

/// <summary>
/// plan_to_change_bike - boolean  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("plan_to_change_bike")]
public bool? PlanToChangeBike { get; set; }

/// <summary>
/// employee_ids - one2many - hr.employee (address_home_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// Help: Related employees based on their private address <br />
/// </summary>
[JsonProperty("employee_ids")]
public long[] EmployeeIds { get; set; }

/// <summary>
/// employees_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("employees_count")]
public int? EmployeesCount { get; set; }

/// <summary>
/// property_product_pricelist - many2one - product.pricelist <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: This pricelist will be used, instead of the default one, for sales to the current partner <br />
/// </summary>
[JsonProperty("property_product_pricelist")]
public long? PropertyProductPricelist { get; set; }

/// <summary>
/// team_id - many2one - crm.team <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: If set, this Sales Team will be used for sales and assignments related to this partner <br />
/// </summary>
[JsonProperty("team_id")]
public long? TeamId { get; set; }

/// <summary>
/// ocn_token - char  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: Used for sending notification to registered devices <br />
/// </summary>
[JsonProperty("ocn_token")]
public string OcnToken { get; set; }

/// <summary>
/// partner_gid - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("partner_gid")]
public int? PartnerGid { get; set; }

/// <summary>
/// additional_info - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("additional_info")]
public string AdditionalInfo { get; set; }

/// <summary>
/// phone_sanitized - char  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: Field used to store sanitized phone number. Helps speeding up searches and comparisons. <br />
/// </summary>
[JsonProperty("phone_sanitized")]
public string PhoneSanitized { get; set; }

/// <summary>
/// phone_sanitized_blacklisted - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: If the sanitized phone number is on the blacklist, the contact won't receive mass mailing sms anymore, from any list <br />
/// </summary>
[JsonProperty("phone_sanitized_blacklisted")]
public bool? PhoneSanitizedBlacklisted { get; set; }

/// <summary>
/// phone_blacklisted - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Indicates if a blacklisted sanitized phone number is a phone number. Helps distinguish which number is blacklisted             when there is both a mobile and phone field in a model. <br />
/// </summary>
[JsonProperty("phone_blacklisted")]
public bool? PhoneBlacklisted { get; set; }

/// <summary>
/// mobile_blacklisted - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Indicates if a blacklisted sanitized phone number is a mobile number. Helps distinguish which number is blacklisted             when there is both a mobile and phone field in a model. <br />
/// </summary>
[JsonProperty("mobile_blacklisted")]
public bool? MobileBlacklisted { get; set; }

/// <summary>
/// phone_mobile_search - char  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("phone_mobile_search")]
public string PhoneMobileSearch { get; set; }

/// <summary>
/// certifications_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("certifications_count")]
public int? CertificationsCount { get; set; }

/// <summary>
/// certifications_company_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("certifications_company_count")]
public int? CertificationsCompanyCount { get; set; }

/// <summary>
/// payment_token_ids - one2many - payment.token (partner_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("payment_token_ids")]
public long[] PaymentTokenIds { get; set; }

/// <summary>
/// payment_token_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("payment_token_count")]
public int? PaymentTokenCount { get; set; }

/// <summary>
/// credit - monetary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Total amount this customer owes you. <br />
/// </summary>
[JsonProperty("credit")]
public decimal? Credit { get; set; }

/// <summary>
/// credit_limit - float  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: Credit limit specific to this partner. <br />
/// </summary>
[JsonProperty("credit_limit")]
public double? CreditLimit { get; set; }

/// <summary>
/// use_partner_credit_limit - boolean  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("use_partner_credit_limit")]
public bool? UsePartnerCreditLimit { get; set; }

/// <summary>
/// show_credit_limit - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("show_credit_limit")]
public bool? ShowCreditLimit { get; set; }

/// <summary>
/// debit - monetary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: Total amount you have to pay to this vendor. <br />
/// </summary>
[JsonProperty("debit")]
public decimal? Debit { get; set; }

/// <summary>
/// debit_limit - monetary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("debit_limit")]
public decimal? DebitLimit { get; set; }

/// <summary>
/// total_invoiced - monetary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("total_invoiced")]
public decimal? TotalInvoiced { get; set; }

/// <summary>
/// currency_id - many2one - res.currency <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("currency_id")]
public long? CurrencyId { get; set; }

/// <summary>
/// journal_item_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("journal_item_count")]
public int? JournalItemCount { get; set; }

/// <summary>
/// property_account_payable_id - many2one - account.account <br />
/// Required: True, Readonly: False, Store: False, Sortable: False <br />
/// Help: This account will be used instead of the default one as the payable account for the current partner <br />
/// </summary>
[JsonProperty("property_account_payable_id")]
public long PropertyAccountPayableId { get; set; }

/// <summary>
/// property_account_receivable_id - many2one - account.account <br />
/// Required: True, Readonly: False, Store: False, Sortable: False <br />
/// Help: This account will be used instead of the default one as the receivable account for the current partner <br />
/// </summary>
[JsonProperty("property_account_receivable_id")]
public long PropertyAccountReceivableId { get; set; }

/// <summary>
/// property_account_position_id - many2one - account.fiscal.position <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: The fiscal position determines the taxes/accounts used for this contact. <br />
/// </summary>
[JsonProperty("property_account_position_id")]
public long? PropertyAccountPositionId { get; set; }

/// <summary>
/// property_payment_term_id - many2one - account.payment.term <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: This payment term will be used instead of the default one for sales orders and customer invoices <br />
/// </summary>
[JsonProperty("property_payment_term_id")]
public long? PropertyPaymentTermId { get; set; }

/// <summary>
/// property_supplier_payment_term_id - many2one - account.payment.term <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: This payment term will be used instead of the default one for purchase orders and vendor bills <br />
/// </summary>
[JsonProperty("property_supplier_payment_term_id")]
public long? PropertySupplierPaymentTermId { get; set; }

/// <summary>
/// ref_company_ids - one2many - res.company (partner_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("ref_company_ids")]
public long[] RefCompanyIds { get; set; }

/// <summary>
/// has_unreconciled_entries - boolean  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// Help: The partner has at least one unreconciled debit and credit since last time the invoices payments matching was performed. <br />
/// </summary>
[JsonProperty("has_unreconciled_entries")]
public bool? HasUnreconciledEntries { get; set; }

/// <summary>
/// last_time_entries_checked - datetime  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// Help: Last time the invoices payments matching was performed for this partner. It is set either if there's not at least an unreconciled debit and an unreconciled credit or if you click the "Done" button. <br />
/// </summary>
[JsonProperty("last_time_entries_checked")]
public DateTime? LastTimeEntriesChecked { get; set; }

/// <summary>
/// invoice_ids - one2many - account.move (partner_id) <br />
/// Required: False, Readonly: True, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("invoice_ids")]
public long[] InvoiceIds { get; set; }

/// <summary>
/// contract_ids - one2many - account.analytic.account (partner_id) <br />
/// Required: False, Readonly: True, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("contract_ids")]
public long[] ContractIds { get; set; }

/// <summary>
/// bank_account_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("bank_account_count")]
public int? BankAccountCount { get; set; }

/// <summary>
/// trust - selection  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("trust")]
public DegreeOfTrustYouHaveInThisDebtorResPartnerOdooEnum? Trust { get; set; }

/// <summary>
/// invoice_warn - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Selecting the "Warning" option will notify user with the message, Selecting "Blocking Message" will throw an exception with the message and block the flow. The Message has to be written in the next field. <br />
/// </summary>
[JsonProperty("invoice_warn")]
public InvoiceResPartnerOdooEnum? InvoiceWarn { get; set; }

/// <summary>
/// invoice_warn_msg - text  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("invoice_warn_msg")]
public string InvoiceWarnMsg { get; set; }

/// <summary>
/// supplier_rank - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("supplier_rank")]
public int? SupplierRank { get; set; }

/// <summary>
/// customer_rank - integer  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("customer_rank")]
public int? CustomerRank { get; set; }

/// <summary>
/// duplicated_bank_account_partners_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("duplicated_bank_account_partners_count")]
public int? DuplicatedBankAccountPartnersCount { get; set; }

/// <summary>
/// document_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("document_count")]
public int? DocumentCount { get; set; }

/// <summary>
/// visitor_ids - one2many - website.visitor (partner_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("visitor_ids")]
public long[] VisitorIds { get; set; }

/// <summary>
/// property_payment_method_id - many2one - account.payment.method <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: Preferred payment method when paying this vendor. This is used to filter vendor bills by preferred payment method to register payments in mass. Use cases: create bank files for batch wires, check runs. <br />
/// </summary>
[JsonProperty("property_payment_method_id")]
public long? PropertyPaymentMethodId { get; set; }

/// <summary>
/// vies_failed_message - char  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("vies_failed_message")]
public string ViesFailedMessage { get; set; }

/// <summary>
/// property_purchase_currency_id - many2one - res.currency <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: This currency will be used, instead of the default one, for purchases from the current partner <br />
/// </summary>
[JsonProperty("property_purchase_currency_id")]
public long? PropertyPurchaseCurrencyId { get; set; }

/// <summary>
/// purchase_order_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("purchase_order_count")]
public int? PurchaseOrderCount { get; set; }

/// <summary>
/// supplier_invoice_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("supplier_invoice_count")]
public int? SupplierInvoiceCount { get; set; }

/// <summary>
/// purchase_warn - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Selecting the "Warning" option will notify user with the message, Selecting "Blocking Message" will throw an exception with the message and block the flow. The Message has to be written in the next field. <br />
/// </summary>
[JsonProperty("purchase_warn")]
public PurchaseOrderResPartnerOdooEnum? PurchaseWarn { get; set; }

/// <summary>
/// purchase_warn_msg - text  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("purchase_warn_msg")]
public string PurchaseWarnMsg { get; set; }

/// <summary>
/// receipt_reminder_email - boolean  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: Automatically send a confirmation email to the vendor X days before the expected receipt date, asking him to confirm the exact date. <br />
/// </summary>
[JsonProperty("receipt_reminder_email")]
public bool? ReceiptReminderEmail { get; set; }

/// <summary>
/// reminder_date_before_receipt - integer  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: Number of days to send reminder email before the promised receipt date <br />
/// </summary>
[JsonProperty("reminder_date_before_receipt")]
public int? ReminderDateBeforeReceipt { get; set; }

/// <summary>
/// website_description - html  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("website_description")]
public string WebsiteDescription { get; set; }

/// <summary>
/// website_short_description - text  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("website_short_description")]
public string WebsiteShortDescription { get; set; }

/// <summary>
/// online_partner_information - char  <br />
/// Required: False, Readonly: True, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("online_partner_information")]
public string OnlinePartnerInformation { get; set; }

/// <summary>
/// account_represented_company_ids - one2many - res.company (account_representative_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("account_represented_company_ids")]
public long[] AccountRepresentedCompanyIds { get; set; }

/// <summary>
/// siret - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("siret")]
public string Siret { get; set; }

/// <summary>
/// sale_order_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("sale_order_count")]
public int? SaleOrderCount { get; set; }

/// <summary>
/// sale_order_ids - one2many - sale.order (partner_id) <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("sale_order_ids")]
public long[] SaleOrderIds { get; set; }

/// <summary>
/// sale_warn - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// Help: Selecting the "Warning" option will notify user with the message, Selecting "Blocking Message" will throw an exception with the message and block the flow. The Message has to be written in the next field. <br />
/// </summary>
[JsonProperty("sale_warn")]
public SalesWarningsResPartnerOdooEnum? SaleWarn { get; set; }

/// <summary>
/// sale_warn_msg - text  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("sale_warn_msg")]
public string SaleWarnMsg { get; set; }

/// <summary>
/// followup_next_action_date - date  <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: The date before which no follow-up action should be taken. <br />
/// </summary>
[JsonProperty("followup_next_action_date")]
public DateTime? FollowupNextActionDate { get; set; }

/// <summary>
/// unreconciled_aml_ids - one2many - account.move.line <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("unreconciled_aml_ids")]
public long[] UnreconciledAmlIds { get; set; }

/// <summary>
/// unpaid_invoice_ids - one2many - account.move <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("unpaid_invoice_ids")]
public long[] UnpaidInvoiceIds { get; set; }

/// <summary>
/// unpaid_invoices_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("unpaid_invoices_count")]
public int? UnpaidInvoicesCount { get; set; }

/// <summary>
/// total_due - monetary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("total_due")]
public decimal? TotalDue { get; set; }

/// <summary>
/// total_overdue - monetary  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("total_overdue")]
public decimal? TotalOverdue { get; set; }

/// <summary>
/// followup_status - selection  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("followup_status")]
public FollowUpStatusResPartnerOdooEnum? FollowupStatus { get; set; }

/// <summary>
/// followup_line_id - many2one - account_followup.followup.line <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("followup_line_id")]
public long? FollowupLineId { get; set; }

/// <summary>
/// followup_reminder_type - selection  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("followup_reminder_type")]
public RemindersResPartnerOdooEnum? FollowupReminderType { get; set; }

/// <summary>
/// followup_responsible_id - many2one - res.users <br />
/// Required: False, Readonly: False, Store: False, Sortable: False <br />
/// Help: Optionally you can assign a user to this field, which will make him responsible for the activities. If empty, we will find someone responsible. <br />
/// </summary>
[JsonProperty("followup_responsible_id")]
public long? FollowupResponsibleId { get; set; }

/// <summary>
/// slide_channel_ids - many2many - slide.channel <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("slide_channel_ids")]
public long[] SlideChannelIds { get; set; }

/// <summary>
/// slide_channel_completed_ids - one2many - slide.channel <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("slide_channel_completed_ids")]
public long[] SlideChannelCompletedIds { get; set; }

/// <summary>
/// slide_channel_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("slide_channel_count")]
public int? SlideChannelCount { get; set; }

/// <summary>
/// slide_channel_company_count - integer  <br />
/// Required: False, Readonly: True, Store: False, Sortable: False <br />
/// </summary>
[JsonProperty("slide_channel_company_count")]
public int? SlideChannelCompanyCount { get; set; }

/// <summary>
/// x_studio_origine - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("x_studio_origine")]
public string XStudioOrigine { get; set; }

/// <summary>
/// x_studio_char_field_VPxcW - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("x_studio_char_field_VPxcW")]
public string XStudioCharFieldVpxcw { get; set; }

/// <summary>
/// x_studio_binary_field_rgBMG - binary  <br />
/// Required: False, Readonly: False, Store: True, Sortable: False <br />
/// </summary>
[JsonProperty("x_studio_binary_field_rgBMG")]
public string XStudioBinaryFieldRgbmg { get; set; }

/// <summary>
/// x_studio_datetime_field_8DNxU - datetime  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("x_studio_datetime_field_8DNxU")]
public DateTime? XStudioDatetimeField8Dnxu { get; set; }

/// <summary>
/// x_studio_code_tiers_mkgt - char  <br />
/// Required: False, Readonly: False, Store: True, Sortable: True <br />
/// </summary>
[JsonProperty("x_studio_code_tiers_mkgt")]
public string XStudioCodeTiersMkgt { get; set; }
}


/// <summary>
/// Help: Status based on activities <br />
 /// Overdue: Due date is already passed <br />
 /// Today: Activity date is today <br />
 /// Planned: Future activities.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum ActivityStateResPartnerOdooEnum
{
[EnumMember(Value = "overdue")]
Overdue = 1,

[EnumMember(Value = "today")]
Today = 2,

[EnumMember(Value = "planned")]
Planned = 3,
}


/// <summary>
/// Help: Type of the exception activity on record.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum ActivityExceptionDecorationResPartnerOdooEnum
{
[EnumMember(Value = "warning")]
Alert = 1,

[EnumMember(Value = "danger")]
Error = 2,
}


/// <summary>
/// Help: All the emails and documents sent to this contact will be translated in this language.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum LanguageResPartnerOdooEnum
{
[EnumMember(Value = "en_US")]
EnglishUS = 1,

[EnumMember(Value = "fr_FR")]
FrenchFranAis = 2,
}


/// <summary>
/// Help: When printing documents and exporting/importing data, time values are computed according to this timezone. <br />
 /// If the timezone is not set, UTC (Coordinated Universal Time) is used. <br />
 /// Anywhere else, time values are computed according to the time offset of your web client.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TimezoneResPartnerOdooEnum
{
[EnumMember(Value = "Africa/Abidjan")]
AfricaAbidjan = 1,

[EnumMember(Value = "Africa/Accra")]
AfricaAccra = 2,

[EnumMember(Value = "Africa/Addis_Ababa")]
AfricaAddisAbaba = 3,

[EnumMember(Value = "Africa/Algiers")]
AfricaAlgiers = 4,

[EnumMember(Value = "Africa/Asmara")]
AfricaAsmara = 5,

[EnumMember(Value = "Africa/Asmera")]
AfricaAsmera = 6,

[EnumMember(Value = "Africa/Bamako")]
AfricaBamako = 7,

[EnumMember(Value = "Africa/Bangui")]
AfricaBangui = 8,

[EnumMember(Value = "Africa/Banjul")]
AfricaBanjul = 9,

[EnumMember(Value = "Africa/Bissau")]
AfricaBissau = 10,

[EnumMember(Value = "Africa/Blantyre")]
AfricaBlantyre = 11,

[EnumMember(Value = "Africa/Brazzaville")]
AfricaBrazzaville = 12,

[EnumMember(Value = "Africa/Bujumbura")]
AfricaBujumbura = 13,

[EnumMember(Value = "Africa/Cairo")]
AfricaCairo = 14,

[EnumMember(Value = "Africa/Casablanca")]
AfricaCasablanca = 15,

[EnumMember(Value = "Africa/Ceuta")]
AfricaCeuta = 16,

[EnumMember(Value = "Africa/Conakry")]
AfricaConakry = 17,

[EnumMember(Value = "Africa/Dakar")]
AfricaDakar = 18,

[EnumMember(Value = "Africa/Dar_es_Salaam")]
AfricaDarEsSalaam = 19,

[EnumMember(Value = "Africa/Djibouti")]
AfricaDjibouti = 20,

[EnumMember(Value = "Africa/Douala")]
AfricaDouala = 21,

[EnumMember(Value = "Africa/El_Aaiun")]
AfricaElAaiun = 22,

[EnumMember(Value = "Africa/Freetown")]
AfricaFreetown = 23,

[EnumMember(Value = "Africa/Gaborone")]
AfricaGaborone = 24,

[EnumMember(Value = "Africa/Harare")]
AfricaHarare = 25,

[EnumMember(Value = "Africa/Johannesburg")]
AfricaJohannesburg = 26,

[EnumMember(Value = "Africa/Juba")]
AfricaJuba = 27,

[EnumMember(Value = "Africa/Kampala")]
AfricaKampala = 28,

[EnumMember(Value = "Africa/Khartoum")]
AfricaKhartoum = 29,

[EnumMember(Value = "Africa/Kigali")]
AfricaKigali = 30,

[EnumMember(Value = "Africa/Kinshasa")]
AfricaKinshasa = 31,

[EnumMember(Value = "Africa/Lagos")]
AfricaLagos = 32,

[EnumMember(Value = "Africa/Libreville")]
AfricaLibreville = 33,

[EnumMember(Value = "Africa/Lome")]
AfricaLome = 34,

[EnumMember(Value = "Africa/Luanda")]
AfricaLuanda = 35,

[EnumMember(Value = "Africa/Lubumbashi")]
AfricaLubumbashi = 36,

[EnumMember(Value = "Africa/Lusaka")]
AfricaLusaka = 37,

[EnumMember(Value = "Africa/Malabo")]
AfricaMalabo = 38,

[EnumMember(Value = "Africa/Maputo")]
AfricaMaputo = 39,

[EnumMember(Value = "Africa/Maseru")]
AfricaMaseru = 40,

[EnumMember(Value = "Africa/Mbabane")]
AfricaMbabane = 41,

[EnumMember(Value = "Africa/Mogadishu")]
AfricaMogadishu = 42,

[EnumMember(Value = "Africa/Monrovia")]
AfricaMonrovia = 43,

[EnumMember(Value = "Africa/Nairobi")]
AfricaNairobi = 44,

[EnumMember(Value = "Africa/Ndjamena")]
AfricaNdjamena = 45,

[EnumMember(Value = "Africa/Niamey")]
AfricaNiamey = 46,

[EnumMember(Value = "Africa/Nouakchott")]
AfricaNouakchott = 47,

[EnumMember(Value = "Africa/Ouagadougou")]
AfricaOuagadougou = 48,

[EnumMember(Value = "Africa/Porto-Novo")]
AfricaPortoNovo = 49,

[EnumMember(Value = "Africa/Sao_Tome")]
AfricaSaoTome = 50,

[EnumMember(Value = "Africa/Timbuktu")]
AfricaTimbuktu = 51,

[EnumMember(Value = "Africa/Tripoli")]
AfricaTripoli = 52,

[EnumMember(Value = "Africa/Tunis")]
AfricaTunis = 53,

[EnumMember(Value = "Africa/Windhoek")]
AfricaWindhoek = 54,

[EnumMember(Value = "America/Adak")]
AmericaAdak = 55,

[EnumMember(Value = "America/Anchorage")]
AmericaAnchorage = 56,

[EnumMember(Value = "America/Anguilla")]
AmericaAnguilla = 57,

[EnumMember(Value = "America/Antigua")]
AmericaAntigua = 58,

[EnumMember(Value = "America/Araguaina")]
AmericaAraguaina = 59,

[EnumMember(Value = "America/Argentina/Buenos_Aires")]
AmericaArgentinaBuenosAires = 60,

[EnumMember(Value = "America/Argentina/Catamarca")]
AmericaArgentinaCatamarca = 61,

[EnumMember(Value = "America/Argentina/ComodRivadavia")]
AmericaArgentinaComodrivadavia = 62,

[EnumMember(Value = "America/Argentina/Cordoba")]
AmericaArgentinaCordoba = 63,

[EnumMember(Value = "America/Argentina/Jujuy")]
AmericaArgentinaJujuy = 64,

[EnumMember(Value = "America/Argentina/La_Rioja")]
AmericaArgentinaLaRioja = 65,

[EnumMember(Value = "America/Argentina/Mendoza")]
AmericaArgentinaMendoza = 66,

[EnumMember(Value = "America/Argentina/Rio_Gallegos")]
AmericaArgentinaRioGallegos = 67,

[EnumMember(Value = "America/Argentina/Salta")]
AmericaArgentinaSalta = 68,

[EnumMember(Value = "America/Argentina/San_Juan")]
AmericaArgentinaSanJuan = 69,

[EnumMember(Value = "America/Argentina/San_Luis")]
AmericaArgentinaSanLuis = 70,

[EnumMember(Value = "America/Argentina/Tucuman")]
AmericaArgentinaTucuman = 71,

[EnumMember(Value = "America/Argentina/Ushuaia")]
AmericaArgentinaUshuaia = 72,

[EnumMember(Value = "America/Aruba")]
AmericaAruba = 73,

[EnumMember(Value = "America/Asuncion")]
AmericaAsuncion = 74,

[EnumMember(Value = "America/Atikokan")]
AmericaAtikokan = 75,

[EnumMember(Value = "America/Atka")]
AmericaAtka = 76,

[EnumMember(Value = "America/Bahia")]
AmericaBahia = 77,

[EnumMember(Value = "America/Bahia_Banderas")]
AmericaBahiaBanderas = 78,

[EnumMember(Value = "America/Barbados")]
AmericaBarbados = 79,

[EnumMember(Value = "America/Belem")]
AmericaBelem = 80,

[EnumMember(Value = "America/Belize")]
AmericaBelize = 81,

[EnumMember(Value = "America/Blanc-Sablon")]
AmericaBlancSablon = 82,

[EnumMember(Value = "America/Boa_Vista")]
AmericaBoaVista = 83,

[EnumMember(Value = "America/Bogota")]
AmericaBogota = 84,

[EnumMember(Value = "America/Boise")]
AmericaBoise = 85,

[EnumMember(Value = "America/Buenos_Aires")]
AmericaBuenosAires = 86,

[EnumMember(Value = "America/Cambridge_Bay")]
AmericaCambridgeBay = 87,

[EnumMember(Value = "America/Campo_Grande")]
AmericaCampoGrande = 88,

[EnumMember(Value = "America/Cancun")]
AmericaCancun = 89,

[EnumMember(Value = "America/Caracas")]
AmericaCaracas = 90,

[EnumMember(Value = "America/Catamarca")]
AmericaCatamarca = 91,

[EnumMember(Value = "America/Cayenne")]
AmericaCayenne = 92,

[EnumMember(Value = "America/Cayman")]
AmericaCayman = 93,

[EnumMember(Value = "America/Chicago")]
AmericaChicago = 94,

[EnumMember(Value = "America/Chihuahua")]
AmericaChihuahua = 95,

[EnumMember(Value = "America/Coral_Harbour")]
AmericaCoralHarbour = 96,

[EnumMember(Value = "America/Cordoba")]
AmericaCordoba = 97,

[EnumMember(Value = "America/Costa_Rica")]
AmericaCostaRica = 98,

[EnumMember(Value = "America/Creston")]
AmericaCreston = 99,

[EnumMember(Value = "America/Cuiaba")]
AmericaCuiaba = 100,

[EnumMember(Value = "America/Curacao")]
AmericaCuracao = 101,

[EnumMember(Value = "America/Danmarkshavn")]
AmericaDanmarkshavn = 102,

[EnumMember(Value = "America/Dawson")]
AmericaDawson = 103,

[EnumMember(Value = "America/Dawson_Creek")]
AmericaDawsonCreek = 104,

[EnumMember(Value = "America/Denver")]
AmericaDenver = 105,

[EnumMember(Value = "America/Detroit")]
AmericaDetroit = 106,

[EnumMember(Value = "America/Dominica")]
AmericaDominica = 107,

[EnumMember(Value = "America/Edmonton")]
AmericaEdmonton = 108,

[EnumMember(Value = "America/Eirunepe")]
AmericaEirunepe = 109,

[EnumMember(Value = "America/El_Salvador")]
AmericaElSalvador = 110,

[EnumMember(Value = "America/Ensenada")]
AmericaEnsenada = 111,

[EnumMember(Value = "America/Fort_Nelson")]
AmericaFortNelson = 112,

[EnumMember(Value = "America/Fort_Wayne")]
AmericaFortWayne = 113,

[EnumMember(Value = "America/Fortaleza")]
AmericaFortaleza = 114,

[EnumMember(Value = "America/Glace_Bay")]
AmericaGlaceBay = 115,

[EnumMember(Value = "America/Godthab")]
AmericaGodthab = 116,

[EnumMember(Value = "America/Goose_Bay")]
AmericaGooseBay = 117,

[EnumMember(Value = "America/Grand_Turk")]
AmericaGrandTurk = 118,

[EnumMember(Value = "America/Grenada")]
AmericaGrenada = 119,

[EnumMember(Value = "America/Guadeloupe")]
AmericaGuadeloupe = 120,

[EnumMember(Value = "America/Guatemala")]
AmericaGuatemala = 121,

[EnumMember(Value = "America/Guayaquil")]
AmericaGuayaquil = 122,

[EnumMember(Value = "America/Guyana")]
AmericaGuyana = 123,

[EnumMember(Value = "America/Halifax")]
AmericaHalifax = 124,

[EnumMember(Value = "America/Havana")]
AmericaHavana = 125,

[EnumMember(Value = "America/Hermosillo")]
AmericaHermosillo = 126,

[EnumMember(Value = "America/Indiana/Indianapolis")]
AmericaIndianaIndianapolis = 127,

[EnumMember(Value = "America/Indiana/Knox")]
AmericaIndianaKnox = 128,

[EnumMember(Value = "America/Indiana/Marengo")]
AmericaIndianaMarengo = 129,

[EnumMember(Value = "America/Indiana/Petersburg")]
AmericaIndianaPetersburg = 130,

[EnumMember(Value = "America/Indiana/Tell_City")]
AmericaIndianaTellCity = 131,

[EnumMember(Value = "America/Indiana/Vevay")]
AmericaIndianaVevay = 132,

[EnumMember(Value = "America/Indiana/Vincennes")]
AmericaIndianaVincennes = 133,

[EnumMember(Value = "America/Indiana/Winamac")]
AmericaIndianaWinamac = 134,

[EnumMember(Value = "America/Indianapolis")]
AmericaIndianapolis = 135,

[EnumMember(Value = "America/Inuvik")]
AmericaInuvik = 136,

[EnumMember(Value = "America/Iqaluit")]
AmericaIqaluit = 137,

[EnumMember(Value = "America/Jamaica")]
AmericaJamaica = 138,

[EnumMember(Value = "America/Jujuy")]
AmericaJujuy = 139,

[EnumMember(Value = "America/Juneau")]
AmericaJuneau = 140,

[EnumMember(Value = "America/Kentucky/Louisville")]
AmericaKentuckyLouisville = 141,

[EnumMember(Value = "America/Kentucky/Monticello")]
AmericaKentuckyMonticello = 142,

[EnumMember(Value = "America/Knox_IN")]
AmericaKnoxIN = 143,

[EnumMember(Value = "America/Kralendijk")]
AmericaKralendijk = 144,

[EnumMember(Value = "America/La_Paz")]
AmericaLaPaz = 145,

[EnumMember(Value = "America/Lima")]
AmericaLima = 146,

[EnumMember(Value = "America/Los_Angeles")]
AmericaLosAngeles = 147,

[EnumMember(Value = "America/Louisville")]
AmericaLouisville = 148,

[EnumMember(Value = "America/Lower_Princes")]
AmericaLowerPrinces = 149,

[EnumMember(Value = "America/Maceio")]
AmericaMaceio = 150,

[EnumMember(Value = "America/Managua")]
AmericaManagua = 151,

[EnumMember(Value = "America/Manaus")]
AmericaManaus = 152,

[EnumMember(Value = "America/Marigot")]
AmericaMarigot = 153,

[EnumMember(Value = "America/Martinique")]
AmericaMartinique = 154,

[EnumMember(Value = "America/Matamoros")]
AmericaMatamoros = 155,

[EnumMember(Value = "America/Mazatlan")]
AmericaMazatlan = 156,

[EnumMember(Value = "America/Mendoza")]
AmericaMendoza = 157,

[EnumMember(Value = "America/Menominee")]
AmericaMenominee = 158,

[EnumMember(Value = "America/Merida")]
AmericaMerida = 159,

[EnumMember(Value = "America/Metlakatla")]
AmericaMetlakatla = 160,

[EnumMember(Value = "America/Mexico_City")]
AmericaMexicoCity = 161,

[EnumMember(Value = "America/Miquelon")]
AmericaMiquelon = 162,

[EnumMember(Value = "America/Moncton")]
AmericaMoncton = 163,

[EnumMember(Value = "America/Monterrey")]
AmericaMonterrey = 164,

[EnumMember(Value = "America/Montevideo")]
AmericaMontevideo = 165,

[EnumMember(Value = "America/Montreal")]
AmericaMontreal = 166,

[EnumMember(Value = "America/Montserrat")]
AmericaMontserrat = 167,

[EnumMember(Value = "America/Nassau")]
AmericaNassau = 168,

[EnumMember(Value = "America/New_York")]
AmericaNewYork = 169,

[EnumMember(Value = "America/Nipigon")]
AmericaNipigon = 170,

[EnumMember(Value = "America/Nome")]
AmericaNome = 171,

[EnumMember(Value = "America/Noronha")]
AmericaNoronha = 172,

[EnumMember(Value = "America/North_Dakota/Beulah")]
AmericaNorthDakotaBeulah = 173,

[EnumMember(Value = "America/North_Dakota/Center")]
AmericaNorthDakotaCenter = 174,

[EnumMember(Value = "America/North_Dakota/New_Salem")]
AmericaNorthDakotaNewSalem = 175,

[EnumMember(Value = "America/Nuuk")]
AmericaNuuk = 176,

[EnumMember(Value = "America/Ojinaga")]
AmericaOjinaga = 177,

[EnumMember(Value = "America/Panama")]
AmericaPanama = 178,

[EnumMember(Value = "America/Pangnirtung")]
AmericaPangnirtung = 179,

[EnumMember(Value = "America/Paramaribo")]
AmericaParamaribo = 180,

[EnumMember(Value = "America/Phoenix")]
AmericaPhoenix = 181,

[EnumMember(Value = "America/Port-au-Prince")]
AmericaPortAuPrince = 182,

[EnumMember(Value = "America/Port_of_Spain")]
AmericaPortOfSpain = 183,

[EnumMember(Value = "America/Porto_Acre")]
AmericaPortoAcre = 184,

[EnumMember(Value = "America/Porto_Velho")]
AmericaPortoVelho = 185,

[EnumMember(Value = "America/Puerto_Rico")]
AmericaPuertoRico = 186,

[EnumMember(Value = "America/Punta_Arenas")]
AmericaPuntaArenas = 187,

[EnumMember(Value = "America/Rainy_River")]
AmericaRainyRiver = 188,

[EnumMember(Value = "America/Rankin_Inlet")]
AmericaRankinInlet = 189,

[EnumMember(Value = "America/Recife")]
AmericaRecife = 190,

[EnumMember(Value = "America/Regina")]
AmericaRegina = 191,

[EnumMember(Value = "America/Resolute")]
AmericaResolute = 192,

[EnumMember(Value = "America/Rio_Branco")]
AmericaRioBranco = 193,

[EnumMember(Value = "America/Rosario")]
AmericaRosario = 194,

[EnumMember(Value = "America/Santa_Isabel")]
AmericaSantaIsabel = 195,

[EnumMember(Value = "America/Santarem")]
AmericaSantarem = 196,

[EnumMember(Value = "America/Santiago")]
AmericaSantiago = 197,

[EnumMember(Value = "America/Santo_Domingo")]
AmericaSantoDomingo = 198,

[EnumMember(Value = "America/Sao_Paulo")]
AmericaSaoPaulo = 199,

[EnumMember(Value = "America/Scoresbysund")]
AmericaScoresbysund = 200,

[EnumMember(Value = "America/Shiprock")]
AmericaShiprock = 201,

[EnumMember(Value = "America/Sitka")]
AmericaSitka = 202,

[EnumMember(Value = "America/St_Barthelemy")]
AmericaStBarthelemy = 203,

[EnumMember(Value = "America/St_Johns")]
AmericaStJohns = 204,

[EnumMember(Value = "America/St_Kitts")]
AmericaStKitts = 205,

[EnumMember(Value = "America/St_Lucia")]
AmericaStLucia = 206,

[EnumMember(Value = "America/St_Thomas")]
AmericaStThomas = 207,

[EnumMember(Value = "America/St_Vincent")]
AmericaStVincent = 208,

[EnumMember(Value = "America/Swift_Current")]
AmericaSwiftCurrent = 209,

[EnumMember(Value = "America/Tegucigalpa")]
AmericaTegucigalpa = 210,

[EnumMember(Value = "America/Thule")]
AmericaThule = 211,

[EnumMember(Value = "America/Thunder_Bay")]
AmericaThunderBay = 212,

[EnumMember(Value = "America/Tijuana")]
AmericaTijuana = 213,

[EnumMember(Value = "America/Toronto")]
AmericaToronto = 214,

[EnumMember(Value = "America/Tortola")]
AmericaTortola = 215,

[EnumMember(Value = "America/Vancouver")]
AmericaVancouver = 216,

[EnumMember(Value = "America/Virgin")]
AmericaVirgin = 217,

[EnumMember(Value = "America/Whitehorse")]
AmericaWhitehorse = 218,

[EnumMember(Value = "America/Winnipeg")]
AmericaWinnipeg = 219,

[EnumMember(Value = "America/Yakutat")]
AmericaYakutat = 220,

[EnumMember(Value = "America/Yellowknife")]
AmericaYellowknife = 221,

[EnumMember(Value = "Antarctica/Casey")]
AntarcticaCasey = 222,

[EnumMember(Value = "Antarctica/Davis")]
AntarcticaDavis = 223,

[EnumMember(Value = "Antarctica/DumontDUrville")]
AntarcticaDumontdurville = 224,

[EnumMember(Value = "Antarctica/Macquarie")]
AntarcticaMacquarie = 225,

[EnumMember(Value = "Antarctica/Mawson")]
AntarcticaMawson = 226,

[EnumMember(Value = "Antarctica/McMurdo")]
AntarcticaMcmurdo = 227,

[EnumMember(Value = "Antarctica/Palmer")]
AntarcticaPalmer = 228,

[EnumMember(Value = "Antarctica/Rothera")]
AntarcticaRothera = 229,

[EnumMember(Value = "Antarctica/South_Pole")]
AntarcticaSouthPole = 230,

[EnumMember(Value = "Antarctica/Syowa")]
AntarcticaSyowa = 231,

[EnumMember(Value = "Antarctica/Troll")]
AntarcticaTroll = 232,

[EnumMember(Value = "Antarctica/Vostok")]
AntarcticaVostok = 233,

[EnumMember(Value = "Arctic/Longyearbyen")]
ArcticLongyearbyen = 234,

[EnumMember(Value = "Asia/Aden")]
AsiaAden = 235,

[EnumMember(Value = "Asia/Almaty")]
AsiaAlmaty = 236,

[EnumMember(Value = "Asia/Amman")]
AsiaAmman = 237,

[EnumMember(Value = "Asia/Anadyr")]
AsiaAnadyr = 238,

[EnumMember(Value = "Asia/Aqtau")]
AsiaAqtau = 239,

[EnumMember(Value = "Asia/Aqtobe")]
AsiaAqtobe = 240,

[EnumMember(Value = "Asia/Ashgabat")]
AsiaAshgabat = 241,

[EnumMember(Value = "Asia/Ashkhabad")]
AsiaAshkhabad = 242,

[EnumMember(Value = "Asia/Atyrau")]
AsiaAtyrau = 243,

[EnumMember(Value = "Asia/Baghdad")]
AsiaBaghdad = 244,

[EnumMember(Value = "Asia/Bahrain")]
AsiaBahrain = 245,

[EnumMember(Value = "Asia/Baku")]
AsiaBaku = 246,

[EnumMember(Value = "Asia/Bangkok")]
AsiaBangkok = 247,

[EnumMember(Value = "Asia/Barnaul")]
AsiaBarnaul = 248,

[EnumMember(Value = "Asia/Beirut")]
AsiaBeirut = 249,

[EnumMember(Value = "Asia/Bishkek")]
AsiaBishkek = 250,

[EnumMember(Value = "Asia/Brunei")]
AsiaBrunei = 251,

[EnumMember(Value = "Asia/Calcutta")]
AsiaCalcutta = 252,

[EnumMember(Value = "Asia/Chita")]
AsiaChita = 253,

[EnumMember(Value = "Asia/Choibalsan")]
AsiaChoibalsan = 254,

[EnumMember(Value = "Asia/Chongqing")]
AsiaChongqing = 255,

[EnumMember(Value = "Asia/Chungking")]
AsiaChungking = 256,

[EnumMember(Value = "Asia/Colombo")]
AsiaColombo = 257,

[EnumMember(Value = "Asia/Dacca")]
AsiaDacca = 258,

[EnumMember(Value = "Asia/Damascus")]
AsiaDamascus = 259,

[EnumMember(Value = "Asia/Dhaka")]
AsiaDhaka = 260,

[EnumMember(Value = "Asia/Dili")]
AsiaDili = 261,

[EnumMember(Value = "Asia/Dubai")]
AsiaDubai = 262,

[EnumMember(Value = "Asia/Dushanbe")]
AsiaDushanbe = 263,

[EnumMember(Value = "Asia/Famagusta")]
AsiaFamagusta = 264,

[EnumMember(Value = "Asia/Gaza")]
AsiaGaza = 265,

[EnumMember(Value = "Asia/Harbin")]
AsiaHarbin = 266,

[EnumMember(Value = "Asia/Hebron")]
AsiaHebron = 267,

[EnumMember(Value = "Asia/Ho_Chi_Minh")]
AsiaHoChiMinh = 268,

[EnumMember(Value = "Asia/Hong_Kong")]
AsiaHongKong = 269,

[EnumMember(Value = "Asia/Hovd")]
AsiaHovd = 270,

[EnumMember(Value = "Asia/Irkutsk")]
AsiaIrkutsk = 271,

[EnumMember(Value = "Asia/Istanbul")]
AsiaIstanbul = 272,

[EnumMember(Value = "Asia/Jakarta")]
AsiaJakarta = 273,

[EnumMember(Value = "Asia/Jayapura")]
AsiaJayapura = 274,

[EnumMember(Value = "Asia/Jerusalem")]
AsiaJerusalem = 275,

[EnumMember(Value = "Asia/Kabul")]
AsiaKabul = 276,

[EnumMember(Value = "Asia/Kamchatka")]
AsiaKamchatka = 277,

[EnumMember(Value = "Asia/Karachi")]
AsiaKarachi = 278,

[EnumMember(Value = "Asia/Kashgar")]
AsiaKashgar = 279,

[EnumMember(Value = "Asia/Kathmandu")]
AsiaKathmandu = 280,

[EnumMember(Value = "Asia/Katmandu")]
AsiaKatmandu = 281,

[EnumMember(Value = "Asia/Khandyga")]
AsiaKhandyga = 282,

[EnumMember(Value = "Asia/Kolkata")]
AsiaKolkata = 283,

[EnumMember(Value = "Asia/Krasnoyarsk")]
AsiaKrasnoyarsk = 284,

[EnumMember(Value = "Asia/Kuala_Lumpur")]
AsiaKualaLumpur = 285,

[EnumMember(Value = "Asia/Kuching")]
AsiaKuching = 286,

[EnumMember(Value = "Asia/Kuwait")]
AsiaKuwait = 287,

[EnumMember(Value = "Asia/Macao")]
AsiaMacao = 288,

[EnumMember(Value = "Asia/Macau")]
AsiaMacau = 289,

[EnumMember(Value = "Asia/Magadan")]
AsiaMagadan = 290,

[EnumMember(Value = "Asia/Makassar")]
AsiaMakassar = 291,

[EnumMember(Value = "Asia/Manila")]
AsiaManila = 292,

[EnumMember(Value = "Asia/Muscat")]
AsiaMuscat = 293,

[EnumMember(Value = "Asia/Nicosia")]
AsiaNicosia = 294,

[EnumMember(Value = "Asia/Novokuznetsk")]
AsiaNovokuznetsk = 295,

[EnumMember(Value = "Asia/Novosibirsk")]
AsiaNovosibirsk = 296,

[EnumMember(Value = "Asia/Omsk")]
AsiaOmsk = 297,

[EnumMember(Value = "Asia/Oral")]
AsiaOral = 298,

[EnumMember(Value = "Asia/Phnom_Penh")]
AsiaPhnomPenh = 299,

[EnumMember(Value = "Asia/Pontianak")]
AsiaPontianak = 300,

[EnumMember(Value = "Asia/Pyongyang")]
AsiaPyongyang = 301,

[EnumMember(Value = "Asia/Qatar")]
AsiaQatar = 302,

[EnumMember(Value = "Asia/Qostanay")]
AsiaQostanay = 303,

[EnumMember(Value = "Asia/Qyzylorda")]
AsiaQyzylorda = 304,

[EnumMember(Value = "Asia/Rangoon")]
AsiaRangoon = 305,

[EnumMember(Value = "Asia/Riyadh")]
AsiaRiyadh = 306,

[EnumMember(Value = "Asia/Saigon")]
AsiaSaigon = 307,

[EnumMember(Value = "Asia/Sakhalin")]
AsiaSakhalin = 308,

[EnumMember(Value = "Asia/Samarkand")]
AsiaSamarkand = 309,

[EnumMember(Value = "Asia/Seoul")]
AsiaSeoul = 310,

[EnumMember(Value = "Asia/Shanghai")]
AsiaShanghai = 311,

[EnumMember(Value = "Asia/Singapore")]
AsiaSingapore = 312,

[EnumMember(Value = "Asia/Srednekolymsk")]
AsiaSrednekolymsk = 313,

[EnumMember(Value = "Asia/Taipei")]
AsiaTaipei = 314,

[EnumMember(Value = "Asia/Tashkent")]
AsiaTashkent = 315,

[EnumMember(Value = "Asia/Tbilisi")]
AsiaTbilisi = 316,

[EnumMember(Value = "Asia/Tehran")]
AsiaTehran = 317,

[EnumMember(Value = "Asia/Tel_Aviv")]
AsiaTelAviv = 318,

[EnumMember(Value = "Asia/Thimbu")]
AsiaThimbu = 319,

[EnumMember(Value = "Asia/Thimphu")]
AsiaThimphu = 320,

[EnumMember(Value = "Asia/Tokyo")]
AsiaTokyo = 321,

[EnumMember(Value = "Asia/Tomsk")]
AsiaTomsk = 322,

[EnumMember(Value = "Asia/Ujung_Pandang")]
AsiaUjungPandang = 323,

[EnumMember(Value = "Asia/Ulaanbaatar")]
AsiaUlaanbaatar = 324,

[EnumMember(Value = "Asia/Ulan_Bator")]
AsiaUlanBator = 325,

[EnumMember(Value = "Asia/Urumqi")]
AsiaUrumqi = 326,

[EnumMember(Value = "Asia/Ust-Nera")]
AsiaUstNera = 327,

[EnumMember(Value = "Asia/Vientiane")]
AsiaVientiane = 328,

[EnumMember(Value = "Asia/Vladivostok")]
AsiaVladivostok = 329,

[EnumMember(Value = "Asia/Yakutsk")]
AsiaYakutsk = 330,

[EnumMember(Value = "Asia/Yangon")]
AsiaYangon = 331,

[EnumMember(Value = "Asia/Yekaterinburg")]
AsiaYekaterinburg = 332,

[EnumMember(Value = "Asia/Yerevan")]
AsiaYerevan = 333,

[EnumMember(Value = "Atlantic/Azores")]
AtlanticAzores = 334,

[EnumMember(Value = "Atlantic/Bermuda")]
AtlanticBermuda = 335,

[EnumMember(Value = "Atlantic/Canary")]
AtlanticCanary = 336,

[EnumMember(Value = "Atlantic/Cape_Verde")]
AtlanticCapeVerde = 337,

[EnumMember(Value = "Atlantic/Faeroe")]
AtlanticFaeroe = 338,

[EnumMember(Value = "Atlantic/Faroe")]
AtlanticFaroe = 339,

[EnumMember(Value = "Atlantic/Jan_Mayen")]
AtlanticJanMayen = 340,

[EnumMember(Value = "Atlantic/Madeira")]
AtlanticMadeira = 341,

[EnumMember(Value = "Atlantic/Reykjavik")]
AtlanticReykjavik = 342,

[EnumMember(Value = "Atlantic/South_Georgia")]
AtlanticSouthGeorgia = 343,

[EnumMember(Value = "Atlantic/St_Helena")]
AtlanticStHelena = 344,

[EnumMember(Value = "Atlantic/Stanley")]
AtlanticStanley = 345,

[EnumMember(Value = "Australia/ACT")]
AustraliaACT = 346,

[EnumMember(Value = "Australia/Adelaide")]
AustraliaAdelaide = 347,

[EnumMember(Value = "Australia/Brisbane")]
AustraliaBrisbane = 348,

[EnumMember(Value = "Australia/Broken_Hill")]
AustraliaBrokenHill = 349,

[EnumMember(Value = "Australia/Canberra")]
AustraliaCanberra = 350,

[EnumMember(Value = "Australia/Currie")]
AustraliaCurrie = 351,

[EnumMember(Value = "Australia/Darwin")]
AustraliaDarwin = 352,

[EnumMember(Value = "Australia/Eucla")]
AustraliaEucla = 353,

[EnumMember(Value = "Australia/Hobart")]
AustraliaHobart = 354,

[EnumMember(Value = "Australia/LHI")]
AustraliaLHI = 355,

[EnumMember(Value = "Australia/Lindeman")]
AustraliaLindeman = 356,

[EnumMember(Value = "Australia/Lord_Howe")]
AustraliaLordHowe = 357,

[EnumMember(Value = "Australia/Melbourne")]
AustraliaMelbourne = 358,

[EnumMember(Value = "Australia/NSW")]
AustraliaNSW = 359,

[EnumMember(Value = "Australia/North")]
AustraliaNorth = 360,

[EnumMember(Value = "Australia/Perth")]
AustraliaPerth = 361,

[EnumMember(Value = "Australia/Queensland")]
AustraliaQueensland = 362,

[EnumMember(Value = "Australia/South")]
AustraliaSouth = 363,

[EnumMember(Value = "Australia/Sydney")]
AustraliaSydney = 364,

[EnumMember(Value = "Australia/Tasmania")]
AustraliaTasmania = 365,

[EnumMember(Value = "Australia/Victoria")]
AustraliaVictoria = 366,

[EnumMember(Value = "Australia/West")]
AustraliaWest = 367,

[EnumMember(Value = "Australia/Yancowinna")]
AustraliaYancowinna = 368,

[EnumMember(Value = "Brazil/Acre")]
BrazilAcre = 369,

[EnumMember(Value = "Brazil/DeNoronha")]
BrazilDenoronha = 370,

[EnumMember(Value = "Brazil/East")]
BrazilEast = 371,

[EnumMember(Value = "Brazil/West")]
BrazilWest = 372,

[EnumMember(Value = "CET")]
CET = 373,

[EnumMember(Value = "CST6CDT")]
CST6CDT = 374,

[EnumMember(Value = "Canada/Atlantic")]
CanadaAtlantic = 375,

[EnumMember(Value = "Canada/Central")]
CanadaCentral = 376,

[EnumMember(Value = "Canada/Eastern")]
CanadaEastern = 377,

[EnumMember(Value = "Canada/Mountain")]
CanadaMountain = 378,

[EnumMember(Value = "Canada/Newfoundland")]
CanadaNewfoundland = 379,

[EnumMember(Value = "Canada/Pacific")]
CanadaPacific = 380,

[EnumMember(Value = "Canada/Saskatchewan")]
CanadaSaskatchewan = 381,

[EnumMember(Value = "Canada/Yukon")]
CanadaYukon = 382,

[EnumMember(Value = "Chile/Continental")]
ChileContinental = 383,

[EnumMember(Value = "Chile/EasterIsland")]
ChileEasterisland = 384,

[EnumMember(Value = "Cuba")]
Cuba = 385,

[EnumMember(Value = "EET")]
EET = 386,

[EnumMember(Value = "EST")]
EST = 387,

[EnumMember(Value = "EST5EDT")]
EST5EDT = 388,

[EnumMember(Value = "Egypt")]
Egypt = 389,

[EnumMember(Value = "Eire")]
Eire = 390,

[EnumMember(Value = "Europe/Amsterdam")]
EuropeAmsterdam = 391,

[EnumMember(Value = "Europe/Andorra")]
EuropeAndorra = 392,

[EnumMember(Value = "Europe/Astrakhan")]
EuropeAstrakhan = 393,

[EnumMember(Value = "Europe/Athens")]
EuropeAthens = 394,

[EnumMember(Value = "Europe/Belfast")]
EuropeBelfast = 395,

[EnumMember(Value = "Europe/Belgrade")]
EuropeBelgrade = 396,

[EnumMember(Value = "Europe/Berlin")]
EuropeBerlin = 397,

[EnumMember(Value = "Europe/Bratislava")]
EuropeBratislava = 398,

[EnumMember(Value = "Europe/Brussels")]
EuropeBrussels = 399,

[EnumMember(Value = "Europe/Bucharest")]
EuropeBucharest = 400,

[EnumMember(Value = "Europe/Budapest")]
EuropeBudapest = 401,

[EnumMember(Value = "Europe/Busingen")]
EuropeBusingen = 402,

[EnumMember(Value = "Europe/Chisinau")]
EuropeChisinau = 403,

[EnumMember(Value = "Europe/Copenhagen")]
EuropeCopenhagen = 404,

[EnumMember(Value = "Europe/Dublin")]
EuropeDublin = 405,

[EnumMember(Value = "Europe/Gibraltar")]
EuropeGibraltar = 406,

[EnumMember(Value = "Europe/Guernsey")]
EuropeGuernsey = 407,

[EnumMember(Value = "Europe/Helsinki")]
EuropeHelsinki = 408,

[EnumMember(Value = "Europe/Isle_of_Man")]
EuropeIsleOfMan = 409,

[EnumMember(Value = "Europe/Istanbul")]
EuropeIstanbul = 410,

[EnumMember(Value = "Europe/Jersey")]
EuropeJersey = 411,

[EnumMember(Value = "Europe/Kaliningrad")]
EuropeKaliningrad = 412,

[EnumMember(Value = "Europe/Kiev")]
EuropeKiev = 413,

[EnumMember(Value = "Europe/Kirov")]
EuropeKirov = 414,

[EnumMember(Value = "Europe/Kyiv")]
EuropeKyiv = 415,

[EnumMember(Value = "Europe/Lisbon")]
EuropeLisbon = 416,

[EnumMember(Value = "Europe/Ljubljana")]
EuropeLjubljana = 417,

[EnumMember(Value = "Europe/London")]
EuropeLondon = 418,

[EnumMember(Value = "Europe/Luxembourg")]
EuropeLuxembourg = 419,

[EnumMember(Value = "Europe/Madrid")]
EuropeMadrid = 420,

[EnumMember(Value = "Europe/Malta")]
EuropeMalta = 421,

[EnumMember(Value = "Europe/Mariehamn")]
EuropeMariehamn = 422,

[EnumMember(Value = "Europe/Minsk")]
EuropeMinsk = 423,

[EnumMember(Value = "Europe/Monaco")]
EuropeMonaco = 424,

[EnumMember(Value = "Europe/Moscow")]
EuropeMoscow = 425,

[EnumMember(Value = "Europe/Nicosia")]
EuropeNicosia = 426,

[EnumMember(Value = "Europe/Oslo")]
EuropeOslo = 427,

[EnumMember(Value = "Europe/Paris")]
EuropeParis = 428,

[EnumMember(Value = "Europe/Podgorica")]
EuropePodgorica = 429,

[EnumMember(Value = "Europe/Prague")]
EuropePrague = 430,

[EnumMember(Value = "Europe/Riga")]
EuropeRiga = 431,

[EnumMember(Value = "Europe/Rome")]
EuropeRome = 432,

[EnumMember(Value = "Europe/Samara")]
EuropeSamara = 433,

[EnumMember(Value = "Europe/San_Marino")]
EuropeSanMarino = 434,

[EnumMember(Value = "Europe/Sarajevo")]
EuropeSarajevo = 435,

[EnumMember(Value = "Europe/Saratov")]
EuropeSaratov = 436,

[EnumMember(Value = "Europe/Simferopol")]
EuropeSimferopol = 437,

[EnumMember(Value = "Europe/Skopje")]
EuropeSkopje = 438,

[EnumMember(Value = "Europe/Sofia")]
EuropeSofia = 439,

[EnumMember(Value = "Europe/Stockholm")]
EuropeStockholm = 440,

[EnumMember(Value = "Europe/Tallinn")]
EuropeTallinn = 441,

[EnumMember(Value = "Europe/Tirane")]
EuropeTirane = 442,

[EnumMember(Value = "Europe/Tiraspol")]
EuropeTiraspol = 443,

[EnumMember(Value = "Europe/Ulyanovsk")]
EuropeUlyanovsk = 444,

[EnumMember(Value = "Europe/Uzhgorod")]
EuropeUzhgorod = 445,

[EnumMember(Value = "Europe/Vaduz")]
EuropeVaduz = 446,

[EnumMember(Value = "Europe/Vatican")]
EuropeVatican = 447,

[EnumMember(Value = "Europe/Vienna")]
EuropeVienna = 448,

[EnumMember(Value = "Europe/Vilnius")]
EuropeVilnius = 449,

[EnumMember(Value = "Europe/Volgograd")]
EuropeVolgograd = 450,

[EnumMember(Value = "Europe/Warsaw")]
EuropeWarsaw = 451,

[EnumMember(Value = "Europe/Zagreb")]
EuropeZagreb = 452,

[EnumMember(Value = "Europe/Zaporozhye")]
EuropeZaporozhye = 453,

[EnumMember(Value = "Europe/Zurich")]
EuropeZurich = 454,

[EnumMember(Value = "GB")]
GB = 455,

[EnumMember(Value = "GB-Eire")]
GBEire = 456,

[EnumMember(Value = "GMT")]
GMT = 457,

[EnumMember(Value = "GMT+0")]
Gmtplus0 = 458,

[EnumMember(Value = "Greenwich")]
Greenwich = 461,

[EnumMember(Value = "HST")]
HST = 462,

[EnumMember(Value = "Hongkong")]
Hongkong = 463,

[EnumMember(Value = "Iceland")]
Iceland = 464,

[EnumMember(Value = "Indian/Antananarivo")]
IndianAntananarivo = 465,

[EnumMember(Value = "Indian/Chagos")]
IndianChagos = 466,

[EnumMember(Value = "Indian/Christmas")]
IndianChristmas = 467,

[EnumMember(Value = "Indian/Cocos")]
IndianCocos = 468,

[EnumMember(Value = "Indian/Comoro")]
IndianComoro = 469,

[EnumMember(Value = "Indian/Kerguelen")]
IndianKerguelen = 470,

[EnumMember(Value = "Indian/Mahe")]
IndianMahe = 471,

[EnumMember(Value = "Indian/Maldives")]
IndianMaldives = 472,

[EnumMember(Value = "Indian/Mauritius")]
IndianMauritius = 473,

[EnumMember(Value = "Indian/Mayotte")]
IndianMayotte = 474,

[EnumMember(Value = "Indian/Reunion")]
IndianReunion = 475,

[EnumMember(Value = "Iran")]
Iran = 476,

[EnumMember(Value = "Israel")]
Israel = 477,

[EnumMember(Value = "Jamaica")]
Jamaica = 478,

[EnumMember(Value = "Japan")]
Japan = 479,

[EnumMember(Value = "Kwajalein")]
Kwajalein = 480,

[EnumMember(Value = "Libya")]
Libya = 481,

[EnumMember(Value = "MET")]
MET = 482,

[EnumMember(Value = "MST")]
MST = 483,

[EnumMember(Value = "MST7MDT")]
MST7MDT = 484,

[EnumMember(Value = "Mexico/BajaNorte")]
MexicoBajanorte = 485,

[EnumMember(Value = "Mexico/BajaSur")]
MexicoBajasur = 486,

[EnumMember(Value = "Mexico/General")]
MexicoGeneral = 487,

[EnumMember(Value = "NZ")]
NZ = 488,

[EnumMember(Value = "NZ-CHAT")]
NZCHAT = 489,

[EnumMember(Value = "Navajo")]
Navajo = 490,

[EnumMember(Value = "PRC")]
PRC = 491,

[EnumMember(Value = "PST8PDT")]
PST8PDT = 492,

[EnumMember(Value = "Pacific/Apia")]
PacificApia = 493,

[EnumMember(Value = "Pacific/Auckland")]
PacificAuckland = 494,

[EnumMember(Value = "Pacific/Bougainville")]
PacificBougainville = 495,

[EnumMember(Value = "Pacific/Chatham")]
PacificChatham = 496,

[EnumMember(Value = "Pacific/Chuuk")]
PacificChuuk = 497,

[EnumMember(Value = "Pacific/Easter")]
PacificEaster = 498,

[EnumMember(Value = "Pacific/Efate")]
PacificEfate = 499,

[EnumMember(Value = "Pacific/Enderbury")]
PacificEnderbury = 500,

[EnumMember(Value = "Pacific/Fakaofo")]
PacificFakaofo = 501,

[EnumMember(Value = "Pacific/Fiji")]
PacificFiji = 502,

[EnumMember(Value = "Pacific/Funafuti")]
PacificFunafuti = 503,

[EnumMember(Value = "Pacific/Galapagos")]
PacificGalapagos = 504,

[EnumMember(Value = "Pacific/Gambier")]
PacificGambier = 505,

[EnumMember(Value = "Pacific/Guadalcanal")]
PacificGuadalcanal = 506,

[EnumMember(Value = "Pacific/Guam")]
PacificGuam = 507,

[EnumMember(Value = "Pacific/Honolulu")]
PacificHonolulu = 508,

[EnumMember(Value = "Pacific/Johnston")]
PacificJohnston = 509,

[EnumMember(Value = "Pacific/Kanton")]
PacificKanton = 510,

[EnumMember(Value = "Pacific/Kiritimati")]
PacificKiritimati = 511,

[EnumMember(Value = "Pacific/Kosrae")]
PacificKosrae = 512,

[EnumMember(Value = "Pacific/Kwajalein")]
PacificKwajalein = 513,

[EnumMember(Value = "Pacific/Majuro")]
PacificMajuro = 514,

[EnumMember(Value = "Pacific/Marquesas")]
PacificMarquesas = 515,

[EnumMember(Value = "Pacific/Midway")]
PacificMidway = 516,

[EnumMember(Value = "Pacific/Nauru")]
PacificNauru = 517,

[EnumMember(Value = "Pacific/Niue")]
PacificNiue = 518,

[EnumMember(Value = "Pacific/Norfolk")]
PacificNorfolk = 519,

[EnumMember(Value = "Pacific/Noumea")]
PacificNoumea = 520,

[EnumMember(Value = "Pacific/Pago_Pago")]
PacificPagoPago = 521,

[EnumMember(Value = "Pacific/Palau")]
PacificPalau = 522,

[EnumMember(Value = "Pacific/Pitcairn")]
PacificPitcairn = 523,

[EnumMember(Value = "Pacific/Pohnpei")]
PacificPohnpei = 524,

[EnumMember(Value = "Pacific/Ponape")]
PacificPonape = 525,

[EnumMember(Value = "Pacific/Port_Moresby")]
PacificPortMoresby = 526,

[EnumMember(Value = "Pacific/Rarotonga")]
PacificRarotonga = 527,

[EnumMember(Value = "Pacific/Saipan")]
PacificSaipan = 528,

[EnumMember(Value = "Pacific/Samoa")]
PacificSamoa = 529,

[EnumMember(Value = "Pacific/Tahiti")]
PacificTahiti = 530,

[EnumMember(Value = "Pacific/Tarawa")]
PacificTarawa = 531,

[EnumMember(Value = "Pacific/Tongatapu")]
PacificTongatapu = 532,

[EnumMember(Value = "Pacific/Truk")]
PacificTruk = 533,

[EnumMember(Value = "Pacific/Wake")]
PacificWake = 534,

[EnumMember(Value = "Pacific/Wallis")]
PacificWallis = 535,

[EnumMember(Value = "Pacific/Yap")]
PacificYap = 536,

[EnumMember(Value = "Poland")]
Poland = 537,

[EnumMember(Value = "Portugal")]
Portugal = 538,

[EnumMember(Value = "ROC")]
ROC = 539,

[EnumMember(Value = "ROK")]
ROK = 540,

[EnumMember(Value = "Singapore")]
Singapore = 541,

[EnumMember(Value = "Turkey")]
Turkey = 542,

[EnumMember(Value = "UCT")]
UCT = 543,

[EnumMember(Value = "US/Alaska")]
USAlaska = 544,

[EnumMember(Value = "US/Aleutian")]
USAleutian = 545,

[EnumMember(Value = "US/Arizona")]
USArizona = 546,

[EnumMember(Value = "US/Central")]
USCentral = 547,

[EnumMember(Value = "US/East-Indiana")]
USEastIndiana = 548,

[EnumMember(Value = "US/Eastern")]
USEastern = 549,

[EnumMember(Value = "US/Hawaii")]
USHawaii = 550,

[EnumMember(Value = "US/Indiana-Starke")]
USIndianaStarke = 551,

[EnumMember(Value = "US/Michigan")]
USMichigan = 552,

[EnumMember(Value = "US/Mountain")]
USMountain = 553,

[EnumMember(Value = "US/Pacific")]
USPacific = 554,

[EnumMember(Value = "US/Samoa")]
USSamoa = 555,

[EnumMember(Value = "UTC")]
UTC = 556,

[EnumMember(Value = "Universal")]
Universal = 557,

[EnumMember(Value = "W-SU")]
WSU = 558,

[EnumMember(Value = "WET")]
WET = 559,

[EnumMember(Value = "Zulu")]
Zulu = 560,

[EnumMember(Value = "Etc/GMT")]
EtcGMT = 561,

[EnumMember(Value = "Etc/GMT+0")]
EtcGmtplus0 = 562,

[EnumMember(Value = "Etc/GMT+1")]
EtcGmtplus1 = 563,

[EnumMember(Value = "Etc/GMT+10")]
EtcGmtplus10 = 564,

[EnumMember(Value = "Etc/GMT+11")]
EtcGmtplus11 = 565,

[EnumMember(Value = "Etc/GMT+12")]
EtcGmtplus12 = 566,

[EnumMember(Value = "Etc/GMT+2")]
EtcGmtplus2 = 567,

[EnumMember(Value = "Etc/GMT+3")]
EtcGmtplus3 = 568,

[EnumMember(Value = "Etc/GMT+4")]
EtcGmtplus4 = 569,

[EnumMember(Value = "Etc/GMT+5")]
EtcGmtplus5 = 570,

[EnumMember(Value = "Etc/GMT+6")]
EtcGmtplus6 = 571,

[EnumMember(Value = "Etc/GMT+7")]
EtcGmtplus7 = 572,

[EnumMember(Value = "Etc/GMT+8")]
EtcGmtplus8 = 573,

[EnumMember(Value = "Etc/GMT+9")]
EtcGmtplus9 = 574,

[EnumMember(Value = "Etc/GMT-0")]
EtcGMT0 = 575,

[EnumMember(Value = "Etc/GMT-1")]
EtcGMT1 = 576,

[EnumMember(Value = "Etc/GMT-10")]
EtcGMT10 = 577,

[EnumMember(Value = "Etc/GMT-11")]
EtcGMT11 = 578,

[EnumMember(Value = "Etc/GMT-12")]
EtcGMT12 = 579,

[EnumMember(Value = "Etc/GMT-13")]
EtcGMT13 = 580,

[EnumMember(Value = "Etc/GMT-14")]
EtcGMT14 = 581,

[EnumMember(Value = "Etc/GMT-2")]
EtcGMT2 = 582,

[EnumMember(Value = "Etc/GMT-3")]
EtcGMT3 = 583,

[EnumMember(Value = "Etc/GMT-4")]
EtcGMT4 = 584,

[EnumMember(Value = "Etc/GMT-5")]
EtcGMT5 = 585,

[EnumMember(Value = "Etc/GMT-6")]
EtcGMT6 = 586,

[EnumMember(Value = "Etc/GMT-7")]
EtcGMT7 = 587,

[EnumMember(Value = "Etc/GMT-8")]
EtcGMT8 = 588,

[EnumMember(Value = "Etc/GMT-9")]
EtcGMT9 = 589,

[EnumMember(Value = "Etc/Greenwich")]
EtcGreenwich = 591,

[EnumMember(Value = "Etc/UCT")]
EtcUCT = 592,

[EnumMember(Value = "Etc/UTC")]
EtcUTC = 593,

[EnumMember(Value = "Etc/Universal")]
EtcUniversal = 594,

[EnumMember(Value = "Etc/Zulu")]
EtcZulu = 595,
}


/// <summary>
/// Help: - Contact: Use this to organize the contact details of employees of a given company (e.g. CEO, CFO, ...). <br />
 /// - Invoice Address : Preferred address for all invoices. Selected by default when you invoice an order that belongs to this company. <br />
 /// - Delivery Address : Preferred address for all deliveries. Selected by default when you deliver an order that belongs to this company. <br />
 /// - Private: Private addresses are only visible by authorized users and contain sensitive data (employee home addresses, ...). <br />
 /// - Other: Other address for the company (e.g. subsidiary, ...)
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum AddressTypeResPartnerOdooEnum
{
[EnumMember(Value = "contact")]
Contact = 1,

[EnumMember(Value = "invoice")]
InvoiceAddress = 2,

[EnumMember(Value = "delivery")]
DeliveryAddress = 3,

[EnumMember(Value = "private")]
PrivateAddress = 4,

[EnumMember(Value = "other")]
OtherAddress = 5,

[EnumMember(Value = "followup")]
FollowUpAddress = 6,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum CompanyTypeResPartnerOdooEnum
{
[EnumMember(Value = "person")]
Individual = 1,

[EnumMember(Value = "company")]
Company = 2,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum DegreeOfTrustYouHaveInThisDebtorResPartnerOdooEnum
{
[EnumMember(Value = "good")]
GoodDebtor = 1,

[EnumMember(Value = "normal")]
NormalDebtor = 2,

[EnumMember(Value = "bad")]
BadDebtor = 3,
}


/// <summary>
/// Help: Selecting the "Warning" option will notify user with the message, Selecting "Blocking Message" will throw an exception with the message and block the flow. The Message has to be written in the next field.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum InvoiceResPartnerOdooEnum
{
[EnumMember(Value = "no-message")]
NoMessage = 1,

[EnumMember(Value = "warning")]
Warning = 2,

[EnumMember(Value = "block")]
BlockingMessage = 3,
}


/// <summary>
/// Help: Selecting the "Warning" option will notify user with the message, Selecting "Blocking Message" will throw an exception with the message and block the flow. The Message has to be written in the next field.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum PurchaseOrderResPartnerOdooEnum
{
[EnumMember(Value = "no-message")]
NoMessage = 1,

[EnumMember(Value = "warning")]
Warning = 2,

[EnumMember(Value = "block")]
BlockingMessage = 3,
}


/// <summary>
/// Help: Selecting the "Warning" option will notify user with the message, Selecting "Blocking Message" will throw an exception with the message and block the flow. The Message has to be written in the next field.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum SalesWarningsResPartnerOdooEnum
{
[EnumMember(Value = "no-message")]
NoMessage = 1,

[EnumMember(Value = "warning")]
Warning = 2,

[EnumMember(Value = "block")]
BlockingMessage = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum FollowUpStatusResPartnerOdooEnum
{
[EnumMember(Value = "in_need_of_action")]
InNeedOfAction = 1,

[EnumMember(Value = "with_overdue_invoices")]
WithOverdueInvoices = 2,

[EnumMember(Value = "no_action_needed")]
NoActionNeeded = 3,
}


/// <summary>
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum RemindersResPartnerOdooEnum
{
[EnumMember(Value = "automatic")]
Automatic = 1,

[EnumMember(Value = "manual")]
Manual = 2,
}

#pragma warning restore S2344
#pragma warning restore S3903 