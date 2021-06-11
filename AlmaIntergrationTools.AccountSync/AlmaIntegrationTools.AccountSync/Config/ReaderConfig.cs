using System.Configuration;

namespace AlmaIntegrationTools.AccountSync.Config
{
    public class ReaderConfig : ConfigurationSection
    {
        [ConfigurationProperty("campus", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Campus
        {
            get => this["campus"]?.ToString();
            set => this["campus"] = value;
        }

        [ConfigurationProperty("domain", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Domain
        {
            get => this["domain"]?.ToString();
            set => this["domain"] = value;
        }
    }
}
