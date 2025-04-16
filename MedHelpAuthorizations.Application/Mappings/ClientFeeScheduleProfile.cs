using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.Base;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientFeeScheduleProfile : Profile
    {
        public ClientFeeScheduleProfile()
        {
			CreateMap<AddEditClientFeeScheduleCommand, ClientFeeScheduleBase>().ReverseMap();
			CreateMap<AddEditClientFeeScheduleCommand, ClientFeeScheduleDto>().ReverseMap();
			CreateMap<AddEditClientFeeScheduleCommand, ClientFeeSchedule>().ReverseMap();
            CreateMap<ClientFeeSchedule, ClientFeeScheduleDto>().ReverseMap();
            CreateMap<AddEditClientFeeScheduleCommand, ClientFeeSchedule>()
			   .ForMember(fs => fs.ImportStatus, map => map.MapFrom(src => (src.CopyClientFeeScheduleId != 0 || src.UploadRequest != null) ? ImportStatusEnum.Pending : src.ImportStatus));
			CreateMap<ClientFeeSchedule, ClientFeeScheduleBase>().ReverseMap();
		}
	}
}