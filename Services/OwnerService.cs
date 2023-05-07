using Microsoft.IdentityModel.Tokens;
using OtomotoSimpleBackend.Data;
using OtomotoSimpleBackend.DTOs;
using OtomotoSimpleBackend.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace OtomotoSimpleBackend.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IConfiguration _configuration;
        private readonly OtomotoContext _otomotoContext;

        public OwnerService(IConfiguration configuration, OtomotoContext otomotoContext)
        {
            _configuration = configuration;
            _otomotoContext = otomotoContext;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        public string CreateToken(OwnerDtoLogin ownerDto)
        {
            var owner = _otomotoContext.Owners.FirstOrDefault(o => o.Email == ownerDto.Email);
            var ownerPermission = owner.Permissions.ToString();


            List<Claim> claims = new List<Claim> 
            {
                new Claim(ClaimTypes.Email, ownerDto.Email),
                new Claim(ClaimTypes.Role, ownerPermission),
            };

            if (ownerPermission == OwnerPermissions.Administrator.ToString())
            {
                claims.Add(new Claim(ClaimTypes.Role, OwnerPermissions.User.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
