using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ApplicationFeature : AuditableEntity<ApplicationFeatureEnum>
    {
        public ApplicationFeature() { }
        public ApplicationFeature(ApplicationFeatureEnum id, string name) 
        { 
            Id = id;
            Name = name;
        }

        [StringLength(16)]
        public string Name { get; set; }
    }
}
