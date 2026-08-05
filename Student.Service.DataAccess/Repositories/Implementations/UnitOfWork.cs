using Student.Service.Core.Entities;
using Student.Service.DataAccess.Data;
using Student.Service.DataAccess.Repositories.Abstarctions;

namespace Student.Service.DataAccess.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IGenericRepository<Core.Entities.Student> Students { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Students = new GenericRepository<Core.Entities.Student>(_context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    

    
}