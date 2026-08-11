using Microsoft.EntityFrameworkCore;
using User.Servic.Business.DTOs;
using User.Servic.Business.Services.Abstractions;
using User.Service.Business.DTOs;
using User.Service.Business.Extensions;
using User.Service.Business.Services.Abstractions;
using User.Service.Core.Entities;
using User.Service.DataAccess.Repositories.Abstarctions;

namespace User.Service.Business.Services.Implementations;

public class UserService(IUnitOfWork unitOfWork, IAuthServiceClient authServiceClient) : IUserService
{
    public async Task<UserDto> RegisterAsync(RegisterDto dto)
    {
        var exists = await unitOfWork.Users.Query().AnyAsync(u => u.Email == dto.Email);
        if (exists) throw new InvalidOperationException("This email already exists.");

        if (dto.RoleIds.Count == 0)
            throw new InvalidOperationException("Role not selected.");

        var user = new Core.Entities.User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow
        };

        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        var roleNames = new List<string>();

        foreach (var roleId in dto.RoleIds.Distinct())
        {
            var role = await unitOfWork.Roles.GetByIdAsync(roleId);
            if (role == null) continue;

            await unitOfWork.UserRoles.AddAsync(new UserRole { UserId = user.Id, RoleId = roleId });
            roleNames.Add(role.Name);
        }

        await unitOfWork.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            IsActive = user.IsActive,
            Roles = roleNames
        };
    }

    public async Task<AccessTokenDto?> LoginAsync(LoginDto dto)
    {
        var user = await unitOfWork.Users.Query()
            .Include(u => u.UserRoles.Where(ur => ur.IsActive))
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions.Where(rp => rp.IsActive))
                        .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.IsActive);

        if (user == null) return null;

        var passwordOk = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!passwordOk) return null;

        var roleNames = new List<string>();
        var permissionNames = new List<string>();

        foreach (var userRole in user.UserRoles)
        {
            if (!userRole.Role.IsActive) continue;

            roleNames.Add(userRole.Role.Name);

            foreach (var rolePermission in userRole.Role.RolePermissions)
            {
                if (!permissionNames.Contains(rolePermission.Permission.Name))
                    permissionNames.Add(rolePermission.Permission.Name);
            }
        }

        var combinedRoleName = string.Join(",", roleNames);

        return await authServiceClient.GenerateTokenAsync(user.Id, user.Email, combinedRoleName, permissionNames);
    }

    public async Task<PagedResultDto<UserDto>> GetAllUsersAsync(UserPaginationParams p)
    {
        IQueryable<Core.Entities.User> query = unitOfWork.Users.Query();

        if (!string.IsNullOrWhiteSpace(p.Name))
            query = query.Where(u => u.FullName.Contains(p.Name));

        if (!string.IsNullOrWhiteSpace(p.Email))
            query = query.Where(u => u.Email.Contains(p.Email));

        var pagedUsers = await query
            .OrderBy(u => u.Id)
            .ToPagedResultAsync(p.PageNumber, p.PageSize);

        var items = new List<UserDto>();

        foreach (var u in pagedUsers.Items)
        {
            var roleNames = await unitOfWork.UserRoles.Query()
                .Where(ur => ur.UserId == u.Id && ur.IsActive && ur.Role.IsActive)
                .Select(ur => ur.Role.Name)
                .ToListAsync();

            items.Add(new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                IsActive = u.IsActive,
                Roles = roleNames
            });
        }

        return new PagedResultDto<UserDto>
        {
            Items = items,
            TotalCount = pagedUsers.TotalCount,
            PageNumber = pagedUsers.PageNumber,
            PageSize = pagedUsers.PageSize
        };
    }

    public async Task<UserDto?> UpdateAsync(int id, UserUpdateDto dto)
    {
        var user = await unitOfWork.Users.GetByIdAsync(id);
        if (user == null) return null;

        var emailTaken = await unitOfWork.Users.Query().AnyAsync(u => u.Email == dto.Email && u.Id != id);
        if (emailTaken) throw new InvalidOperationException("This email already exists.");

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        unitOfWork.Users.Update(user);
        await unitOfWork.SaveChangesAsync();

        var roleNames = await unitOfWork.UserRoles.Query()
            .Where(ur => ur.UserId == id && ur.IsActive)
            .Select(ur => ur.Role.Name)
            .ToListAsync();

        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            IsActive = user.IsActive,
            Roles = roleNames
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await unitOfWork.Users.GetByIdAsync(id);
        if (user == null) return false;

        user.IsActive = false;
        unitOfWork.Users.Update(user);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddRoleToUserAsync(int userId, int roleId)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) throw new InvalidOperationException("User not found.");

        var role = await unitOfWork.Roles.GetByIdAsync(roleId);
        if (role == null) throw new InvalidOperationException("Role not found.");

        var existing = await unitOfWork.UserRoles.Query()
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

        if (existing != null)
        {
            if (existing.IsActive) return true;

            existing.IsActive = true;
            unitOfWork.UserRoles.Update(existing);
            await unitOfWork.SaveChangesAsync();
            return true;
        }

        await unitOfWork.UserRoles.AddAsync(new UserRole { UserId = userId, RoleId = roleId });
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId)
    {
        var userRole = await unitOfWork.UserRoles.Query()
            .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId && ur.IsActive);

        if (userRole == null) return false;

        userRole.IsActive = false;
        unitOfWork.UserRoles.Update(userRole);
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}