using Student.Service.Business.Exceptions;
using Student.Service.Business.Model;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace Student.Service.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) 
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionResponseAsync(context, ex);
        }
    }

    private async Task HandleExceptionResponseAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;

        var deepEx = exception.GetDeepInnerException();
        string logMessage = $"Path:{context.Request.Path} Method:{context.Request.Method}  " +
                             $"{deepEx.Message} - {deepEx.StackTrace}";

        ErrorModel response;

        switch (exception)
        {
            case NotFoundException notFound:
                context.Response.StatusCode = notFound.StatusCode;
                response = new ErrorModel { StatusCode = notFound.StatusCode, Message = notFound.Message };
                _logger.LogWarning(logMessage);
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = new ErrorModel
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = "An unexpected error occurred. Please try again later."
                };
                _logger.LogError(logMessage);
                break;
        }

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
} 