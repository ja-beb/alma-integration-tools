using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Bursar
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = BursarFeed.Namespace)]
    public class BursarExportData
    {
        [XmlArray("userExportedList")]
        [XmlArrayItem("userExportedFineFeesList", IsNullable = false)]
        public List<FineFeeData> UserExportedList { get; set; }

        [XmlElement("exportNumber")]
        public ulong Number { get; set; }

        [XmlIgnore()]
        public bool ExportNumberSpecified { get; set; }
    }
}
