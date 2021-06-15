using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance.Models
{
    /// <summary>
    /// Payment address.
    /// </summary>
    public class PaymentAddress
    {

        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("preferred")]
        public bool IsPreferred { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("line1")]
        public String Line1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("line2")]
        public String Line2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("line3")]
        public String Line3 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("line4")]
        public String Line4 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("line5")]
        public String Line5 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("city")]
        public String City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("postalCode")]
        public String PostalCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("stateProvince")]
        public String StateProvince { get; set; }

        /// <summary>
        /// Values taken from CountryCodes Alma Code Table
        /// </summary>
        [XmlElement("country")]
        public String Country { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("note")]
        public String Note { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [XmlArray("types")]
        [XmlArrayItem(ElementName = "type")]
        public List<String> Types { get; set; }

    }
}


