using MedHelpAuthorizations.Application.Extensions;using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;using MedHelpAuthorizations.Application.Helpers;using MedHelpAuthorizations.Application.Specifications.Base;using MedHelpAuthorizations.Domain.Entities.Enums;using MedHelpAuthorizations.Domain.Entities.IntegratedServices;using System.Collections.Generic;using System.Linq;namespace MedHelpAuthorizations.Application.Specifications{
	//public class ClaimStatusDashboardDetailsFilterSpecification : HeroSpecification<ClaimStatusTransaction> //   { //       public ClaimStatusDashboardDetailsFilterSpecification(IClaimStatusDashboardDetailsQuery filters) //       {
	//		IncludeStrings.Add("ClaimStatusBatchClaim.ClaimStatusBatch.ClientInsurance");
	//		IncludeStrings.Add("ClaimLineItemStatus");
	//		IncludeStrings.Add("ClaimStatusBatchClaim.Patient"); // AA-171
	//		IncludeStrings.Add("ClaimStatusBatchClaim.Patient.Person"); // AA-171
	//		IncludeStrings.Add("ClaimStatusBatchClaim.ClientProvider"); // AA-178
	//		IncludeStrings.Add("ClaimStatusBatchClaim.ClientProvider.Person"); // AA-178
	//		Criteria = t => true;
	//		Criteria = Criteria.And(t => !t.IsDeleted);

	//		if (!string.IsNullOrEmpty(filters.FlattenedLineItemStatus))//AA-120
	//		{
	//			//var categories = ClaimFiltersHelpers.ConvertStringToExceptionReasonCategoryEnumList(query.ExceptionReasonCategoryIds, false);
	//			//var categories = filters.FlattenedLineItemStatus.Split(',').ToList();
	//			//List<ClaimLineItemStatusEnum> parsedEnumValues = categories.Select(c => ClaimFiltersHelpers.ParseClaimLineItemStatusEnum(c)).ToList();

	//			List<ClaimLineItemStatusEnum> parsedEnumValues = ClaimFiltersHelpers.GetClaimLineItemStatusByGroupedStatus(filters.FlattenedLineItemStatus);
	//			Criteria = Criteria.And(c => c.ClaimLineItemStatusId.HasValue && parsedEnumValues.Contains((ClaimLineItemStatusEnum)c.ClaimLineItemStatusId));

	//			//foreach (var category in categories)
	//			//{
	//			//	if (category == ClaimLineItemStatusEnum.Unknown.GetDescription())
	//			//	{
	//			//		//Include Items that are considered "Denied" but are not mapped to a category
	//			//		Criteria = Criteria.Or(t => t.ClaimLineItemStatusId == null || t.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Unknown);
	//			//	}
	//			//	else
	//			//	{
	//			//		ClaimLineItemStatusEnum parsedEnumValue = ClaimFiltersHelpers.ParseClaimLineItemStatusEnum(category);

	//			//	}
	//			//}

	//		}


	//		// Do the Same logic that flattened the statuses on client side... but in reverse
	//		//if (filters.FlattenedLineItemStatus.Contains("Other")) //  //         { //  //             Criteria = Criteria.Or(t => ReadOnlyObjects.ReadOnlyObjects.OtherClaimLineItemStatuses.Contains(t.ClaimLineItemStatus.Id)); //  //         } //  //         if (filters.FlattenedLineItemStatus.Contains("Paid/Approved")) //  //         { //  //             Criteria = Criteria.Or(t => ReadOnlyObjects.ReadOnlyObjects.PaidApprovedClaimLineItemStatuses.Contains(t.ClaimLineItemStatus.Id)); //  //         } //  //         if (filters.FlattenedLineItemStatus.Contains("Not-Adjudicated")) //  //         { //  //             Criteria = Criteria.Or(t => ReadOnlyObjects.ReadOnlyObjects.NotAdjudicatedClaimLineItemStatuses.Contains(t.ClaimLineItemStatus.Id)); //  //         } //  //         if (filters.FlattenedLineItemStatus.Contains("Denied")) //  //         { //  //             Criteria = Criteria.Or(t => ReadOnlyObjects.ReadOnlyObjects.DeniedClaimLineItemStatuses.Contains(t.ClaimLineItemStatus.Id) && !t.ExceptionReason.Contains("Voided")); //  //         } //  //         if (filters.FlattenedLineItemStatus.Contains("Zero Pay")) //  //         { //  //             Criteria = Criteria.Or(t => ReadOnlyObjects.ReadOnlyObjects.ZeroPayClaimLineItemStatuses.Contains(t.ClaimLineItemStatus.Id)); //  //         } //  //         if(filters.FlattenedLineItemStatus.Contains("Reviewed"))///Tapi-125
 //  //         { //  //             Criteria = Criteria.Or(t => !ReadOnlyObjects.ReadOnlyObjects.ErrorClaimLineItemStatuses.Contains(t.ClaimLineItemStatus.Id)); //  //         } //  //         if (filters.FlattenedLineItemStatus.Contains("InProcess")) //AA-189
 //  //         { //  //             Criteria = Criteria.Or(t => ReadOnlyObjects.ReadOnlyObjects.ErrorClaimLineItemStatuses.Contains(t.ClaimLineItemStatus.Id)); //  //         } //  //         if (filters.PatientId != 0 && filters.PatientId is not null)
 //  //         {
 //  //             Criteria = Criteria.Or(t => t.ClaimStatusBatchClaim.PatientId == filters.PatientId);
 //  //         } //  //         if (filters.FlattenedLineItemStatus.Contains("Export All"))   //AA-311 //  //         { //  //             Criteria = Criteria.Or(t => !ReadOnlyObjects.ReadOnlyObjects.ErrorClaimLineItemStatuses.Contains(t.ClaimLineItemStatus.Id)); //  //         } //       } //   }}