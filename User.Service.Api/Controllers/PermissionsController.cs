using Microsoft.AspNetCore.Mvc;
using User.Servic.Business.DTOs.PagitationsDtos;
using User.Servic.Business.DTOs.PermissionDtos;
using User.Service.Business.Services.Abstractions;

namespace User.Service.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermissionsController(IPermissionService permissionService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams p)
    {
        var result = await permissionService.GetAllAsync(p);
        return Ok(result);
    }
    
   

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await permissionService.GetByIdAsync(id);
        return Ok(result);
    }




    [HttpPost]
    public async Task<IActionResult> Create(PermissionCreateDto dto)
    {
        var result = await permissionService.CreateAsync(dto);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PermissionUpdateDto dto)
    {        
            var result = await permissionService.UpdateAsync(id, dto);
            return  Ok(result);
        
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await permissionService.DeleteAsync(id);
        return Ok();
    }

    [HttpDelete("{id}/remove")]
    public async Task<IActionResult> Remove(int id)
    {
        var success = await permissionService.HardDeleteAsync(id);
        return Ok();
    }
}
