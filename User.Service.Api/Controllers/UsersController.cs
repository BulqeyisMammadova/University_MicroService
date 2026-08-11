using Microsoft.AspNetCore.Mvc;
using User.Servic.Business.DTOs;
using User.Service.Business.DTOs;
using User.Service.Business.Services.Abstractions;

namespace User.Service.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var result = await userService.RegisterAsync(dto);
        if(result == null) return BadRequest("This email is already exists or role not selected.");
        return Ok(result);
        
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await userService.LoginAsync(dto);
        if(result == null) return BadRequest("This email is not registered or password is incorrect.");
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] UserPaginationParams p)
    {
        var result = await userService.GetAllUsersAsync(p);
        if(result == null) return BadRequest ("Failed to retrieve users.");
        return Ok(result);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UserUpdateDto dto)
    {
          var result = await userService.UpdateAsync(id, dto);
            return result == null ? NotFound() : Ok(result);
       
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await userService.DeleteAsync(id);
        return success ? Ok() : NotFound();
    }

    
    [HttpPost("{id}/roles/{roleId}")]
    public async Task<IActionResult> AddRole(int id, int roleId)
    {
       
            var result = await userService.AddRoleToUserAsync(id, roleId);
            return Ok(result);
        
       
    }

    [HttpDelete("{id}/roles/{roleId}")]
    public async Task<IActionResult> RemoveRole(int id, int roleId)
    {
        var success = await userService.RemoveRoleFromUserAsync(id, roleId);
        return success ? Ok() : NotFound();
    }
}
