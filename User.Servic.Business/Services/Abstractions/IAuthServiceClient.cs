using User.Service.Business.DTOs;
using User.Service.Core.Enum;

namespace User.Servic.Business.Services.Abstractions;

public interface IAuthServiceClient
{
    Task<AccessTokenDto> GenerateTokenAsync(int userId, string email, Role role);
    
}
