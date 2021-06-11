using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.AccountSync.Models
{

    [Serializable]

    [XmlType(AnonymousType = true)]
    public class Phone : BaseInfo
    {
        /// <summary>
        /// The phone number.
        /// </summary>
        [XmlElement("phone_number", Type = typeof(string))]
        public string Number { get; set; }

        /// <summary>
        /// The different Phone types.
        /// </summary>
        [XmlArray("phone_types")]
        [XmlArrayItem("phone_type", Type = typeof(string))]
        public List<string> Types { get; set; }

        /// <summary>
        /// Flag for if this number is preferred for SMS.
        /// </summary>
        [XmlAttribute("preferred_sms")]
        public bool IsPreferredSms { get; set; }

        /// <summary>
        /// Construct instance of the phone class.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="types"></param>
        public Phone(string number, string[] types)
        {
            Number = number;
            Types = types.ToList();
            IsPreferred = false;
            IsPreferredSms = false;
        }

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Phone()
        {
            Types = new List<string>();
        }
    }
}
