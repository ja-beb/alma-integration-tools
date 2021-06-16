using System.Configuration;

namespace AlmaIntegrationTools.Config
{
    /// <summary>
    /// SFTP Server configuration object.
    /// </summary>
    public class ServerConfig : ConfigurationSection
    {
        /// <summary>
        /// Server host.
        /// </summary>
        [ConfigurationProperty("host", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Host
        {
            get => this["host"]?.ToString();
            set => this["host"] = value;
        }

        /// <summary>
        /// Server port (defaults to 22).
        /// </summary>
        [ConfigurationProperty("port", DefaultValue = 22, IsKey = true, IsRequired = true)]
        public int Port
        {
            get => int.TryParse(this["port"]?.ToString(), out int result) ? result : 22;
            set => this["host"] = value;
        }

        /// <summary>
        /// Username.
        /// </summary>
        [ConfigurationProperty("user", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string User
        {
            get => this["user"]?.ToString();
            set => this["user"] = value;
        }

        /// <summary>
        /// Path to server folder.
        /// </summary>
        [ConfigurationProperty("path", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Path
        {
            get => this["path"]?.ToString();
            set => this["path"] = value;
        }

        /// <summary>
        /// User's private key to access server.
        /// </summary>
        [ConfigurationProperty("key", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Key
        {
            get => this["key"]?.ToString();
            set => this["key"] = value;
        }

        /// <summary>
        /// Server's fingerprint.
        /// </summary>
        [ConfigurationProperty("fingerprint", DefaultValue = "", IsKey = true, IsRequired = false)]
        public string Fingerprint
        {
            get => this["fingerprint"]?.ToString();
            set => this["fingerprint"] = value;
        }
    }

}
