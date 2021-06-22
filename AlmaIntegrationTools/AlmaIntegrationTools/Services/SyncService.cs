using AlmaIntegrationTools.Settings;
using AlmaIntegrationTools.Sftp;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.Services
{
    abstract public class SyncService<T>
    {
        /// <summary>
        /// 
        /// </summary>
        internal readonly string FileExtension;

        /// <summary>
        /// 
        /// </summary>
        internal readonly TransferSettings ImportSettings;

        /// <summary>
        /// 
        /// </summary>
        internal readonly TransferSettings ExportSettings;

        /// <summary>
        /// 
        /// </summary>
        public DirectoryInfo DirectoryInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syncOptions"></param>
        public SyncService(IOptions<SyncSettings> syncOptions)
        {
            FileExtension = syncOptions.Value?.FileExtension;
            ImportSettings = syncOptions.Value?.ImportSettings;
            ExportSettings = syncOptions.Value?.ExportSettings;
        }

        /// <summary>
        /// Write data object.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="feed"></param>
        /// <returns></returns>
        abstract public void Write(DirectoryInfo directoryInfo, T feed);

        /// <summary>
        /// Fetch files from remote server.
        /// </summary>
        /// <param name="sessionOptions"></param>
        /// <param name="remotePath"></param>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public IReadOnlyList<FileInfo> Fetch(DirectoryInfo directoryInfo)
        {
            IReadOnlyList<FileInfo> files;
            using Session session = new(ImportSettings.SessionOptions);
            {
                session.Open();
                files = session.GetFiles(ImportSettings.Path, directoryInfo.FullName, false, filename => !filename.StartsWith('.') && filename.EndsWith($".{FileExtension}"));
                session.Close();
            }
            return files;
        }

        /// <summary>
        /// Compress file and send to remote server.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        public void Send(DirectoryInfo directoryInfo)
        {
            FileInfo zipFile = new($"{directoryInfo.FullName}.zip");
            ZipFile.CreateFromDirectory(directoryInfo.FullName, zipFile.FullName);
            using Session session = new(ExportSettings.SessionOptions);
            {
                session.Open();
                session.PutFile(zipFile.FullName, $"{ExportSettings.Path}/{zipFile.Name}", true);
                session.Close();
            }
        }

        /// <summary>
        /// Perform XML sync action.  Process file list from pickup directory to generate output zip.
        /// </summary>
        /// <returns></returns>
        public virtual void Parse(FileInfo file, DirectoryInfo directoryInfo)
        {
            XmlSerializer xmlSerializer = new(typeof(T));
            using FileStream fileStream = file.OpenRead();
            {
                Write(directoryInfo, (T)xmlSerializer.Deserialize(fileStream));
            }
        }
    }
}
