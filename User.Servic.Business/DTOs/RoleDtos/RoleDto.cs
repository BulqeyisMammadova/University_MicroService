using User.Servic.Business.DTOs.PermissionDtos;

namespace User.Servic.Business.DTOs.RoleDtos;

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<PermissionDto> Permissions { get; set; } = new();
}



