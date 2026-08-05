namespace University.Service.Business.DTOs;

public class UniversityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}


public class UniversityCreateDto
{
    public string Name { get; set; } = string.Empty;
}


