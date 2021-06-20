using AlmaIntegrationTools.AccountSync.Models;
using AlmaIntegrationTools.AccountSync.Reader;
using AlmaIntegrationTools.AccountSync.Settings;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace AlmaIntegrationTools.AccountSync.Services
{
    /// <summary>
    /// The Bursar sync service. 
    /// This service pulls file from SFTP server, converts to plain text and uploads to another SFTP server.
    /// </summary>
    public class SyncService : AlmaIntegrationTools.Services.SyncService<UserCollection>, AlmaIntegrationTools.Services.ISyncService<UserCollection>
    {
        /// <summary>
        /// Default campus code.
        /// </summary>
        readonly string Campus;

        /// <summary>
        /// Domain for this university.
        /// </summary>
        readonly string Domain;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="syncOptions"></param>
        public SyncService(IOptions<SyncSettings> syncOptions) : base(syncOptions as IOptions<AlmaIntegrationTools.Settings.SyncSettings>)
        {
            Campus = syncOptions.Value?.Campus;
            Domain = syncOptions.Value?.Domain;
        }

        /// <summary>
        /// Ascync write function.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="feed"></param>
        /// <returns></returns>
        public override void Write(DirectoryInfo directoryInfo, UserCollection collection)
        {
            using FileStream stream = new FileInfo(Path.Combine(directoryInfo.FullName, String.Format("{0}.xml", Guid.NewGuid().ToString("N")))).OpenWrite();
            {
                XmlSerializer serializer = new(collection.GetType());
                XmlSerializerNamespaces myNamespaces = new();
                myNamespaces.Add("", "");
                serializer.Serialize(stream, collection, myNamespaces);
            }
        }

        public override void Parse(FileInfo file, DirectoryInfo directoryInfo)
        {
            UserCollection collection = new();
            using IPatronReader<User> reader = new PatronFileReader(new StreamReader(file.FullName), str => Regex.Replace(str, @"[\u0001]", Regex.Replace(str, @"[^\u0020-\u007E]", " ")))
            {
                CountryCodes = CountryCodes.Fetch(),
                CampusCode = Campus,
                EmailFormat = string.Format("@{0}", Domain),
            };
            {
                reader.Open();
                for (User user = reader.ReadNext(); null != user; user = reader.ReadNext())
                {
                    collection.Add(user);
                }
                reader.Close();
            }
            Write(directoryInfo, collection);
        }

    }
}




