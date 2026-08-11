using Microsoft.EntityFrameworkCore;
using User.Servic.Business.DTOs;
using User.Service.Business.DTOs;
using User.Service.Business.Extensions;
using User.Service.Business.Services.Abstractions;
using User.Service.Core.Entities;
using User.Service.DataAccess.Repositories.Abstarctions;

namespace User.Service.Business.Services.Implementations;

public class PermissionService(IUnitOfWork unitOfWork) : IPermissionService
{
    public async Task<PagedResultDto<PermissionDto>> GetAllAsync(PaginationParams p)
    {
        var query = unitOfWork.Permissions.Query().Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(p.Name))
            query = query.Where(x => x.Name.Contains(p.Name));

        return await query
            .OrderBy(x => x.Name)
            .Select(x => new PermissionDto { Id = x.Id, Name = x.Name, IsActive = x.IsActive })
            .ToPagedResultAsync(p.PageNumber, p.PageSize);
    }

    public async Task<PermissionDto> CreateAsync(PermissionCreateDto dto)
    {
        var exists = await unitOfWork.Permissions.Query().AnyAsync(p => p.Name == dto.Name);
        if (exists) throw new Exception("This permission already exists.");

        var permission = new Permission { Name = dto.Name };
        await unitOfWork.Permissions.AddAsync(permission);
        await unitOfWork.SaveChangesAsync();

        return new PermissionDto { Id = permission.Id, Name = permission.Name, IsActive = permission.IsActive };
    }

    public async Task<PermissionDto?> GetByIdAsync(int id)
    {
        var permission = await unitOfWork.Permissions.GetByIdAsync(id);
        if (permission == null) return null;

        return new PermissionDto { Id = permission.Id, Name = permission.Name, IsActive = permission.IsActive };
    }

    public async Task<PermissionDto?> UpdateAsync(int id, PermissionUpdateDto dto)
    {
        var permission = await unitOfWork.Permissions.GetByIdAsync(id);
        if (permission == null) return null;

        var nameTaken = await unitOfWork.Permissions.Query().AnyAsync(p => p.Name == dto.Name && p.Id != id);
        if (nameTaken) throw new Exception("This permission name already exists.");

        permission.Name = dto.Name;
       
        unitOfWork.Permissions.Update(permission);
        await unitOfWork.SaveChangesAsync();

        return new PermissionDto { Id = permission.Id, Name = permission.Name,  IsActive = permission.IsActive };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var permission = await unitOfWork.Permissions.GetByIdAsync(id);
        if (permission == null) return false;

        permission.IsActive = false;
        unitOfWork.Permissions.Update(permission);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HardDeleteAsync(int id)
    {
        var permission = await unitOfWork.Permissions.GetByIdAsync(id);
        if (permission == null) return false;

        unitOfWork.Permissions.Delete(permission);  
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}