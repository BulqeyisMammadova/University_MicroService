using Teacher.Service.Business.DTOs;

namespace Teacher.Service.Business.Services.Abstractions;

public interface ITeacherService
{
    Task<IEnumerable<TeacherDto>> GetAllAsync(PaginationParams p);
    Task<TeacherDto?> GetByIdAsync(int id);
    Task<TeacherDto> CreateAsync(TeacherCreateDto dto);
    Task<TeacherDto?> UpdateAsync(int id, TeacherCreateDto dto);
    Task<bool> DeleteAsync(int id);

    Task<bool> AddPhoneAsync(int teacherId, AddPhoneDto dto);
    Task<bool> RemovePhoneAsync(int teacherId, int phoneId);
    Task<bool> AssignSubjectAsync(int teacherId, AssignSubjectDto dto);
    Task<bool> RemoveSubjectAsync(int teacherId, int subjectId);
}
