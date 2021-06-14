using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance
{
    [Serializable]
    [XmlRoot("payment_data", Namespace = "http://com/exlibris/repository/acq/invoice/xmlbeans")]
    public class PaymentData
    {
        [XmlArray("invoice_list")]
        [XmlArrayItem(ElementName = "invoice")]
        public List<Invoice> List { get; set; }
    }

}
