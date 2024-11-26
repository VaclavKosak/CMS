using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace CMS.Web.Services;

public class EmailSender(IConfiguration configuration) : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress(configuration["Email:Name"], configuration["Email:Email"]));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = message };

        using var client = new SmtpClient { ServerCertificateValidationCallback = (s, c, h, e) => true };

        client.Connect(configuration["Email:Server"], Convert.ToInt32(configuration["Email:Port"]),
            Convert.ToBoolean(configuration["Email:SSL"])); // true pokud je to s ssl, pokud bez tak false
        client.Authenticate(configuration["Email:Email"], configuration["Email:Password"]);

        client.Send(emailMessage);
        client.Disconnect(true);

        return Task.CompletedTask;
    }
}