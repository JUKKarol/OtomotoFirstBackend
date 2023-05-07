using AutoMapper;
using OtomotoSimpleBackend.DTOs;
using OtomotoSimpleBackend.Entities;
using OtomotoSimpleBackend.Services;
using OtomotoSimpleBackend.Enums;

namespace OtomotoSimpleBackend.Utilities.Mappings
{
    public class OwnerMappingProfile : Profile
    {
        private readonly IOwnerService _ownerService;

        public OwnerMappingProfile(IOwnerService ownerService)
        {
            _ownerService = ownerService;

            CreateMap<Owner, OwnerDtoPublic>();

            CreateMap<OwnerDtoRegistration, Owner>()
              .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
              .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
              .ForMember(dest => dest.VeryficationToken, opt => opt.MapFrom(src => _ownerService.CreateRandomToken()))
              .AfterMap((src, dest, ctx) =>
              {
                  byte[] passwordHash, passwordSalt;
                  _ownerService.CreatePasswordHash(src.Password, out passwordHash, out passwordSalt);
                  dest.PasswordHash = passwordHash;
                  dest.PasswordSalt = passwordSalt;
              });
        }
    }
}
