using System;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Bursar
{

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = BursarFeed.Namespace)]
    public class UserIdentifier
    {

        [XmlElement("type")]
        public string Type { get; set; }

        [XmlElement("value")]
        public uint Value { get; set; }

        [XmlIgnore]
        [XmlElement("owneredEntity")]
        public UserOwneredEntity OwneredEntity { get; set; }
    }
}
