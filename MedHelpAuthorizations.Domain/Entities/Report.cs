using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class Report : AuditableEntity<ReportsEnum>
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(80)]
        public string Code { get; set; }

        public ReportCategoryEnum ReportCategoryId { get; set; }

        //Navigation to reportcategory

    }
}
