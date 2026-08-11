using Auth.Service.Business.DTOs;

namespace Auth.Service.Business.Services.Abstractions;

public interface IRefreshTokenStore
{
    Task SaveAsync(string refreshToken, int userId, string email, string roleName, List<string> permissions, TimeSpan expiry);
    Task<TokenRequestDto?> GetAsync(string refreshToken);
    Task RemoveAsync(string refreshToken);
}