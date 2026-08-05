using Auth.Service.Core.Entities;

namespace Auth.Service.DataAccess.Repositories.Abstarctions;

public interface IUnitOfWork
{
    IGenericRepository<RefreshToken> RefreshTokens { get; }
    Task<int> SaveChangesAsync();
}