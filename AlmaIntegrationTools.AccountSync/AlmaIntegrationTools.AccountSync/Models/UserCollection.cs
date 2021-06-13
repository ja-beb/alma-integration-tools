using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.AccountSync.Models
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    [XmlRoot("users", IsNullable = false)]
    public class UserCollection
    {
        /// <summary>
        /// Collection of users being exported.
        /// </summary>
        [XmlElement("user")]
        public List<User> Users { get; set; }

        /// <summary>
        /// Count of users being exported.
        /// </summary>
        [XmlAttribute("total_record_count")] 
        public int Count { 
            get => Users.Count;
            set { }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UserCollection()
        {
            Users = new ();
        }

        /// <summary>
        /// Add new user to this collection.
        /// </summary>
        /// <param name="user"></param>
        public void Add(User user) => this.Users.Add(user);
    }
}
