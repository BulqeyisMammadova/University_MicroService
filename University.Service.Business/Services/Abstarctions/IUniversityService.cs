using University.Service.Business.DTOs;

namespace University.Service.Business.Services.Abstarctions;

public interface IUniversityService
{
    Task<IEnumerable<UniversityDto>> GetAllAsync(PaginationParams paginationParams);
    Task<UniversityDto?> GetByIdAsync(int id);
    Task<UniversityDto> CreateAsync(UniversityCreateDto universityCreateDto);
    Task<UniversityDto?> UpdateAsync(int id, UniversityCreateDto universityUpdateDto);
    Task<bool> DeleteAsync(int id);
}
