using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance.Models
{
    /// <summary>
    /// Payment data.
    /// </summary>
    [Serializable]
    [XmlRoot("payment_data", Namespace = "http://com/exlibris/repository/acq/invoice/xmlbeans")]
    public class PaymentData
    {
        /// <summary>
        /// List of invoices in this payment feed.
        /// </summary>
        [XmlArray("invoice_list")]
        [XmlArrayItem(ElementName = "invoice")]
        public List<Invoice> List { get; set; }
    }

}
