using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using University.Service.Business.DTOs;
using University.Service.Business.Services.Abstarctions;

namespace University.Service.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UniversitiesController(IUniversityService universityService ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]  PaginationParams p)
    {
        var list = await universityService.GetAllAsync(p);
        return Ok(list);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var university = await universityService.GetByIdAsync(id);
        if (university == null) return NotFound();
        return Ok(university);

    }    

    [HttpPost]
    public async Task<IActionResult> Create(UniversityCreateDto dto)
    {
        var result = await universityService.CreateAsync(dto);
        return Ok(result);
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await universityService.DeleteAsync(id);
        if (success == false) return NotFound();
        return Ok();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UniversityCreateDto universityUpdateDto)
    {
        var result = await universityService.UpdateAsync(id, universityUpdateDto);
        if(result == null) return NotFound();
        return  Ok(result);
    }

}

