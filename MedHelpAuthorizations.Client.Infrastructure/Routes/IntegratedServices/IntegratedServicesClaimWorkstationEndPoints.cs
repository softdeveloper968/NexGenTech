using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
using static MedHelpAuthorizations.Client.Shared.Models.DashboardPresets.DashboardPresets;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetNotesByAuthorizationId;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public static class IntegratedServicesClaimWorkstationEndPoints
    {
        public static string Save = "api/v1/tenant/ClaimWorkstation";
        public static string GetClaimWorkStationDataByCriteriaPostEndPoint = "api/v1/tenant/ClaimWorkstation/GetClaimWorkStationDataByCriteria";
        public static string SaveClaimStatusMarkAsPaid = "/api/v1/tenant/ClaimWorkstation/MarkAsPaidClaimStatus";
        public static string SaveClaimStatusChangeStatus = "api/v1/tenant/ClaimWorkstation/ChangeStatusClaimStatus";

        #region Claim Status Workstation Notes
        public static string GetClaimStatusWorkstationNotesByClaimTransactionId(int claimTransactionId) => $"/api/v1/tenant/ClaimStatusWorkstationNotes/claimTransactionId/{claimTransactionId}";
        public static string GetClaimStatusWorkstationNotesById(int id) => $"/api/v1/tenant/ClaimStatusWorkstationNotes/{id}";
        public static string SaveClaimStatusWorkstationNotes() => $"/api/v1/tenant/ClaimStatusWorkstationNotes";
        public static string DeleteClaimStatusWorkstationNotesById(int id) => $"/api/v1/tenant/ClaimStatusWorkstationNotes/{id}";

        #endregion
    }
}
