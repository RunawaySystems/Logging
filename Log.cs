using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Text;
using System.IO;

using RunawaySystems.Logging;

/// <summary> Static utility class for sending logging events. </summary>
/// <remarks> Kept out of the Logging namespace for easy access from the rest of the assembly. </remarks>
public static class Log {

    public delegate void LogEntry(RunawaySystems.Logging.LogEntry entry);
    public static event LogEntry MessageLogged;

    static Log() {
        System.AppDomain.CurrentDomain.UnhandledException += (sender, exception) => BroadcastLog($"{exception.ExceptionObject.GetType()}: {((System.Exception)exception.ExceptionObject).Message}", ((System.Exception)exception.ExceptionObject).TargetSite.DeclaringType.Name, Verbosity.Fatal);
    }

    [Conditional("DEBUG")]
    public static void Debug(string message, [CallerFilePath] string callerPath = null) => BroadcastLog(message, callerPath, Verbosity.Debug);
    public static void Trace(string message, [CallerFilePath] string callerPath = null) => BroadcastLog(message, callerPath, Verbosity.Trace);
    public static void Info(string message, [CallerFilePath] string callerPath = null) => BroadcastLog(message, callerPath, Verbosity.Info);
    public static void Warning(string message, [CallerFilePath] string callerPath = null) => BroadcastLog(message, callerPath, Verbosity.Warning);
    public static void Error(string message, [CallerFilePath] string callerPath = null) => BroadcastLog(message, callerPath, Verbosity.Error);
    public static void Fatal(string message, [CallerFilePath] string callerPath = null) => BroadcastLog(message, callerPath, Verbosity.Fatal);


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void BroadcastLog(string message, string callerPath, Verbosity verbosity) {
        string caller = Path.GetFileNameWithoutExtension(callerPath);
        MessageLogged?.Invoke(new RunawaySystems.Logging.LogEntry(caller, System.DateTime.Now, verbosity, message));
    }
}