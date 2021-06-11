using System;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.AccountSync.Models
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class Identifier
    {
        /// <summary>
        /// The identifier type.
        /// </summary>
        [XmlElement("id_type", Type = typeof(string))]
        public string Type { get; set; }

        /// <summary>
        /// The identifier value.
        /// </summary>
        [XmlElement("value", Type = typeof(string))]
        public string Value { get; set; }

        /// <summary>
        /// identifier's status.
        /// </summary>
        [XmlElement("status", Type = typeof(string))]
        public string Status { get; set; }

        /// <summary>
        /// identifier's note.
        /// </summary>
        [XmlElement("note", Type = typeof(string))]
        public string Note { get; set; }

        /// <summary>
        /// Construct instance of a user identifer.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="string"></param>
        public Identifier(string type, string value, string status)
        {
            Type = type;
            Value = value;
            Status = status;
        }

        /// <summary>
        /// Construct an active instance of the user identifer.
        /// </summary>
        /// <param name="type"></param>s
        /// <param name="value"></param>
        public Identifier(string type, string value) : this(type, value, "ACTIVE")
        { }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Identifier()
        { }
    }
}
