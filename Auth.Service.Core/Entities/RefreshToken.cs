using Auth.Service.Core.Entities.Common;

namespace Auth.Service.Core.Entities;

public class RefreshToken : BaseEntity
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string Permissions { get; set; } = string.Empty; 
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}