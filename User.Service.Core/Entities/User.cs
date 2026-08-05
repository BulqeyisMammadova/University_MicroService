using User.Service.Core.Entities.Common;
using User.Service.Core.Enum;

namespace User.Service.Core.Entities;

public class User: BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
