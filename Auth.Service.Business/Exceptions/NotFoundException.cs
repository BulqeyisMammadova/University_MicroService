using Microsoft.AspNetCore.Http;
namespace Auth.Service.Business.Exceptions;

public class NotFoundException : Exception
{
    public int StatusCode { get; set; } = StatusCodes.Status404NotFound;

    public NotFoundException(string message) : base(message) { }
    
}
