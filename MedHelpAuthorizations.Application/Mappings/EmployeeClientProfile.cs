using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Commands;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class EmployeeClientProfile : Profile
    {
        public EmployeeClientProfile()
        {
            //AddEditEmployeeClientCommand -> EmployeeClient
            CreateMap<AddEditEmployeeClientCommand, EmployeeClient>()
                .ForMember(dest => dest.Employee, map => map.Ignore());

            CreateMap<EmployeeClientDto, EmployeeClient>().ReverseMap();
            CreateMap<EmployeeClientDto, EmployeeClientViewModel>().ReverseMap();
            CreateMap<EmployeeClientDto, AddEditEmployeeClientCommand>().ReverseMap();
            CreateMap<EmployeeClient, EmployeeClientViewModel>()
                //.ForPath(dest => dest.EmployeeNumber, map => map.MapFrom(src => src.Employee.EmployeeNumber))
                .ForMember(dest => dest.EmployeeId, map => map.MapFrom(src => src.EmployeeId))
                .ForPath(dest => dest.UserId, map => map.MapFrom(src => src.Employee.UserId));
                //.ForMember(dest => dest.EmployeeClientInsurancesString, map => map.MapFrom(src => src.EmployeeClientInsurancesString))
                //.ForMember(dest => dest.EmployeeClientLocationsString, map => map.MapFrom(src => src.EmployeeClientLocationsString))
                //.ForMember(dest => dest.EmployeeClientAlphaSplitsString, map => map.MapFrom(src => src.EmployeeClientAlphaSplitsString))
                //.ForMember(dest => dest.AssignedEmployeeRolesString, map => map.MapFrom(src => src.AssignedEmployeeRolesString))
                //.ForMember(dest => dest.AssignedAverageDailyClaimCount, map => map.MapFrom(src => src.AssignedAverageDailyClaimCount));
        }
    }
}
