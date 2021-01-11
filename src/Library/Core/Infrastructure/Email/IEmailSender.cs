using System.Threading.Tasks;

namespace Core.Infrastructure.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        void SendEmail(string email, string subject, string content);
    }
}