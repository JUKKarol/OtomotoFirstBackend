using AutoMapper;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using OtomotoSimpleBackend.Data;
using OtomotoSimpleBackend.DTOs;
using OtomotoSimpleBackend.Entities;

namespace OtomotoSimpleBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly OtomotoContext _context;
        private readonly IMapper _mapper;

        public EmailController(OtomotoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult SendMail(Guid offerId)
        {
            var offer =  _context.Offers
               .AsNoTracking()
               .Where(o => o.Id == offerId)
               .Select(o => _mapper.Map<OfferDto>(o))
               .FirstOrDefault();

            string body = $"New car for sale! {offer.Brand} {offer.Model}, {offer.Body} price: {offer.PriceInEur}";

            string emailaddress = "osborne.quigley@ethereal.email";
            string password = "kctS7pfDRe2PVbHmZ9";

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailaddress));
            email.To.Add(MailboxAddress.Parse(emailaddress));
            email.Subject = "Test";
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            var smtp = new MailKit.Net.Smtp.SmtpClient();

            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailaddress, password);
            smtp.Send(email);
            smtp.Disconnect(true);

            return Ok();
        }
    }
}
