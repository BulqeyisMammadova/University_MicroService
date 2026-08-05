using Teacher.Service.Business.DTOs;

namespace Teacher.Service.Business.Services.Abstractions;

public interface ISubjectService
{
    Task<IEnumerable<SubjectDto>> GetAllAsync(PaginationParams p);
    Task<SubjectDto?> GetByIdAsync(int id);
    Task<SubjectDto> CreateAsync(SubjectCreateDto dto);
    Task<SubjectDto?> UpdateAsync(int id, SubjectCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
