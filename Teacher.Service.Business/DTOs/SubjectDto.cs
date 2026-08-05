namespace Teacher.Service.Business.DTOs;

public class SubjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}


public class SubjectCreateDto
{
    public string Name { get; set; } = string.Empty;
}

public class AssignSubjectDto
{
    public int SubjectId { get; set; }
}
