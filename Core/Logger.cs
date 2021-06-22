using System;
using System.Collections.Generic;

namespace RunawaySystems.Logging {
    /// <summary>
    /// Manages all the state for the logging system. <br/>
    /// Use this class to <see cref="Initialize"/> and configure the logging system.
    /// </summary>
    public class Logger {

        private static Logger instance;
        private List<ILogger> loggers;

        public static Logger Initialize() {
            instance = new Logger();
            instance.loggers = new List<ILogger>();
            return instance;
        }

        public static void Register(ILogger logger) {
            if (instance.loggers.Contains(logger))
                throw new ArgumentException($"Identical logger already exists for {logger}");

            instance.loggers.Add(logger);
        }
    }
}
