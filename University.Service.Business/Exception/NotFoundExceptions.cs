using Microsoft.AspNetCore.Http;

namespace University.Service.Business.Exception;

public class NotFoundExceptions : System.Exception
{
    public int StatusCode { get; } = StatusCodes.Status404NotFound;

    public NotFoundExceptions(string message) : base(message) { }
    
}
