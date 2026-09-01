namespace University.Service.Business.Exception;

public static class ExceptionExtensions
{
    public static Exceptions GetDeepInnerException(this Exceptions exception)
    {
        var current = exception;
        while (current.InnerException != null)
        {
            current = current.InnerException;
        }
        return current;
 
}