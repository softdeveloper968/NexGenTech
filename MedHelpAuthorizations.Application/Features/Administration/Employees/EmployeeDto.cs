using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Features.Persons.ViewModels;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Administration.Employees
{
    public class EmployeeDto
    {
        public int? Id { get; set; } 
        public string EmployeeNumber { get; set; }

        public int OverallAverageDailyClaimCount { get; set; }

        public EmployeeDto EmployeeManager { get; set; }

        public int? EmployeeManagerId { get; set; }

        public EmployeeRoleEnum? DefaultEmployeeRoleId { get; set; }

        public List<EmployeeClientViewModel> EmployeeClientViewModels { get; set; }

        public decimal? OverallExpectedMonthlyCashCollections { get; set; }

        public int RemainingAverageDailyClaim { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        //public UserResponse User { get; set; } //AA-230
        public bool DefaultReceiveReport { get; set; }
        public bool UpdateReportReceiveForAllClients { get; set; } = false;
    }
}
