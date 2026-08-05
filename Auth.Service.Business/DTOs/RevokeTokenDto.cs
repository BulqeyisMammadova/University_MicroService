namespace Auth.Service.Business.DTOs;

public class RevokeTokenDto
{
    public string RefreshToken { get; set; } = string.Empty;
}