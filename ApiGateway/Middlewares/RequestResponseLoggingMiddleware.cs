using System.Text;

namespace ApiGateway.Middlewares;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestBody = await ReadRequestBodyAsync(context.Request);

        _logger.LogInformation(
            "REQUEST: {Method} {Path} | Body: {Body}",
            context.Request.Method, context.Request.Path, requestBody);

        var originalBodyStream = context.Response.Body;
        using var memoryStream = new MemoryStream();
        context.Request.Body = memoryStream;

        await _next(context);
        
         var responseBody = await ReadResponseBodyAsync(memoryStream);

        _logger.LogInformation(
            "RESPONSE: {StatusCode} | Body: {Body}",
            context.Response.StatusCode, responseBody);

       
        memoryStream.Position = 0; 
        var originalResponseBodyStream = context.Response.Body;await memoryStream.CopyToAsync(originalResponseBodyStream) ;
        context.Response.Body =  originalResponseBodyStream;
    }

    private static async Task<string> ReadRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();

        using var reader = new StreamReader(
            request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 1024,
            leaveOpen: true);

        var body = await reader.ReadToEndAsync();

        request.Body.Position = 0;

        return body;
    }

    private static async Task<string> ReadResponseBodyAsync(MemoryStream memoryStream)
    {
        memoryStream.Position = 0;

        using var reader = new StreamReader(
            memoryStream,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            bufferSize: 1024,
            leaveOpen: true);

        var body = await reader.ReadToEndAsync();
        return body;
    }
}