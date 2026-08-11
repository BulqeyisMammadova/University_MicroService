namespace User.Service.Business.DTOs;

public class RolePermissionDto
{
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}