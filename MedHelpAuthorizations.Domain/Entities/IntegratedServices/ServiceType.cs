using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ServiceType
    {
        [Key]
        public ServiceTypeEnum ServiceTypeId { get; set; }

        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(80)]
        public string Description { get; set; }
    }
}
