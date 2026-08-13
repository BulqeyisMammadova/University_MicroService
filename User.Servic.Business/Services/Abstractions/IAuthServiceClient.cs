using User.Servic.Business.DTOs.TokenDtos;

namespace User.Servic.Business.Services.Abstractions;

public interface IAuthServiceClient
{
    Task<AccessTokenDto> GenerateTokenAsync(int userId, string email, string roleName, List<string> permissions);
}