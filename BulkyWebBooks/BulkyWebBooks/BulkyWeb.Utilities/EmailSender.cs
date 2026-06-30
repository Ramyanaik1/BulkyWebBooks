using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
namespace BulkyWeb.Utilities
{
    public class EmailSender : IEmailSender
    {
        private readonly string? _smtpSecret;
        public EmailSender(IConfiguration _config)
        {
            _smtpSecret  = _config.GetValue<string>("SendGrid:ApiKey");
            // You can add any necessary initialization here, such as setting up SMTP client, API keys, etc.
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(_smtpSecret);

            var from = new SendGrid.Helpers.Mail.EmailAddress("parikshitjalihal@gmail.com", "Bulky Book");
            var to = new SendGrid.Helpers.Mail.EmailAddress(email);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, "This is the plain text body", htmlMessage);

            var response = await client.SendEmailAsync(msg);

            Console.WriteLine(response.StatusCode); // Should be 202 Accepted if successful
        }
    }
}
