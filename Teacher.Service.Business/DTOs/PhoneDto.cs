namespace Teacher.Service.Business.DTOs;

public class PhoneDto
{
    public int Id { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}

public class AddPhoneDto
{
    public string PhoneNumber { get; set; } = string.Empty;
}