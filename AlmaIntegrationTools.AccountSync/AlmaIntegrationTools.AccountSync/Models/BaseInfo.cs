using System;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.AccountSync.Models
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public abstract class BaseInfo
    {
        /// <summary>
        /// Is this a preferred contact entry.
        /// </summary>
        [XmlAttribute("preferred")]
        public bool IsPreferred { get; set; }

        /// <summary>
        /// Segment type of this entry.
        /// </summary>
        [XmlAttribute("segment_type")]
        public string SegmentType { get; set; }
    }
}
