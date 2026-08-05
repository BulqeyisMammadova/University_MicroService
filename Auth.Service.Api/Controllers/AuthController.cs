using Auth.Service.Business.DTOs;
using Auth.Service.Business.Services.Abstarctions;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Service.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ITokenService tokenService) : ControllerBase
{
    [HttpPost("token")]
    public async Task<IActionResult> GenerateToken(TokenRequestDto dto)
    {
        var result = await tokenService.GenerateTokenAsync(dto);
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenDto dto)
    {
        var result = await tokenService.RefreshTokenAsync(dto.RefreshToken);

        if (result == null) return Unauthorized(new { message = "Refresh token yararsizdir." });
        return Ok(result);
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke(RevokeTokenDto dto)
    {
        var success = await tokenService.RevokeAsync(dto.RefreshToken);
        if (!success) return NotFound();
        return Ok();
    }
}