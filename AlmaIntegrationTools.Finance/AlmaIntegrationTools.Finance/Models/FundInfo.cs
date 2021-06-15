using System;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance.Models
{

    public class FundInfo
    {
        /// <summary>
        /// The amount which is to be paid by the fund(in the invoice's currency)
        /// </summary>
        [XmlElement("amount")]
        public Amount Amount { get; set; }

        /// <summary>
        /// The amount which is to be paid by the fund (in the fund's currency)
        /// </summary>
        [XmlElement("local_amount")]
        public Amount LocalAmount { get; set; }

        /// <summary>
        /// Fund code
        /// </summary>
        [XmlElement("code")]
        public String Code { get; set; }


        /// <summary>
        /// Fund name
        /// </summary>
        [XmlElement("name")]
        public String Name { get; set; }

        /// <summary>
        ///   The financial year to which the fund applies.
        /// </summary>
        [XmlElement("fiscal_period")]
        public String FiscalPeriod { get; set; }

        /// <summary>
        /// The ID that is used to link to other system keys
        /// </summary>
        [XmlElement("external_id")]
        public String ExternalId { get; set; }

        /// <summary>
        /// Fund type LEDGER, SUMMARY, ALLOCATED
        /// </summary>
        [XmlElement("type")]
        public String Type { get; set; }

        /// <summary>
        /// Fund type code table
        /// </summary>
        [XmlElement("fund_type")]
        public String FundType { get; set; }

        /// <summary>
        /// Fund type code table Description
        /// </summary>
        [XmlElement("fund_type_desc")]
        public String FundTypeDescription { get; set; }

        /// <summary>
        ///  Fund code
        /// </summary>
        [XmlElement("ledger_code")]
        public String LedgerCode { get; set; }

        /// <summary>
        /// Fund name
        /// </summary>
        [XmlElement("ledger_name")]
        public String LedgerName { get; set; }
    }
}


