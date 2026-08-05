using Student.Service.Business.DTOs;

namespace Student.Service.Business.Services.Abstarctions;

public interface IStudentService
{
    Task<IEnumerable<StudentDto>> GetAllAsync(PaginationParams p);
    Task<StudentDto?> GetByIdAsync(int id);
    Task<StudentDto> CreateAsync(StudentCreateDto dto);
    Task<StudentDto?> UpdateAsync(int id, StudentCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
