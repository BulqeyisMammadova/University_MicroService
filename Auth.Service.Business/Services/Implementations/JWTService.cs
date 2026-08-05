using Auth.Service.Business.DTOs;
using Auth.Service.Business.Services.Abstarctions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.Service.Business.Services.Implementations;

public class JWTService : IJWTService
{
    private readonly JwtOptions _optionsDto;

    public JWTService(IConfiguration configuration)
    {
        _optionsDto = configuration.GetSection("JWTOptions").Get<JwtOptions>() ?? new JwtOptions();

    }

    public AccessTokenDto CreateAccessToken(List<Claim> claims)
    {
        JwtHeader jwtHeader = CreateJwtHeader();
        JwtPayload payload = CreateJwtPayload(claims);

        JwtSecurityToken jwtSecurityToken = new(jwtHeader, payload);
        JwtSecurityTokenHandler handler = new();
        string token = handler.WriteToken(jwtSecurityToken);

        string refreshToken = Guid.NewGuid().ToString();
        return new()
        {
            Token = token,
            ExpiredDate = DateTime.UtcNow.AddMinutes(_optionsDto.ExpiredDate),
            RefreshToken = refreshToken,
            RefreshTokenDate = DateTime.UtcNow.AddDays(_optionsDto.RefreshTokenExpiredDays)
        };
    }

    private JwtPayload CreateJwtPayload(List<Claim> claims)
    {
        return new(
            issuer: _optionsDto.Issuer,
            audience: _optionsDto.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_optionsDto.ExpiredDate)
        );
    }

    private JwtHeader CreateJwtHeader()
    {
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_optionsDto.SecretKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);
        JwtHeader jwtHeader = new JwtHeader(signingCredentials);
        return jwtHeader;
    }
}