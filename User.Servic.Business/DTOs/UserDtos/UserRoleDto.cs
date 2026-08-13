using User.Servic.Business.DTOs.PermissionDtos;

namespace User.Servic.Business.DTOs.UserDtos;

public class UserRoleDto
{
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public bool IsActive { get; set; }          
    public List<PermissionDto> Permissions { get; set; } = new();
}
