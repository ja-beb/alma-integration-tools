using System;
using System.Collections.Specialized;
using System.IO;
using System.Xml.Serialization;
using WinSCP;

namespace AlmaIntegrationTools
{
    public abstract class SyncProgram<T>
    {
        public NameValueCollection Config
        {
            get; set;
        }

        protected XmlSerializer xmlSerializer;
        public XmlSerializer XmlSerializer
        {
            get
            {
                if (null == xmlSerializer)
                {
                    xmlSerializer = new(typeof(T));
                }
                return xmlSerializer;
            }
        }

        protected XmlSerializerNamespaces xmlSerializerNamespaces;
        public XmlSerializerNamespaces XmlSerializerNamespaces
        {
            get
            {
                if (null == xmlSerializerNamespaces)
                {
                    xmlSerializerNamespaces = new();
                }
                return xmlSerializerNamespaces;
            }
            set
            {
                xmlSerializerNamespaces = value;
            }
        }

        protected SessionOptions sessionOptions;
        public SessionOptions SessionOptions
        {
            get
            {
                if (null == sessionOptions)
                {
                    sessionOptions = new()
                    {
                        Protocol = Protocol.Sftp,
                        HostName = Config.Get("Sftp.HostName"),
                        PortNumber = int.Parse(Config.Get("Sftp.PortNumber")),
                        UserName = Config.Get("Sftp.Username"),
                        SshHostKeyFingerprint = Config.Get("Sftp.SshHostKeyFingerprint"),
                        SshPrivateKeyPath = Config.Get("Sftp.SshPrivateKeyPath"),
                    };
                }
                return sessionOptions;
            }
        }

        public String WorkingPath
        {
            get => Config.Get("WorkingPath");
        }

        public string ImportFile
        {
            get => Config.Get("ImportFile");
        }

        public SyncProgram(NameValueCollection config)
        {
            Config = config;
        }

        /// <summary>
        /// Write logging output.
        /// </summary>
        /// <param name="message"></param>
        static public void WriteLog(string message)
        {
            Console.WriteLine("[{0}] {1}: {2}", DateTime.Now.ToUniversalTime(), System.AppDomain.CurrentDomain.FriendlyName, message);
        }

        public DirectoryInfo CreateDirectory(string[] path)
        {
            DirectoryInfo directoryInfo = new(Path.Combine(path));
            WriteLog(String.Format("Create folder {0}", directoryInfo.FullName));
            if (directoryInfo.Exists)
            {
                directoryInfo.Delete(true);
            }
            directoryInfo.Create();
            WriteLog("Completed fetching files");
            return directoryInfo;
        }

        public FileInfo[] Fetch(SessionOptions sessionOptions, string remotePath, string localPath)
        {
            WriteLog("Fetch(s) file from SFTP server");
            WriteLog(String.Format("Remote file = {0}", remotePath));
            WriteLog(String.Format("Local file = {0}", localPath));
            WriteLog(String.Format("Host address = {0}:{1}", sessionOptions.HostName, sessionOptions.PortNumber));
            WriteLog(String.Format("User account = {0}", sessionOptions.UserName));

            DirectoryInfo directoryInfo = CreateDirectory(new string[] { localPath, "data" });
            Console.WriteLine("Fetch files from {0}: {1}", sessionOptions.HostName, remotePath);
            using Session session = new();
            {
                session.Open(sessionOptions);
                session.GetFiles(remotePath, directoryInfo.FullName, false);
                session.Close();
            }
            return directoryInfo.GetFiles("*.xml");
        }

        public void Write(FileInfo fileinfo, T data)
        {
            FileStream stream = fileinfo.OpenWrite();
            XmlSerializer.Serialize(stream, data, XmlSerializerNamespaces);
            stream.Close();
        }

        abstract public void Execute();

    }
}
