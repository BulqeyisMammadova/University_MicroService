using University.Service.DataAccess.Data;
using University.Service.DataAccess.Repositories.Abstarctions;

namespace University.Service.DataAccess.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IGenericRepository<Core.Entities.University> Universities { get; }
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Universities = new GenericRepository<Core.Entities.University>(_context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
   
}
