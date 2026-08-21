using Student.Service.Business.DTOs;
using Student.Service.Business.Exceptions;
using Student.Service.Business.Extensions;
using Student.Service.Business.Services.Abstarctions;
using Student.Service.DataAccess.Repositories.Abstarctions;

namespace Student.Service.Business.Services.Implementations;

public class StudentService(IUnitOfWork unitOfWork) : IStudentService
{
    public async Task<IEnumerable<StudentDto>> GetAllAsync(PaginationParams p)
    {
        return await unitOfWork.Students.Query()
            .OrderBy(s => s.Id)
            .Select(s => new StudentDto
            {
                Id = s.Id,
                FullName = s.FullName,
                UniversityId = s.UniversityId,
                TeacherId = s.TeacherId
            })
            .ToPagedAsync(p);
    }

    public async Task<StudentDto?> GetByIdAsync(int id)
    {
        var student = await unitOfWork.Students.GetByIdAsync(id);
        if (student == null) throw new NotFoundException("Student not found.");

        return MapToDto(student);
    }

    public async Task<StudentDto> CreateAsync(StudentCreateDto dto)
    {
        var student = new Core.Entities.Student
        {
            FullName = dto.FullName,
            UniversityId = dto.UniversityId,
            TeacherId = dto.TeacherId
        };

        await unitOfWork.Students.AddAsync(student);
        await unitOfWork.SaveChangesAsync();

        return MapToDto(student);
    }

    public async Task<StudentDto?> UpdateAsync(int id, StudentCreateDto dto)
    {
        var student = await unitOfWork.Students.GetByIdAsync(id);
        if (student == null) throw new NotFoundException("Student not found");

        student.FullName = dto.FullName;
        student.UniversityId = dto.UniversityId;
        student.TeacherId = dto.TeacherId;

        unitOfWork.Students.Update(student);
        await unitOfWork.SaveChangesAsync();

        return MapToDto(student);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await unitOfWork.Students.GetByIdAsync(id);
        if (student == null) throw new NotFoundException("Student not found");

        unitOfWork.Students.Delete(student);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    private static StudentDto MapToDto(Core.Entities.Student student)
    {
        return new StudentDto
        {
            Id = student.Id,
            FullName = student.FullName,
            UniversityId = student.UniversityId,
            TeacherId = student.TeacherId
        };
    }
}