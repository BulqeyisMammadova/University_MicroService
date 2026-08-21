using Microsoft.AspNetCore.Http;

namespace User.Servic.Business.Exceptions;

public class BadRequestException : Exception
{
    public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;
    public BadRequestException(string message) : base(message) { }
}


   