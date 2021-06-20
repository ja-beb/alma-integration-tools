using AlmaIntegrationTools.Sftp;

namespace AlmaIntegrationTools.Settings
{
    /// <summary>
    /// Transfer settings.
    /// </summary>
    public class TransferSettings
    {
        /// <summary>
        /// Path on remote server.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Connection session options.
        /// </summary>
        public SessionOptions SessionOptions { get; set; }
    }
}
