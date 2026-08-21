using Auth.Service.Business.DTOs;
using Auth.Service.Business.Exceptions;
using Auth.Service.Business.Services.Abstarctions;
using Auth.Service.Business.Services.Abstractions;
using System.Security.Claims;

namespace Auth.Service.Business.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly IJWTService _jwtService;
    private readonly IRefreshTokenStore _refreshStore;

    public TokenService(IJWTService jwtService, IRefreshTokenStore refreshStore)
    {
        _jwtService = jwtService;
        _refreshStore = refreshStore;
    }

    public async Task<AccessTokenDto> GenerateTokenAsync(TokenRequestDto dto)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, dto.UserId.ToString()),
            new(ClaimTypes.Email, dto.Email),
            new(ClaimTypes.Role, dto.RoleName)
        };

        foreach (var permission in dto.Permissions.Distinct())
        {
            claims.Add(new Claim("Permission", permission));
        }

        var token = _jwtService.CreateAccessToken(claims);

        await _refreshStore.SaveAsync(token.RefreshToken, dto.UserId, dto.Email, dto.RoleName, dto.Permissions, TimeSpan.FromDays(7));

        return token;
    }

    public async Task<AccessTokenDto?> RefreshTokenAsync(string refreshToken)
    {
        var stored = await _refreshStore.GetAsync(refreshToken);
        if (stored == null) throw new NotFoundException("RefreshToken not found");

        await _refreshStore.RemoveAsync(refreshToken);

        return await GenerateTokenAsync(stored);
    }

    public async Task<bool> RevokeAsync(string refreshToken)
    {
        var stored = await _refreshStore.GetAsync(refreshToken);
        if (stored == null) throw new NotFoundException("RefreshToken not found");

        await _refreshStore.RemoveAsync(refreshToken);
        return true;
    }
}