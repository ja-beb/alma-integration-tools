namespace AlmaIntegrationTools.Settings
{
    /// <summary>
    /// Sync program settings.
    /// </summary>
    public class SyncSettings
    {
        /// <summary>
        /// Base working direectory for processing files.
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// File extension to pickup from remote server.
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Export server settings.
        /// </summary>
        public TransferSettings ExportSettings { get; set; }

        /// <summary>
        /// Import server settings.
        /// </summary>
        public TransferSettings ImportSettings { get; set; }
    }
}
