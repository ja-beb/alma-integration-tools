using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance.Models
{
    public class InvoiceLine 
    {
        /// <summary>
        /// The invoice line number (need to contain numbers only )
        /// </summary>
        [XmlElement("line_number")]
        public String Number { get; set; }

        /// <summary>
        /// The type of the invoice line. Possible values are listed in 'Invoice Line Types' code table.
        /// </summary>
        [XmlElement("line_type")]
        public String Type { get; set; }

        /// <summary>
        /// Free text note
        /// </summary>
        [XmlElement("note")]
        public String Note { get; set; }

        /// <summary>
        /// The quantity for the invoice line
        /// </summary>
        [XmlElement("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// The report code related to the invoice line
        /// </summary>
        [XmlElement("reporting_code")]
        public String ReportingCode { get; set; }

        /// <summary>
        /// The subscription dates range of continuous po line invoice line
        /// </summary>
        [XmlElement("subscription_dates_range")]
        public String SubscriptionDateRange { get; set; }

        /// <summary>
        /// The total price for the invoice line(including additional charges, vat etc.)
        /// </summary>
        [XmlElement("total_price")]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// The price for the invoice line
        /// </summary>
        [XmlElement("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// Information about the invoice line related PO line.
        /// </summary>
        [XmlElement("po_line_info")]
        public PoLineInfo PoLineInfo { get; set; }

        /// <summary>
        /// List of funds related to an invoice line.
        /// </summary>
        [XmlArray("fund_info_list")]
        [XmlArrayItem(ElementName = "fund_info")]
        public List<FundInfo> FundInfoList { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("vendor_tax")]
        public String VendorTax{ get; set; }

        /// <summary>
        /// The price note for the invoice line
        /// </summary>
        [XmlElement("price_note")]
        public String PriceNote { get; set; }


        /// <summary>
        /// The secondary report code related to the invoice line
        /// </summary>
        [XmlElement("secondary_reporting_code")]
        public String SecondaryReportingCode { get; set; }

        /// <summary>
        /// The tertiary report code related to the invoice line
        /// </summary>
        [XmlElement("tertiary_reporting_code")]
        public String TertiaryReportingCode { get; set; }

        /// <summary>
        /// Amount of VAT to be paid on the invoice line, will be set only if vat_in_invoice_line_level is set.
        /// </summary>
        [XmlElement("vat_amount")]
        public decimal VatAmount { get; set; }

        /// <summary>
        /// VAT Percentage of the invoice line total amount.
        /// </summary>
        [XmlElement("vat_percentage")]
        public decimal VatPercentage { get; set; }

        /// <summary>
        /// Invoice line VAT code
        /// </summary>
        [XmlElement("vat_code")]
        public String VatCode { get; set; }

        /// <summary>
        /// Invoice line VAT Description
        /// </summary>
        [XmlElement("vat_description")]
        public String VatDescription { get; set; }

        /// <summary>
        /// The additional information of continuous po line invoice line
        /// </summary>
        [XmlElement("additional_information")]
        public String AdditionalInformation{ get; set; }
    }
}


