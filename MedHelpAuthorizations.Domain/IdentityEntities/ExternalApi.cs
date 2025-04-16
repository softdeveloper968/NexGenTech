using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.IdentityEntities.Enums;
using System;

namespace MedHelpAuthorizations.Domain.IdentityEntities
{
	public class ExternalApi : AuditableEntity<ExternalApiEnum>
	{
		public ExternalApi() { }
		public string Code { get; set; }
		public string Name { get; set; } 
		public string Description { get; set; }
	}
}
