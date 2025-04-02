using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocationSpeciality.Queries.GetClientLocationSpecilalityQuery.cs;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
	public class ClientLocationSpecilalityProfile : Profile
	{
		public ClientLocationSpecilalityProfile()
		{
			CreateMap<ClientLocationSpeciality, GetClientLocationSpecilalityResponse>().ReverseMap();
		}
	}
}
