namespace OtomotoSimpleBackend.DTOs
{
    public class OfferDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Body { get; set; }
        public string Description { get; set; }
        public int PriceInEur { get; set; }
        public string FuelType { get; set; }
        public double EngineSizeInL { get; set; }
        public int HorsePower { get; set; }
        public bool AutomaticTransmission { get; set; }
        public int ProductionYear { get; set; }
        public int Milleage { get; set; }
        public Guid OwnerId { get; set; }
    }
}
