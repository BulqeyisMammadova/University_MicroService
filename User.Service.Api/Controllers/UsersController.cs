using Microsoft.AspNetCore.Mvc;
using User.Servic.Business.DTOs.MailDtos;
using User.Servic.Business.DTOs.PagitationsDtos;
using User.Servic.Business.DTOs.UserDtos;
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
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var result = await userService.LoginAsync(dto);
        return Ok(result);
    }




    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(ConfirmMailDto dto)
    {
        var success = await userService.ConfirmEmailAsync(dto);
        return  Ok() ;
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
    {
        await userService.ForgotPasswordAsync(dto);
        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
    {
        var success = await userService.ResetPasswordAsync(dto);
        return  Ok() ;
    }








    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] UserPaginationParams p)
    {
        var result = await userService.GetAllUsersAsync(p);
        if(result == null) return BadRequest ("Failed to retrieve users.");
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await userService.GetByIdAsync(id);
        return  Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UserUpdateDto dto)
    {
        var result = await userService.UpdateAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await userService.DeleteAsync(id);
        return Ok();
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
        return  Ok() ;
    }
}
