using Student.Service.Core.Entities.Common;

namespace Student.Service.Core.Entities;

public class Student : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public int TeacherId { get; set; }
    public int UniversityId { get; set; }
}
