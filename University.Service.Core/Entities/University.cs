using University.Service.Core.Entities.Common;

namespace University.Service.Core.Entities;

public class University : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}
