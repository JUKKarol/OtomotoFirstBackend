using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtomotoSimpleBackend.Data;
using OtomotoSimpleBackend.DTOs;
using OtomotoSimpleBackend.Entities;
using OtomotoSimpleBackend.Enums;
using System.Security.Claims;

namespace OtomotoSimpleBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly OtomotoContext _context;
        private readonly IMapper _mapper;

        public OfferController(OtomotoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("offers")]
        public async Task<IActionResult> GetOffers()
        {
            var offers = await _context.Offers
            .AsNoTracking()
            .Select(o => _mapper.Map<OfferDto>(o))
            .ToListAsync();

            return Ok(offers);
        }

        [HttpGet("offers/{offerId}")]
        public async Task<IActionResult> GetOfferById(Guid offerId)
        {
            var offer = await _context.Offers
                .AsNoTracking()
                .Where(o => o.Id == offerId)
                .Select(o => _mapper.Map<OfferDto>(o))
                .FirstOrDefaultAsync();

            if (offer == null)
            {
                return NotFound("Offer not found");
            }

            return Ok(offer);
        }

        [HttpPost("offers"), Authorize(Roles = "User")]
        public async Task<IActionResult> CreateOffer(OfferDto offerDto)
        {
            var ownerEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var owner = await _context.Owners.FirstOrDefaultAsync(o => o.Email == ownerEmail);
            if (owner == null)
            {
                return NotFound("Owner doesn't exist");
            }

            var offer = _mapper.Map<Offer>(offerDto);
            offer.OwnerId = owner.Id;

            await _context.Offers.AddAsync(offer);
            await _context.SaveChangesAsync();

            return Ok(offerDto);
        }

        [HttpPut("offers/{offerId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> PutOffer(Guid offerId, [FromBody] OfferDto offerDto)
        {
            var ownerEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var ownerRole = HttpContext.User.FindFirstValue(ClaimTypes.Role);

            var authorizedOwner = await _context.Owners.FirstOrDefaultAsync(o => o.Email == ownerEmail);
            var existingOffer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == offerId);
            offerDto.OwnerId = authorizedOwner.Id;

            if (existingOffer == null)
            {
                return NotFound("Offer doesn't exist");
            }

            if (ownerRole != OwnerPermissions.Administrator.ToString() && authorizedOwner.Id != existingOffer.OwnerId)
            {
                return BadRequest("Permission denied");
            }

            _mapper.Map(offerDto, existingOffer);

            await _context.SaveChangesAsync();

            return Ok(existingOffer);
        }

        [HttpDelete("offers/{offerId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteOffer(Guid offerId)
        {
            var ownerEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var ownerRole = HttpContext.User.FindFirstValue(ClaimTypes.Role);

            var authorizedOwner = await _context.Owners.FirstOrDefaultAsync(o => o.Email == ownerEmail);
            var existingOffer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == offerId);

            if (existingOffer == null)
            {
                return NotFound("Offer doesn't exist");
            }

            if (ownerRole != OwnerPermissions.Administrator.ToString() && authorizedOwner.Id != existingOffer.OwnerId)
            {
                return BadRequest("Permission denied");
            }

            var offerDto = _mapper.Map<OfferDto>(existingOffer);

            _context.Offers.Remove(existingOffer);
            await _context.SaveChangesAsync();

            return Ok(offerDto);
        }
    }
}