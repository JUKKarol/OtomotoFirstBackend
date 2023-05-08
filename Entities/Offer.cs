using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OtomotoSimpleBackend.Entities
{
    public class Offer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
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
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid OwnerId { get; set; }

        [JsonIgnore]
        public Owner Owner { get; set; }
    }
}
