using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.AccountSync.Models
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class Email : BaseInfo
    {
        /// <summary>
        /// The email address. Mandatory.
        /// </summary>
        [XmlElement("email_address", Type = typeof(string))]
        public string Address { get; set; }

        /// <summary>
        /// The email address' related description.
        /// </summary>
        [XmlElement("description", Type = typeof(string))]
        public string Description { get; set; }

        /// <summary>
        /// Email types.
        /// </summary>
        [XmlArray("email_types")]
        [XmlArrayItem("email_type", Type = typeof(string))]
        public List<string> Types { get; set; }

        /// <summary>
        /// Construct instance of the email class.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="description"></param>
        /// <param name="types"></param>
        public Email(string address, string description, string[] types)
        {
            this.Address = address;
            this.Description = description;
            this.Types = types.ToList();
            IsPreferred = false;
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Email()
        { }
    }

}
