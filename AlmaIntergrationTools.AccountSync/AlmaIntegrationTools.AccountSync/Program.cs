using AlmaIntegrationTools.AccountSync.Models;
using AlmaIntegrationTools.AccountSync.Reader;
using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
using WinSCP;
using AlmaIntegrationTools.Config;

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
                // Load config
                AlmaIntegrationTools.AccountSync.Config.ServersSectionGroup serviceConfigSection = AlmaIntegrationTools.AccountSync.Config.ServersSectionGroup.Instance();

                // create folder structures:
                DirectoryInfo baseDirectory = new(serviceConfigSection.Path.Value);
                if (!baseDirectory.Exists) baseDirectory.Create();

                DirectoryInfo workingDirectory = CreateDirectory(new string[]{
                    baseDirectory.FullName,
                    DateTime.Now.ToString("yyyyddMMHHmmss")
                });

                DirectoryInfo uploadDirectory = CreateDirectory(new string[] {
                    workingDirectory.FullName,
                    "temp"
                });

                // download files from server
                FileInfo[] files = DownloadFiles(serviceConfigSection.ImportServer.Path, uploadDirectory, new SessionOptions()
                {
                    Protocol = Protocol.Sftp,
                    HostName = serviceConfigSection.ImportServer.Host,
                    PortNumber = serviceConfigSection.ImportServer.Port,
                    UserName = serviceConfigSection.ImportServer.User,
                    SshHostKeyFingerprint = serviceConfigSection.ImportServer.Fingerprint,
                    SshPrivateKeyPath = serviceConfigSection.ImportServer.Key,
                });

                // Parse import file.
                foreach (FileInfo fileInfo in files)
                {
                    using IPatronReader<User> reader = new PatronFileReader(new StreamReader(fileInfo.FullName))
                    {
                        CountryCodes = CountryCodes.Fetch(),
                        CampusCode = serviceConfigSection.Reader.Campus,
                        SchoolEmailFormat = string.Format("@{0}", serviceConfigSection.Reader.Domain),
                    };
                    ParseFile(reader, new(Path.Combine(workingDirectory.FullName, String.Format("{0}.xml", 0 == fileInfo.Extension.Length ? fileInfo.Name : fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length)))));
                }

                // remove upload folder
                uploadDirectory.Delete(true);

                // zip and send parsed files
                FileInfo zipFile = CompressOutput(workingDirectory);
                Send(zipFile, String.Format("{0}/{1}", serviceConfigSection.ExportServer.Path, zipFile.Name), new()
                {
                    Protocol = Protocol.Sftp,
                    HostName = serviceConfigSection.ExportServer.Host,
                    PortNumber = serviceConfigSection.ExportServer.Port,
                    UserName = serviceConfigSection.ExportServer.User,
                    SshHostKeyFingerprint = serviceConfigSection.ExportServer.Fingerprint,
                    SshPrivateKeyPath = serviceConfigSection.ExportServer.Key,
                });

                // remove working folders and zip
                workingDirectory.Delete(true);
                zipFile.Delete();
            }

            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
            }

            catch (IOException exception)
            {
                Console.Error.WriteLine(exception.Message);
            }

            catch (FormatException)
            {
                Console.Error.WriteLine("Invalid configuration setting: update the application config file.");
            }
        }

        /// <summary>
        /// Parse file.
        /// </summary>
        /// <param name="file"></param>
        static void ParseFile(IPatronReader<User> reader, FileInfo exportFile)
        {
            UserCollection collection = new();
            reader.Open();
            for (User user = reader.ReadNext(); null != user; user = reader.ReadNext()) collection.Add(user);
            reader.Close();

            //Create the serializer
            FileStream stream = exportFile.OpenWrite();
            XmlSerializer serializer = new(typeof(UserCollection));
            XmlSerializerNamespaces myNamespaces = new();
            myNamespaces.Add("", "");
            serializer.Serialize(stream, collection, myNamespaces);
            stream.Close();
        }

        /// <summary>
        /// Create compressed version of file.
        /// </summary>
        static FileInfo CompressOutput(DirectoryInfo path)
        {
            FileInfo file = new(String.Format("{0}.zip", path.FullName));
            ZipFile.CreateFromDirectory(path.FullName, file.FullName);
            return file;
        }

        /// <summary>
        /// Send file to serve using the SSH.Net API.
        /// </summary>
        /// <param name="file"></param>
        static void Send(FileInfo file, string remoteFile, SessionOptions sessionOptions)
        {
            using Session session = new();
            {
                session.Open(sessionOptions);
                session.PutFiles(file.FullName, remoteFile, true);
                session.Close();
            }
        }

        /// <summary>
        /// Create directory for processing files.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static DirectoryInfo CreateDirectory(string[] path)
        {
            DirectoryInfo directoryInfo = new(Path.Combine(path));
            if (directoryInfo.Exists) directoryInfo.Delete();
            directoryInfo.Create();
            return directoryInfo;
        }

        /// <summary>
        /// Download files from server
        /// </summary>
        /// <param name="remotePath"></param>
        /// <param name="localDirectory"></param>
        /// <param name="sessionOptions"></param>
        /// <returns></returns>
        static FileInfo[] DownloadFiles(string remotePath, DirectoryInfo localDirectory, SessionOptions sessionOptions)
        {
            using Session session = new();
            {
                session.Open(sessionOptions);
                session.GetFiles(remotePath, localDirectory.FullName, false);
                session.Close();
            }
            return localDirectory.GetFiles("*.txt");
        }
    }
}
