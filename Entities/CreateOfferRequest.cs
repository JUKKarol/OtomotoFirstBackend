using System.ComponentModel.DataAnnotations;

namespace OtomotoSimpleBackend.Entities
{
    public class CreateOfferRequest
    {
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public double EngineSizeInL { get; set; }
        [Required]
        public int ProductionYear { get; set; }
        [Required]
        public int Milleage { get; set; }
        [Required]
        public Guid OwnerId { get; set; }
    }
}
