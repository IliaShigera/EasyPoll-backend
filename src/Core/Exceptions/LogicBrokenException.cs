namespace EasyPoll.Core.Exceptions;

public sealed class LogicBrokenException : ApplicationException
{
    public LogicBrokenException(string message) : base(message)
    {
    }
}