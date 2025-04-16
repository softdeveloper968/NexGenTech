using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using static MedHelpAuthorizations.Client.Shared.Models.DashboardPresets.DashboardPresets;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimWorkstationFilterSpecification : HeroSpecification<ClaimStatusTransaction>
    {
        public ClaimWorkstationFilterSpecification(IClaimWorkstationDetailQuery query, int clientId, ClaimWorkstationSearchOptions claimWorkstationSearchOptions = null)
        {
            Includes.Add(t => t.ClaimStatusBatchClaim);
            Includes.Add(t => t.ClaimStatusBatchClaim.ClaimStatusBatch);
            Includes.Add(t => t.ClaimStatusBatchClaim.Patient); //AA-171
            Includes.Add(t => t.ClaimStatusBatchClaim.Patient.Person); //AA-171
            Includes.Add(t => t.ClaimStatusBatchClaim.ClaimStatusBatch.AuthType);
            Includes.Add(t => t.ClaimStatusBatchClaim.ClaimStatusBatch.ClientInsurance);
            Includes.Add(t => t.ClaimStatusBatchClaim.ClaimStatusBatch.ClientInsurance.RpaInsurance);
            Includes.Add(t => t.ClaimStatusExceptionReasonCategory);
            Includes.Add(t => t.ClaimStatusTransactionLineItemStatusChangẹ);

            Criteria = p => true;
            Criteria = Criteria.And(c => !c.IsDeleted);
            Criteria = Criteria.And(c => c.ClientId == clientId);
            //As per discussion: Exclude Error type status value from Api and Dropdown: AA-56
            //As per discussion: with Jim, Kevin, exclude Paid and Approved claim status from workstation: AA-63:
            //Criteria = Criteria.And(c => c.ClaimLineItemStatusId != ClaimLineItemStatusEnum.Error && c.ClaimLineItemStatusId != ClaimLineItemStatusEnum.Paid && c.ClaimLineItemStatusId != ClaimLineItemStatusEnum.Approved);

            var otherClaimStatusGroup = ReadOnlyObjects.ReadOnlyObjects.OtherClaimLineItemStatuses;
            var paidApprovedClaimStatusGroup = ReadOnlyObjects.ReadOnlyObjects.PaidApprovedClaimLineItemStatuses;
            var nonAdjudicatedClaimStatusesGroup = ReadOnlyObjects.ReadOnlyObjects.OpenClaimLineItemStatuses;
            var deniedClaimLineItemStatuses = ReadOnlyObjects.ReadOnlyObjects.DeniedClaimLineItemStatuses;
            var zeroPayClaimLineItemStatuses = ReadOnlyObjects.ReadOnlyObjects.ZeroPayBundledClaimLineItemStatuses;
			var errorClaimLineItemStatuses = ReadOnlyObjects.ReadOnlyObjects.ErrorClaimLineItemStatuses;

            ///As per discussion with Jim/kevin, [March 09] [TAPI-125]
            ///Claim workstation default execute for Denial category.
            if (!string.IsNullOrEmpty(query.ClaimStatusCategory))
            {
                switch (query.ClaimStatusCategory)
                {
                    case "OtherClaimStatusesGroup":
                        {
                            Criteria = Criteria.And(t => otherClaimStatusGroup.Contains(t.ClaimLineItemStatus.Id));
                            break;
                        }
                    case "PaidApprovedClaimStatusesGroup":
                        {
                            Criteria = Criteria.And(t => paidApprovedClaimStatusGroup.Contains(t.ClaimLineItemStatus.Id));
                            break;
                        }
                    case "Not-AdjudicatedClaimStatusesGroup":
                        {
                            Criteria = Criteria.And(t => nonAdjudicatedClaimStatusesGroup.Contains(t.ClaimLineItemStatus.Id));
                            break;
                        }
                    case "DeniedClaimStatusesGroup":
                        {
                            Criteria = Criteria.And(t => deniedClaimLineItemStatuses.Contains(t.ClaimLineItemStatus.Id) && !t.ExceptionReason.Contains("Voided"));
                            break;
                        }
                    case "ZeroPayClaimStatusesGroup":
                        {
                            Criteria = Criteria.And(t => zeroPayClaimLineItemStatuses.Contains(t.ClaimLineItemStatus.Id));
                            break;
                        }
                    case "ReviewedClaimStatusesGroup":
                        {
                            Criteria = Criteria.And(t => !errorClaimLineItemStatuses.Contains(t.ClaimLineItemStatus.Id));
                            break;
                        }

                    default:
                        {
                            Criteria = Criteria.And(c => c.ClaimLineItemStatusId.HasValue && deniedClaimLineItemStatuses.Contains<ClaimLineItemStatusEnum>((ClaimLineItemStatusEnum)c.ClaimLineItemStatusId));
                            break;
                        }
                }
            }
            else
            {
                Criteria = Criteria.And(c => c.ClaimLineItemStatusId.HasValue && deniedClaimLineItemStatuses.Contains<ClaimLineItemStatusEnum>((ClaimLineItemStatusEnum)c.ClaimLineItemStatusId));
            }

            //As per discussion with Jim: once claim status transaction record is updated then exclude that record from list:AA-63
            Criteria = Criteria.And(c => (c.ClaimLineItemStatusValue != "Manual Edit"));

            /// UnComment this code if filter claim workstation records by search string with Patient FullName.
            /// Filter Claim Workstation data by SearchString:
            /// Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.PatientFirstName.ToLower().Contains(query.SearchString)||c.ClaimStatusBatchClaim.PatientLastName.ToLower().Contains(query.SearchString));
            switch (query.PresetFilterTypeSelectionType)
            {
                case PresetFilterTypeEnum.TransactionDate:
                    {
                        if (query.ClaimStatusTransactionChangeStartDate.HasValue)
                        {
                            Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimStatusTransaction != null
                                    && (c.ClaimStatusBatchClaim.ClaimStatusTransaction.LastModifiedOn == null
                                        ? c.ClaimStatusBatchClaim.ClaimStatusTransaction.CreatedOn.Date
                                      >= query.ClaimStatusTransactionChangeStartDate.Value.Date
                                        : c.ClaimStatusBatchClaim.ClaimStatusTransaction.LastModifiedOn.Value.Date
                                      >= query.ClaimStatusTransactionChangeStartDate.Value.Date));
                        }
                        if (query.LastClaimStatusCharged.HasValue)
                        {
                            Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimStatusTransaction != null
                                    && (c.ClaimStatusBatchClaim.ClaimStatusTransaction.LastModifiedOn == null
                                        ? c.ClaimStatusBatchClaim.ClaimStatusTransaction.CreatedOn.Date
                                      <= query.LastClaimStatusCharged.Value.Date
                                        : c.ClaimStatusBatchClaim.ClaimStatusTransaction.LastModifiedOn.Value.Date
                                      <= query.LastClaimStatusCharged.Value.Date));
                        }
                    }
                    break;
                case PresetFilterTypeEnum.ServiceDate:
                    {
                        if (query.ClaimStatusTransactionChangeStartDate.HasValue)
                        {
                            Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.DateOfServiceFrom.Value.Date >= query.ClaimStatusTransactionChangeStartDate.Value.Date);
                        }
                        if (query.LastClaimStatusCharged.HasValue)
                        {
                            Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.DateOfServiceTo.Value.Date <= query.LastClaimStatusCharged.Value.Date);
                        }
                    }

                    break;
                case PresetFilterTypeEnum.ReceivedDate:
                    {
                        if (query.ClaimStatusTransactionChangeStartDate.HasValue)
                        {
                            Criteria = Criteria.And(c => c.CreatedOn.Date >= query.ClaimStatusTransactionChangeStartDate.Value.Date);
                        }
                        if (query.LastClaimStatusCharged.HasValue)
                        {
                            Criteria = Criteria.And(c => c.CreatedOn.Date <= query.LastClaimStatusCharged.Value.Date);
                        }
                    }
                    break;
                case PresetFilterTypeEnum.BilledOnDate:
                    {
                        if (query.ClaimStatusTransactionChangeStartDate.HasValue)
                        {
                            Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimBilledOn != null && c.ClaimStatusBatchClaim.ClaimBilledOn.Value.Date >= query.ClaimStatusTransactionChangeStartDate.Value.Date);
                        }
                        if (query.LastClaimStatusCharged.HasValue)
                        {
                            Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimBilledOn != null && c.ClaimStatusBatchClaim.ClaimBilledOn.Value.Date <= query.LastClaimStatusCharged.Value.Date);
                        }
                    }
                    break;
                default:
                    {
                        //if (query.ClaimStatusTransactionChangeStartDate.HasValue)
                        //{
                        //    Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimBilledOn != null && c.ClaimStatusBatchClaim.ClaimBilledOn.Value.Date >= query.ClaimStatusTransactionChangeStartDate.Value.Date);
                        //}
                        //if (query.LastClaimStatusCharged.HasValue)
                        //{
                        //    Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimBilledOn != null && c.ClaimStatusBatchClaim.ClaimBilledOn.Value.Date <= query.LastClaimStatusCharged.Value.Date);
                        //}

                        ///Fixed Issue if there is not any filter selection type or it is 0 then default it works for transaction date.
                        if (query.ClaimStatusTransactionChangeStartDate.HasValue)
                        {
                            Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimStatusTransaction != null
                                    && (c.ClaimStatusBatchClaim.ClaimStatusTransaction.LastModifiedOn == null
                                        ? c.ClaimStatusBatchClaim.ClaimStatusTransaction.CreatedOn.Date
                                      >= query.ClaimStatusTransactionChangeStartDate.Value.Date
                                        : c.ClaimStatusBatchClaim.ClaimStatusTransaction.LastModifiedOn.Value.Date
                                      >= query.ClaimStatusTransactionChangeStartDate.Value.Date));
                        }
                        if (query.LastClaimStatusCharged.HasValue)
                        {
                            Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimStatusTransaction != null
                                    && (c.ClaimStatusBatchClaim.ClaimStatusTransaction.LastModifiedOn == null
                                        ? c.ClaimStatusBatchClaim.ClaimStatusTransaction.CreatedOn.Date
                                      <= query.LastClaimStatusCharged.Value.Date
                                        : c.ClaimStatusBatchClaim.ClaimStatusTransaction.LastModifiedOn.Value.Date
                                      <= query.LastClaimStatusCharged.Value.Date));
                        }
                    }
                    break;
            }

            if (query.PreviousStatus.HasValue)
            {
                Criteria = Criteria.And(c => c.ClaimStatusTransactionLineItemStatusChangẹ.PreviousClaimLineItemStatusId == query.PreviousStatus);
            }

            if (query.CurrentStatus.HasValue)
            {
                Criteria = Criteria.And(c => c.ClaimLineItemStatusId == query.CurrentStatus);
            }


            /// search functionality for Patient first name, last name, Bactch number, Policy number, Claim number, Date of Birth.
            /// TAPI:121:
            if (claimWorkstationSearchOptions != null)
            {
                if (!string.IsNullOrWhiteSpace(claimWorkstationSearchOptions.PatientFirstName) || !string.IsNullOrWhiteSpace(claimWorkstationSearchOptions.PatientLastName))
                {
                    Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.Patient.Person.FirstName.Contains(claimWorkstationSearchOptions.PatientFirstName));
                    Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.Patient.Person.LastName.Contains(claimWorkstationSearchOptions.PatientLastName));
                }
                if (!string.IsNullOrWhiteSpace(claimWorkstationSearchOptions.BatchNumber))
                {
                    Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimStatusBatch.BatchNumber.Equals(claimWorkstationSearchOptions.BatchNumber));
                }
                if (!string.IsNullOrWhiteSpace(claimWorkstationSearchOptions.PolicyNumber))
                {
                    Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.PolicyNumber.Equals(claimWorkstationSearchOptions.PolicyNumber));
                }
                if (claimWorkstationSearchOptions.DateOfBirth.HasValue)
                {
                    Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.Patient.Person.DateOfBirth.Equals(claimWorkstationSearchOptions.DateOfBirth.Value));
                }
                if (!string.IsNullOrWhiteSpace(claimWorkstationSearchOptions.ClaimNumber))
                {
                    Criteria = Criteria.And(c => c.ClaimStatusBatchClaim.ClaimNumber.Equals(claimWorkstationSearchOptions.ClaimNumber));
                }
            }

            ///EN-101
            ///Filter based on patient Id
            if (query.PatientId.HasValue && query.PatientId > 0)
            {
                Criteria = Criteria.And(claimStatusTransaction => claimStatusTransaction.ClaimStatusBatchClaim != null && claimStatusTransaction.ClaimStatusBatchClaim.Patient != null && claimStatusTransaction.ClaimStatusBatchClaim.Patient.Id > 0 && claimStatusTransaction.ClaimStatusBatchClaim.Patient.Id == query.PatientId);
            }
        }
    }
}
