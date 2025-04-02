using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ProviderLevel : AuditableEntity<ProviderLevelEnum>
    {
        public ProviderLevel() { }
        public ProviderLevel(ProviderLevelEnum id, string code, string name, string description) 
        { 
            Id = id;
            Code = code;
            Name = name;
            Description = description;
        }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(96)]
        public string Description { get; set; }

        [StringLength(36)]
        public string Code { get; set; }


        #region Navigation Objects
        #endregion
    }
}
