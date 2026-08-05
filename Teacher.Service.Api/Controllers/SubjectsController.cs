using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teacher.Service.Business.DTOs;
using Teacher.Service.Business.Services.Abstractions;
using Teacher.Service.Business.Services.Implementations;

namespace Teacher.Service.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class SubjectsController(ISubjectService subjectService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams p)
    {
        var result = await subjectService.GetAllAsync(p);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var subject = await subjectService.GetByIdAsync(id);
        if (subject == null)
            return NotFound();

        return Ok(subject);
    }
    [HttpPost]
    public async Task<IActionResult> Create(SubjectCreateDto dto)
    {
        var result = await subjectService.CreateAsync(dto);
        if (result == null) return NotFound();
        return Ok(result);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SubjectCreateDto dto)
    {
        var result = await subjectService.UpdateAsync(id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await subjectService.DeleteAsync(id);
        if(success == false) return NotFound();
        return Ok(success);    
    }


}

