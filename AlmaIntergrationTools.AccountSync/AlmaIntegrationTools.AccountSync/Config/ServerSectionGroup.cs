using System.Configuration;

namespace AlmaIntegrationTools.AccountSync.Config
{
    public class ServersSectionGroup : AlmaIntegrationTools.Config.ServersSectionGroup
    {
        [ConfigurationProperty("reader", IsRequired = true)]
        public ReaderConfig Reader
        {
            get => base.Sections["reader"] as ReaderConfig;
        }

        static public ServersSectionGroup Instance() => ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).SectionGroups["sync"] as ServersSectionGroup;
    }
}
