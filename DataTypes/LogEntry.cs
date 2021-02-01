using System;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace RunawaySystems.Logging {

    /// <summary> Individual XML based structured log. </summary>
    public class LogEntry : IXmlSerializable {

        public LogEntry(string caller, DateTime writeTime, Verbosity verbosity, string message) {
            Caller = caller;
            WriteTime = writeTime;
            Verbosity = verbosity;
            Message = message;
        }

        /// <summary> The file that called for the log to be written. </summary> 
        public string Caller { get; private set; }
        public DateTime WriteTime { get; private set; }
        public Verbosity Verbosity { get; private set; }
        public string Message { get; private set; }


        XmlSchema IXmlSerializable.GetSchema() => null;
        public void ReadXml(XmlReader reader) {
            Caller = reader.ReadContentAsString();
            Verbosity = (Verbosity)reader.ReadElementContentAsInt();
            Message = reader.ReadContentAsString();
            WriteTime = reader.ReadContentAsDateTime();
            reader.Read();
        }

        public void WriteXml(XmlWriter writer) {
            writer.WriteStartElement(nameof(LogEntry));
            writer.WriteAttributeString(nameof(Caller), Caller);
            writer.WriteAttributeString(nameof(Verbosity), Verbosity.ToString());
            writer.WriteAttributeString(nameof(Message), Message);
            writer.WriteAttributeString(nameof(WriteTime), WriteTime.ToString());
            writer.WriteEndElement();
        }
    }
}
