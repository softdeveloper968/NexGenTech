using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
	public class X12ClaimCodeType : AuditableEntity<X12ClaimCodeTypeEnum>
	{
		public X12ClaimCodeType() { }
		public X12ClaimCodeType(X12ClaimCodeTypeEnum id, string code, string name, string description)
		{
			Id = id;
			Code = code;
			Name = name;
			Description = description;
		}


		[StringLength(12)]
		public string Code { get; set; }

		[StringLength(50)]
		public string Name { get; set; }

		[StringLength(96)]
		public string Description { get; set; }
	}
}