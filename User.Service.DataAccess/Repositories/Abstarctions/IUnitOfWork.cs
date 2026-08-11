using User.Service.Core.Entities;

namespace User.Service.DataAccess.Repositories.Abstarctions;

public interface IUnitOfWork
{
    IGenericRepository<User.Service.Core.Entities.User> Users { get; }
    IGenericRepository<Role> Roles { get; }
    IGenericRepository<Permission> Permissions { get; }
    IGenericRepository<RolePermission> RolePermissions { get; }
    IGenericRepository<UserRole> UserRoles { get; }
    Task<int> SaveChangesAsync();
}
