namespace OtomotoSimpleBackend.Entities
{
    public class Owner
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string City { get; set; }

        public List<Offer> Offers { get; set; } // Właściwość nawigacyjna do ofert
    }
}
