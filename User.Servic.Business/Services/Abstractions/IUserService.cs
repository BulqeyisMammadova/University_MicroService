using User.Service.Business.DTOs;

namespace User.Service.Business.Services.Abstractions;

public interface IUserService
{
    Task<UserDto> RegisterAsync(RegisterDto dto);
    Task<AccessTokenDto?> LoginAsync(LoginDto dto);
}