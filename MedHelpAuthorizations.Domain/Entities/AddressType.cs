using System.ComponentModel.DataAnnotations;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class AddressType : AuditableEntity<AddressTypeEnum>
    {
        public AddressType() { }
        public AddressType(AddressTypeEnum id, string name) 
        {
            Id = id;
            Name = name;
        }

        [StringLength(11)]
        public string Name { get; set; }
    }
}
