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
        public async Task<IActionResult> GetOffers()
        {
            var offers = await _context.Offers
            .AsNoTracking()
            .Select(o => _mapper.Map<OfferDto>(o))
            .ToListAsync();

            return Ok(offers);
        }

        [HttpGet("GetOwners")]
        public async Task<IActionResult> GetOwners()
        {
            var owners = await _context.Owners
                .AsNoTracking()
                .Select(o => _mapper.Map<OwnerDtoPublic>(o))
                .ToListAsync();

            return Ok(owners);
        }

        [HttpGet("GetOwnerOffers/{ownerId}")]
        public async Task<IActionResult> GetOwnerOffers(Guid ownerId)
        {
            var offers = await _context.Offers
                .AsNoTracking()
                .Where(o => o.OwnerId == ownerId)
                .Select(o => _mapper.Map<OfferDto>(o))
                .ToListAsync();

            return Ok(offers);
        }

        [HttpGet("GetOfferById/{offerId}")]
        public async Task<IActionResult> GetOfferById(Guid offerId)
        {
            var offer = await _context.Offers
                .AsNoTracking()
                .Where(o => o.Id == offerId)
                .Select(o => _mapper.Map<OfferDto>(o))
                .ToListAsync();

            return Ok(offer);
        }

        [HttpPost("CreateOffer")]
        public async Task<IActionResult> CreateOffer(OfferDto offerDto)
        {
            var owner = await _context.Owners.FirstOrDefaultAsync(o => o.Id == offerDto.OwnerId);
            if (owner == null)
            {
                return NotFound("Owner doesn't exist");
            }

            var offer = _mapper.Map<Offer>(offerDto);

            await _context.Offers.AddAsync(offer);
            await _context.SaveChangesAsync();

            return Ok(offerDto);
        }

        [HttpPost("CreateOwner")]
        public async Task<IActionResult> CreateOwner(OwnerDtoRegistration ownerDto)
        {
            var owner = _mapper.Map<Owner>(ownerDto);

            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();

            return Ok(owner);
        }

        [HttpPut("PutOffer/{id}")]
        public async Task<IActionResult> PutOffer(Guid id, [FromBody] OfferDto offerDto)
        {
            var existingOffer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == id);

            if (existingOffer == null)
            {
                return NotFound("Offer doesn't exist");
            }

            _mapper.Map(offerDto, existingOffer);

            await _context.SaveChangesAsync();

            return Ok(existingOffer);
        }

        [HttpPut("PutOwner/{id}")]
        public async Task<IActionResult> PutOwner(Guid id, [FromBody] OwnerDtoRegistration ownerDto)
        {
            var existingOwner = await _context.Owners.FirstOrDefaultAsync(o => o.Id == id);

            if (existingOwner == null)
            {
                return NotFound("Owner doesn't exist");
            }

            _mapper.Map(ownerDto, existingOwner);

            await _context.SaveChangesAsync();

            return Ok(existingOwner);
        }

        [HttpDelete("DeleteOffer{id}")]
        public async Task<IActionResult> DeleteOffer(Guid id)
        {
            var offer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == id);

            if (offer == null)
            {
                return NotFound("Offer doesn't exist");
            }

            var offerDto = _mapper.Map<OfferDto>(offer);

            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();

            return Ok(offerDto);
        }

        [HttpDelete("DeleteOwner{id}")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            var owner = await _context.Owners.FirstOrDefaultAsync(o => o.Id == id);

            if (owner == null)
            {
                return NotFound("Offer doesn't exist");
            }

            var ownerDto = _mapper.Map<OwnerDtoPublic>(owner);

            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();

            return Ok(ownerDto);
        }
    }
}