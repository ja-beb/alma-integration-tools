using Renci.SshNet.Common;
using System;
using System.IO;
using System.Xml.Serialization;

namespace AlmaIntergrationTools
{
    /// <summary>
    /// Base sync program.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    abstract public class XmlSyncProgram<T> : SyncProgram
    {
        /// <summary>
        /// Main execution.
        /// </summary>
        /// <param name="fileExtension"></param>
        public override void Run()
        {
            try
            {
                FileInfo[] files = Fetch("xml");
                XmlSerializer xmlSerializer = new(typeof(T));
                foreach (FileInfo file in files)
                {
                    // Process import file.
                    using FileStream fileStream = file.OpenRead();
                    {
                        Process((T) xmlSerializer.Deserialize(fileStream));
                    }
                }

                // zip and send to server.
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
        /// 
        /// </summary>
        /// <param name="feed"></param>
        abstract public void Process(T feed);
    }
}
