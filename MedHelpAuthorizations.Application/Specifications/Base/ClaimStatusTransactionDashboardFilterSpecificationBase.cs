using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Shared.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Specifications.Base
{
	public class ClaimStatusTransactionDashboardFilterSpecificationBase : HeroSpecification<ClaimStatusTransaction>
	{
		public ClaimStatusTransactionDashboardFilterSpecificationBase(IClaimStatusDashboardStandardQuery query, int clientId)
		{
			Includes.Add(t => t.ClaimStatusBatchClaim);
			Includes.Add(t => t.ClaimStatusBatchClaim.ClaimStatusBatch);
			Includes.Add(t => t.ClaimStatusBatchClaim.ClaimStatusBatch.AuthType);
			Includes.Add(t => t.ClaimStatusBatchClaim.ClaimStatusBatch.ClientInsurance);
			Includes.Add(t => t.ClaimStatusBatchClaim.ClaimStatusBatch.ClientInsurance.RpaInsurance);
			Includes.Add(t => t.ClaimStatusExceptionReasonCategory);
			Includes.Add(t => t.ClaimStatusBatchClaim.ClientLocation);

			Criteria = c => true;
			Criteria = Criteria.And(c => !c.IsDeleted);
			Criteria = Criteria.And(c => c.ClientId == clientId);

			//if (query.ClientInsuranceId > 0)
			//{
			//    Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimStatusBatch.ClientInsuranceId == query.ClientInsuranceId);
			//}


			//if (query.AuthTypeId > 0)
			//{
			//    Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimStatusBatch.AuthTypeId == query.AuthTypeId);
			//}
			////if (query.ClaimStatusBatchId > 0)
			////{
			////    Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimStatusBatch.Id == query.ClaimStatusBatchId);
			////}

			if (!string.IsNullOrEmpty(query.ClientInsuranceIds))//AA-120
			{
				Criteria = Criteria.And(c => ClaimFiltersHelpers.ConvertStringToList(query.ClientInsuranceIds, true).Contains(c.ClaimStatusBatchClaim.ClaimStatusBatch.ClientInsuranceId));
			}

			if (!string.IsNullOrEmpty(query.AuthTypeIds))//AA-120
			{
				Criteria = Criteria.And(c => ClaimFiltersHelpers.ConvertStringToList(query.AuthTypeIds, true).Contains(c.ClaimStatusBatchClaim.ClaimStatusBatch.AuthTypeId ?? 0));
			}

			//if (query.ExceptionReasonCategory != null)
			//{
			//    if (query.ExceptionReasonCategoryIds == ClaimStatusExceptionReasonCategoryEnum.Other)
			//    {
			//        //Include Items that are considered "Denied" but are not mapped to a category
			//        Criteria = Criteria.And(t => t.ClaimStatusExceptionReasonCategoryId == null || t.ClaimStatusExceptionReasonCategoryId == ClaimStatusExceptionReasonCategoryEnum.Other);
			//    }
			//    else
			//    {
			//        Criteria = Criteria.And(c => c.ClaimStatusExceptionReasonCategoryId == query.ExceptionReasonCategory);
			//    }
			//}
			if (!string.IsNullOrEmpty(query.ExceptionReasonCategoryIds))//AA-120
			{
				//var categories = ClaimFiltersHelpers.ConvertStringToExceptionReasonCategoryEnumList(query.ExceptionReasonCategoryIds, false);
				var categories = query.ExceptionReasonCategoryIds?.Split(',')?.ToList() ?? new List<string>();
				var categoryIds = categories.Select(c => ClaimFiltersHelpers.ParseClaimStatusExceptionReasonCategoryEnum(c)).ToList();
				if (categoryIds.Contains(ClaimStatusExceptionReasonCategoryEnum.Other))
				{
					//Include Items that are considered "Denied" but are not mapped to a category
					Criteria = Criteria.And(c => categoryIds.Contains((ClaimStatusExceptionReasonCategoryEnum)c.ClaimStatusExceptionReasonCategoryId)
					|| c.ClaimStatusExceptionReasonCategoryId == null);
				}
				else
				{
					Criteria = Criteria.And(c => (categoryIds.Contains((ClaimStatusExceptionReasonCategoryEnum)c.ClaimStatusExceptionReasonCategoryId)));
				}


			}
			//if (!string.IsNullOrWhiteSpace(query.ProcedureCode))
			//{
			//    Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ProcedureCode.Trim() == query.ProcedureCode.Trim());
			//}
			if (!string.IsNullOrWhiteSpace(query.ProcedureCodes))//AA-120
			{
				Criteria = Criteria.And(c => ClaimFiltersHelpers.ConvertProcedureCodesToList(query.ProcedureCodes).Contains(c.ClaimStatusBatchClaim.ProcedureCode.Trim()));
			}


			if (query.ReceivedFrom != null)
			{
				Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.CreatedOn.Date >= query.ReceivedFrom.Value.Date);
			}
			if (query.ReceivedTo != null)
			{
				Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.CreatedOn.Date <= query.ReceivedTo.Value.Date);
			}

			if (query.DateOfServiceFrom != null)
			{
				Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.DateOfServiceFrom.Value.Date >= query.DateOfServiceFrom.Value.Date);
			}
			if (query.DateOfServiceTo != null)
			{
				Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.DateOfServiceTo.Value.Date <= query.DateOfServiceTo.Value.Date);
			}

			if (query.TransactionDateFrom != null)
			{
				Criteria = Criteria.And(c => c.LastModifiedOn == null ? c.CreatedOn.Date >= query.TransactionDateFrom.Value.Date : c.LastModifiedOn.Value.Date >= query.TransactionDateFrom.Value.Date);
			}
			if (query.TransactionDateTo != null)
			{
				Criteria = Criteria.And(c => c.LastModifiedOn == null ? c.CreatedOn.Date <= query.TransactionDateTo.Value.Date : c.LastModifiedOn.Value.Date <= query.TransactionDateTo.Value.Date);
			}

			if (query.ClaimBilledFrom != null)
			{
				Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimBilledOn.Value.Date >= query.ClaimBilledFrom.Value.Date);
			}
			if (query.ClaimBilledTo != null)
			{
				Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimBilledOn.Value.Date <= query.ClaimBilledTo.Value.Date);
			}
			if (!string.IsNullOrEmpty(query.ClientLocationIds))//AA-207
			{
				Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClientLocationId.HasValue && ClaimFiltersHelpers.ConvertStringToList(query.ClientLocationIds, true).Contains((int)c.ClaimStatusBatchClaim.ClientLocationId));
			}
			if (!string.IsNullOrEmpty(query.ClientProviderIds))//AA-207
			{
				Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClientProviderId.HasValue && ClaimFiltersHelpers.ConvertStringToList(query.ClientProviderIds, true).Contains((int)c.ClaimStatusBatchClaim.ClientProviderId));
			}
		}
	}
}
