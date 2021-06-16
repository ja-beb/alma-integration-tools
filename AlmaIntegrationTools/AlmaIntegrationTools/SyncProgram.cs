using AlmaIntegrationTools.Config;
using AlmaIntegrationTools.Sftp;
using System;
using System.IO;
using System.IO.Compression;

namespace AlmaIntergrationTools
{
    /// <summary>
    /// Base sync program.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    abstract public class SyncProgram
    {
        /// <summary>
        /// Sync program config options.
        /// </summary>
        public ServersSectionGroup Config { get; set; }

        /// <summary>
        /// Sync program base working directory.
        /// </summary>
        internal DirectoryInfo baseDirectory;
        public DirectoryInfo BaseDirectory
        {
            get
            {
                if (null == baseDirectory)
                {
                    baseDirectory = CreateDirectory(Path.Combine(Config?.Path.Value, Guid.NewGuid().ToString("N")));
                }
                return baseDirectory;
            }
        }

        /// <summary>
        /// Export directory for processed files.
        /// </summary>
        internal DirectoryInfo exportDirectory;
        public DirectoryInfo ExportDirectory
        {
            get
            {
                if (null == exportDirectory)
                {
                    exportDirectory = CreateDirectory(Path.Combine(BaseDirectory.FullName, DateTime.Now.ToString("yyyyMMddhhmmss")));
                }
                return exportDirectory;
            }
        }

        /// <summary>
        /// Fetch files from remote server.
        /// </summary>
        /// <param name="sessionOptions"></param>
        /// <param name="remotePath"></param>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public FileInfo[] Fetch(string fileExtension)
        {
            using Session session = new(new SessionOptions()
            {
                HostName = Config?.ImportServer.Host,
                PortNumber = null == Config ? 0 : Config.ImportServer.Port,
                UserName = Config?.ImportServer.User,
                SshPrivateKeyPath = Config?.ImportServer.Key,
                SshHostKeyFingerprint = Config?.ImportServer.Fingerprint
            });
            {
                session.Open();
                session.GetFiles(Config?.ImportServer.Path, BaseDirectory.FullName, false, filename => !filename.StartsWith('.') && filename.EndsWith($".{fileExtension}"));
                session.Close();
            }
            return BaseDirectory.GetFiles($"*.{fileExtension}");
        }

        /// <summary>
        /// Send files to export server.
        /// </summary>
        /// <param name="sessionOptions"></param>
        /// <param name="fileInfo"></param>
        /// <param name="remotePath"></param>
        public void Send(FileInfo fileInfo)
        {
            using Session session = new(new SessionOptions()
            {
                HostName = Config?.ExportServer.Host,
                PortNumber = null == Config ? 0 : Config.ExportServer.Port,
                UserName = Config?.ExportServer.User,
                SshPrivateKeyPath = Config?.ExportServer.Key,
                SshHostKeyFingerprint = Config?.ExportServer.Fingerprint
            });
            {
                session.Open();
                session.PutFile(fileInfo.FullName, String.Format($"{Config?.ExportServer.Path}/{fileInfo.Name}"), true);
                session.Close();
            }
        }
        /// <summary>
        /// Main execution.
        /// </summary>
        /// <param name="fileExtension"></param>
        abstract public void Run();

        /// <summary>
        /// Log program error.
        /// </summary>
        /// <param name="exception"></param>
        static public void LogError(Exception exception) => LogError(exception.Message);

        /// <summary>
        /// Log program error.
        /// </summary>
        /// <param name="message"></param>
        static public void LogError(string message) => Console.Error.WriteLine(message);

        /// <summary>
        /// Compress directory to send.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        static public FileInfo Compress(DirectoryInfo directoryInfo)
        {
            FileInfo zipFile = new(String.Format("{0}.zip", directoryInfo.FullName));
            ZipFile.CreateFromDirectory(directoryInfo.FullName, zipFile.FullName);
            return zipFile;
        }

        /// <summary>
        /// Create directory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public DirectoryInfo CreateDirectory(string path)
        {
            DirectoryInfo baseDirectory = new(path);
            baseDirectory.Create();
            return baseDirectory;
        }
    }
}
