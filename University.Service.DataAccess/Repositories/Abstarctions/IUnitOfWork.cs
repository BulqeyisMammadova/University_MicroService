using University.Service.DataAccess.Data;

namespace University.Service.DataAccess.Repositories.Abstarctions;

public interface IUnitOfWork
{
    IGenericRepository<Core.Entities.University> Universities { get; }
    Task<int> SaveChangesAsync();
}
