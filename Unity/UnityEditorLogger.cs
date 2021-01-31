using System.Collections.Generic;

namespace RunawaySystems.Logging {
    public class UnityEditorLogger : ILogger {

        private int fontSize;

        //message colors
        private static readonly string typeColor    = "<color=#008060>";
        private static readonly string traceColor   = "<color=#000000>";
        private static readonly string debugColor   = "<color=#000000>";
        private static readonly string infoColor    = "<color=#000000>";
        private static readonly string warningColor = "<color=#FF3F00>";
        private static readonly string errorColor   = "<color=#FF0000>";
        private static readonly string fatalColor   = "<color=#A729C6>";

        public UnityEditorLogger(int fontSize = 15) {
            this.fontSize = fontSize;
#if UNITY_EDITOR
            Log.MessageLogged += Write;
#endif
        }

        private static readonly Dictionary<Verbosity, string> VerbosityString = new Dictionary<Verbosity, string> {
            { Verbosity.Fatal,   $"{fatalColor}FATAL: "},
            { Verbosity.Error,   $"{errorColor}ERROR: "},
            { Verbosity.Warning, $"{warningColor}WARNING: "},
            { Verbosity.Info,    $"{infoColor}"},
            { Verbosity.Debug,   $"{debugColor}DEBUG: " },
            { Verbosity.Trace,   $"{traceColor}TRACE: " },
        };

        public void Write(LogEntry entry) {
#if !UNITY_EDITOR
            return;
#endif
            string logMessage = $"<size=22> </size><size={fontSize}>{typeColor}<b>{entry.Caller}: </b></color>{VerbosityString[entry.Verbosity]}{entry.Message}</color></size>";
            switch (entry.Verbosity) {
                default:
                case Verbosity.Trace:
                case Verbosity.Debug:
                case Verbosity.Info:
                    UnityEngine.Debug.Log(logMessage);
                    return;
                case Verbosity.Warning:
                    UnityEngine.Debug.LogWarning(logMessage);
                    return;
                case Verbosity.Error:
                case Verbosity.Fatal:
                    UnityEngine.Debug.LogError(logMessage);
                    return;
            }
        }

        public override bool Equals(object obj) => obj is UnityEditorLogger;
    }

    public static partial class LoggerExtensions {
        public static Logger WithUnityEditorLogging(this Logger logger, int fontSize = 15) {
            logger.Register(new UnityEditorLogger(fontSize));
            return logger;
        }
    }
}
