using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class GetClientReportFilterResponse
    {
        public int Id { get; set; }
        /// <summary>
        /// Base Report Id.
        /// </summary>
        public ReportsEnum ReportId { get; set; }
        /// <summary>
        /// Report By Category Details.
        /// </summary>
        public GetAllReportsResponse Report { get; set; }
        /// <summary>
        /// Client id
        /// </summary>
        public int ClientId { get; set; }
        
        /// <summary>
        /// Saved Filter Name
        /// </summary>
        public string FilterName { get; set; }
        
        /// <summary>
        /// Saved Filter data as Json Format.
        /// </summary>
        public string FilterConfiguration { get; set; }
        
        /// <summary>
        /// Selected Saved Filter as Default Filter.
        /// Only one filter mark as Saved Filter.
        /// </summary>
        public bool HasDefaultFilter { get; set; }
        
        /// <summary>
        /// If RunDefaultFilter mark as True then Default filter run On-Initialize.
        /// </summary>
        public bool RunSavedDefaultFilter { get; set; }
        
        /// <summary>
        /// Handle Expand property..
        /// </summary>
        public bool IsExpanded { get; set; } = false;
        
        /// <summary>
        /// Handle show or hide Expansion.
        /// </summary>
        public bool ShowExpansion { get; set; } = false;

        /// <summary>
        /// Current Login UserId.
        /// </summary>
        public string CreatedByUserId { get; set; }

    }
}
