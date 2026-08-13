using User.Service.Core.Entities.Entity;
using User.Service.DataAccess.Data;
using User.Service.DataAccess.Repositories.Abstarctions;

namespace User.Service.DataAccess.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IGenericRepository<Core.Entities.Entity.User> Users { get; }
    public IGenericRepository<Role> Roles { get; }
    public IGenericRepository<Permission> Permissions { get; }
    public IGenericRepository<RolePermission> RolePermissions { get; }
    public IGenericRepository<UserRole> UserRoles { get; }

    public IGenericRepository<VerificationToken> VerificationTokens { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new GenericRepository<Core.Entities.Entity.User>(_context);
        Roles = new GenericRepository<Role>(_context);
        Permissions = new GenericRepository<Permission>(_context);
        RolePermissions = new GenericRepository<RolePermission>(_context);
        UserRoles = new GenericRepository<UserRole>(_context);
        VerificationTokens = new GenericRepository<VerificationToken>(_context);  

    }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}