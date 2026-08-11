using User.Service.Business.DTOs;

namespace User.Servic.Business.Services.Abstractions;

public interface IAuthServiceClient
{
    Task<AccessTokenDto> GenerateTokenAsync(int userId, string email, string roleName, List<string> permissions);
}