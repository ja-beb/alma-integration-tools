using AlmaIntergrationTools.Bursar;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace AlmaIntergrationTools.ProcessExport
{
    class Program : SyncProgram<BursarFeed>
    {
        public Program(NameValueCollection config) : base(config)
        { }

        public override void Execute()
        {
            DirectoryInfo workingPath = CreateDirectory(new string[] { WorkingPath, DateTime.Now.ToString("yyyyddMMHHmm") });
            DirectoryInfo exportPath = CreateDirectory(new string[] { workingPath.FullName, "export" });
            foreach (FileInfo file in Fetch(SessionOptions, ImportFile, workingPath.FullName))
            {
                // Process import file.
                using FileStream fileStream = file.OpenRead();
                {
                    Write(new(Path.Combine(exportPath.FullName, file.Name)), XmlSerializer.Deserialize(fileStream) as BursarFeed);
                }
            }
        }

        static void Main()
        {
            try
            {
                Program program = new Program(ConfigurationManager.AppSettings);                
                program.XmlSerializerNamespaces.Add("xb", BursarFeed.Namespace);
                program.XmlSerializerNamespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                program.Execute();
            }

            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
            }

            catch (IOException exception)
            {
                Console.Error.WriteLine(exception.Message);
            }

            // Invalid configuration error.
            catch (FormatException)
            {
                Console.Error.WriteLine("Invalid configuration setting: update the application config file.");
            }
        }
    }
}
