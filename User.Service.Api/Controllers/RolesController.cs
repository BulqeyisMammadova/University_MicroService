using Microsoft.AspNetCore.Mvc;
using User.Servic.Business.DTOs.PagitationsDtos;
using User.Servic.Business.DTOs.RoleDtos;
using User.Service.Business.Services.Abstractions;

namespace User.Service.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams p) {
        var result = await roleService.GetAllAsync(p);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await roleService.GetByIdAsync(id);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoleCreateDto dto)
    {
       var result = await roleService.CreateAsync(dto);
       return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, RoleUpdateDto dto)
    {
        
            var result = await roleService.UpdateAsync(id, dto);
            return result == null ? NotFound() : Ok(result);
        
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        
            var success = await roleService.DeleteAsync(id);
            return success ? Ok() : NotFound();
       
    }

    [HttpDelete("{id}/remove")]
    public async Task<IActionResult> HardDelete(int id)
    {
         var success = await roleService.HardDeleteAsync(id);
            return success ? Ok() : NotFound();
       
    }

    [HttpGet("{id}/permissions")]
    public async Task<IActionResult> GetRolePermissions(int id) {
        var result = await roleService.GetRolePermissionsAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/permissions/{permissionId}")]
    public async Task<IActionResult> AddPermission(int id, int permissionId)
    {
        
            var result = await roleService.AddPermissionToRoleAsync(id, permissionId);
            return Ok(result);
        
    }

    [HttpDelete("{id}/permissions/{permissionId}")]
    public async Task<IActionResult> RemovePermission(int id, int permissionId)
    {
        var success = await roleService.RemovePermissionFromRoleAsync(id, permissionId);
        return success ? Ok() : NotFound();
    }
}