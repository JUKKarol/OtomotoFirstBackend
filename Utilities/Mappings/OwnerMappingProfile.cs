using AutoMapper;
using OtomotoSimpleBackend.DTOs;
using OtomotoSimpleBackend.Entities;

namespace OtomotoSimpleBackend.Utilities.Mappings
{
    public class OwnerMappingProfile : Profile
    {
        public OwnerMappingProfile()
        {
            CreateMap<Owner, OwnerDtoPublic>().ReverseMap();

            CreateMap<Owner, OwnerDtoRegistration>().ReverseMap();
        }
    }
}
