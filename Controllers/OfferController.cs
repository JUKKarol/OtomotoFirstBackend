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

        [HttpGet("GetOffers")]
        public async Task<IActionResult> GetOffers()
        {
            var offers = await _context.Offers
            .AsNoTracking()
            .Select(o => _mapper.Map<OfferDto>(o))
            .ToListAsync();

            return Ok(offers);
        }

        [HttpGet("GetOfferById/{offerId}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetOfferById(Guid offerId)
        {
            var offer = await _context.Offers
                .AsNoTracking()
                .Where(o => o.Id == offerId)
                .Select(o => _mapper.Map<OfferDto>(o))
                .ToListAsync();

            return Ok(offer);
        }

        [HttpPost("CreateOffer"), Authorize(Roles = "User")]
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

        [HttpPut("PutOffer/{offerId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> PutOffer(Guid offerId, [FromBody] OfferDto offerDto)
        {
            var ownerEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var ownerRole = HttpContext.User.FindFirstValue(ClaimTypes.Role);

            var owner = await _context.Owners.FirstOrDefaultAsync(o => o.Email == ownerEmail);
            var existingOffer = await _context.Offers.FirstOrDefaultAsync(o => o.Id == offerId);
            offerDto.OwnerId = owner.Id;


            if (ownerRole != OwnerPermissions.Administrator.ToString() && owner.Id != existingOffer.OwnerId)
            {
                return BadRequest("Permission denied");
            }

            if (existingOffer == null)
            {
                return NotFound("Offer doesn't exist");
            }

            _mapper.Map(offerDto, existingOffer);

            await _context.SaveChangesAsync();

            return Ok(existingOffer);
        }

        [HttpDelete("DeleteOffer{id}"), Authorize(Roles = "User")]
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
    }
}