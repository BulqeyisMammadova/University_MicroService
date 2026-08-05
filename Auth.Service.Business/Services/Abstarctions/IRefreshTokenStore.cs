using Auth.Service.Business.DTOs;
using Auth.Service.Core.Enums;

namespace Auth.Service.Business.Services.Abstractions;

public interface IRefreshTokenStore
{
    Task SaveAsync(string refreshToken, int userId, string email, Role role, TimeSpan expiry);
    Task<TokenRequestDto?> GetAsync(string refreshToken);
    Task RemoveAsync(string refreshToken);
}