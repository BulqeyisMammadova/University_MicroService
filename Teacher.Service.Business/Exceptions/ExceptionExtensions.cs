namespace Teacher.Service.Business.Exceptions;

public static class ExceptionExtensions
{
    public static Exception GetDeepInnerException(this Exception exception)
    {
        var current = exception;
        while (current.InnerException != null)
        {
            current = current.InnerException;
        }
        return current;
    }
}