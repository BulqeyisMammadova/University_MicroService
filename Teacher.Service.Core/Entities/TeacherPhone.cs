using Teacher.Service.Core.Entities.Common;

namespace Teacher.Service.Core.Entities;

public class TeacherPhone : BaseEntity
{
    public string PhoneNumber { get; set; } = string.Empty;

    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; } = null!;
}
