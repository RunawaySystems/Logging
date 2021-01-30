using System;
using System.Collections.Generic;

namespace RunawaySystems.Logging {
    /// <summary>
    /// Manages all the state for the logging system. <br/>
    /// Use this class to <see cref="Initialize"/> and configure the logging system.
    /// </summary>
    public class Logger {

        private ConsoleLogger consoleLogger;
        private Dictionary<string, FileLogger> fileLoggers;

        public static Logger Initialize() {
            var logger = new Logger();
            logger.fileLoggers = new Dictionary<string, FileLogger>();
            return logger;
        }

        /// <summary> Idempotent, only one <see cref="ConsoleLogger"/> will ever exist at a time. </summary>
        public Logger WithConsoleLogging() {
            if (consoleLogger is null)
                consoleLogger = new ConsoleLogger();

            return this;
        }

        /// <summary> You can have as many <see cref="FileLogger"/>s as you like so long as they output to different locations. </summary>
        public Logger WithFileLogging(Uri logDirectory, string logName) {
            string filePath = $"{logDirectory.AbsoluteUri}{logName}";

            if (fileLoggers.ContainsKey(filePath)) 
                Log.Error($"there is already a {nameof(FileLogger)} outputting to {filePath}, new {nameof(FileLogger)} will not be created.");
            else
                fileLoggers.Add(filePath, new FileLogger(logDirectory, logName));

            return this;
        }
    }
}
