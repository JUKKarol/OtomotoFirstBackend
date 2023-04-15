namespace OtomotoSimpleBackend.Entities
{
    public class Owner
    {
        public Guid Id { get; set; }
        public string Firstame { get; set; }
        public string Lastame { get; set; }
        public int PhoneNumber { get; set; }
        public int City { get; set; }
        public List<Offer> Offers { get; set; } // Właściwość nawigacyjna do ofert właściciela
    }
}
