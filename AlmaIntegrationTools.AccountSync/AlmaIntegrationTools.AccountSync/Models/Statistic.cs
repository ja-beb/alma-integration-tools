using System;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.AccountSync.Models
{

    [Serializable]
    [XmlType(AnonymousType = true)]
    public class Statistic
    {
        /// <summary>
        /// The statistic's Categories. Possible codes are listed in 'User Statistical Categories' code table.
        /// </summary>
        [XmlElement("statistic_category")]
        public string Category { get; set; }

        /// <summary>
        /// The statistic's type. Possible codes are listed in 'User Category Types' code table.
        /// </summary>
        [XmlAttribute("segment_type")]
        public string SegmentType { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="category"></param>
        /// <param name="segmentType"></param>
        public Statistic(string category, string segmentType)
        {
            Category = category;
            SegmentType = segmentType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Statistic()
        { }
    }
}
