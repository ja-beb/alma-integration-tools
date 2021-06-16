using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System;
using System.IO;
using System.Security.Cryptography;

namespace AlmaIntegrationTools.Sftp
{
    /// <summary>
    /// SFTP session.
    /// </summary>
    public class Session : IDisposable
    {
        /// <summary>
        /// Is disposed flag.
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// SFTP client options.
        /// </summary>
        SessionOptions SessionOptions { get; set; }

        /// <summary>
        /// SFTP Client instance.
        /// </summary>
        SftpClient SftpClient { get; set; }

        /// <summary>
        /// Cosntruct session.
        /// </summary>
        /// <param name="sessionOptions"></param>
        public Session(SessionOptions sessionOptions)
        {
            SessionOptions = sessionOptions;
            ConnectionInfo connectionInfo = new(
                sessionOptions.HostName,
                sessionOptions.PortNumber,
                sessionOptions.UserName,
                new PrivateKeyAuthenticationMethod(sessionOptions.UserName, new PrivateKeyFile[] { new(sessionOptions.SshPrivateKeyPath) })
            );
            SftpClient = new(connectionInfo);
        }

        /// <summary>
        /// Open connection.
        /// </summary>
        public void Open()
        {
            if (!string.IsNullOrEmpty(SessionOptions.SshHostKeyFingerprint))
            {
                SftpClient.HostKeyReceived += (object sender, HostKeyEventArgs e) => e.CanTrust = String.Format("{0} {1} {2}", e.HostKeyName, e.KeyLength, Convert.ToBase64String(new SHA256Managed().ComputeHash(e.HostKey))) == SessionOptions.SshHostKeyFingerprint;
            }
            SftpClient.Connect();
        }

        /// <summary>
        /// Close connection.
        /// </summary>
        public void Close()
        {
            SftpClient.Disconnect();
        }

        /// <summary>
        /// Get remote files.
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localPath"></param>
        /// <param name="remove"></param>
        public void GetFiles(string remotePath, string localPath, bool remove) => GetFiles(remotePath, localPath, remove, filename => !filename.StartsWith("."));

        /// <summary>
        /// Get remote files.
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localPath"></param>
        /// <param name="remove"></param>
        /// <param name="filter"></param>
        public void GetFiles(string remotePath, string localPath, bool remove, Func<string,bool> filter)
        {
            foreach (SftpFile sftpFile in SftpClient.ListDirectory(remotePath))
            {
                if (filter(sftpFile.Name)) GetFile(String.Format($"{remotePath}/{sftpFile.Name}"), Path.Combine(localPath, sftpFile.Name), remove);
            }
        }

        public void GetFile(string remoteFile, string localFile, bool remove)
        {
            using Stream stream = File.OpenWrite(localFile);
            {
                SftpClient.DownloadFile(remoteFile, stream);
                if (remove) SftpClient.DeleteFile(remoteFile);
            }
        }

        /// <summary>
        /// Put multiple files on server.
        /// </summary>
        /// <param name="localPaths"></param>
        /// <param name="remotePath"></param>
        /// <param name="remove"></param>
        public void PutFiles(string[] localPaths, string remotePath, bool remove)
        {
            foreach (string path in localPaths) PutFile(path, remotePath, remove);
        }

        /// <summary>
        /// Put file on server.
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="remotePath"></param>
        /// <param name="remove"></param>
        public void PutFile(string localPath, string remotePath, bool remove)
        {
            FileInfo fileInfo = new(localPath);
            using var uplfileStream = fileInfo.OpenRead();
            {
                SftpClient.UploadFile(uplfileStream, remotePath, remove);
            }
        }

        /// <summary>
        /// Dispose of this object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of this object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing) SftpClient?.Dispose();
            _disposed = true;
        }
    }
}
