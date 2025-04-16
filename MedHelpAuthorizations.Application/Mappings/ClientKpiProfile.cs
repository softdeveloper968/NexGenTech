using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Commands.AddEdit;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientKpiProfile : Profile
    {

        public ClientKpiProfile()
        {
            CreateMap<ClientKpi, ClientKpiDto>().ReverseMap();
			CreateMap<AddEditClientKpisCommand, ClientKpi>().ReverseMap();
			CreateMap<Features.Admin.ClientKpi.Commands.AddEditAdminClientKpisCommand, ClientKpi>().ReverseMap();
		}
    }
}
