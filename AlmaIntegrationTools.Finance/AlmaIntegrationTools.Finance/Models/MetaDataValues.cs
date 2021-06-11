using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance
{
    public class MetaDataValues
    {

        /// <summary>
        /// The identifier value
        /// </summary>
        [XmlElement("acqterms_identifier")]
        public string AermsIdentifier { get; set; }
        /// <summary>
        /// The identifier Type ISBN or ISSN.
        /// </summary>
        [XmlElement("acqterms_identifierType")]
        public string AcqTermsIdentifierType { get; set; }

        /// <summary>
        /// The Place of the item creation
        /// </summary>
        [XmlElement("acqterms_place")]
        public string AcqTermsPlace { get; set; }

        /// <summary>
        ///  The creator of the item
        /// </summary>
        [XmlElement("creator")]
        public string Creator { get; set; }
        /// <summary>
        /// The date of the item
        /// </summary>
        [XmlElement("date")]
        public string Date { get; set; }

        /// <summary>
        /// The publisher of the item
        /// </summary>
        [XmlElement("publisher")]
        public string Publisher { get; set; }
    }
}
