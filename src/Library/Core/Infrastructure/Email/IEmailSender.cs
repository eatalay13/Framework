namespace Core.Infrastructure.Email
{
    public interface IEmailSender
    {
        void SendEmail(string email, string subject, string content);
    }
}