using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.AccountSync.Models
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class Address : BaseInfo
    {
        /// <summary>
        /// Address types.
        /// </summary>
        [XmlArray("address_types")]
        [XmlArrayItem("address_type", typeof(string))]
        public List<string> Types { get; set; }

        /// <summary>
        /// The address' relevant city.
        /// </summary>
        [XmlElement("city", typeof(string))]
        public string City { get; set; }

        /// <summary>
        /// The address' relevant country. Possible codes are listed in the 'Country Codes' code table.
        /// </summary>
        [XmlElement("country", typeof(string))]
        public string Country { get; set; }

        /// <summary>
        /// The date after which the address is no longer active.
        /// </summary>
        [XmlElement("end_date", typeof(DateTime), DataType = "date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Line 1 of the address.
        /// </summary>
        [XmlElement("line1", typeof(string))]
        public string Line1 { get; set; }

        /// <summary>
        /// Line 2 of the address.
        /// </summary>
        [XmlElement("line2", typeof(string))]
        public string Line2 { get; set; }

        /// <summary>
        /// Line 3 of the address.
        /// </summary>
        [XmlElement("line3", typeof(string))]
        public string Line3 { get; set; }

        /// <summary>
        /// Line 4 of the address.
        /// </summary>
        [XmlElement("line4", typeof(string))]
        public string Line4 { get; set; }

        /// <summary>
        /// Line 5 of the address.
        /// </summary>
        [XmlElement("line5", typeof(string))]
        public string Line5 { get; set; }

        /// <summary>
        /// The address' relevant postal code.
        /// </summary>
        [XmlElement("postal_code", typeof(string))]
        public string PostalCode { get; set; }

        /// <summary>
        /// The date from which the address is deemed to be active.
        /// </summary>
        [XmlElement("start_date", typeof(DateTime), DataType = "date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The address' relevant state.
        /// </summary>
        [XmlElement("state_province", typeof(string))]
        public string State { get; set; }

        /// <summary>
        /// Address constructor.
        /// </summary>
        /// <param name="line1"></param>
        /// <param name="city"></param>
        /// <param name="types"></param>
        public Address(string line1, string city, string[] types)
        {
            Line1 = line1;
            City = city;
            Types = types.ToList();
            IsPreferred = false;
        }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Address()
        { }
    }

}
