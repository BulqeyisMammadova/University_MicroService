using Microsoft.EntityFrameworkCore;
using Teacher.Service.Business.DTOs;
using Teacher.Service.Business.Extensions;
using Teacher.Service.Business.Services.Abstractions;
using Teacher.Service.Core.Entities;
using Teacher.Service.DataAccess.Repositories.Abstarctions;

namespace Teacher.Service.Business.Services.Implementations;

public class TeacherService(IUnitOfWork unitOfWork) : ITeacherService
{
    public async Task<IEnumerable<TeacherDto>> GetAllAsync(PaginationParams p)
    {
        return await unitOfWork.Teachers.Query()
            .OrderBy(t => t.Id)
            .Select(t => new TeacherDto
            {
                Id = t.Id,
                FullName = t.FullName,
                UniversityId = t.UniversityId,
                Phones = t.Phones
                    .Select(p => new PhoneDto { Id = p.Id, PhoneNumber = p.PhoneNumber })
                    .ToList(),
            })
            .ToPagedAsync(p);
    }

    public async Task<TeacherDto?> GetByIdAsync(int id)
    {
        var teacher = await unitOfWork.Teachers.Query()
            .Include(t => t.Phones)
            .Include(t => t.TeacherSubjects)
                .ThenInclude(ts => ts.Subject)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (teacher == null) return null;

        return MapToDto(teacher);
    }

    public async Task<TeacherDto> CreateAsync(TeacherCreateDto dto)
    {
        var teacher = new Core.Entities.Teacher
        {
            FullName = dto.FullName,
            UniversityId = dto.UniversityId

        };

        foreach (var phone in dto.PhoneNumbers)
        {
            teacher.Phones.Add(new TeacherPhone { PhoneNumber = phone });
        }

        await unitOfWork.Teachers.AddAsync(teacher);
        await unitOfWork.SaveChangesAsync();

        return MapToDto(teacher);
    }

    public async Task<TeacherDto?> UpdateAsync(int id, TeacherCreateDto dto)
    {
        var teacher = await unitOfWork.Teachers.Query()
            .Include(t => t.Phones)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (teacher == null) return null;

        teacher.FullName = dto.FullName;
        teacher.UniversityId = dto.UniversityId;

        foreach (var oldPhone in teacher.Phones.ToList())
        {
            unitOfWork.TeacherPhones.Delete(oldPhone);
        }
        teacher.Phones.Clear();

        foreach (var phone in dto.PhoneNumbers)
        {
            teacher.Phones.Add(new TeacherPhone { PhoneNumber = phone });
        }

        unitOfWork.Teachers.Update(teacher);
        await unitOfWork.SaveChangesAsync();

        return MapToDto(teacher);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var teacher = await unitOfWork.Teachers.GetByIdAsync(id);
        if (teacher == null) return false;

        unitOfWork.Teachers.Delete(teacher);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddPhoneAsync(int teacherId, AddPhoneDto dto)
    {
        var teacher = await unitOfWork.Teachers.GetByIdAsync(teacherId);
        if (teacher == null) return false;

        var phone = new TeacherPhone
        {
            TeacherId = teacherId,
            PhoneNumber = dto.PhoneNumber
        };

        await unitOfWork.TeacherPhones.AddAsync(phone);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemovePhoneAsync(int teacherId, int phoneId)
    {
        var phone = await unitOfWork.TeacherPhones.GetByIdAsync(phoneId);
        if (phone == null || phone.TeacherId != teacherId) return false;

        unitOfWork.TeacherPhones.Delete(phone);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AssignSubjectAsync(int teacherId, AssignSubjectDto dto)
    {
        var teacherExists = await unitOfWork.Teachers.GetByIdAsync(teacherId);
        var subjectExists = await unitOfWork.Subjects.GetByIdAsync(dto.SubjectId);

        if (teacherExists == null || subjectExists == null) return false;

        var alreadyAssigned = await unitOfWork.TeacherSubjects.Query()
            .AnyAsync(ts => ts.TeacherId == teacherId && ts.SubjectId == dto.SubjectId);

        if (alreadyAssigned) return false;

        var teacherSubject = new TeacherSubject
        {
            TeacherId = teacherId,
            SubjectId = dto.SubjectId
        };

        await unitOfWork.TeacherSubjects.AddAsync(teacherSubject);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveSubjectAsync(int teacherId, int subjectId)
    {
        var teacherSubject = await unitOfWork.TeacherSubjects.Query()
            .FirstOrDefaultAsync(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId);

        if (teacherSubject == null) return false;

        unitOfWork.TeacherSubjects.Delete(teacherSubject);
        await unitOfWork.SaveChangesAsync();
        return true;
    }

    private static TeacherDto MapToDto(Core.Entities.Teacher teacher)
    {
        return new TeacherDto
        {
            Id = teacher.Id,
            FullName = teacher.FullName,
            UniversityId = teacher.UniversityId,
            Phones = teacher.Phones
                .Select(p => new PhoneDto { Id = p.Id, PhoneNumber = p.PhoneNumber })
                .ToList(),
            Subjects = teacher.TeacherSubjects
                .Where(ts => ts.Subject != null)
                .Select(ts => new SubjectDto { Id = ts.Subject.Id, Name = ts.Subject.Name })
                .ToList()
        };
    }
}