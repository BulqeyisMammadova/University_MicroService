using User.Servic.Business.DTOs;
using User.Service.Business.DTOs;

namespace User.Service.Business.Services.Abstractions;

public interface IPermissionService
{
    Task<PagedResultDto<PermissionDto>> GetAllAsync(PaginationParams p);
    Task<PermissionDto> CreateAsync(PermissionCreateDto dto);
    Task<PermissionDto?> GetByIdAsync(int id);
    Task<PermissionDto?> UpdateAsync(int id, PermissionUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> HardDeleteAsync(int id);
}
