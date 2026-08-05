using Microsoft.EntityFrameworkCore;
using User.Servic.Business.Services.Abstractions;
using User.Service.Business.DTOs;
using User.Service.Business.Services.Abstractions;
using User.Service.DataAccess.Repositories.Abstarctions;

namespace User.Service.Business.Services.Implementations;

public class UserService(IUnitOfWork unitOfWork, IAuthServiceClient authServiceClient) : IUserService
{
    public async Task<UserDto> RegisterAsync(RegisterDto dto)
    {
        var exists = await unitOfWork.Users.Query().AnyAsync(u => u.Email == dto.Email);
        if (exists) throw new InvalidOperationException("Bu email artiq qeydiyyatdan kecib.");

        var user = new Core.Entities.User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            CreatedAt = DateTime.UtcNow
        };

        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<AccessTokenDto?> LoginAsync(LoginDto dto)
    {
        var user = await unitOfWork.Users.Query().FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return null;

        var passwordOk = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!passwordOk) return null;

        return await authServiceClient.GenerateTokenAsync(user.Id, user.Email, user.Role);
    }
}