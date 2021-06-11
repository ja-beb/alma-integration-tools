using System.Configuration;

namespace AlmaIntegrationTools.AccountSync.Config
{
    public class ServersSectionGroup : ConfigurationSectionGroup
    {
        [ConfigurationProperty("import", IsRequired = true)]
        public ServerConfig ImportServer
        {
            get => base.Sections["import"] as ServerConfig;
        }

        [ConfigurationProperty("export", IsRequired = true)]
        public ServerConfig ExportServer
        {
            get => base.Sections["export"] as ServerConfig;
        }

        [ConfigurationProperty("path", IsRequired = true)]
        public PathConfig Path
        {
            get => base.Sections["path"] as PathConfig;
        }

        [ConfigurationProperty("reader", IsRequired = true)]
        public ReaderConfig Reader
        {
            get => base.Sections["reader"] as ReaderConfig;
        }
    }
}
