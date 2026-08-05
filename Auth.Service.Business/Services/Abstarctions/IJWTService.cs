using Auth.Service.Business.DTOs;
using System.Security.Claims;

namespace Auth.Service.Business.Services.Abstarctions;

public interface IJWTService
{
    AccessTokenDto CreateAccessToken(List<Claim> claims);
}
