using Microsoft.EntityFrameworkCore;
using User.Servic.Business.DTOs;
using User.Service.Business.DTOs;
using User.Service.Business.Extensions;
using User.Service.Business.Services.Abstractions;
using User.Service.Core.Entities;
using User.Service.DataAccess.Repositories.Abstarctions;

namespace User.Service.Business.Services.Implementations;

public class RoleService(IUnitOfWork unitOfWork) : IRoleService
{
    public async Task<PagedResultDto<RoleDto>> GetAllAsync(PaginationParams p)
    {
        IQueryable<Role> query = unitOfWork.Roles.Query().Where(r => r.IsActive);

        if (!string.IsNullOrWhiteSpace(p.Name))
            query = query.Where(r => r.Name.Contains(p.Name));

        var pagedRoles = await query
            .OrderBy(r => r.Id)
            .ToPagedResultAsync(p.PageNumber, p.PageSize);

        var items = new List<RoleDto>();

        foreach (var role in pagedRoles.Items)
        {
            var permissionNames = await unitOfWork.RolePermissions.Query()
                .Where(rp => rp.RoleId == role.Id && rp.IsActive && rp.Permission.IsActive)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();

            items.Add(new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                IsActive = role.IsActive,
                Permissions = permissionNames
            });
        }

        return new PagedResultDto<RoleDto>
        {
            Items = items,
            TotalCount = pagedRoles.TotalCount,
            PageNumber = pagedRoles.PageNumber,
            PageSize = pagedRoles.PageSize
        };
    }

    public async Task<RoleDto?> GetByIdAsync(int id)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(id);
        if (role == null) return null;

        var permissionNames = await unitOfWork.RolePermissions.Query()
            .Where(rp => rp.RoleId == id && rp.IsActive && rp.Permission.IsActive)
            .Select(rp => rp.Permission.Name)
            .OrderBy(name => name)
            .ToListAsync();

        return new RoleDto { Id = role.Id, Name = role.Name, IsActive = role.IsActive, Permissions = permissionNames };
    }

    public async Task<RoleDto> CreateAsync(RoleCreateDto dto)
    {
        var exists = await unitOfWork.Roles.Query().AnyAsync(r => r.Name == dto.Name);
        if (exists) throw new InvalidOperationException("This role already exists.");

        var role = new Role { Name = dto.Name };
        await unitOfWork.Roles.AddAsync(role);
        await unitOfWork.SaveChangesAsync();

        var permissionNames = new List<string>();

        foreach (var permissionId in dto.PermissionIds.Distinct())
        {
            var permission = await unitOfWork.Permissions.GetByIdAsync(permissionId);
            if (permission == null) continue;

            await unitOfWork.RolePermissions.AddAsync(new RolePermission { RoleId = role.Id, PermissionId = permissionId });
            permissionNames.Add(permission.Name);
        }

        await unitOfWork.SaveChangesAsync();

        return new RoleDto { Id = role.Id, Name = role.Name, IsActive = role.IsActive, Permissions = permissionNames };
    }

    public async Task<RoleDto?> UpdateAsync(int id, RoleUpdateDto dto)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(id);
        if (role == null) return null;

        var nameTaken = await unitOfWork.Roles.Query().AnyAsync(r => r.Name == dto.Name && r.Id != id);
        if (nameTaken) throw new InvalidOperationException("This role name already exists.");

        role.Name = dto.Name;
        unitOfWork.Roles.Update(role);
        await unitOfWork.SaveChangesAsync();

        var permissionNames = await unitOfWork.RolePermissions.Query()
            .Where(rp => rp.RoleId == id && rp.IsActive)
            .Select(rp => rp.Permission.Name)
            .ToListAsync();

        return new RoleDto { Id = role.Id, Name = role.Name, IsActive = role.IsActive, Permissions = permissionNames };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(id);
        if (role == null) return false;

        var hasActiveUsers = await unitOfWork.UserRoles.Query()
            .AnyAsync(ur => ur.RoleId == id && ur.IsActive);
        if (hasActiveUsers)
            throw new InvalidOperationException("This role has active users.");

        role.IsActive = false;
        unitOfWork.Roles.Update(role);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HardDeleteAsync(int id)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(id);
        if (role == null) return false;

        var hasUsers = await unitOfWork.UserRoles.Query().AnyAsync(ur => ur.RoleId == id);
        if (hasUsers)
            throw new InvalidOperationException("There are users assigned to this role, please change their roles first.");

        unitOfWork.Roles.Delete(role);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<List<PermissionDto>> GetRolePermissionsAsync(int roleId)
    {
        return await unitOfWork.RolePermissions.Query()
            .Where(rp => rp.RoleId == roleId && rp.IsActive && rp.Permission.IsActive)
            .Select(rp => new PermissionDto
            {
                Id = rp.Permission.Id,
                Name = rp.Permission.Name,
                IsActive = rp.Permission.IsActive
            })
            .ToListAsync();
    }

    public async Task<bool> AddPermissionToRoleAsync(int roleId, int permissionId)
    {
        var role = await unitOfWork.Roles.GetByIdAsync(roleId);
        if (role == null) throw new InvalidOperationException("Role not found.");

        var permission = await unitOfWork.Permissions.GetByIdAsync(permissionId);
        if (permission == null) throw new InvalidOperationException("Permission not found.");

        var existing = await unitOfWork.RolePermissions.Query()
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        if (existing != null)
        {
            if (existing.IsActive) return true;

            existing.IsActive = true;
            unitOfWork.RolePermissions.Update(existing);
            await unitOfWork.SaveChangesAsync();
            return true;
        }

        await unitOfWork.RolePermissions.AddAsync(new RolePermission { RoleId = roleId, PermissionId = permissionId });
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemovePermissionFromRoleAsync(int roleId, int permissionId)
    {
        var rolePermission = await unitOfWork.RolePermissions.Query()
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId && rp.IsActive);

        if (rolePermission == null) return false;

        rolePermission.IsActive = false;
        unitOfWork.RolePermissions.Update(rolePermission);
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}