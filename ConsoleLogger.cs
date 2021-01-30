using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace RunawaySystems.Logging {

    /// <summary> Disyplays logs through the system's console/stdout. </summary>
    public class ConsoleLogger {

        public ConsoleLogger() { Log.MessageLogged += WriteMessage; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteMessage(LogEntry entry) {
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.Write($"[{DateTime.Now}] ");

            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.Write($"{entry.Caller}: ");

            SetConsoleColorFromVerbosity(entry.Verbosity);
            System.Console.Write($"{entry.Message}\n");

            System.Console.ForegroundColor = ConsoleColor.White;
        }

        private void SetConsoleColorFromVerbosity(Verbosity verbosity) {
            switch (verbosity) {
                case Verbosity.Trace:
                    System.Console.ForegroundColor = ConsoleColor.DarkGray;
                    return;
                case Verbosity.Debug:
                    System.Console.ForegroundColor = ConsoleColor.Gray;
                    return;
                case Verbosity.Info:
                    System.Console.ForegroundColor = ConsoleColor.White;
                    return;
                case Verbosity.Warning:
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    return;
                case Verbosity.Error:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    return;
                case Verbosity.Fatal:
                    System.Console.ForegroundColor = ConsoleColor.Magenta;
                    return;
                default:
                    throw new NotImplementedException($"Logging verbosity {verbosity} is not known by the {nameof(ConsoleLogger)}!");
            };
        }
    }
}

