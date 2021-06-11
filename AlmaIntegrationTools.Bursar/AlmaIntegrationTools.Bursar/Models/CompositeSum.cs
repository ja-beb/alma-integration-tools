using System;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Bursar
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = BursarFeed.Namespace)]
    public class CompositeSum
    {
        [XmlElement("currency")]
        public string Currency { get; set; }

        [XmlElement("sum")]
        public decimal Sum { get; set; }

        [XmlElement("vat", IsNullable = true)]
        public object Vat { get; set; }
    }
}
