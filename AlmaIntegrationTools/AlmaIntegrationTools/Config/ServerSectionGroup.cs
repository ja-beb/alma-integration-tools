using System.Configuration;

namespace AlmaIntegrationTools.Config
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

        static public ServersSectionGroup Instance() => ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).SectionGroups["sync"] as ServersSectionGroup;
    }
}
