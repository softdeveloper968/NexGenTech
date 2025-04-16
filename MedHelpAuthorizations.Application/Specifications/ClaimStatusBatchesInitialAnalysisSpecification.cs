using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
	public class ClaimStatusBatchesInitialAnalysisSpecification : HeroSpecification<ClaimStatusBatch>
	{
		public ClaimStatusBatchesInitialAnalysisSpecification(bool isForInitialAnalysis = false)
		{
			Includes.Add(b => b.Client);
			Criteria = b => true;

			if (isForInitialAnalysis)
			{
				Criteria = Criteria.And(b => b.CreatedOn <= b.Client.InitialAnalysisEndOn.Value);
			}
			else
			{
				Criteria = Criteria.And(b => b.Client.InitialAnalysisEndOn.HasValue ? b.CreatedOn >= b.Client.InitialAnalysisEndOn.Value : true);
			}
		}
	}
}
