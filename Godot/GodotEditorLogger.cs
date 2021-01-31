using Godot;

namespace RunawaySystems.Logging {
    public class GodotEditorLogger : ILogger {

        public GodotEditorLogger() {
            Log.MessageLogged += Write;
        }

        public void Write(LogEntry entry) {
            GD.Print($"[{entry.WriteTime}] {entry.Verbosity.ToString().Capitalize()}: {entry.Caller}: {entry.Message}");
        }

        public override bool Equals(object obj) => obj is GodotEditorLogger;
    }

    public static partial class LoggerExtensions {
        public static Logger WithGodotEditorLogging(this Logger logger) {
            logger.Register(new GodotEditorLogger());
            return logger;
        }
    }
}
