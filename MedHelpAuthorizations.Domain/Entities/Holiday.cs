using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class Holiday : AuditableEntity<HolidaysEnum>
    {
        public Holiday() { }
        public Holiday(HolidaysEnum id, string code, string name, string description)
        {
            Id = id;
            Code = code;
            Description = description;
        }

        [StringLength(90)]
        public string Description { get; set; }

        [StringLength(30)]
        public string Code { get; set; }

        public int Month { get; set; }
    }
}
