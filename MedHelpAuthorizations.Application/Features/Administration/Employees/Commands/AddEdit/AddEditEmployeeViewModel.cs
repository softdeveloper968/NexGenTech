using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Features.Administration.Employees.Commands.AddEdit
{
    public class AddEditEmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }

        public long? MobilePhoneNumber { get; set; }

        public string EmployeeNumber { get; set; }

        public int OverallAverageDailyClaimCount { get; set; } = 0;

        public long? OfficePhoneNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? AdministrativeTime { get; set; }

        //public EmployeeRole EmployeeRole { get; set; }
        public Employee EmployeeManager { get; set; }

        public EmployeeRoleEnum? DefaultEmployeeRoleId { get; set; }

        public int? EmployeeManagerId { get; set; } = null;

        public string UserId { get; set; } //AA-206
    }
}
