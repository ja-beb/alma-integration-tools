using System;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.Bursar.Models
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = BursarFeed.Namespace)]
    public class UserOwneredEntity
    {
        [XmlElement("InstitutionId")]
        public ulong InstitutionId { get; set; }
    }
}
