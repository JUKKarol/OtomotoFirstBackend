using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtomotoSimpleBackend.Data;
using OtomotoSimpleBackend.DTOs;
using OtomotoSimpleBackend.Entities;
using OtomotoSimpleBackend.Enums;
using OtomotoSimpleBackend.Services;
using System.Data;
using System.Security.Claims;

namespace OtomotoSimpleBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly OtomotoContext _context;
        private readonly IMapper _mapper;
        private readonly IOwnerService _ownerService;

        public OwnerController(OtomotoContext context, IMapper mapper, IOwnerService ownerService)
        {
            _context = context;
            _mapper = mapper;
            _ownerService = ownerService;
        }

        [HttpGet("owners"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetOwners()
        {
            var owners = await _context.Owners
                .AsNoTracking()
                .Select(o => _mapper.Map<OwnerDtoPublic>(o))
                .ToListAsync();

            return Ok(owners);
        }

        [HttpGet("owners/{ownerId}"), Authorize(Roles = "User")]
        public async Task<IActionResult> GetOwnerOffers(Guid ownerId)
        {
            var offers = await _context.Offers
                .AsNoTracking()
                .Where(o => o.OwnerId == ownerId)
                .Select(o => _mapper.Map<OfferDto>(o))
                .ToListAsync();

            if (offers.Count == 0)
            {
                return NotFound("Owner didn't have any offers");
            }

            return Ok(offers);
        }

        [HttpPut("owners/{id}"), Authorize(Roles = "User")]
        public async Task<IActionResult> PutOwner(Guid id, [FromBody] OwnerDtoRegistration ownerDto)
        {
            var ownerEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var ownerRole = HttpContext.User.FindFirstValue(ClaimTypes.Role);

            var authorizedOwner = await _context.Owners.FirstOrDefaultAsync(o => o.Email == ownerEmail);
            var existingOwner = await _context.Owners.FirstOrDefaultAsync(o => o.Id == id);

            if (existingOwner == null)
            {
                return NotFound("Owner doesn't exist");
            }

            if (ownerRole != OwnerPermissions.Administrator.ToString() && authorizedOwner.Id != existingOwner.Id)
            {
                return BadRequest("Permission denied");
            }

            _mapper.Map(ownerDto, existingOwner);

            await _context.SaveChangesAsync();

            return Ok(ownerDto);
        }

        [HttpPut("owners-role/{id}"), Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ChangeOwnersRole(Guid id, OwnerPermissions permissions)
        {
            var existingOwner = await _context.Owners.FirstOrDefaultAsync(o => o.Id == id);

            if (existingOwner == null)
            {
                return NotFound("Owner doesn't exist");
            }

            existingOwner.Permissions = permissions;

            await _context.SaveChangesAsync();

            return Ok(existingOwner);
        }

        [HttpDelete("owners/{id}"), Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteOwner(Guid id)
        {
            var ownerEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var ownerRole = HttpContext.User.FindFirstValue(ClaimTypes.Role);

            var authorizedOwner = await _context.Owners.Include(o => o.Offers).FirstOrDefaultAsync(o => o.Email == ownerEmail);
            var existingOwner = await _context.Owners.FirstOrDefaultAsync(o => o.Id == id);

            if (existingOwner == null)
            {
                return NotFound("Owner doesn't exist");
            }

            if (ownerRole != OwnerPermissions.Administrator.ToString() && authorizedOwner.Id != existingOwner.Id)
            {
                return BadRequest("Permission denied");
            }

            var ownerDto = _mapper.Map<OwnerDtoPublic>(existingOwner);

            _context.Owners.Remove(existingOwner);
            await _context.SaveChangesAsync();

            return Ok(ownerDto);
        }
    }
}