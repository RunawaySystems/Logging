using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.IO;
using System;

using RunawaySystems.Logging;

/// <summary> Stateless handle for interacting with <see cref="RunawaySystems.Logging"/>. </summary>
/// <remarks> Placed in the global namespace to avoid needing to include RunawaySystems.Logging in every file. (I hope you're logging in nearly every file) </remarks>
public static class Log {

    public static event Action<LogEntry> MessageLogged;

    static Log() {
        AppDomain.CurrentDomain.UnhandledException += (sender, exception) => BroadcastLog($"{exception.ExceptionObject.GetType()}: {((System.Exception)exception.ExceptionObject).Message}", ((Exception)exception.ExceptionObject).TargetSite.DeclaringType.Name, Verbosity.Fatal);
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
        MessageLogged?.Invoke(new LogEntry(caller, DateTime.Now, verbosity, message));
    }
}