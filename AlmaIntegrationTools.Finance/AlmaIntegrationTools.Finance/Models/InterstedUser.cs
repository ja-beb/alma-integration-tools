using System.Xml.Serialization;

namespace AlmaIntergrationTools.Finance
{
    public class InterstedUser
    {

        /// <summary>
        /// The first name as shown on alma
        /// </summary>
        [XmlElement("first_name")]
        public string NameFirst { get; set; }

                /// <summary>
        /// The last name as shown on alma
        /// </summary>
        [XmlElement("last_name")]
        public string NameLast { get; set; }

        /// <summary>
        /// The user name 
        /// </summary>
        [XmlElement("user_name")]
        public string Username { get; set; }


        /// <summary>
        /// The user perfered email
        /// </summary>
        [XmlElement("email")]
        public string Email { get; set; }

        /// <summary>
        /// The user alma identifier id
        /// </summary>
        [XmlElement("id")]
        public string Id { get; set; }
    }
}
