using System;
using System.Threading.Tasks;
// using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
// using MimeKit;

namespace CMS.Web.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // var emailMessage = new MimeMessage();
            //
            // emailMessage.From.Add(new MailboxAddress(_configuration["Email:Name"], _configuration["Email:Email"]));
            // emailMessage.To.Add(new MailboxAddress("", email));
            // emailMessage.Subject = subject;
            // emailMessage.Body = new TextPart("html") {Text = message};
            //
            // using var client = new SmtpClient {ServerCertificateValidationCallback = (s, c, h, e) => true};
            //
            // client.Connect(_configuration["Email:Server"], Convert.ToInt32(_configuration["Email:Port"]),
            //     Convert.ToBoolean(_configuration["Email:SSL"])); // true pokud je to s ssl, pokud bez tak false
            // client.Authenticate(_configuration["Email:Email"], _configuration["Email:Password"]);
            //
            // client.Send(emailMessage);
            // client.Disconnect(true);
            
            return Task.CompletedTask;
        }
    }
}