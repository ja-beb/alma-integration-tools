using AlmaIntegrationTools.Bursar.Models;
using AlmaIntegrationTools.Services;
using AlmaIntegrationTools.Settings;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;

namespace AlmaIntegrationTools.Bursar.Services
{
    /// <summary>
    /// The Bursar sync service. 
    /// This service pulls file from SFTP server, converts to plain text and uploads to another SFTP server.
    /// </summary>
    public class SyncService : SyncService<BursarFeed>, ISyncService<BursarFeed>
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="syncOptions"></param>
        public SyncService(IOptions<SyncSettings> syncOptions) : base(syncOptions)
        { }

        /// <summary>
        /// Ascync write function.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="feed"></param>
        /// <returns></returns>
        public override void Write(DirectoryInfo directoryInfo, BursarFeed feed)
        {
            DirectoryInfo feedDirectory = new(Path.Combine(directoryInfo.FullName, feed.Number.ToString()));
            feedDirectory.Create();
            foreach (FineFeeData data in feed.GetList().Where(data => !data.ExportNumberSpecified).SelectMany(data => data.UserExportedList))
            {
                WriteData(feedDirectory, data);
            }
        }

        /// <summary>
        /// Write fine fee data to an output file.
        /// </summary>
        /// <param name="directoryInfo"></param>
        /// <param name="feed"></param>
        static public void WriteData(DirectoryInfo directoryInfo, FineFeeData data)
        {
            string filename = String.Format("{0}.txt", null == data.User ? data.PatronName.GetHashCode() : data.User.Value);
            using StreamWriter streamWriter = new(Path.Combine(directoryInfo.FullName, filename));
            {
                streamWriter.WriteLine($"{data.PatronName} (#{data.User?.Value})");
                streamWriter.WriteLine("");

                decimal total = 0;
                foreach (UserFineFee userFineFee in data.FineFeeList)
                {
                    total += userFineFee.CompositeSum.Sum;
                    streamWriter.WriteLine(userFineFee.Type);
                    streamWriter.WriteLine("");
                    streamWriter.WriteLine($"{userFineFee.Library} - {userFineFee.Location}");
                    streamWriter.WriteLine(String.Format($"{userFineFee.Title} ({userFineFee.CallNumber})"));
                    streamWriter.WriteLine(String.Format("Last Transaction Date: {0}", userFineFee.LastTransactionDate));
                    if (null != userFineFee.DueDate)
                    {
                        streamWriter.WriteLine(String.Format("Due Date: {0}", userFineFee.DueDate));
                    }
                    streamWriter.WriteLine("");
                    streamWriter.WriteLine(String.Format("Transaction Id: {0}", userFineFee.BursarTransactionId));
                    streamWriter.WriteLine(String.Format("Amount: {0:0.00} {1}", userFineFee.CompositeSum.Sum, userFineFee.CompositeSum.Currency));
                    if (!string.IsNullOrEmpty(userFineFee.Comment))
                    {
                        streamWriter.WriteLine("");
                        streamWriter.WriteLine($"Comments: {userFineFee.Comment}");
                    }
                    streamWriter.WriteLine("");
                }
                streamWriter.WriteLine("");
                streamWriter.WriteLine(String.Format("Total: {0:0.00}", total));
            }
        }
    }
}




