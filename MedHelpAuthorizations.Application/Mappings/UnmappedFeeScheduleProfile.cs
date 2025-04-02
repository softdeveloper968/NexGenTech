using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Base;
using MedHelpAuthorizations.Application.Features.Administration.UnMappedClientFeeSchedule.Queries.GetAllPaged;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class UnmappedFeeScheduleProfile : Profile
    {
        public UnmappedFeeScheduleProfile() 
        {
            CreateMap<UnmappedFeeScheduleCpt, GetAllUnmappedFeeScheduleResponse>()
            .ForMember(dest => dest.ProcedureCode, opt => opt.MapFrom(src => src.ClientCptCode.Code)) 
            .ForMember(dest => dest.ClientInsurance, opt => opt.MapFrom(src => src.ClientInsurance))
            .ForMember(dest => dest.ClientCptCode, opt => opt.MapFrom(src => src.ClientCptCode));

            // Map between other entities like ClientInsurance to ClientInsuranceDto, ClientCptCode to ClientCptCodeDto
            CreateMap<ClientInsurance, ClientInsuranceDto>();
            CreateMap<ClientCptCode, ClientCptCodeDto>();
        }
        
    }
}
