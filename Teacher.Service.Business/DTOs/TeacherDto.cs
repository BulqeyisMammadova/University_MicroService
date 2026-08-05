namespace Teacher.Service.Business.DTOs;


public class TeacherDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int UniversityId { get; set; }
    public List<PhoneDto> Phones { get; set; } = new();
    public List<SubjectDto> Subjects { get; set; } = new();
}


public class TeacherCreateDto
{
    public string FullName { get; set; } = string.Empty;
    public int UniversityId { get; set; }
    public List<string> PhoneNumbers { get; set; } = new();

}
