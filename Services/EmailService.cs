using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using OtomotoSimpleBackend.Data;
using OtomotoSimpleBackend.Entities;
using System.Net.Mail;
using System.Net;

namespace OtomotoSimpleBackend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly OtomotoContext _otomotoContext;

        public EmailService(IConfiguration configuration, OtomotoContext otomotoContext)
        {
            _configuration = configuration;
            _otomotoContext = otomotoContext;
        }
        public void SendMail(string subject, string body, string recipientEmailAddress)
        {
            string emailAddress = _configuration.GetSection("EmailPass:EmailAddress").Value!;
            string password = _configuration.GetSection("EmailPass:EmailPassword").Value!;

            //var email = new MimeMessage();
            //email.From.Add(MailboxAddress.Parse(emailAddress));
            //email.To.Add(MailboxAddress.Parse(recipientEmailAddress));
            //email.Subject = subject;
            //email.Body = new TextPart(TextFormat.Html) { Text = body };

            //var smtp = new MailKit.Net.Smtp.SmtpClient();

            //smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            //smtp.Authenticate(emailAddress, password);
            //smtp.Send(email);
            //smtp.Disconnect(true);

            var fromAddress = new MailAddress(emailAddress, "Info");
            var toAddress = new MailAddress(recipientEmailAddress, recipientEmailAddress);

            var smtp = new System.Net.Mail.SmtpClient
            {
                Host = "smtp.ethereal.email",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, password)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        public void SendVeryficationToken(string recipientEmailAddress, string token)
        {
            string subject = "Your veryfication code!";
            string body = $"There is your verification token: {token}\r\n Thanks for registration!";

            SendMail(subject, body, recipientEmailAddress);
        }

        public void SendPasswordResetToken(string recipientEmailAddress, string token)
        {
            string subject = "Your password reset code!";
            string body = $"There is your password reset token: {token}";

            SendMail(subject, body, recipientEmailAddress);
        }
    }
}
