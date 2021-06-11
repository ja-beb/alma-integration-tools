using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance
{

    [Serializable]
    public class Invoice
    {

        /// <summary>
        /// The invoice number, as appears on the invoice. This may be used to identify the invoice (with vendor_code) during import, if a unique_identifier is not supplied.
        /// </summary>
        [XmlElement("invoice_number")]
        public String Number { get; set; }

        /// <summary>
        /// The invoice reference number, as appears on the invoice
        /// </summary>
        [XmlElement("invoice_ref_num")]
        public String ReferenceNumber { get; set; }

        /// <summary>
        /// The invoice owner, as appears on the invoice 
        /// </summary>
        [XmlElement("invoice_owner")]
        public String Owner { get; set; }

        /// <summary>
        /// Amount of this invoice.
        /// </summary>
        [XmlElement("invoice_amount")]
        public Amount InvoiceAmount { get; set; }

        /// <summary>
        /// The code of the vendor from which the invoice was issued. This may be used to identify the invoice (with invoice_number) during import, if a unique_identifier is not supplied. 
        /// </summary>
        [XmlElement("vendor_code")]
        public String VendorCode { get; set; }

        /// <summary>
        /// The name of the vendor from which the invoice was issued. 
        /// </summary>
        [XmlElement("vendor_name")]
        public String VendorName { get; set; }

        /// <summary>
        /// The Financial Sys Code of the vendor from which the invoice was issued. 
        /// </summary>
        [XmlElement("vendor_FinancialSys_Code")]
        public String VendorFinancialSystemCode { get; set; }

        /// <summary>
        /// Indication whether the vendor is liable for vat 
        /// </summary>
        [XmlElement("vendor_liable_for_vat")]
        public bool VendorLiableForVat { get; set; }

        /// <summary>
        /// The specific vendor account related to the invoice 
        /// </summary>
        [XmlElement("vendor_national_tax_id")]
        public String VendorNationalTaxId { get; set; }

        /// <summary>
        /// List of vendor payment address.
        /// </summary>
        [XmlArray("vendor_payment_address_list")]
        [XmlArrayItem(ElementName = "payment_address")]
        public List<PaymentAddress> PaymentAddresses { get; set; }

        /// <summary>
        /// The specific vendor account related to the invoice 
        /// </summary>
        [XmlElement("vendor_account_code")]
        public String VendorAccountCode { get; set; }

        /// <summary>
        /// Unique identifier of the invoice. This value can be used when importing invoices in order to identify the invoice. If not specified during import, the invoice number and vendor code combination will be used to identify the invoice. 
        /// </summary>
        [XmlElement("unique_identifier")]
        public String UniqueIdentifier { get; set; }

        /// <summary>
        /// The invoice date, same format as appears on the invoices 
        /// </summary>
        [XmlElement("invoice_date")]
        public String InvoiceDate { get; set; }

        /// <summary>
        /// Payment method of the invoice. Possible values are listed in 'Payment Method' code table. 
        /// </summary>
        [XmlElement("payment_method")]
        public String PaymentMethod { get; set; }

        /// <summary>
        /// Indication whether the invoice is prepaid
        /// </summary>
        [XmlElement("prepaid_ind")]
        // Indication whether the invoice is prepaid
        public bool IsPrepaid { get; set; }

        /// <summary>
        /// The number of invoice attachments. The actual attachments can be retrieved using API. 
        /// </summary>
        [XmlElement("number_of_attachments")]
        public int AttachementCount { get; set; }


        /// <summary>
        /// Notes associated with this invoice.
        /// </summary>
        [XmlArray("noteList", IsNullable = true)]
        [XmlArrayItem(ElementName = "note")]
        public List<Note> Notes { get; set; }

        /// <summary>
        /// The additional code of the vendor from which the invoice was issued. 
        /// </summary>
        [XmlElement("vendor_additional_code")]
        public String VendorCodeAdditional { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("invoice_ownered_entity")]
        public OwneredEntity OwneredEntity { get; set; }

        /// <summary>
        /// VAT charge that is included in the invoice.
        /// </summary>
        [XmlElement("vat_info")]
        public VatInfo VatInfo { get; set; }
        /// <summary>
        /// List of lines whithin an invoice
        /// </summary>
        [XmlArray("invoice_line_list")]
        [XmlArrayItem(ElementName = "invoice_line")]
        public List<InvoiceLine> Lines { get; set; }


        /// <summary>
        /// The name of the user first and last that approved this invoice. 
        /// </summary>
        [XmlElement("approved_by")]
        public String ApprovedBy { get; set; }


        /// <summary>
        /// The Financial system codeof the vendor Account from which the invoice was issued. 
        /// </summary>
        [XmlElement("vendor_account_FinancialSys_Code")]
        // 
        public String VendorAccountFinancialSystemCode { get; set; }

        /// <summary>
        /// Additional charges that are included in the invoice.
        /// </summary>
        [XmlElement("additional_charges")]
        public AdditionalCharges AdditionalCharges { get; set; }



        /// <summary>
        /// List of exchange rates whithin an invoice
        /// </summary>
        [XmlArray("invoice_exchange_rate_list")]
        [XmlArrayItem(ElementName = "exchange_rate")]
        public List<ExchangeRate> ExchangeRates { get; set; }

    }


}