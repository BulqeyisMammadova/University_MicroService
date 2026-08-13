using User.Servic.Business.DTOs.PagitationsDtos;
using User.Servic.Business.DTOs.PermissionDtos;
using User.Servic.Business.DTOs.RoleDtos;

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