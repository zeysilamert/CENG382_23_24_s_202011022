using System;

public class LogHandler
{
    private readonly ILogger logger;

    public LogHandler(ILogger logger)
    {
        if (logger == null)
        {
            throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
        }
        this.logger = logger;
    }

    public void AddLog(LogRecord log)
    {
        if (log == null)
        {
            throw new ArgumentNullException(nameof(log), "LogRecord cannot be null");
        }

        logger.Log(log); // This sends the log to the provided ILogger implementation
    }
}
