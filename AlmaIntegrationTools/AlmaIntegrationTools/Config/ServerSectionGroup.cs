using System.Configuration;

namespace AlmaIntegrationTools.Config
{
    /// <summary>
    /// Program configuration. 
    /// </summary>
    public class ServersSectionGroup : ConfigurationSectionGroup
    {
        /// <summary>
        /// Import server.
        /// </summary>
        [ConfigurationProperty("import", IsRequired = true)]
        public ServerConfig ImportServer
        {
            get => base.Sections["import"] as ServerConfig;
        }

        /// <summary>
        /// Export server.
        /// </summary>
        [ConfigurationProperty("export", IsRequired = true)]
        public ServerConfig ExportServer
        {
            get => base.Sections["export"] as ServerConfig;
        }

        /// <summary>
        /// Working path.
        /// </summary>
        [ConfigurationProperty("path", IsRequired = true)]
        public PathConfig Path
        {
            get => base.Sections["path"] as PathConfig;
        }

        /// <summary>
        /// Return instance of this object.
        /// </summary>
        /// <returns></returns>
        static public ServersSectionGroup Instance() => ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).SectionGroups["sync"] as ServersSectionGroup;
    }
}
