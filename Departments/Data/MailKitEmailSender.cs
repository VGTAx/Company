using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace Company.Data
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly SmptSettings _smtpSettings;

        public MailKitEmailSender(SmptSettings smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Company", "myemailforstudyasp.net@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("",email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = htmlMessage
            };

            using(var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465);
                await client.AuthenticateAsync("myemailforstudyasp.net@gmail.com", "imojzoszildvvzjy");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }


    }
}
