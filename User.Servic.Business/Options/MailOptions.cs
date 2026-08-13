namespace User.Servic.Business.Options;

public class MailOptions
{
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set;}
    public string SmtpEmail { get; set; } = string.Empty;
    public string SmtpPassword { get; set;} = string.Empty;
    public string SenderName { get; set; } = string.Empty;
}
