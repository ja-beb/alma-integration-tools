using AlmaIntegrationTools.AccountSync.Config;
using AlmaIntegrationTools.AccountSync.Models;
using AlmaIntegrationTools.AccountSync.Reader;
using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using WinSCP;

namespace AlmaIntegrationTools.AccountSync
{

    /// <summary>
    /// Main program execution.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main execution. 
        /// </summary>
        /// <param name="args"></param>
        public static void Main()
        {
            try
            {
                // init.
                Config.ServersSectionGroup serversSectionGroup = Config.ServersSectionGroup.Instance();
                DirectoryInfo baseDirectory = CreateDirectory(new string[] { 
                    serversSectionGroup.Path.Value, 
                    Guid.NewGuid().ToString("N") 
                });

                // import.
                FileInfo[] files = Fetch(new SessionOptions()
                    {
                        Protocol = Protocol.Sftp,
                        HostName = serversSectionGroup.ImportServer.Host,
                        PortNumber = serversSectionGroup.ImportServer.Port,
                        UserName = serversSectionGroup.ImportServer.User,
                        SshHostKeyFingerprint = serversSectionGroup.ImportServer.Fingerprint,
                        SshPrivateKeyPath = serversSectionGroup.ImportServer.Key,
                    },
                    serversSectionGroup.ImportServer.Path, 
                    CreateDirectory(new string[] { baseDirectory.FullName, "import" })
                 );

                // convert.
                DirectoryInfo exportDirectory = CreateDirectory(new string[] { 
                    baseDirectory.FullName, 
                    DateTime.Now.ToString("yyyyMMddhhmmss") 
                });
                ConvertToXml(files, exportDirectory, serversSectionGroup.Reader);
                
                // send.
                FileInfo exportFile = Compress(exportDirectory);
                Send(new SessionOptions()
                    {
                        Protocol = Protocol.Sftp,
                        HostName = serversSectionGroup.ExportServer.Host,
                        PortNumber = serversSectionGroup.ExportServer.Port,
                        UserName = serversSectionGroup.ExportServer.User,
                        SshHostKeyFingerprint = serversSectionGroup.ExportServer.Fingerprint,
                        SshPrivateKeyPath = serversSectionGroup.ExportServer.Key,
                    },
                    exportFile,
                    serversSectionGroup.ExportServer.Path
                );

                // cleanup.
                baseDirectory.Delete(true);
            }

            catch (ArgumentException exception)
            {
                LogError(exception);
            }

            catch (IOException exception)
            {
                LogError(exception);
            }

            catch (FormatException)
            {
                LogError("Invalid configuration setting: update the application config file.");
            }
        }

        /// <summary>
        /// Log program error.
        /// </summary>
        /// <param name="exception"></param>
        static void LogError(Exception exception) => LogError(exception.Message);

        /// <summary>
        /// Log program error.
        /// </summary>
        /// <param name="message"></param>
        static void LogError(string message) => Console.Error.WriteLine(message);

        /// <summary>
        /// Fetch files from remote server.
        /// </summary>
        /// <param name="sessionOptions"></param>
        /// <param name="remotePath"></param>
        /// <param name="directoryInfo"></param>
        /// <returns></returns>
        static public FileInfo[] Fetch(SessionOptions sessionOptions, string remotePath, DirectoryInfo directoryInfo)
        {
            using Session importSession = new();
            {
                importSession.Open(sessionOptions);
                importSession.GetFiles(remotePath, directoryInfo.FullName, false);
                importSession.Close();
            }
            return directoryInfo.GetFiles("*.sif");
        }

        /// <summary>
        /// Convert files from SIF to XML format.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="directoryInfo"></param>
        /// <param name="config"></param>
        static public void ConvertToXml(FileInfo[] files, DirectoryInfo directoryInfo, ReaderConfig config)
        {
            foreach (FileInfo fileInfo in files)
            {
                UserCollection collection = new();
                using IPatronReader<User> reader = new PatronFileReader(new StreamReader(fileInfo.FullName), str => Regex.Replace(str, @"[\u0001]", Regex.Replace(str, @"[^\u0020-\u007E]", " ")))
                {
                    CountryCodes = CountryCodes.Fetch(),
                    CampusCode = config.Campus,
                    EmailFormat = string.Format("@{0}", config.Domain),
                };
                {
                    reader.Open();
                    for (User user = reader.ReadNext(); null != user; user = reader.ReadNext())
                    {
                        collection.Add(user);
                    }
                    reader.Close();
                }

                // Serialize output.
                string filename = String.Format("{0}.xml", 0 == fileInfo.Extension.Length ? fileInfo.Name : fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length));
                using FileStream stream = new FileInfo(Path.Combine(directoryInfo.FullName, filename)).OpenWrite();
                {
                    XmlSerializer serializer = new(collection.GetType());
                    XmlSerializerNamespaces myNamespaces = new();
                    myNamespaces.Add("", "");
                    serializer.Serialize(stream, collection, myNamespaces);
                }
            }
        }

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
        /// Send files to export server.
        /// </summary>
        /// <param name="sessionOptions"></param>
        /// <param name="fileInfo"></param>
        /// <param name="remotePath"></param>
        static public void Send(SessionOptions sessionOptions, FileInfo fileInfo, string remotePath)
        {
            // Send to export server and remove zip file.
            using Session exportSession = new();
            {
                exportSession.Open(sessionOptions);
                exportSession.PutFiles(fileInfo.FullName, String.Format("{0}/{1}", remotePath, fileInfo.Name), true);
                exportSession.Close();
            }
        }

        /// <summary>
        /// Create directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static DirectoryInfo CreateDirectory(string[] path) => CreateDirectory(Path.Combine(path));

        /// <summary>
        /// Create directory.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static DirectoryInfo CreateDirectory(string path)
        {
            DirectoryInfo directoryInfo = new(path);
            directoryInfo.Create();
            return directoryInfo;
        }

    }
}
