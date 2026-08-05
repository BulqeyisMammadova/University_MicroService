using Teacher.Service.Core.Entities.Common;

namespace Teacher.Service.Core.Entities;

public class Teacher : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public int UniversityId { get; set; }
    public ICollection<TeacherPhone> Phones { get; set; } = new List<TeacherPhone>();
    public ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
}
