using System.Text.Json;
using University.Service.Business.Exceptions;
using University.Service.Business.Models;

namespace University.Service.Api.Middlewares;

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
        catch (NotFoundException ex)
        {
            _logger.LogWarning(
                "Not found: {Message} | Path: {Path} | Method: {Method}{NewLine}StackTrace: {StackTrace}",
                ex.Message, context.Request.Path, context.Request.Method, Environment.NewLine, ex.StackTrace);

            await WriteErrorResponseAsync(context, ex.StatusCode, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Unhandled exception: {Message} | Path: {Path} | Method: {Method}{NewLine}StackTrace: {StackTrace}",
                ex.Message, context.Request.Path, context.Request.Method, Environment.NewLine, ex.StackTrace);

            await WriteErrorResponseAsync(context, StatusCodes.Status500InternalServerError,
                "An unexpected error occurred. Please try again later.");
        }
    }

    private static async Task WriteErrorResponseAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var response = new ErrorModel { StatusCode = statusCode, Message = message };
        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}