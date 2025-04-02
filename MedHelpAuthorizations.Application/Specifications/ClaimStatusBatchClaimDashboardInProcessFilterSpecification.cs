using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Linq;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusBatchClaimDashboardInProcessFilterSpecification : ClaimStatusBatchClaimDashboardFilterSpecification
    {
        public ClaimStatusBatchClaimDashboardInProcessFilterSpecification(IClaimStatusDashboardStandardQuery query, int clientId) : base(query, clientId)
        {
            Includes.Add(c => c.Patient);
            Includes.Add(c => c.Patient.Person);
            Includes.Add(c => c.ClaimStatusTransaction);

            Criteria = Criteria.And(c => c.ClaimStatusTransactionId == null || c.ClaimStatusTransaction.ClaimLineItemStatus == null ||
            ReadOnlyObjects.ReadOnlyObjects.ErrorClaimLineItemStatuses.Contains(c.ClaimStatusTransaction.ClaimLineItemStatusId.Value));

            if (query.PatientId is not null && query.PatientId != 0)
            {
                Criteria = Criteria.And(t => t.PatientId == query.PatientId);
            }

            //if (string.IsNullOrEmpty(query.CommaDelimitedLineItemStatusIds))
            //{
            //    Criteria = Criteria.And(c => c.ClaimStatusTransactionId == null || ReadOnlyObjects.ReadOnlyObjects.ErrorClaimLineItemStatuses.Contains(c.ClaimStatusTransaction.ClaimLineItemStatusId.Value));
            //}
            //if (query.CommaDelimitedLineItemStatusIds == "Other")
            //{
            //    Criteria = Criteria.And(t => t.ClaimStatusTransaction.ClaimLineItemStatusId.HasValue && ReadOnlyObjects.ReadOnlyObjects.OtherClaimLineItemStatuses.Contains(t.ClaimStatusTransaction.ClaimLineItemStatusId.Value));
            //}
            //if (query.CommaDelimitedLineItemStatusIds == "Paid/Approved")
            //{
            //    Criteria = Criteria.And(t => t.ClaimStatusTransaction.ClaimLineItemStatusId.HasValue && ReadOnlyObjects.ReadOnlyObjects.PaidApprovedClaimLineItemStatuses.Contains(t.ClaimStatusTransaction.ClaimLineItemStatusId.Value));
            //}
            //if (query.CommaDelimitedLineItemStatusIds == "Not-Adjudicated")
            //{
            //    Criteria = Criteria.And(t => t.ClaimStatusTransaction.ClaimLineItemStatusId.HasValue && ReadOnlyObjects.ReadOnlyObjects.NotAdjudicatedClaimLineItemStatuses.Contains(t.ClaimStatusTransaction.ClaimLineItemStatusId.Value));
            //}
            //if (query.CommaDelimitedLineItemStatusIds == "Denied")
            //{
            //    Criteria = Criteria.And(t => t.ClaimStatusTransaction.ClaimLineItemStatusId.HasValue && ReadOnlyObjects.ReadOnlyObjects.DeniedClaimLineItemStatuses.Contains(t.ClaimStatusTransaction.ClaimLineItemStatusId.Value) && !t.ClaimStatusTransaction.ExceptionReason.Contains("Voided"));
            //}
            //if (query.CommaDelimitedLineItemStatusIds == "Zero Pay")
            //{
            //    Criteria = Criteria.And(t => t.ClaimStatusTransaction.ClaimLineItemStatusId.HasValue && ReadOnlyObjects.ReadOnlyObjects.ZeroPayClaimLineItemStatuses.Contains(t.ClaimStatusTransaction.ClaimLineItemStatusId.Value));
            //}
            //if (query.CommaDelimitedLineItemStatusIds == "Reviewed")///Tapi-125
            //{
            //    Criteria = Criteria.And(t => t.ClaimStatusTransaction.ClaimLineItemStatusId.HasValue && !ReadOnlyObjects.ReadOnlyObjects.ErrorClaimLineItemStatuses.Contains(t.ClaimStatusTransaction.ClaimLineItemStatusId.Value));
            //}
            //if (query.CommaDelimitedLineItemStatusIds == "InProcess") //AA-189
            //{
            //    Criteria = Criteria.And(c => c.ClaimStatusTransactionId == null || ReadOnlyObjects.ReadOnlyObjects.ErrorClaimLineItemStatuses.Contains(c.ClaimStatusTransaction.ClaimLineItemStatusId.Value));
            //}
            //if (query.CommaDelimitedLineItemStatusIds == "Export All")
            //{
            //    Criteria = Criteria.And(c => c.ClaimStatusTransactionId == null 
            //                                    || ReadOnlyObjects.ReadOnlyObjects.ErrorClaimLineItemStatuses.Contains(c.ClaimStatusTransaction.ClaimLineItemStatusId.Value)
            //                                    || ReadOnlyObjects.ReadOnlyObjects.OtherClaimLineItemStatuses.Contains(c.ClaimStatusTransaction.ClaimLineItemStatusId.Value)
            //                                    || ReadOnlyObjects.ReadOnlyObjects.NotAdjudicatedClaimLineItemStatuses.Contains(c.ClaimStatusTransaction.ClaimLineItemStatusId.Value));
            //}
            //if (query.PatientId != 0 && query.PatientId is not null)
            //{
            //    Criteria = Criteria.And(t => t.PatientId == query.PatientId);
            //}
        }
    }
}
