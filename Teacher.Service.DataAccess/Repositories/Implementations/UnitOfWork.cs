using Teacher.Service.Core.Entities;
using Teacher.Service.DataAccess.Data;
using Teacher.Service.DataAccess.Repositories.Abstarctions;

namespace Teacher.Service.DataAccess.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IGenericRepository<Core.Entities.Teacher> Teachers { get; }
    public IGenericRepository<Core.Entities.TeacherPhone> TeacherPhones { get; }
    public IGenericRepository<Core.Entities.Subject> Subjects { get; }
    public IGenericRepository<Core.Entities.TeacherSubject> TeacherSubjects { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Teachers = new GenericRepository<Core.Entities.Teacher>(_context);
        TeacherPhones = new GenericRepository<Core.Entities.TeacherPhone>(_context);
        Subjects = new GenericRepository<Core.Entities.Subject>(_context);
        TeacherSubjects = new GenericRepository<Core.Entities.TeacherSubject>(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    
}