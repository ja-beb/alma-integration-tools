using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance
{
    public class PaymentAddress
    {

        [XmlAttribute("preferred")]
        public bool IsPrefered { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("line1")]
        public String Line1 { get; set; }

        [XmlElement("line2")]
        public String Line2 { get; set; }

        [XmlElement("line3")]
        public String Line3 { get; set; }

        [XmlElement("line4")]
        public String Line4 { get; set; }

        [XmlElement("line5")]
        public String Line5 { get; set; }

        [XmlElement("city")]
        public String City { get; set; }

        [XmlElement("postalCode")]
        public String PostalCode { get; set; }

        [XmlElement("stateProvince")]
        public String StateProvince { get; set; }

        [XmlElement("country")]
        public String Country { get; set; }

        [XmlElement("note")]
        public String Note { get; set; }

        [XmlArray("types")]
        [XmlArrayItem(ElementName = "type")]
        public List<String> Types { get; set; }

    }
}


