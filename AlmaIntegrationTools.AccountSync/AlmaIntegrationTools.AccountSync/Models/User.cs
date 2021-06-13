using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.AccountSync.Models
{

    [Serializable]
    public class User
    {
        /// <summary>
        /// The code of the campus related to the user.
        /// </summary>
        [XmlElement("campus_code")]
        public string CampusCode { get; set; }

        /// <summary>
        /// List of the user's contacts information.
        /// </summary>
        [XmlElement("contact_info")]
        public ContactInfo ContactInfo { get; set; }

        /// <summary>
        /// The date after which the user no longer has the role.
        /// </summary>
        [XmlElement("expiry_date", typeof(System.DateTime), DataType = "date")]
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// The user's first name.
        /// </summary>
        [XmlElement("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// The user's middle name.
        /// </summary>
        [XmlElement("middle_name")]
        public string MiddleName { get; set; }

        /// <summary>
        /// The user's last name.
        /// </summary>
        [XmlElement("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// The user's middle name.
        /// </summary>
        [XmlElement("full_name")]
        public string FullName
        {
            get {
                if (string.IsNullOrEmpty(MiddleName)) return String.Format("{0} {1}", FirstName, LastName);
                else if (1 == MiddleName.Length) return String.Format("{0} {1}. {2}", FirstName, MiddleName, LastName);
                else return String.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
            }
            set { }
        }

        /// <summary>
        /// The user's preferred first name.
        /// </summary>
        [XmlElement("pref_first_name")]
        public string PreferredFirstName { get; set; }

        /// <summary>
        /// The user's preferred last name.
        /// </summary>
        [XmlElement("pref_last_name")]
        public string PreferredLastName { get; set; }

        /// <summary>
        /// The user's preferred middle name.
        /// </summary>
        [XmlElement("pref_middle_name")]
        public string PreferredMiddleName { get; set; }

        /// <summary>
        /// The user's preferred language.
        /// </summary>
        [XmlElement("preferred_language")]
        public string PreferredLanguage { get; set; }

        /// <summary>
        /// The primary identifier of the user.
        /// </summary>
        [XmlElement("primary_id")]
        public string PrimaryId { get; set; }

        /// <summary>
        /// The date on which the user is purged from the system.
        /// </summary>
        [XmlElement("purge_date", typeof(System.DateTime), DataType = "date")]
        public DateTime PurgeDate { get; set; }

        /// <summary>
        /// Status of user account.
        /// </summary>
        [XmlElement("status")]
        public string Status { get; set; }

        /// <summary>
        /// The date of the last update to user status.
        /// </summary>
        [XmlElement("status_date", typeof(System.DateTime), DataType = "date")]
        public DateTime StatusDate { get; set; }

        /// <summary>
        /// The group within the institution to which the user belongs.
        /// </summary>
        [XmlElement("user_group")]
        public string Group { get; set; }

        /// <summary>
        /// List of the user's additional identifiers.
        /// </summary>
        [XmlArray("user_identifiers")]
        [XmlArrayItem("user_identifier", typeof(Identifier))]
        public List<Identifier> Identifiers { get; set; }

        /// <summary>
        /// List of the user's related notes.
        /// </summary>
        [XmlArray("user_notes")]
        [XmlArrayItem("user_note", typeof(Note))]
        public List<Note> Notes { get; set; }

        /// <summary>
        /// List of the user's related statistics.
        /// </summary>
        [XmlArray("user_statistics")]
        [XmlArrayItem("user_statistic", Type = typeof(Statistic))]
        public List<Statistic> Statistics { get; set; }

        /// <summary>
        /// The user's title
        /// </summary>
        [XmlElement("user_title")]
        public string Title { get; set; }

        /// <summary>
        /// Determine if this is a student based on user's group.
        /// </summary>
        [XmlIgnore]
        public bool IsStudent
        {
            get => Group switch
            {
                "UNDG" => true,
                "GRAD" => true,
                "POSTDOC" => true,
                _ => false
            };
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public User()
        {
            Identifiers = new();
            Notes = new();
            Statistics = new();
            ContactInfo = new();
        }
    }

}
