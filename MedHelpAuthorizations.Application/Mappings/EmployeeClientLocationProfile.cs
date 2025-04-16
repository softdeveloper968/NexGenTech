using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClientLocations;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class EmployeeClientLocationProfile : Profile
    {
        public EmployeeClientLocationProfile()
        {
            CreateMap<EmployeeClientLocation, EmployeeClientLocationDto>()
             .ForPath(x => x.EligibilityClientLocationId, map => map.MapFrom(y => y.ClientLocation.EligibilityLocationId))
             .ForPath(x => x.ClientLocationName, map => map.MapFrom(y => y.ClientLocation.Name));

            CreateMap<EmployeeClientLocationDto, EmployeeClientLocation>();
        }
    }
}
