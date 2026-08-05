using User.Service.DataAccess.Data;
using User.Service.DataAccess.Repositories.Abstarctions;

namespace User.Service.DataAccess.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IGenericRepository<User.Service.Core.Entities.User> Users { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new GenericRepository<User.Service.Core.Entities.User>(_context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}