using System;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.Bursar.Models
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = BursarFeed.Namespace)]
    public class OwneredEntity
    {
        [XmlElement("InstitutionId")]
        public ushort InstitutionId { get; set; }

        [XmlElement("libraryId", IsNullable = true)]
        public ulong? LibraryId { get; set; }

        [XmlElement("libraryCode", IsNullable = true)]
        public string LibraryCode { get; set; }

    }
}
