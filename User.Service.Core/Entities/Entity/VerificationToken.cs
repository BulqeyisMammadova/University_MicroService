using User.Service.Core.Entities.Common;
using User.Service.Core.Entities.Enum;

namespace User.Service.Core.Entities.Entity;

public class VerificationToken : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public string Token { get; set; } = string.Empty;
    public VerificationTokenType Type { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; } = false;

}

