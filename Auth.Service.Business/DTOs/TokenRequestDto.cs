using Auth.Service.Core.Enums;

namespace Auth.Service.Business.DTOs;

public class TokenRequestDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public Role Role { get; set; } 
}
