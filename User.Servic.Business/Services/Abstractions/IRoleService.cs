using User.Servic.Business.DTOs;
using User.Service.Business.DTOs;

namespace User.Service.Business.Services.Abstractions;

public interface IRoleService
{
    Task<PagedResultDto<RoleDto>> GetAllAsync(PaginationParams p);
    Task<RoleDto?> GetByIdAsync(int id);
    Task<RoleDto> CreateAsync(RoleCreateDto dto);
    Task<RoleDto?> UpdateAsync(int id, RoleUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> HardDeleteAsync(int id);
    Task<List<PermissionDto>> GetRolePermissionsAsync(int roleId);
    Task<bool> AddPermissionToRoleAsync(int roleId, int permissionId);
    Task<bool> RemovePermissionFromRoleAsync(int roleId, int permissionId);

}