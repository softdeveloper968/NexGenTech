using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
	internal class ClaimStatusBatchCliamByClientIdSpecification : HeroSpecification<ClaimStatusBatchClaim>
	{
		/// <summary>
		/// Represents a specification used to filter ClaimStatusBatchClaims by a specific ClientId, excluding supplanted and deleted claims.
		/// </summary>
		public ClaimStatusBatchCliamByClientIdSpecification(int clientId)
		{
			Criteria = p => p.ClientId == clientId;
			Criteria = Criteria.And(c => !c.IsSupplanted);
			Criteria = Criteria.And(c => !c.IsDeleted);
		}
	}
}
