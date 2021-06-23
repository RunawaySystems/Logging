using System.Text;
using Godot;

namespace RunawaySystems.Logging {
    /// <summary> Output target for Godot's editor console. (using GD.Print) </summary>
    public class GodotEditorLogger : ILogger {

        // parameters
        bool includeWriteTime;
        bool includeCaller;
        bool includeVerbosity;

        // state
        private StringBuilder logWriter = new StringBuilder();

        public GodotEditorLogger(bool includeWriteTime, bool includeCaller, bool includeVerbosity) {
            this.includeWriteTime = includeWriteTime;
            this.includeCaller = includeCaller;
            this.includeVerbosity = includeVerbosity;

            Log.MessageLogged += Write;
        }

        public void Write(LogEntry entry) {
            if (includeWriteTime) 
                logWriter.Append($"[{entry.WriteTime}] ");
            if (includeCaller)
                logWriter.Append($"{entry.Caller}: ");
            if (includeVerbosity)
                logWriter.Append($"{entry.Verbosity}: ");


            logWriter.Append(entry.Message);
            GD.Print(logWriter.ToString());
            logWriter.Clear();
        }

        public override bool Equals(object obj) => obj is GodotEditorLogger;
    }

    public static partial class LoggerExtensions {
        /// <inheritdoc cref="GodotEditorLogger"/>
        public static Logger WithGodotEditorLogging(this Logger logger, bool includeWriteTime = false, bool includeCaller = true, bool includeVerbosity = true) {
            Logger.Register(new GodotEditorLogger(includeWriteTime, includeVerbosity, includeCaller));
            return logger;
        }
    }
}
