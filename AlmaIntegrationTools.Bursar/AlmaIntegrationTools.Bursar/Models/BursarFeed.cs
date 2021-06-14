using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Bursar
{
    [Serializable]
    [XmlType(AnonymousType = true, Namespace = BursarFeed.Namespace)]
    [XmlRoot("xml-fragment", Namespace = BursarFeed.Namespace, IsNullable = false)]
    public class BursarFeed
    {
        [XmlIgnore]
        public const string Namespace = "http://com/exlibris/urm/rep/externalsysfinesfees/xmlbeans";

        [XmlElement("userExportedFineFees")]
        public List<BursarExportData> ExportedFineFees { get; set; }
    }
}

