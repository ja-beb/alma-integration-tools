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

        [XmlIgnore]
        public ulong Number
        {
            get
            {
                foreach (BursarExportData data in ExportedFineFees)
                {
                    if (data.ExportNumberSpecified) return data.Number;
                }
                return 0;
            }
        }

        [XmlElement("userExportedFineFees")]
        public List<BursarExportData> ExportedFineFees { get; set; }
    }
}

