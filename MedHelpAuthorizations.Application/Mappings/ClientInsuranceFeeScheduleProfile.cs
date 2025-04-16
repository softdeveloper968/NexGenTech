using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule.Command.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule.Queries.GetAllPaged;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientInsuranceFeeScheduleProfile : Profile
    {
        public ClientInsuranceFeeScheduleProfile()
        {
			CreateMap<GetAllClientInsuranceFeeScheduleResponse,ClientInsuranceFeeSchedule>().ReverseMap();
			CreateMap<ClientInsuranceFeeScheduleDto, ClientInsuranceFeeSchedule>()
				.ForMember(dest => dest.IsActive, map => map.MapFrom(src => src.Id != 0 ? true : src.IsActive));
			CreateMap<ClientInsuranceFeeSchedule, ClientInsuranceFeeScheduleDto>();
			CreateMap<ClientInsuranceFeeScheduleDto, AddEditClientInsuranceFeeScheduleCommand>().ReverseMap();
			CreateMap<GetAllClientInsuranceFeeScheduleResponse, AddEditClientInsuranceFeeScheduleCommand>().ReverseMap();
			CreateMap<GetAllClientInsuranceFeeScheduleResponse, AddEditClientFeeScheduleCommand>().ReverseMap();
			CreateMap<AddEditClientInsuranceFeeScheduleCommand,ClientInsuranceFeeSchedule>().ReverseMap();
        }
    }
}
