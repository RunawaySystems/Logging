using System.Collections.Generic;
using System.Text;

namespace RunawaySystems.Logging {
    /// <summary> Output target for Unity's log.txt file, and also the editor console if running from the editor. </summary>
    public class UnityLogger : ILogger {

        private int fontSize;

        private static readonly string lightThemeTypeColor = "<color=#008060>";
        private static readonly string lightThemeFatalColor = "<color=#A729C6>";
        private static readonly string lightThemeErrorColor = "<color=#FF0000>";
        private static readonly string lightThemeWarningColor = "<color=#FF3F00>";
        private static readonly string lightThemeInfoColor = "<color=#000000>";
        private static readonly string lightThemeDebugColor = "<color=#000000>";
        private static readonly string lightThemeTraceColor = "<color=#000000>";

        private static readonly string darkThemeTypeColor = "<color=#4EC9AB>";
        private static readonly string darkThemeFatalColor = "<color=#A729C6>";
        private static readonly string darkThemeErrorColor = "<color=#FF0000>";
        private static readonly string darkThemeWarningColor = "<color=#FFF43F>";
        private static readonly string darkThemeInfoColor = "<color=#EFEFEF>";
        private static readonly string darkThemeDebugColor = "<color=#7F7F7F>";
        private static readonly string darkThemeTraceColor = "<color=#7F7F7F>";

        private StringBuilder logWriter = new StringBuilder();

        public UnityLogger(int fontSize = 14) {
            this.fontSize = fontSize;
            Log.MessageLogged += Write;
        }

        /// <summary> Dictionary holding predefined color/label string for every verbosity level. Prevents constantly making new strings, and should be as fast as switch. </summary>
        private static readonly Dictionary<Verbosity, string> lightThemeVerbosityString = new Dictionary<Verbosity, string> {
            [Verbosity.Fatal] = lightThemeFatalColor,
            [Verbosity.Error] = lightThemeErrorColor,
            [Verbosity.Warning] = lightThemeWarningColor,
            [Verbosity.Info] = lightThemeInfoColor,
            [Verbosity.Debug] = lightThemeDebugColor,
            [Verbosity.Trace] = lightThemeTraceColor
        };

        /// <summary> Dictionary holding predefined color/label string for every verbosity level. Prevents constantly making new strings, and should be as fast as switch. </summary>
        private static readonly Dictionary<Verbosity, string> darkThemeVerbosityString = new Dictionary<Verbosity, string> {
            [Verbosity.Fatal] = darkThemeFatalColor,
            [Verbosity.Error] = darkThemeErrorColor,
            [Verbosity.Warning] = darkThemeWarningColor,
            [Verbosity.Info] = darkThemeInfoColor,
            [Verbosity.Debug] = darkThemeDebugColor,
            [Verbosity.Trace] = darkThemeTraceColor
        };

        public void Write(LogEntry entry) {
            logWriter.Append($"<size=22> </size><size={fontSize}>");

            // format message colors for light or dark theme
            if (UnityEditor.EditorGUIUtility.isProSkin) { // dark skin
                logWriter.Append(darkThemeTypeColor)
                         .Append("<b>")
                         .Append(entry.Caller)
                         .Append(": </b></color>")
                         .Append(darkThemeVerbosityString[entry.Verbosity]);
            }
            else { // light skin 
                logWriter.Append(lightThemeTypeColor)
                         .Append("<b>")
                         .Append(entry.Caller)
                         .Append(": </b></color>")
                         .Append(lightThemeVerbosityString[entry.Verbosity]);
            }

            logWriter.Append(entry.Message)
                     .Append("</color></size>\n");

            switch (entry.Verbosity) {
                default:
                case Verbosity.Trace:
                case Verbosity.Debug:
                case Verbosity.Info:
                    UnityEngine.Debug.Log(logWriter.ToString());
                    break;
                case Verbosity.Warning:
                    UnityEngine.Debug.LogWarning(logWriter.ToString());
                    break;
                case Verbosity.Error:
                case Verbosity.Fatal:
                    UnityEngine.Debug.LogError(logWriter.ToString());
                    break;
            }

            logWriter.Clear();
        }

        public override bool Equals(object obj) => obj is UnityLogger;
    }

    public static partial class LoggerExtensions {
        /// <inheritdoc cref="UnityLogger"/>
        public static Logger WithUnityLogging(this Logger logger, int fontSize = 14) {
            Logger.Register(new UnityLogger(fontSize));
            return logger;
        }
    }
}
