using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeScheduleSpecialties;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
	public class ClientFeeScheduleSpecialtyProfile : Profile
	{
		public ClientFeeScheduleSpecialtyProfile()
		{
			CreateMap<ClientFeeScheduleSpecialty, ClientFeeScheduleSpecialtyDto>().ReverseMap();
		}
	}
}
