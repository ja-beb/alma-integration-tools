using System;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance.Models
{

    /// <summary>
    /// Total amounts for invoice.
    /// </summary>
    public class Amount 
    {

        /// <summary>
        /// The currency. Possible values are listed in 'Currency Code Table' code table.
        /// </summary>
        [XmlElement("currency")]
        public String Currency { get; set; }

        /// <summary>
        /// The total sum
        /// </summary>
        [XmlElement("sum")]
        public decimal Sum { get; set; }

        /// <summary>
        /// Rate for forgein currency exchange.
        /// </summary>
        [XmlIgnore]
        [XmlElement("explicit_rate")]
        public decimal ExplicitRate { get; set; }
    }

}


