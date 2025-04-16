using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ApiIntegration : AuditableEntity<ApiIntegrationEnum>
    {
        public ApiIntegration() { }
        public ApiIntegration(ApiIntegrationEnum id, string code, string name, string description, bool requirePayerIdentifier, bool requireTaxId, bool requirePolicyNumber, bool requireDateOfBirth, bool groupClaimLines, ApiIntegrationTypeEnum apiIntegrationTypeId = ApiIntegrationTypeEnum.Internal)
        { 
            Id = id;
            Code = code;
            Name = name;
            Description = description;
            ApiIntegrationTypeId = apiIntegrationTypeId;
			RequirePayerIdentifier = requirePayerIdentifier; 
            RequireTaxId = requireTaxId;
            RequireDateOfBirth = requireDateOfBirth;
            RequirePolicyNumber = requirePolicyNumber;
            ApiIntegrationTypeId = apiIntegrationTypeId;
		}
        public bool RequirePayerIdentifier { get; set; } = true;

		public bool RequireTaxId { get; set; } = true;

        public bool RequirePolicyNumber { get; set; } = true;

        public bool RequireDateOfBirth { get; set; } = true;

		public bool GroupClaimLines { get; set; } = true;

		[StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(24)]
        public string Code { get; set; }

        public ApiIntegrationTypeEnum? ApiIntegrationTypeId { get; set; }


        [ForeignKey(nameof(ApiIntegrationTypeId))]
        public virtual ApiIntegrationType ApiIntegrationType { get; set; }


		#region Navigation Objects



		#endregion
	}
}
