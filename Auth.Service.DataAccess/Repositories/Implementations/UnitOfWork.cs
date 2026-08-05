using Auth.Service.Core.Entities;
using Auth.Service.DataAccess.Data;
using Auth.Service.DataAccess.Repositories.Abstarctions;

namespace Auth.Service.DataAccess.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IGenericRepository<RefreshToken> RefreshTokens { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        RefreshTokens = new GenericRepository<RefreshToken>(_context);
    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}