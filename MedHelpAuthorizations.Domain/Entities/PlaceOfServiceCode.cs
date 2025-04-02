using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class PlaceOfServiceCode : AuditableEntity<PlaceOfServiceCodeEnum>
    {
        public PlaceOfServiceCode() { }
        public PlaceOfServiceCode(PlaceOfServiceCodeEnum id, string lookupName, string name)
        { 
            Id = id;
            LookupName = lookupName;
            Name = Name;
        }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(40)]
        public string LookupName { get; set; }

        public string PlaceOfServiceCodeString => $"{this.Id:00}";
    }
}
