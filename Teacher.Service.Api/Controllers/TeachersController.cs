using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teacher.Service.Business.DTOs;
using Teacher.Service.Business.Services.Abstractions;

namespace Teacher.Service.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeachersController(ITeacherService teacherService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams p)
    {
        var result = await teacherService.GetAllAsync(p);
        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var teacher = await teacherService.GetByIdAsync(id);
      

        return Ok(teacher);
    }


    [HttpPost]
    public async Task<IActionResult> Create(TeacherCreateDto dto)
    {
        if (dto.PhoneNumbers == null) return NotFound();
        var result = await teacherService.CreateAsync(dto);
        return Ok(result);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TeacherCreateDto dto)
    {
        var result = await teacherService.UpdateAsync(id, dto);
        return Ok(result);
    }    


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await teacherService.DeleteAsync(id);
        return Ok();
    }

    [HttpPost("{teacherId}/subjects")]
    public async Task<IActionResult> AssignSubject(int teacherId, AssignSubjectDto dto)
    {
        var success = await teacherService.AssignSubjectAsync(teacherId, dto);
        return Ok(success);
    }

    [HttpDelete("{teacherId}/subjects/{subjectId}")]
    public async Task<IActionResult> RemoveSubject(int teacherId, int subjectId)
    {
        var success = await teacherService.RemoveSubjectAsync(teacherId, subjectId);
        return Ok();
    }

    [HttpPost("{teacherId}/phones")]
    public async Task<IActionResult> AddPhone(int teacherId, AddPhoneDto dto)
    {
        var success = await teacherService.AddPhoneAsync(teacherId, dto);
        return Ok();
    }

    [HttpDelete("{teacherId}/phones/{phoneId}")]
    public async Task<IActionResult> RemovePhone(int teacherId, int phoneId)
    {
        var success = await teacherService.RemovePhoneAsync(teacherId, phoneId);
        return Ok();
    }


}

