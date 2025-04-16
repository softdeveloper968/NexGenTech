using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class DayOfWeek : AuditableEntity<DaysOfWeekEnum>
    {
        public DayOfWeek() { }
        public DayOfWeek(DaysOfWeekEnum id, string code, string name, string description)
        {
            Id = id;
            Code = code;
            Description = description;
        }

        [StringLength(10)]
        public string Description { get; set; }

        [StringLength(10)]
        public string Code { get; set; }
    }
}
