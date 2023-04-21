using AutoMapper;
using OtomotoSimpleBackend.DTOs;
using OtomotoSimpleBackend.Entities;

namespace OtomotoSimpleBackend.Utilities.Mappings
{
    public class OfferMappingProfile : Profile
    {
        public OfferMappingProfile()
        {
            CreateMap<OfferDto, Offer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.Owner, opt => opt.Ignore());

            CreateMap<Offer, OfferDto>();
        }
    }
}
