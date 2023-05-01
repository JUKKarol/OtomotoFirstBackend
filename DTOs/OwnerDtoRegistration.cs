using System.ComponentModel.DataAnnotations;

namespace OtomotoSimpleBackend.DTOs
{
    public class OwnerDtoRegistration
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string City { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6), MaxLength(40)]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
