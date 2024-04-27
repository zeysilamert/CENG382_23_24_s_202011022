using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class FileLogger : ILogger
{
    private readonly string logFilePath;

    public FileLogger(string logFilePath)
    {
        this.logFilePath = logFilePath;
    }

    public void Log(LogRecord log)
    {
        var logJson = JsonSerializer.Serialize(log, new JsonSerializerOptions {WriteIndented = true});
        File.AppendAllText(logFilePath, logJson + "\n");
    }
}
