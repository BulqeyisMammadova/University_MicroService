using Teacher.Service.Core.Entities.Common;

namespace Teacher.Service.Core.Entities;

public class Subject : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
}
