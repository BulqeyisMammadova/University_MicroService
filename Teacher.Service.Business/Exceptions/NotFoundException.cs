using Microsoft.AspNetCore.Http;

namespace Teacher.Service.Business.Exceptions;

public class NotFoundException : Exception
{
    public int StatusCode { get; } = StatusCodes.Status404NotFound;

    public NotFoundException(string message) : base(message) { }
}
