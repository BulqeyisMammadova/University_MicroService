using User.Service.Core.Entities.Common;

namespace User.Service.Core.Entities;

public class Permission : BaseEntity
{
    public string Name { get; set; } = string.Empty;         
    public bool IsActive { get; set; } = true;
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
