using Microsoft.AspNetCore.Http;

namespace University.Service.Business.Exceptions;

public class NotFoundException : System.Exception
{
    public int StatusCode { get; } = StatusCodes.Status404NotFound;

    public NotFoundException(string message) : base(message) { }
    
}
