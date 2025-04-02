using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    /// <summary>
    /// AuthorizationStatus    /// 
    /// Authorization Status is a Status that represents the stage of approval.
    /// </summary>
    public class AuthorizationStatus : AuditableEntity<AuthorizationStatusEnum>
    {
        public AuthorizationStatus()
        {

        }

        public AuthorizationStatus(AuthorizationStatusEnum id, string name, string description) 
        {
            Id = id;
            Name = name;
            Description = description;
        }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }
    }
}
