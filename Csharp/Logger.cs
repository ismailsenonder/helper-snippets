using System;
using System.IO;

public class Logger
{
    private static string logFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
    private static bool emptyLineBetweenLogs = true;

    private static string GetLogFileName()
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        return Path.Combine(logFolderPath, $"{date}.log");
    }

    public static void WriteLog(string message, )
    {
        // Ensure the Logs folder exists
        if (!Directory.Exists(logFolderPath))
        {
            Directory.CreateDirectory(logFolderPath);
        }

        string logFileName = GetLogFileName();
        using var writer = new StreamWriter(logFileName, true);
        writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        if (emptyLineBetweenLogs)
        {
            writer.WriteLine("");
        }
    }
}