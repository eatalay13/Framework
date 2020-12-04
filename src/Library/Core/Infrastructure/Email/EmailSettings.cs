namespace Core.Infrastructure.Email
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
    }
}