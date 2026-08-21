using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student.Service.Business.DTOs;
using Student.Service.Business.Services.Abstarctions;

namespace Student.Service.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController(IStudentService studentService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams p)
    {
        var list = await studentService.GetAllAsync(p);
        return Ok(list);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var student = await studentService.GetByIdAsync(id);
        return Ok(student);
    }

    [HttpPost]
    public async Task<IActionResult> Create(StudentCreateDto dto)
    {
        var result = await studentService.CreateAsync(dto);
        return Ok(result);
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await studentService.DeleteAsync(id);       
        return Ok();
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, StudentCreateDto studentUpdateDto)
    {
        var result = await studentService.UpdateAsync(id, studentUpdateDto);
        return Ok(result);
    }

}
