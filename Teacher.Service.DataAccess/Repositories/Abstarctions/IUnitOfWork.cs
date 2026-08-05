namespace Teacher.Service.DataAccess.Repositories.Abstarctions;

public interface IUnitOfWork
{
    IGenericRepository<Core.Entities.Teacher> Teachers { get; }
    IGenericRepository<Core.Entities.Subject> Subjects { get; }
    IGenericRepository<Core.Entities.TeacherSubject> TeacherSubjects { get; }
    IGenericRepository<Core.Entities.TeacherPhone> TeacherPhones { get; }
    Task<int> SaveChangesAsync();
}
