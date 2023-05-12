using OtomotoSimpleBackend.Data;
using OtomotoSimpleBackend.Entities;
using System.Net.Mail;
using System.Net;
using FluentEmail.Core;
using FluentEmail.Smtp;

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

            var fromAddress = new MailAddress(emailAddress, "Info");
            var toAddress = new MailAddress(recipientEmailAddress, recipientEmailAddress);

            var sender = new SmtpSender(() => new SmtpClient("smtp.ethereal.email")
            {
                EnableSsl = true,
                Port = 587,
                Credentials = new NetworkCredential(emailAddress, password)
            });

            Email.DefaultSender = sender;

            var email = Email
                .From(emailAddress)
                .To(recipientEmailAddress)
                .Subject(subject)
                .Body(body, isHtml: true);

            email.Send();
        }

        public void SendVeryficationToken(string recipientEmailAddress, string token)
        {
            string subject = "Your veryfication code!";
            string body = $"There is your verification token: {token} Thanks for registration!";

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
