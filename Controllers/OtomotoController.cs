using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var offers = _context.Offers
                .AsNoTracking()
                .Select(o => new
                {
                    o.Id,
                    o.Brand,
                    o.Model,
                    o.EngineSizeInL,
                    o.ProductionYear,
                    o.Milleage,
                    o.CreatedDate,
                    o.OwnerId
                })
                .ToList();

            return Ok(offers);
        }

        [HttpGet("GetOwners")]
        public IActionResult GetOwners()
        {
            var owners = _context.Owners
                .AsNoTracking()
                .Include(o => o.Offers)
                .ToList();

            var result = owners.Select(owner => new
            {
                owner.Id,
                owner.FirstName,
                owner.LastName,
                offersCount = owner.Offers.Count
            });

            return Ok(result);
        }

        [HttpGet("GetOwnerOffers/{ownerId}")]
        public IActionResult GetOwnerOffers(Guid ownerId)
        {
            var offers = _context.Offers
                .AsNoTracking()
                .Where(o => o.OwnerId == ownerId)
                .Select(o => new
                {
                    o.Id,
                    o.Brand,
                    o.Model,
                    o.EngineSizeInL,
                    o.ProductionYear,
                    o.Milleage,
                    o.CreatedDate,
                    o.OwnerId
                })
                .ToList();

            return Ok(offers);
        }

        [HttpGet("GetOfferById/{offerId}")]
        public IActionResult GetOfferById(Guid offerId)
        {
            var offer = _context.Offers
                .AsNoTracking()
                .Where(o => o.Id == offerId)
                .ToList();

            return Ok(offer);
        }

        [HttpPost("CreateOffer")]
        public IActionResult CreateOffer(CreateOfferRequest request)
        {
            var owner = _context.Owners.FirstOrDefault(o => o.Id == request.OwnerId);
            if (owner == null)
            {
                return NotFound("Owner doesn't exist");
            }

            var offer = new Offer
            {
                Brand = request.Brand,
                Model = request.Model,
                EngineSizeInL = request.EngineSizeInL,
                ProductionYear = request.ProductionYear,
                Milleage = request.Milleage,
                Owner = owner,
            };

            _context.Offers.Add(offer);
            _context.SaveChanges();

            return Ok(offer);
        }

        [HttpPost("CreateOwner")]
        public IActionResult CreateOwner(CreateOwnerRequest request)
        {
            var owner = new Owner
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                City = request.City,
                Offers = new List<Offer>()
            };

            _context.Owners.Add(owner);
            _context.SaveChanges();

            return Ok(owner);
        }

        [HttpPut("PutOffer/{id}")]
        public IActionResult PutOffer(Guid id, [FromBody] CreateOfferRequest updatedOfferRequest)
        {
            var existingOffer = _context.Offers.FirstOrDefault(o => o.Id == id);

            if (existingOffer == null)
            {
                return NotFound("Offer doesn't exist");
            }

            existingOffer.Brand = updatedOfferRequest.Brand;
            existingOffer.Model = updatedOfferRequest.Model;
            existingOffer.EngineSizeInL = updatedOfferRequest.EngineSizeInL;
            existingOffer.ProductionYear = updatedOfferRequest.ProductionYear;
            existingOffer.Milleage = updatedOfferRequest.Milleage;
            existingOffer.OwnerId = updatedOfferRequest.OwnerId;

            _context.SaveChanges();

            return Ok(existingOffer);
        }

        [HttpPut("PutOwner/{id}")]
        public IActionResult PutOwner(Guid id, [FromBody] CreateOwnerRequest updatedOwnerRequest)
        {
            var existingOwner = _context.Owners.FirstOrDefault(o => o.Id == id);

            if (existingOwner == null)
            {
                return NotFound("Owner doesn't exist");
            }

            existingOwner.FirstName = updatedOwnerRequest.FirstName;
            existingOwner.LastName = updatedOwnerRequest.LastName;
            existingOwner.PhoneNumber = updatedOwnerRequest.PhoneNumber;
            existingOwner.City = updatedOwnerRequest.City;

            _context.SaveChanges();

            return Ok(existingOwner);
        }

        [HttpDelete("DeleteOffer{id}")]
        public IActionResult DeleteOffer(Guid id)
        {
            var offer = _context.Offers.FirstOrDefault(o => o.Id == id);

            if (offer == null)
            {
                return NotFound("Offer doesn't exist");
            }

            _context.Offers.Remove(offer);
            _context.SaveChanges();

            return Ok(offer);
        }

        [HttpDelete("DeleteOwner{id}")]
        public IActionResult DeleteOwner(Guid id)
        {
            var owner = _context.Owners.FirstOrDefault(o => o.Id == id);

            if (owner == null)
            {
                return NotFound("Offer doesn't exist");
            }

            _context.Owners.Remove(owner);
            _context.SaveChanges();

            return Ok(owner);
        }
    }
}
