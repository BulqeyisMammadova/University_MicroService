using User.Servic.Business.DTOs;
using User.Service.Business.DTOs;

namespace User.Service.Business.Services.Abstractions;

public interface IUserService
{
    Task<UserDto> RegisterAsync(RegisterDto dto);
    Task<AccessTokenDto?> LoginAsync(LoginDto dto);
    Task<PagedResultDto<UserDto>> GetAllUsersAsync(UserPaginationParams p);
    Task<UserDto?> UpdateAsync(int id, UserUpdateDto dto);

    Task<bool> DeleteAsync(int id);

    Task<bool> AddRoleToUserAsync(int userId, int roleId);
    Task<bool> RemoveRoleFromUserAsync(int userId, int roleId);
}