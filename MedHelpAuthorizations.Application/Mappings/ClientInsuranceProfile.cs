using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Base;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetById;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClientInsuranceProfile : Profile
    {
        public ClientInsuranceProfile()
        {
            CreateMap<AddEditInsuranceCommand, ClientInsurance>().ReverseMap();
            CreateMap<GetAllPagedInsurancesResponse, ClientInsurance>().ReverseMap();
            CreateMap<GetInsuranceByIdResponse, ClientInsurance>().ReverseMap();
            CreateMap<ClientInsuranceDto, ClientInsurance>().ReverseMap();
        }
    }
}
