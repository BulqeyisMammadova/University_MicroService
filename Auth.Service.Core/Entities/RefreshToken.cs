using Auth.Service.Core.Entities.Common;
using Auth.Service.Core.Enums;

namespace Auth.Service.Core.Entities;

public class RefreshToken : BaseEntity
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
