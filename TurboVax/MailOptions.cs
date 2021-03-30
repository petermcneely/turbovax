namespace TurboVax
{
    public class MailOptions
    {
        public string SmtpHost { get; set; }
        public int Port { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; } = true;
    }
}
