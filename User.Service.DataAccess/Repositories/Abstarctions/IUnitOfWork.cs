namespace User.Service.DataAccess.Repositories.Abstarctions;

public interface IUnitOfWork
{
    IGenericRepository<User.Service.Core.Entities.User> Users { get; }
    Task<int> SaveChangesAsync();
}
