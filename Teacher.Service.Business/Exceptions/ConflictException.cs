using Microsoft.AspNetCore.Http;

namespace Teacher.Service.Business.Exceptions;

public class ConflictException : Exception
{
    public int StatusCode { get; } = StatusCodes.Status409Conflict;

    public ConflictException(string message) : base(message) { }
}