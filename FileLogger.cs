
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;

namespace RunawaySystems.Logging {

    /// <summary> Listens to events from the <see cref=Log"/> class and writes them to an xml file. </summary>
    public class FileLogger {

        private XmlWriter writer;
        private FileStream stream;

        /// <summary> Creates log file and begins writing to it immediately. </summary>
        /// <param name="logDirectory"> Absolute path to the folder that will contain the log. </param>
        /// <param name="logName"> Name of the file that will contain the log. </param>
        public FileLogger(Uri logDirectory, string logName) {

            try { stream = new FileStream(Path.Combine(logDirectory.AbsolutePath, $"{logName}.xml"), FileMode.Create); }
            catch (System.Exception exception) {
                Log.Error(exception.Message);
                Log.Error($"{nameof(FileLogger)} will not be used.");
                return;
            }

            var xmlSettings = new XmlWriterSettings {
                Indent = true,
                ConformanceLevel = ConformanceLevel.Document
            };
            writer = XmlWriter.Create(stream, xmlSettings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Log", @"data:runawaysystems/logging");
            Log.MessageLogged += Write;
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
        }

        void OnProcessExit(object sender, EventArgs arguments) => Close();

        public void Close() {
            Log.MessageLogged -= Write;
            AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Dispose();
            stream.Dispose();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(LogEntry entry) {
            entry.WriteXml(writer);
            writer.Flush();
        }
    }
}
