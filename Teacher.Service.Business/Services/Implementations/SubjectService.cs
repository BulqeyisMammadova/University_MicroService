using Teacher.Service.Business.DTOs;
using Teacher.Service.Business.Exceptions;
using Teacher.Service.Business.Extensions;
using Teacher.Service.Business.Services.Abstractions;
using Teacher.Service.Core.Entities;
using Teacher.Service.DataAccess.Repositories.Abstarctions;


namespace Teacher.Service.Business.Services.Implementations;

public class SubjectService(IUnitOfWork unitOfWork) : ISubjectService
{
    public async Task<IEnumerable<SubjectDto>> GetAllAsync(PaginationParams p)
    {
        return await unitOfWork.Subjects.Query()
            .OrderBy(s => s.Id)
            .Select(s => new SubjectDto { Id = s.Id, Name = s.Name })
            .ToPagedAsync(p);
    }

    public async Task<SubjectDto?> GetByIdAsync(int id)
    {
        var subject = await unitOfWork.Subjects.GetByIdAsync(id);
        if (subject == null) throw new NotFoundException("Teacher not found");

        return new SubjectDto { Id = subject.Id, Name = subject.Name };
    }

    public async Task<SubjectDto> CreateAsync(SubjectCreateDto dto)
    {
        var subject = new Subject { Name = dto.Name };

        await unitOfWork.Subjects.AddAsync(subject);
        await unitOfWork.SaveChangesAsync();

        return new SubjectDto { Id = subject.Id, Name = subject.Name };
    }

    public async Task<SubjectDto?> UpdateAsync(int id, SubjectCreateDto dto)
    {
        var subject = await unitOfWork.Subjects.GetByIdAsync(id);
        if (subject == null) throw new NotFoundException("Teacher not found");

        subject.Name = dto.Name;
        unitOfWork.Subjects.Update(subject);
        await unitOfWork.SaveChangesAsync();

        return new SubjectDto { Id = subject.Id, Name = subject.Name };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var subject = await unitOfWork.Subjects.GetByIdAsync(id);
        if (subject == null) throw new NotFoundException("Teacher not found");

        unitOfWork.Subjects.Delete(subject);
        await unitOfWork.SaveChangesAsync();
        return true;
    }
}