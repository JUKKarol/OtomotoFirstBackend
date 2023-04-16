using System.ComponentModel.DataAnnotations;

namespace OtomotoSimpleBackend.Entities
{
    public class CreateOwnerRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int PhoneNumber { get; set; }

        [Required]
        public string City { get; set; }
    }

}
