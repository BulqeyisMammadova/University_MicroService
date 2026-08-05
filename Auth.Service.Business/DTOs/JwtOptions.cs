namespace Auth.Service.Business.DTOs;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiredDate { get; set; } 
    public int RefreshTokenExpiredDays { get; set; } = 7; 
}