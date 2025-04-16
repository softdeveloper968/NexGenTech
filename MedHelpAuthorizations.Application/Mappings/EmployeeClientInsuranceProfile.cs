
using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClientInsurances;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class EmployeeClientInsuranceProfile : Profile
    {
        public EmployeeClientInsuranceProfile()
        {
            CreateMap<EmployeeClientInsurance, EmployeeClientInsuranceDto>()
                .ForPath(x => x.ClientInsuranceName, map => map.MapFrom(y => y.ClientInsurance.Name));

            CreateMap<EmployeeClientInsuranceDto, EmployeeClientInsurance>();
        }
    }
}
