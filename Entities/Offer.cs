namespace OtomotoSimpleBackend.Entities
{
    public class Offer
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public double EngineSizeInL { get; set; }
        public int ProductionYear { get; set; }
        public int Milleage { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid OwnerId { get; set; }
    }
}
