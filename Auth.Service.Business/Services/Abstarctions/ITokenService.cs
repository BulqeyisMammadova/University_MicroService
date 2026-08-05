using Auth.Service.Business.DTOs;

namespace Auth.Service.Business.Services.Abstarctions;

public interface ITokenService
{
    Task<AccessTokenDto> GenerateTokenAsync(TokenRequestDto dto);
    Task<AccessTokenDto?> RefreshTokenAsync(string refreshToken);
    Task<bool> RevokeAsync(string refreshToken);
}