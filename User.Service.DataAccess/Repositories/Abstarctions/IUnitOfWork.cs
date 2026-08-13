using User.Service.Core.Entities.Entity;

namespace User.Service.DataAccess.Repositories.Abstarctions;

public interface IUnitOfWork
{
    IGenericRepository<Core.Entities.Entity.User> Users { get; }
    IGenericRepository<Role> Roles { get; }
    IGenericRepository<Permission> Permissions { get; }
    IGenericRepository<RolePermission> RolePermissions { get; }
    IGenericRepository<UserRole> UserRoles { get; }
    IGenericRepository<VerificationToken> VerificationTokens { get; }   

    Task<int> SaveChangesAsync();
}
