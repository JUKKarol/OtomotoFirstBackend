using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtomotoSimpleBackend.Data;
using OtomotoSimpleBackend.DTOs;
using OtomotoSimpleBackend.Entities;

namespace OtomotoSimpleBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OtomotoController : ControllerBase
    {
        private readonly OtomotoContext _context;
        private readonly IMapper _mapper;

        public OtomotoController(OtomotoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetOffers")]
        public IActionResult GetOffers()
        {
            var offers = _context.Offers
            .AsNoTracking()
            .Select(o => _mapper.Map<OfferDto>(o))
            .ToList();

            return Ok(offers);
        }

        [HttpGet("GetOwners")]
        public IActionResult GetOwners()
        {
            var owners = _context.Owners
                .AsNoTracking()
                .Select(o => _mapper.Map<OwnerDtoPublic>(o))
                .ToList();

            return Ok(owners);
        }

        [HttpGet("GetOwnerOffers/{ownerId}")]
        public IActionResult GetOwnerOffers(Guid ownerId)
        {
            var offers = _context.Offers
                .AsNoTracking()
                .Where(o => o.OwnerId == ownerId)
                .Select(o => _mapper.Map<OfferDto>(o))
                .ToList();

            return Ok(offers);
        }

        [HttpGet("GetOfferById/{offerId}")]
        public IActionResult GetOfferById(Guid offerId)
        {
            var offer = _context.Offers
                .AsNoTracking()
                .Where(o => o.Id == offerId)
                .Select(o => _mapper.Map<OfferDto>(o))
                .ToList();

            return Ok(offer);
        }

        [HttpPost("CreateOffer")]
        public IActionResult CreateOffer(OfferDto offerDto)
        {
            var owner = _context.Owners.FirstOrDefault(o => o.Id == offerDto.OwnerId);
            if (owner == null)
            {
                return NotFound("Owner doesn't exist");
            }

            var offer = _mapper.Map<Offer>(offerDto);

            _context.Offers.Add(offer);
            _context.SaveChanges();

            return Ok(offerDto);
        }

        [HttpPost("CreateOwner")]
        public IActionResult CreateOwner(OwnerDtoRegistration ownerDto)
        {
            var owner = _mapper.Map<Owner>(ownerDto);

            _context.Owners.Add(owner);
            _context.SaveChanges();

            return Ok(owner);
        }

        [HttpPut("PutOffer/{id}")]
        public IActionResult PutOffer(Guid id, [FromBody] OfferDto offerDto)
        {
            var existingOffer = _context.Offers.FirstOrDefault(o => o.Id == id);

            if (existingOffer == null)
            {
                return NotFound("Offer doesn't exist");
            }

            _mapper.Map(offerDto, existingOffer);

            _context.SaveChanges();

            return Ok(existingOffer);
        }

        [HttpPut("PutOwner/{id}")]
        public IActionResult PutOwner(Guid id, [FromBody] OwnerDtoRegistration ownerDto)
        {
            var existingOwner = _context.Owners.FirstOrDefault(o => o.Id == id);

            if (existingOwner == null)
            {
                return NotFound("Owner doesn't exist");
            }

            _mapper.Map(ownerDto, existingOwner);

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
