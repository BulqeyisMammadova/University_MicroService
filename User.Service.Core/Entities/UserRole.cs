using User.Service.Core.Entities.Common;

namespace User.Service.Core.Entities;

public class UserRole : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}