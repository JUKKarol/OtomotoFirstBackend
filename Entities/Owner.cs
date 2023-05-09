using OtomotoSimpleBackend.Enums;
using System.Text.Json.Serialization;

namespace OtomotoSimpleBackend.Entities
{
    public class Owner
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string VeryficationToken { get; set; }
        public DateTime? VerifiedAt { get; set; } = null;
        public string PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; } = null;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public OwnerPermissions Permissions { get; set; } = OwnerPermissions.User;

        public List<Offer> Offers { get; set; }
    }
}
