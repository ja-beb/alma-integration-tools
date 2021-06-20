namespace AlmaIntegrationTools.Sftp
{
    /// <summary>
    /// SFTP Session options.
    /// </summary>
    public class SessionOptions
    {
        /// <summary>
        /// Remote hostname.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Remote host port number.
        /// </summary>
        public int PortNumber { get; set; }

        /// <summary>
        /// User to use for upload.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Servers SSH host key fingerprint.
        /// </summary>
        public string HostKeyFingerprint { get; set; }

        /// <summary>
        /// SSH Private key path (does not support putty keys, must be converted to OpenSSL using putty-gen)
        /// </summary>
        public string PrivateKeyPath { get; set; }

    }
}
