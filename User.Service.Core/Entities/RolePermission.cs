using User.Service.Core.Entities.Common;

namespace User.Service.Core.Entities;

public class RolePermission : BaseEntity
{
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public int PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;

    public bool IsActive { get; set; } = true;
}

