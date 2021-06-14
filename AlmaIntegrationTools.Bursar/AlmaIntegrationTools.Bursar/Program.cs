using AlmaIntegrationTools.Config;
using System;
using System.IO;
using System.Xml.Serialization;
using WinSCP;

namespace AlmaIntergrationTools.Bursar
{
    class Program
    {

        static void Main()
        {
            try
            {
                ServersSectionGroup serversSectionGroup = ServersSectionGroup.Instance();
                DirectoryInfo baseDirectory = new(Path.Combine(
                    serversSectionGroup.Path.Value,
                    Guid.NewGuid().ToString("N")
                ));
                baseDirectory.Create();

                // import.
                FileInfo[] files = Fetch(new()
                {
                    Protocol = Protocol.Sftp,
                    HostName = serversSectionGroup.ImportServer.Host,
                    PortNumber = serversSectionGroup.ImportServer.Port,
                    UserName = serversSectionGroup.ImportServer.User,
                    SshHostKeyFingerprint = serversSectionGroup.ImportServer.Fingerprint,
                    SshPrivateKeyPath = serversSectionGroup.ImportServer.Key,
                },
                    serversSectionGroup.ImportServer.Path,
                    baseDirectory
                 );

                // convert.
                XmlSerializer xmlSerializer = new(typeof(BursarFeed));
                using (StreamWriter outputFile = new(Path.Combine(baseDirectory.FullName, "results.txt")))
                {
                    foreach (FileInfo file in files)
                    {
                        // Process import file.
                        using FileStream fileStream = file.OpenRead();
                        {
                            BursarFeed feed = xmlSerializer.Deserialize(fileStream) as BursarFeed;
                            foreach (BursarExportData data in feed.ExportedFineFees)
                            {
                                if (!data.ExportNumberSpecified)
                                {
                                    outputFile.WriteLine(String.Format("Number_________________: {0}", data.Number));
                                    foreach (FineFeeData fineFeeData in data.UserExportedList)
                                    {
                                        outputFile.WriteLine(String.Format("Patron Name___: {0}", fineFeeData.PatronName));
                                        if (null != fineFeeData.User)
                                        {
                                            outputFile.WriteLine(String.Format("User Type:____: {0}", fineFeeData.User.Type));
                                            outputFile.WriteLine(String.Format("User Value____: {0}", fineFeeData.User.Value));
                                            if (null != fineFeeData.User.OwneredEntity)
                                            {
                                                outputFile.WriteLine(String.Format("Institution Id: {0}", fineFeeData.User.OwneredEntity.InstitutionId));
                                            }
                                        }
                                        outputFile.WriteLine("User Fines/Fees:");
                                        foreach (UserFineFee userFineFee in fineFeeData.FineFeeList)
                                        {
                                            outputFile.WriteLine(String.Format("\t Barcode______________: {0}", userFineFee.Barcode));
                                            outputFile.WriteLine(String.Format("\t Bursar Transaction Id: {0}", userFineFee.BursarTransactionId));
                                            outputFile.WriteLine(String.Format("\t Call Number__________: {0}", userFineFee.CallNumber));
                                            outputFile.WriteLine(String.Format("\t Comment______________: {0}", userFineFee.Comment));
                                            outputFile.WriteLine(String.Format("\t Compoosite Sum_______: {0}", userFineFee.CompositeSum));
                                            outputFile.WriteLine(String.Format("\t Due Date_____________: {0}", userFineFee.DueDate));
                                            outputFile.WriteLine(String.Format("\t Internal Location____: {0}", userFineFee.InternalLocation));
                                            outputFile.WriteLine(String.Format("\t Last Transaction Date: {0}", userFineFee.LastTransactionDate));
                                            outputFile.WriteLine(String.Format("\t Library______________: {0}", userFineFee.Library));
                                            outputFile.WriteLine(String.Format("\t Location_____________: {0}", userFineFee.Location));
                                            outputFile.WriteLine(String.Format("\t Title________________: {0}", userFineFee.Title));
                                            outputFile.WriteLine(String.Format("\t Type_________________: {0}", userFineFee.Type));
                                            outputFile.WriteLine(String.Format("\t Institution Id_______: {0}", userFineFee.OwneredEntity.InstitutionId));
                                            outputFile.WriteLine(String.Format("\t Library Code_________: {0}", userFineFee.OwneredEntity.LibraryCode));
                                            outputFile.WriteLine(String.Format("\t Library Id___________: {0}", userFineFee.OwneredEntity.LibraryId));
                                            outputFile.WriteLine("");
                                        }
                                    }
                                    outputFile.WriteLine("");
                                }
                            }
                        }
                    }
                }
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
            return directoryInfo.GetFiles("*.xml");
        }

    }
}
