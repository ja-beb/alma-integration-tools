using AlmaIntegrationTools.Config;
using System;
using System.IO;
using System.Linq;

namespace AlmaIntergrationTools.Bursar
{
    /// <summary>
    /// Main program execution. 
    /// </summary>
    class Program : XmlSyncProgram<BursarFeed>
    {
        /// <summary>
        ///  Main program execution.
        /// </summary>
        static void Main()
        {
            SyncProgram syncProgram = new Program()
            {
                Config = ServersSectionGroup.Instance()
            };
            syncProgram.Run();
        }

        /// <summary>
        /// Write feed to output.
        /// </summary>
        /// <param name="feedDirectory"></param>
        /// <param name="fineFeeData"></param>
        static void Write(StreamWriter streamWriter, FineFeeData fineFeeData)
        {

            streamWriter.WriteLine($"{fineFeeData.PatronName} (#{fineFeeData.User?.Value})");
            streamWriter.WriteLine("");

            decimal total = 0;
            foreach (UserFineFee userFineFee in fineFeeData.FineFeeList)
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

        /// <summary>
        /// Process feed.
        /// </summary>
        /// <param name="feed"></param>
        public override void Process(BursarFeed feed)
        {
            DirectoryInfo feedDirectory = CreateDirectory(Path.Combine(ExportDirectory.FullName, feed.Number.ToString()));
            foreach (FineFeeData data in feed.ExportedFineFees.ToList().Where(data => !data.ExportNumberSpecified).SelectMany(data => data.UserExportedList))
            {
                string filename = String.Format("{0}.txt", null == data.User ? data.PatronName.GetHashCode() : data.User.Value);
                using StreamWriter streamWriter = new(Path.Combine(feedDirectory.FullName, filename));
                {
                    Write(streamWriter, data);
                }
            }
        }
    }
}
