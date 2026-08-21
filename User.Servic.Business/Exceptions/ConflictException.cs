using Microsoft.AspNetCore.Http;

namespace User.Servic.Business.Exceptions;

public class ConflictException : Exception
{
    public int StatusCode { get; set; } = StatusCodes.Status409Conflict;
    public ConflictException(string message) : base(message) { }
    
}





   