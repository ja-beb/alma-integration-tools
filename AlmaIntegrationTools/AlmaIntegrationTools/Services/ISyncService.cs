using System.Collections.Generic;
using System.IO;

namespace AlmaIntegrationTools.Services
{
    /// <summary>
    /// Sync service used to pull files from a remote SFTP server, parse file and upload as zip to new server.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISyncService<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        IReadOnlyList<FileInfo> Fetch(DirectoryInfo directoryInfo);

        /// <summary>
        /// Parse data from current format to new format.
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="directory"></param>
        void Parse(FileInfo fileInfo, DirectoryInfo directory);

        /// <summary>
        /// Write data to new file.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="data"></param>
        void Write(DirectoryInfo directoryInfo, T data);

        /// <summary>
        /// Send files to server.
        /// </summary>
        /// <param name="directoryInfo"></param>
        void Send(DirectoryInfo directoryInfo);
    }
}
