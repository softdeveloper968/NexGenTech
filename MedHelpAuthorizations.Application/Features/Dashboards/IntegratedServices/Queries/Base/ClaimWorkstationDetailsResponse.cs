using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public class ClaimWorkstationDetailsResponse : ClaimStatusDashboardDetailsResponseBase
    {
        public int Id { get; set; }
        public ClaimStatusExceptionReasonCategoryEnum? ExceptionReasonCategoryId { get; set; }
        public string Modifiers { get; set; }
        public decimal? WriteoffAmount { get; set; }
        public bool IsExpanded { get; set; } = false;
        public bool ShowExpansion { get; set; } = false;
    }
    public class ClaimWorkstationSearchOptions
    {
        public string? PatientFirstName { get; set; }
        public string? PatientLastName { get; set; }
        public string? BatchNumber { get; set; }
        public string? PolicyNumber { get; set; }
        public string? ClaimNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
