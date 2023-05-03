﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OtomotoSimpleBackend.Data;
using OtomotoSimpleBackend.DTOs;
using OtomotoSimpleBackend.Entities;
using OtomotoSimpleBackend.Services;

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

        [HttpPost("RegisterOwner")]
        public async Task<IActionResult> RegisterOwner(OwnerDtoRegistration ownerDto)
        {
            if (_context.Owners.Any(u => u.Email == ownerDto.Email))
            {
                return BadRequest("User already exists");
            }

            _ownerService.CreatePasswordHash(ownerDto.Password,
                out byte[] passwordHash
                , out byte[] passwordSalt);

            var owner = _mapper.Map<Owner>(ownerDto);

            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();

            return Ok("User successfully created");
        }

        [HttpPost("LoginOwner")]
        public async Task<IActionResult> LoginOwner(OwnerDtoLogin ownerDto)
        {
            var owner = await _context.Owners.FirstOrDefaultAsync(o => o.Email == ownerDto.Email);

            if (owner == null)
            {
                return BadRequest("User not found");
            }

            if (!_ownerService.VerifyPasswordHash(ownerDto.Password, owner.PasswordHash, owner.PasswordSalt))
            {
                return BadRequest("Password incorect");
            }

            if (owner.VerifiedAt == null)
            {
                return BadRequest("Not verified");
            }

            return Ok("Logged in");
        }

        [HttpPost("VerifyOwner")]
        public async Task<IActionResult> VerifyOwner(string token)
        {
            var owner = await _context.Owners.FirstOrDefaultAsync(u => u.VeryficationToken == token);
            if (owner == null)
            {
                return BadRequest("Invalid token");
            }

            owner.VerifiedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok("User verified");
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