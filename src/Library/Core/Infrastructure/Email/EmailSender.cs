using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Core.Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }

        public void SendEmail(string email, string subject, string content)
        {
            var client = new SmtpClient(_emailSettings.Host);
            var from = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName, Encoding.UTF8);
            var to = new MailAddress(email);
            var message = new MailMessage(from, to) {Body = content};

            var someArrows = new string(new[] {'\u2190', '\u2191', '\u2192', '\u2193'});
            message.Body += Environment.NewLine + someArrows;
            message.BodyEncoding = Encoding.UTF8;

            message.Subject = subject;
            message.SubjectEncoding = Encoding.UTF8;

            const string userState = "Flixada.com";
            client.SendAsync(message, userState);
            message.Dispose();
        }
    }
}