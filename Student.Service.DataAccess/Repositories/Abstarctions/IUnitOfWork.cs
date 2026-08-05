namespace Student.Service.DataAccess.Repositories.Abstarctions;

public interface IUnitOfWork
{
    IGenericRepository<Core.Entities.Student> Students { get; }
    Task<int> SaveChangesAsync();
}
