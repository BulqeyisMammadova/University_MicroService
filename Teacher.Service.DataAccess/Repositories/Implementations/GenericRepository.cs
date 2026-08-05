using Microsoft.EntityFrameworkCore;
using Teacher.Service.Core.Entities.Common;
using Teacher.Service.DataAccess.Data;
using Teacher.Service.DataAccess.Repositories.Abstarctions;

namespace Teacher.Service.DataAccess.Repositories.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}