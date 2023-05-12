using AutoMapper;
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
    public class AuthController : ControllerBase
    {
        private readonly OtomotoContext _context;
        private readonly IMapper _mapper;
        private readonly IOwnerService _ownerService;
        private readonly IEmailService _emialService;

        public AuthController(OtomotoContext context, IMapper mapper, IOwnerService ownerService, IEmailService emialService)
        {
            _context = context;
            _mapper = mapper;
            _ownerService = ownerService;
            _emialService = emialService;
        }

        [HttpPost("register")]
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

            var createdUser = _context.Owners.FirstOrDefault(u => u.Email == ownerDto.Email);
            string token = createdUser.VeryficationToken;
            string userEmail = createdUser.Email;

            _emialService.SendVeryficationToken(userEmail, token);

            return Ok("User successfully created, confirmation code was sent");
        }

        [HttpPost("login")]
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

            string token = _ownerService.CreateToken(ownerDto);

            return Ok(token);
        }

        [HttpPost("verify")]
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

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string userEmail)
        {
            var owner = await _context.Owners.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (owner == null)
            {
                return BadRequest("User not found");
            }

            owner.PasswordResetToken = _ownerService.CreateRandomToken();
            owner.ResetTokenExpires = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            _emialService.SendVeryficationToken(userEmail, owner.PasswordResetToken);

            return Ok("Reset code was sent to your email");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResettPassword(OwnerDtoResetPassword ownerDto)
        {
            var owner = await _context.Owners.FirstOrDefaultAsync(u => u.PasswordResetToken == ownerDto.Token);
            if (owner == null || owner.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token.");
            }

            _ownerService.CreatePasswordHash(ownerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            owner.PasswordHash = passwordHash;
            owner.PasswordSalt = passwordSalt;
            owner.PasswordResetToken = null;
            owner.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok("Password successfully reset.");
        }
    }
}
