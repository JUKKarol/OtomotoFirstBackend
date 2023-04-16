using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using OtomotoSimpleBackend.Entities;

namespace OtomotoSimpleBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtomotoController : ControllerBase
    {
        private readonly OtomotoContext _context;

        public OtomotoController(OtomotoContext context)
        {
            _context = context;
        }

        [HttpGet("GetOffers")]
        public IActionResult GetOffers()
        {
            var offers = _context.Offers.ToList(); // Pobranie wszystkich ofert z bazy danych

            return Ok(offers); // Zwrócenie ofert jako odpowiedź HTTP 200 OK
        }

        [HttpGet("GetOwners")]
        public IActionResult GetOwners()
        {
            var owners = _context.Owners.ToList(); // Pobranie wszystkich ofert z bazy danych

            return Ok(owners); // Zwrócenie ofert jako odpowiedź HTTP 200 OK
        }

        [HttpGet("GetOwnerOffers/{ownerId}")]
        public IActionResult GetOwnerOffers(Guid ownerId)
        {
            var offers = _context.Offers
                .Where(o => o.OwnerId == ownerId)
                .ToList(); // Pobranie wszystkich ofert z bazy danych

            return Ok(offers); // Zwrócenie ofert jako odpowiedź HTTP 200 OK
        }

        [HttpGet("GetOfferById/{offerId}")]
        public IActionResult GetOfferById(Guid offerId)
        {
            var offer = _context.Offers
                .Where(o => o.Id == offerId)
                .ToList(); // Pobranie wszystkich ofert z bazy danych

            return Ok(offer); // Zwrócenie ofert jako odpowiedź HTTP 200 OK
        }

        [HttpPost("CreateOffer")]
        public IActionResult CreateOffer(CreateOfferRequest request)
        {
            var owner = _context.Owners.FirstOrDefault(o => o.Id == request.OwnerId);
            if (owner == null)
            {
                return NotFound("Owner doesn't exist");
            }

            // Tworzenie nowej oferty
            var offer = new Offer
            {
                Brand = request.Brand,
                Model = request.Model,
                EngineSizeInL = request.EngineSizeInL,
                ProductionYear = request.ProductionYear,
                Milleage = request.Milleage,
                Owner = owner, // Przypisanie istniejącego właściciela do oferty
            };

            // Zapisanie nowej oferty do bazy danych
            _context.Offers.Add(offer);
            _context.SaveChanges();

            // Zwrócenie odpowiedzi HTTP
            return Ok(offer);
        }

        [HttpPost("CreateOwner")]
        public IActionResult CreateOwner(CreateOwnerRequest request)
        {
            // Tworzenie nowego właściciela
            var owner = new Owner
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                City = request.City,
                Offers = new List<Offer>()  // Przypisanie pustej listy ofert
            };

            // Zapisanie nowego właściciela do bazy danych
            _context.Owners.Add(owner);
            _context.SaveChanges();

            // Zwrócenie odpowiedzi HTTP
            return Ok(owner);
        }

        [HttpPut("PutOffer/{id}")]
        public IActionResult PutOffer(Guid id, [FromBody] CreateOfferRequest updatedOfferRequest)
        {
            var existingOffer = _context.Offers.FirstOrDefault(o => o.Id == id); // Pobranie istniejącej oferty z bazy danych

            if (existingOffer == null)
            {
                return NotFound("Offer doesn't exist");
            }

            // Zaktualizuj dane oferty na podstawie przesłanych informacji
            existingOffer.Brand = updatedOfferRequest.Brand;
            existingOffer.Model = updatedOfferRequest.Model;
            existingOffer.EngineSizeInL = updatedOfferRequest.EngineSizeInL;
            existingOffer.ProductionYear = updatedOfferRequest.ProductionYear;
            existingOffer.Milleage = updatedOfferRequest.Milleage;
            existingOffer.OwnerId = updatedOfferRequest.OwnerId;

            _context.SaveChanges(); // Zapisz zmiany w bazie danych

            return Ok(existingOffer); // Zwróć zaktualizowaną ofertę jako odpowiedź HTTP 200 OK
        }

        [HttpPut("PutOwner/{id}")]
        public IActionResult PutOwner(Guid id, [FromBody] CreateOwnerRequest updatedOwnerRequest)
        {
            var existingOwner = _context.Owners.FirstOrDefault(o => o.Id == id); // Pobranie istniejącej oferty z bazy danych

            if (existingOwner == null)
            {
                return NotFound("Owner doesn't exist");
            }

            // Zaktualizuj dane oferty na podstawie przesłanych informacji
            existingOwner.FirstName = updatedOwnerRequest.FirstName;
            existingOwner.LastName = updatedOwnerRequest.LastName;
            existingOwner.PhoneNumber = updatedOwnerRequest.PhoneNumber;
            existingOwner.City = updatedOwnerRequest.City;

            _context.SaveChanges(); // Zapisz zmiany w bazie danych

            return Ok(existingOwner); // Zwróć zaktualizowaną ofertę jako odpowiedź HTTP 200 OK
        }

        [HttpDelete("DeleteOffer{id}")]
        public IActionResult DeleteOffer(Guid id)
        {
            // Znajdź ofertę do usunięcia na podstawie podanego ID
            var offer = _context.Offers.FirstOrDefault(o => o.Id == id);

            if (offer == null)
            {
                return NotFound("Offer doesn't exist"); // Jeśli oferta nie istnieje, zwróć odpowiedź HTTP 404 Not Found
            }

            _context.Offers.Remove(offer); // Usuń ofertę z kontekstu bazy danych
            _context.SaveChanges(); // Zapisz zmiany w bazie danych

            return Ok(offer);
        }

        [HttpDelete("DeleteOwner{id}")]
        public IActionResult DeleteOwner(Guid id)
        {
            // Znajdź ofertę do usunięcia na podstawie podanego ID
            var owner = _context.Owners.FirstOrDefault(o => o.Id == id);

            if (owner == null)
            {
                return NotFound("Offer doesn't exist"); // Jeśli oferta nie istnieje, zwróć odpowiedź HTTP 404 Not Found
            }

            _context.Owners.Remove(owner); // Usuń ofertę z kontekstu bazy danych
            _context.SaveChanges(); // Zapisz zmiany w bazie danych

            return Ok(owner);
        }

    }
}
