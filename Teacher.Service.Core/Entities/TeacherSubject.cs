using Teacher.Service.Core.Entities.Common;

namespace Teacher.Service.Core.Entities;

public class TeacherSubject : BaseEntity
{
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;

    public int SubjectId { get; set; }
    public Subject Subject { get; set; } = null!;
}