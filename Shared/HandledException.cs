namespace SycappsWeb.Shared;

public class HandledException : Exception
{
    public HandledException(string message, Exception originalException)
    {
        Message = message;
        OriginalException = originalException;
    }
    public HandledException(string message)
    {
        Message = message;
    }
    public string? Message { get; private set; }
    public Exception? OriginalException { get; private set; }
}
