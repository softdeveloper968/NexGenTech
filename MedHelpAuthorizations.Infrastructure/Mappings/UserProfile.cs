using AutoMapper;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Responses.Identity;

namespace MedHelpAuthorizations.Infrastructure.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserResponse>()
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrWhiteSpace(src.PhoneNumber) ? (long?)null : long.Parse(src.PhoneNumber)));
            CreateMap<UserResponse, ApplicationUser>();
            CreateMap<UserMasterResponse, ApplicationUser>().ReverseMap();
            CreateMap<ChatUserResponse, ApplicationUser>().ReverseMap()
                .ForMember(dest => dest.EmailAddress, source => source.MapFrom(source => source.Email)); //Specific Mapping

            CreateMap<ApplicationUser, UserMasterResponse>()
    .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom<PhoneNumberResolver>());
        }
    }

    public class PhoneNumberResolver : IValueResolver<ApplicationUser, UserMasterResponse, long?>
    {
        public long? Resolve(ApplicationUser source, UserMasterResponse destination, long? destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PhoneNumber))
            {
                return null;
            }

            if (long.TryParse(source.PhoneNumber, out var parsedNumber))
            {
                return parsedNumber;
            }

            return null;
        }
    }
}