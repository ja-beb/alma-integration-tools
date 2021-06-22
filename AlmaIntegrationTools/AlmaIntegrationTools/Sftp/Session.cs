using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                sessionOptions.Username,
                new PrivateKeyAuthenticationMethod(sessionOptions.Username, new PrivateKeyFile[] { new(sessionOptions.PrivateKeyPath) })
            );
            SftpClient = new(connectionInfo);
        }

        /// <summary>
        /// Open connection.
        /// </summary>
        public void Open()
        {
            if (!string.IsNullOrEmpty(SessionOptions.HostKeyFingerprint))
            {
                SftpClient.HostKeyReceived += (object sender, HostKeyEventArgs e) => e.CanTrust = String.Format("{0} {1} {2}", e.HostKeyName, e.KeyLength, Convert.ToBase64String(new SHA256Managed().ComputeHash(e.HostKey))) == SessionOptions.HostKeyFingerprint;
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
        public FileInfo GetFiles(string remotePath, string localPath, bool remove) => GetFiles(remotePath, localPath, remove, filename => !filename.StartsWith(".")).FirstOrDefault();

        /// <summary>
        /// Get remote files.
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localPath"></param>
        /// <param name="remove"></param>
        /// <param name="filter"></param>
        public IReadOnlyList<FileInfo> GetFiles(string remotePath, string localPath, bool remove, Func<string,bool> filter)
        {
            List<FileInfo> files = new();
            foreach (SftpFile sftpFile in SftpClient.ListDirectory(remotePath))
            {
                if (filter(sftpFile.Name))
                {
                    string filename = Path.Combine(localPath, sftpFile.Name);
                    GetFile($"{remotePath}/{sftpFile.Name}", filename, remove);
                    files.Add(new FileInfo(filename));
                }
            }
            return files.AsReadOnly();
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
