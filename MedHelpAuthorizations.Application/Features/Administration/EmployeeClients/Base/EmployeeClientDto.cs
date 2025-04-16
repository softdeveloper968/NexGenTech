using MedHelpAuthorizations.Shared.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClientAlphaSplits;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClientInsurances;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClientLocations;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeRoles;
using MedHelpAuthorizations.Application.Features.Administration.Employees;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base
{
    public class EmployeeClientDto
    {
        public int? Id { get; set; }

        public int ClientId { get; set; }

        public int? EmployeeId { get; set; }

        public EmployeeDto Employee { get; set; }

        public List<ClientEmployeeRoleDto> AssignedClientEmployeeRoles { get; set; }

        public List<EmployeeClientAlphaSplitDto> EmployeeClientAlphaSplits { get; set; }

        public List<EmployeeClientLocationDto> EmployeeClientLocations { get; set; }

        public List<EmployeeClientInsuranceDto> EmployeeClientInsurances { get; set; }

        public string EmployeeClientInsurancesString => EmployeeClientInsurances != null && EmployeeClientInsurances.Any() ? string.Join(",", EmployeeClientInsurances.Select(i => i.ClientInsuranceName)) : string.Empty;

        public string EmployeeClientLocationsString => EmployeeClientLocations != null && EmployeeClientLocations.Any() ? string.Join(",", EmployeeClientLocations.Select(l => l.ClientLocationName)) : string.Empty;

        public string EmployeeClientAlphaSplitsString
        {
            get
            {
                if (EmployeeClientAlphaSplits != null && EmployeeClientAlphaSplits.Any())
                {
                    var alphaSplitDescriptions = EmployeeClientAlphaSplits.Select(a => a.AlphaSplitId != Domain.Entities.Enums.AlphaSplitEnum.CustomRange ? a.AlphaSplitId.GetDescription() : $"{a.CustomBeginAlpha}-{a.CustomEndAlpha}");
                    return string.Join(",", alphaSplitDescriptions);
                }
                return string.Empty;
            }
        }
        public string AssignedEmployeeRolesString => AssignedClientEmployeeRoles != null && AssignedClientEmployeeRoles.Any() ? string.Join(",", AssignedClientEmployeeRoles.Select(r => r.EmployeeRoleId.GetDescription())) : string.Empty;

        public int AssignedAverageDailyClaimCount { get; set; }

        public decimal ExpectedMonthlyCashCollections { get; set; }
        public bool ReceiveReport { get; set; }
    }
}
