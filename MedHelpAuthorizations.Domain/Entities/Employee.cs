using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class Employee : AuditableEntity<int>, ISoftDelete
    {
        public Employee() 
        {
            EmployeeClients = new List<EmployeeClient>();
        }

        public string EmployeeNumber { get; set; }

        public int OverallAverageDailyClaimCount { get; set; } = 0;

        //public int? PersonId { get; set; }

        public int? EmployeeManagerId { get; set; }

        public EmployeeRoleEnum? DefaultEmployeeRoleId { get; set; }

        public decimal? OverallExpectedMonthlyCashCollections { get; set; }

        public int RemainingAverageDailyClaim => EmployeeClients.Any() ? (OverallAverageDailyClaimCount - EmployeeClients.Sum(x => x.AssignedAverageDailyClaimCount)) : OverallAverageDailyClaimCount;


        [ForeignKey("DefaultEmployeeRoleId")]
        public virtual EmployeeRole DefaultEmployeeRole { get; set; }


        //[ForeignKey("PersonId")]
        //public virtual Person Person { get; set; }


        [ForeignKey("EmployeeManagerId")]
        public virtual Employee EmployeeManager { get; set; }

        public virtual ICollection<EmployeeClient> EmployeeClients { get; set; }

        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public bool DefaultReceiveReport { get; set; } //AA-289
    }
}

