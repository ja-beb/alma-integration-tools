using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.AccountSync.Models
{
    /// <summary>
    ///  List of the user's contacts information.
    /// </summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class ContactInfo
    {
        /// <summary>
        /// List of user's addresses.
        /// </summary>
        [XmlArray("addresses")]
        [XmlArrayItem("address", IsNullable = false, Type = typeof(Address))]
        public List<Address> Addresses { get; set; }

        /// <summary>
        /// List of user's emails.
        /// </summary>
        [XmlArray("emails")]
        [XmlArrayItem("email", IsNullable = false, Type = typeof(Email))]
        public List<Email> Emails { get; set; }

        /// <summary>
        /// List of user's phones.
        /// </summary>
        [XmlArray("phones")]
        [XmlArrayItem("phone", IsNullable = false, Type = typeof(Phone))]
        public List<Phone> Phones { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ContactInfo()
        {
            Addresses = new();
            Emails = new(); 
            Phones = new();
        }
    }
}
