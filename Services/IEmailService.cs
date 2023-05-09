namespace OtomotoSimpleBackend.Services
{
    public interface IEmailService
    {
        void SendMail(string subject, string body, string recipientEmailAddress);
        void SendVeryficationToken(string recipientEmailAddress, string token);
        void SendPasswordResetToken(string recipientEmailAddress, string token);
    }
}
