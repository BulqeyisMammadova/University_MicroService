namespace Student.Service.Business.DTOs;

public class StudentDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int UniversityId { get; set; }
    public int TeacherId { get; set; }
}


public class StudentCreateDto
{
    public string FullName { get; set; } = string.Empty;
    public int UniversityId { get; set; }
    public int TeacherId { get; set; }
}
