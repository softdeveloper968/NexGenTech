using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ApiIntegrationType : AuditableEntity<ApiIntegrationTypeEnum>
    {
        public ApiIntegrationType() { }
        public ApiIntegrationType(ApiIntegrationTypeEnum id, string code, string name, string description) 
        { 
            Id = id;
            Code = code;
            Name = name;
            Description = description;
        }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(24)]
        public string Code { get; set; }


        #region Navigation Objects



        #endregion
    }
}
