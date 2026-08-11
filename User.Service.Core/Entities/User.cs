using User.Service.Core.Entities.Common;

namespace User.Service.Core.Entities;

public class User: BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
