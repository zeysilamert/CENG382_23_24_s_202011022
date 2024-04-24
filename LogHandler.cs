public class LogHandler
{
    private readonly ILogger logger;

    public LogHandler(ILogger logger)
    {
        this.logger = logger;
    }

    public void AddLog(LogRecord log)
    {
        logger.Log(log);
    }
}
