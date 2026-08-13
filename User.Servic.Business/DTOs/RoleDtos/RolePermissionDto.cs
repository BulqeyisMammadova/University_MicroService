namespace User.Servic.Business.DTOs.RoleDtos;

public class RolePermissionDto
{
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}