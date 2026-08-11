namespace User.Service.Business.DTOs;

public class RoleCreateDto
{
    public string Name { get; set; } = string.Empty;
    public List<int> PermissionIds { get; set; } = new();
}
