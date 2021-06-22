using System.Collections.Generic;
using System.Text;

namespace RunawaySystems.Logging {
    /// <summary> Output target for Unity's log.txt file, and also the editor console if running from the editor. </summary>
    public class UnityLogger : ILogger {

        private int fontSize;

        private StringBuilder logWriter = new StringBuilder();

        public UnityLogger(int fontSize = 15) {
            this.fontSize = fontSize;
            Log.MessageLogged += Write;
        }

        public void Write(LogEntry entry) {

            logWriter.Append(entry.Caller)
                     .Append(": ")
                     .Append(entry.Verbosity)
                     .Append(": ")
                     .Append(entry.Message)
                     .Append('\n');

            System.Console.Write(logWriter.ToString());

            logWriter.Clear();
        }

        public override bool Equals(object obj) => obj is UnityLogger;
    }

    public static partial class LoggerExtensions {
        /// <inheritdoc cref="UnityLogger"/>
        public static Logger WithUnityLogging(this Logger logger, int fontSize = 15) {
            logger.Register(new UnityLogger(fontSize));
            return logger;
        }
    }
}
