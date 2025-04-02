using AutoMapper;
using MedHelpAuthorizations.Application.Features.Admin.Client.Models;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllByClientId;
using MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Queries.GetClientKpiByClientId;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetClientsByCriteria;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Domain.Entities;
using System.Linq;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<GetClientsByCriteriaResponse, Domain.Entities.Client>().ReverseMap();
            CreateMap<AddEditClientCommand, Domain.Entities.Client>().ReverseMap();
            CreateMap<AddEditClientCommand, EmployeeClient>().ReverseMap();
            CreateMap<EmployeeClientViewModel, EmployeeClient>().ReverseMap();
            CreateMap<EmployeeClientLocation, GetClientLocationsByClientIdResponse>().ReverseMap();
            CreateMap<ClientLocation, GetClientLocationsByClientIdResponse>().ReverseMap();
            CreateMap<GetAllClientInsurancesByClientIdResponse, ClientInsurance>().ReverseMap();
            CreateMap<GetAllClientInsurancesByClientIdResponse, EmployeeClientInsurance>().ReverseMap();
            CreateMap<GetClientLocationsByClientIdResponse, ClientLocation>().ReverseMap();
            //CreateMap<AddEditClientLevelKpiViewModel, ClientEmployeeKpi>().ReverseMap();
            CreateMap<GetClientKpiByClientIdQueryResponse, ClientKpi>().ReverseMap();
            CreateMap<ClientSpecialityDto, ClientSpecialty>().ReverseMap();

            CreateMap<Features.Administration.Clients.Queries.GetById.GetClientByIdResponse, Domain.Entities.Client>().ReverseMap();
            CreateMap<Features.Administration.Clients.Queries.GetById.GetClientByIdResponse, AddEditClientCommand>().ReverseMap();
            CreateMap<Features.Administration.Clients.Queries.GetById.GetClientByIdResponse, ClientKpi>().ReverseMap();

            //CreateMap<GetClientByIdResponse, AddEditClientCommand>()
            //  .ForMember(dest => dest.ClientApiIntegrationKeys, opt => opt.Ignore())
            //.ReverseMap();
        }
    }
}
