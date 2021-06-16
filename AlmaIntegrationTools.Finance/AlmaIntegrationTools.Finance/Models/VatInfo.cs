using System;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance.Models
{
    public class VatInfo
    {
        /// <summary>
        /// Indication whether the VAT amount is expended from the invoice line's fund. If set to false, the VAT amount is expended from a separate fund (adjustment invoice line).
        /// </summary>
        [XmlElement("expended_from_fund_ind")]
        public bool IsExpendedFromFund{ get; set; }


        /// <summary>
        /// Indication whether the invoice total amount includes VAT, this field is deprecated, use vat_type instead, for backwards compatibility it will return true if VAT type is INCLUSIVE.
        /// </summary>
        [XmlElement("inclusive_ind")]
        public bool IsIncludedInAmount { get; set; }

        /// <summary>
        /// Amount of VAT to be paid on the invoice, either inclusive or exclusive.
        /// </summary>
        [XmlElement("vat_amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Percentage of the invoice total amount, either inclusive or exclusive.
        /// </summary>
        [XmlElement("vat_percentage")]
        public decimal Percentage { get; set; }

        /// <summary>
        /// Invoice VAT code
        /// </summary>
        [XmlElement("vat_code")]
        public String Code { get; set; }

        /// <summary>
        /// Invoice VAT Description
        /// </summary>
        [XmlElement("vat_description")]
        public String Description { get; set; }

        /// <summary>
        /// Text that indicates that the invoice is in a foreign currency and the VAT is charged in the local currency.
        /// </summary>
        [XmlElement("vat_type")]
        public String Type{ get; set; }

        [XmlElement("vendor_tax")]
        public String Tax { get; set; }

        /// <summary>
        /// Indication whether the invoice vat is in invoice line level
        /// </summary>
        [XmlElement("vat_in_invoice_line_level")]
        public bool IsLineLevel { get; set; }

    }

}


