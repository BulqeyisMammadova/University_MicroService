using Microsoft.AspNetCore.Http;

namespace Student.Service.Business.Exceptions;

public class NotFoundException : Exception
{
    public int StatusCode { get; } = StatusCodes.Status404NotFound;

    public NotFoundException(string message) : base(message) { }
}