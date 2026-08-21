namespace Auth.Service.Business.Models;

public class ErrorModel
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
}
