using System;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance.Models
{

    public class Note 
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("content")]
        public String Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlElement("owneredEntity")]
        public OwneredEntity OwneredEntity { get; set; }
    }
}


