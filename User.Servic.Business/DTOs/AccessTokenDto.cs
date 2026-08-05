namespace User.Service.Business.DTOs;

public class AccessTokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiredDate { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenDate { get; set; }
}