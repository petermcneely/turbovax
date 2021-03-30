using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TurboVax
{
    public class Texter
    {
        public Texter(
            IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task Text(string serializedLocations)
        {
            var phoneNumbers = configuration.GetValue<string>("PhoneNumbers").Split('|');

            var mailOptions = configuration.Get<MailOptions>();

            var smtpClient = new SmtpClient(mailOptions.SmtpHost)
            {
                Port = mailOptions.Port,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mailOptions.FromEmail, mailOptions.Password),
                EnableSsl = mailOptions.EnableSsl,
            };

            var message = new MailMessage
            {
                From = new MailAddress(mailOptions.FromEmail),
                Subject = "Found vaccine sites",
                Body = serializedLocations,
            };
            foreach (var phone in phoneNumbers)
            {
                message.To.Add(phone);
            }
            await smtpClient.SendMailAsync(message);
        }

        private readonly IConfiguration configuration;
    }
}
