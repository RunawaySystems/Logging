using System;
using System.Collections.Generic;

namespace RunawaySystems.Logging {
    /// <summary>
    /// Manages all the state for the logging system. <br/>
    /// Use this class to <see cref="Initialize"/> and configure the logging system.
    /// </summary>
    public class Logger {

        private List<ILogger> loggers;

        public static Logger Initialize() {
            var logger = new Logger();
            logger.loggers = new List<ILogger>();
            return logger;
        }

        public void Register(ILogger logger) {
            if (loggers.Contains(logger))
                throw new ArgumentException($"Identical logger already exists for {logger}");

            loggers.Add(logger);
        }
    }
}
