namespace OtomotoSimpleBackend.Entities
{
    public class OwnerDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
