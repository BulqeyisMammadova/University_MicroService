using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using User.Servic.Business.Options;
using User.Servic.Business.Services.Abstractions;

namespace User.Servic.Business.Services.Implementations;

public class EmailService(IOptions<MailOptions> options): IEmailService
{
    private readonly MailOptions _options = options.Value;

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        using var client = new SmtpClient(_options.SmtpHost, _options.SmtpPort)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_options.SmtpEmail, _options.SmtpPassword),
            EnableSsl = true
        }; 

        var message = new MailMessage
        {
            From = new MailAddress(_options.SmtpEmail, _options.SenderName),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(toEmail);
        await client.SendMailAsync(message);
    }
}


