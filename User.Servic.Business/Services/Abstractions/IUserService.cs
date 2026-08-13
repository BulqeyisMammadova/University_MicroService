using User.Servic.Business.DTOs.MailDtos;
using User.Servic.Business.DTOs.PagitationsDtos;
using User.Servic.Business.DTOs.TokenDtos;
using User.Servic.Business.DTOs.UserDtos;

namespace User.Service.Business.Services.Abstractions;

public interface IUserService
{
    Task<UserDto> RegisterAsync(RegisterDto dto);
    Task<AccessTokenDto?> LoginAsync(LoginDto dto);
    Task<PagedResultDto<UserDto>> GetAllUsersAsync(UserPaginationParams p);
    Task<UserDto?> UpdateAsync(int id, UserUpdateDto dto);
    Task<bool> DeleteAsync(int id);


    Task<GetUserDto?> GetByIdAsync(int id);
    Task<bool> AddRoleToUserAsync(int userId, int roleId);
    Task<bool> RemoveRoleFromUserAsync(int userId, int roleId);


    Task<bool> ConfirmEmailAsync(ConfirmMailDto dto);
    Task ForgotPasswordAsync(ForgotPasswordDto dto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
}


 
   