using System;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance.Models
{


    public class ExchangeRate
    {
        /// <summary>
        /// Rate for forgein currency exchange.
        /// </summary>
        [XmlElement("rate")]
        public decimal Rate { get; set; }

        /// <summary>
        /// The currency: Possible values are listed in 'Currency Code Table' code table. 
        /// </summary>
        [XmlElement("currency")]
        public String Currency { get; set; }

        /// <summary>
        /// Indication whether the exchange rate is explicit (by user definition) or implicit (By Alma definitions)
        /// </summary>
        [XmlElement("explicit_ind")]
        public bool IsExplicit { get; set; }
    }
}