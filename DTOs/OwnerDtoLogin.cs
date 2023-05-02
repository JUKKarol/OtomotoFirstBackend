using System.ComponentModel.DataAnnotations;

namespace OtomotoSimpleBackend.DTOs
{
    public class OwnerDtoLogin
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
