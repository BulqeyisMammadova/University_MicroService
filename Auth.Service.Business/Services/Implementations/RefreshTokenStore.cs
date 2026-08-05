using Microsoft.EntityFrameworkCore;
using Auth.Service.Business.DTOs;
using Auth.Service.Business.Services.Abstractions;
using Auth.Service.Core.Entities;
using Auth.Service.Core.Enums;
using Auth.Service.DataAccess.Repositories.Abstarctions;

namespace Auth.Service.Business.Services.Implementations;

public class RefreshTokenStore : IRefreshTokenStore
{
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenStore(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task SaveAsync(string refreshToken, int userId, string email, Role role, TimeSpan expiry)
    {
        var entity = new RefreshToken
        {
            Token = refreshToken,
            UserId = userId,
            Email = email,
            Role = role,
            ExpiresAt = DateTime.UtcNow.Add(expiry)
        };

        await _unitOfWork.RefreshTokens.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<TokenRequestDto?> GetAsync(string refreshToken)
    {
        var entity = await _unitOfWork.RefreshTokens.Query()
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

        if (entity == null || entity.ExpiresAt < DateTime.UtcNow) return null;

        return new TokenRequestDto { UserId = entity.UserId, Email = entity.Email, Role = entity.Role };
    }

    public async Task RemoveAsync(string refreshToken)
    {
        var entity = await _unitOfWork.RefreshTokens.Query()
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

        if (entity == null) return;

        _unitOfWork.RefreshTokens.Delete(entity);
        await _unitOfWork.SaveChangesAsync();
    }
}