using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
	public class X12ClaimCategory : AuditableEntity<X12ClaimCategoryEnum>
	{
		public X12ClaimCategory() { }
		public X12ClaimCategory(X12ClaimCategoryEnum id, string name, string description)
		{
			Id = id;
			Name = name;
			Description = description;
		}

		[StringLength(50)]
		public string Name { get; set; }

		[StringLength(96)]
		public string Description { get; set; }
	}
}