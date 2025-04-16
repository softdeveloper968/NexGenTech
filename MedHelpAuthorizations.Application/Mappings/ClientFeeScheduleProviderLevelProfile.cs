
using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeScheduleProviderLevels;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientFeeScheduleProviderLevelProfile : Profile
    {
        public ClientFeeScheduleProviderLevelProfile()
        {
            CreateMap<ClientFeeScheduleProviderLevel, ClientFeeScheduleProviderLevelDto>().ReverseMap();
        }
    }
}
