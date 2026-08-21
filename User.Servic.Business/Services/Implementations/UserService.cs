using Microsoft.EntityFrameworkCore;
using User.Servic.Business.DTOs.MailDtos;
using User.Servic.Business.DTOs.PagitationsDtos;
using User.Servic.Business.DTOs.PermissionDtos;
using User.Servic.Business.DTOs.TokenDtos;
using User.Servic.Business.DTOs.UserDtos;
using User.Servic.Business.Exceptions;
using User.Servic.Business.Services.Abstractions;
using User.Service.Business.Extensions;
using User.Service.Business.Services.Abstractions;
using User.Service.Core.Entities.Entity;
using User.Service.Core.Entities.Enum;
using User.Service.DataAccess.Repositories.Abstarctions;

namespace User.Service.Business.Services.Implementations;

public class UserService(IUnitOfWork unitOfWork, IAuthServiceClient authServiceClient, IEmailService emailService) : IUserService
{
    public async Task<UserDto> RegisterAsync(RegisterDto dto)
    {
        var exists = await unitOfWork.Users.Query().AnyAsync(u => u.Email == dto.Email);
        if (exists) throw new ConflictException("This email already exists.");
        if (dto.RoleIds.Count == 0)
            throw new BadRequestException("Role not selected.");
        var user = new Core.Entities.Entity.User
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

        
        var token = Guid.NewGuid().ToString().Substring(0, 4);

        await unitOfWork.VerificationTokens.AddAsync(new VerificationToken
        {
            UserId = user.Id,
            Token = token,
            Type = VerificationTokenType.EmailConfirmation,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        });
        await unitOfWork.SaveChangesAsync();


        await emailService.SendEmailAsync(user.Email, "Verify your account",
            $"<p>To verify your account  <b>{token}</b></p>");
       ;
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
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.IsActive);

        if (user == null) throw new NotFoundException("User not found.");
        var isConfirmed = await unitOfWork.Users.Query()
            .Where(u => u.Id == user.Id)
            .Select(u => u.IsEmailConfirmed)
            .FirstOrDefaultAsync();

        if (!isConfirmed) throw new NotFoundException("User not found"); 

        var passwordOk = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!passwordOk)  throw new NotFoundException("User not found"); ;

        var activeRoleIds = await unitOfWork.UserRoles.Query()
            .Where(ur => ur.UserId == user.Id && ur.IsActive)
            .Select(ur => ur.RoleId)
            .ToListAsync();

        var roleNames = new List<string>();
        var permissionNames = new List<string>();

        foreach (var roleId in activeRoleIds)
        {
            var role = await unitOfWork.Roles.GetByIdAsync(roleId);
            if (role == null || !role.IsActive) continue;

            roleNames.Add(role.Name);

            var permissions = await unitOfWork.RolePermissions.Query()
                .Where(rp => rp.RoleId == roleId && rp.IsActive && rp.Permission.IsActive)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();

            foreach (var permissionName in permissions)
            {
                if (!permissionNames.Contains(permissionName))
                    permissionNames.Add(permissionName);
            }
        }

        var combinedRoleName = string.Join(",", roleNames);

        return await authServiceClient.GenerateTokenAsync(user.Id, user.Email, combinedRoleName, permissionNames);
    }

    public async Task<PagedResultDto<UserDto>> GetAllUsersAsync(UserPaginationParams p)
    {
        IQueryable<Core.Entities.Entity.User> query = unitOfWork.Users.Query();

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
            var activeRoleNames = await unitOfWork.UserRoles.Query()
                .Where(ur => ur.UserId == u.Id && ur.IsActive && ur.Role.IsActive)
                .Select(ur => ur.Role.Name)
                .ToListAsync();

            items.Add(new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                IsActive = u.IsActive,
                IsConfirmed = u.IsEmailConfirmed,
                Roles = activeRoleNames
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

    public async Task<GetUserDto?> GetByIdAsync(int id)
    {
        var user = await unitOfWork.Users.GetByIdAsync(id);
        if (user == null) throw new NotFoundException("User not found"); ;

        var userRoles = await unitOfWork.UserRoles.Query()
            .Where(ur => ur.UserId == id)
            .ToListAsync();

        var roles = new List<UserRoleDto>();

        foreach (var userRole in userRoles)
        {
            var role = await unitOfWork.Roles.GetByIdAsync(userRole.RoleId);
            if (role == null) continue;

            var permissions = await unitOfWork.RolePermissions.Query()
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => new PermissionDto
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name,
                    IsActive = rp.IsActive
                })
                .ToListAsync();

            roles.Add(new UserRoleDto
            {
                RoleId = role.Id,
                RoleName = role.Name,
                IsActive = userRole.IsActive,
                Permissions = permissions
            });
        }

        return new GetUserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            IsActive = user.IsActive,
            Roles = roles
        };
    }

    public async Task<UserDto?> UpdateAsync(int id, UserUpdateDto dto)
    {
        var user = await unitOfWork.Users.GetByIdAsync(id);
        if (user == null) throw new NotFoundException("User not found"); ;

        var emailTaken = await unitOfWork.Users.Query().AnyAsync(u => u.Email == dto.Email && u.Id != id);
        if (emailTaken)   throw new ConflictException("This email already exists.");

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        unitOfWork.Users.Update(user);
        await unitOfWork.SaveChangesAsync();

        var roleNames = await unitOfWork.UserRoles.Query()
            .Where(ur => ur.UserId == id && ur.IsActive && ur.Role.IsActive)
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
        if (user == null) throw new NotFoundException("User not found");

        user.IsActive = false;
        unitOfWork.Users.Update(user);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddRoleToUserAsync(int userId, int roleId)
    {
        var user = await unitOfWork.Users.GetByIdAsync(userId);
       if (user == null) throw new NotFoundException("User not found.");

        var role = await unitOfWork.Roles.GetByIdAsync(roleId);
        if (role == null) throw new NotFoundException("Role not found.");

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

    public async Task<bool> ConfirmEmailAsync(ConfirmMailDto dto)
    {
        var token = await unitOfWork.VerificationTokens.Query()
            .FirstOrDefaultAsync(t => t.Token == dto.Token
                && t.Type == VerificationTokenType.EmailConfirmation
                && !t.IsUsed
                && t.ExpiresAt > DateTime.UtcNow);

        if (token == null) return false;

        var user = await unitOfWork.Users.GetByIdAsync(token.UserId);
        if (user == null) return false;

        user.IsEmailConfirmed = true;
        unitOfWork.Users.Update(user);

        token.IsUsed = true;
        unitOfWork.VerificationTokens.Update(token);

        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto dto)
    {
        var user =await unitOfWork.Users.Query()
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.IsActive);
        if (user == null) return;

        var token = Guid.NewGuid().ToString().Substring(0, 4);
        await unitOfWork.VerificationTokens.AddAsync(new VerificationToken
        {
            UserId = user.Id,
            Token = token,
            Type = VerificationTokenType.PasswordReset,
            ExpiresAt = DateTime.UtcNow.AddHours(1),
        });
        await unitOfWork.SaveChangesAsync();

        await emailService.SendEmailAsync(user.Email,"Reset Password",
            $"<p>To reset your password, click <b>{token}</b></p>");
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var token = await unitOfWork.VerificationTokens.Query().
            FirstOrDefaultAsync(t=> t.Token == dto.Token 
            && t.Type == VerificationTokenType.PasswordReset
            && !t.IsUsed
            && t.ExpiresAt > DateTime.UtcNow);
        if(token == null) return false;

        var user = await unitOfWork.Users.GetByIdAsync(token.UserId);
        if (user == null) return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
        unitOfWork.Users.Update(user);

        token.IsUsed = true;
        unitOfWork.VerificationTokens.Update(token);
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}

