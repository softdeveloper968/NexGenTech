using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Shared.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class EmployeeClient : AuditableEntity<int>, IClientRelationship
    {
        public EmployeeClient() 
        {
            AssignedClientEmployeeRoles = new HashSet<ClientEmployeeRole>();
            EmployeeClientAlphaSplits = new HashSet<EmployeeClientAlphaSplit>();
            EmployeeClientInsurances = new HashSet<EmployeeClientInsurance>();
            EmployeeClientLocations = new HashSet<EmployeeClientLocation>();
        }
        public int EmployeeId { get; set; }

        public int ClientId { get; set; }

        public int AssignedAverageDailyClaimCount { get; set; } = 0;

        public decimal ExpectedMonthlyCashCollections { get; set; } = 0.0m;

        [NotMapped]
        public string EmployeeClientInsurancesString => EmployeeClientInsurances != null && EmployeeClientInsurances.Any() ? string.Join(",", EmployeeClientInsurances.Select(i => i.ClientInsurance?.Name)) : string.Empty;

        [NotMapped]
        public string EmployeeClientLocationsString => EmployeeClientLocations != null && EmployeeClientLocations.Any() ? string.Join(",", EmployeeClientLocations.Select(l => l.ClientLocation?.Name)) : string.Empty;

        [NotMapped]
        public string AssignedEmployeeRolesString => AssignedClientEmployeeRoles != null && AssignedClientEmployeeRoles.Any() ? string.Join(",", AssignedClientEmployeeRoles.Select(r => r.EmployeeRoleId.GetDescription())) : string.Empty;

        [NotMapped]
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

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        public bool ReceiveReport { get; set; }

		#region Navigation Objects

		public virtual ICollection<ClientEmployeeRole> AssignedClientEmployeeRoles { get; set; }
        public virtual ICollection<EmployeeClientAlphaSplit> EmployeeClientAlphaSplits { get; set; }
        public virtual ICollection<EmployeeClientLocation> EmployeeClientLocations { get; set; }
        public virtual ICollection<EmployeeClientInsurance> EmployeeClientInsurances { get; set; }
        //public virtual ICollection<ClientEmployeeKpi> ClientEmployeeKpis { get; set; }

        #endregion

    }
}
