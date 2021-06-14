using System;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Bursar
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = BursarFeed.Namespace)]
    public class UserFineFee
    {

        [XmlElement("fineFeeComment")]
        public string Comment { get; set; }

        [XmlElement("fineFeeType")]
        public string Type { get; set; }

        [XmlElement("owneredEntity")]
        public OwneredEntity OwneredEntity { get; set; }

        [XmlElement("lastTransactionDate")]
        public string LastTransactionDate { get; set; }

        [XmlElement("itemTitle")]
        public string Title { get; set; }

        [XmlElement("itemCallNumebr")]
        public string CallNumber { get; set; }

        [XmlElement("itemLibrary")]
        public string Library { get; set; }

        [XmlElement("itemLocation")]
        public string Location { get; set; }

        [XmlElement("itemInternalLocation")]
        public string InternalLocation { get; set; }

        [XmlElement("itemDueDate", IsNullable = true)]
        public object DueDate { get; set; }

        [XmlElement("itemBarcode")]
        public string Barcode { get; set; }

        [XmlElement("compositeSum")]
        public CompositeSum CompositeSum { get; set; }

        [XmlElement("bursarTransactionId")]
        public ulong BursarTransactionId { get; set; }
    }
}
