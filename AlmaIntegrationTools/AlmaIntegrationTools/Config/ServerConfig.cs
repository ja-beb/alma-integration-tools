using System.Configuration;

namespace AlmaIntegrationTools.Config
{
    public class ServerConfig : ConfigurationSection
    {
        [ConfigurationProperty("host", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Host
        {
            get => this["host"]?.ToString();
            set => this["host"] = value;
        }

        [ConfigurationProperty("port", DefaultValue = 22, IsKey = true, IsRequired = true)]
        public int Port
        {
            get => int.TryParse(this["port"]?.ToString(), out int result) ? result : 22;
            set => this["host"] = value;
        }

        [ConfigurationProperty("user", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string User
        {
            get => this["user"]?.ToString();
            set => this["user"] = value;
        }

        [ConfigurationProperty("path", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Path
        {
            get => this["path"]?.ToString();
            set => this["path"] = value;
        }

        [ConfigurationProperty("key", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Key
        {
            get => this["key"]?.ToString();
            set => this["key"] = value;
        }

        [ConfigurationProperty("fingerprint", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Fingerprint
        {
            get => this["fingerprint"]?.ToString();
            set => this["fingerprint"] = value;
        }
    }

}
