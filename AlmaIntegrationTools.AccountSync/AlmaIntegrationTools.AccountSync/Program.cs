using AlmaIntegrationTools.AccountSync.Config;
using AlmaIntegrationTools.AccountSync.Models;
using AlmaIntegrationTools.AccountSync.Reader;
using AlmaIntergrationTools;
using Renci.SshNet.Common;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.AccountSync
{

    /// <summary>
    /// Main program execution.
    /// </summary>
    public class Program : SyncProgram
    {


        /// <summary>
        /// Main execution.
        /// </summary>
        /// <param name="fileExtension"></param>
        public override void Run()
        {
            try
            {
                FileInfo[] files = Fetch("sif");
                ConvertToXml(files, ExportDirectory, (Config as ServersSectionGroup)?.Reader);
                FileInfo exportFile = Compress(ExportDirectory);
                Send(exportFile);
                BaseDirectory.Delete(true);
            }

            catch (SshAuthenticationException exception)
            {
                LogError(exception);
            }

            catch (ArgumentException exception)
            {
                LogError(exception);
            }

            catch (IOException exception)
            {
                LogError(exception);
            }

            // Invalid configuration error.
            catch (FormatException)
            {
                LogError("Invalid configuration setting: update the application config file.");
            }
        }

        /// <summary>
        /// Main execution. 
        /// </summary>
        /// <param name="args"></param>
        public static void Main()
        {
            SyncProgram syncProgram = new Program()
            {
                Config = AccountSync.Config.ServersSectionGroup.Instance()
            };
            syncProgram.Run();
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

    }
}
