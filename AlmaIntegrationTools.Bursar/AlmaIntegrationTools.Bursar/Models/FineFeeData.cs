using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.Bursar.Models
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = BursarFeed.Namespace)]
    public class FineFeeData
    {
        [XmlElement("user")]
        public UserIdentifier User { get; set; }

        [XmlElement("patronName")]
        public string PatronName { get; set; }

        [XmlArray("finefeeList")]
        [XmlArrayItem("userFineFee", IsNullable = false)]
        public List<UserFineFee> FineFeeList { get; set; }
    }
}
