using System.Configuration;

namespace AlmaIntegrationTools.Config
{
    public class PathConfig : ConfigurationSection
    {
        [ConfigurationProperty("value", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Value
        {
            get => this["value"]?.ToString();
            set => this["value"] = value;
        }
    }
}
