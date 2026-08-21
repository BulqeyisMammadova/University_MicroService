using University.Service.Business.DTOs;
using University.Service.Business.Exception;
using University.Service.Business.Extensions;
using University.Service.Business.Services.Abstarctions;
using University.Service.Core.Entities;
using University.Service.DataAccess.Repositories.Abstarctions;

namespace University.Service.Business.Services.Implementations;

public class UniversityService(IUnitOfWork unitOfWork) : IUniversityService
{
    public async Task<IEnumerable<UniversityDto>> GetAllAsync(PaginationParams paginationParams)
    {
        var universities = await unitOfWork.Universities.Query()
            .OrderBy(u => u.Id)
            .Select(u => new UniversityDto
            {
                Id = u.Id,
                Name = u.Name
            })
            .ToPagedAsync(paginationParams);

        return universities;
    }

    public async Task<UniversityDto?> GetByIdAsync(int id)
    {
        var university = await unitOfWork.Universities.GetByIdAsync(id);
        if (university == null) throw new NotFoundExceptions("University not found ");

        return new UniversityDto
        {
            Id = university.Id,
            Name = university.Name
        };
    }

    public async Task<UniversityDto> CreateAsync(UniversityCreateDto universityCreateDto)
    {
        var university = new Core.Entities.University
        {
            Name = universityCreateDto.Name
        };

        await unitOfWork.Universities.AddAsync(university);
        await unitOfWork.SaveChangesAsync();

        return new UniversityDto
        {
            Id = university.Id,
            Name = university.Name
        };
    }

    public async Task<UniversityDto?> UpdateAsync(int id, UniversityCreateDto universityUpdateDto)
    {
        var university = await unitOfWork.Universities.GetByIdAsync(id);
        if (university == null) throw new NotFoundExceptions("University not found ");

        university.Name = universityUpdateDto.Name;
        unitOfWork.Universities.Update(university);
        await unitOfWork.SaveChangesAsync();

        return new UniversityDto
        {
            Id = university.Id,
            Name = university.Name
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var university = await unitOfWork.Universities.GetByIdAsync(id);
        if (university == null) throw new NotFoundExceptions("University not found ");

        unitOfWork.Universities.Delete(university);
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}