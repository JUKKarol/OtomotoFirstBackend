using OtomotoSimpleBackend.Enums;

namespace OtomotoSimpleBackend.DTOs
{
    public class OwnerDtoPublic
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string City { get; set; }
        public DateTime CreatedDate { get; set; }
        public OwnerPermissions Permissions { get; set; }
    }
}
