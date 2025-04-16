using System;
using System.Threading.Tasks;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using static MedHelpAuthorizations.Client.Shared.Models.DashboardPresets.DashboardPresets;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetNotesByAuthorizationId;
using System.Collections.Generic;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusWorkstation;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ClaimWorkstation
{
    public interface IIntegratedServicesClaimWorkstationManager : IManager
    {
        Task<PaginatedResult<ClaimWorkstationDetailsResponse>> GetClaimWorkStationDataByCriteria(DateTime? claimStatusTransactionChangeStartDate, DateTime? lastClaimStatusCharged, PresetFilterTypeEnum? presetFilterTypeSelection, ClaimLineItemStatusEnum? previousStatus, ClaimLineItemStatusEnum? currentStatus, string clientInsuranceIds = default(string), string exceptionReasonCategoryIds = default(string), string serviceTypeIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerIds = default(string), string claimReportCategory = default(string), ClaimWorkstationSearchOptions claimWorkstationSearchOptions = null, int pageNumber = 0, int pageSize = 10, int? patientId = null);
        Task<IResult<int>> SaveAsync(AddEditClaimStatusWorkstationCommand request);
        Task<IResult<int>> SaveClaimStatusMarkAsPaidAsync(AddEditClaimStatusMarkAsPaidCommand request);
        Task<IResult<int>> SaveClaimStatusChangeStatusAsync(AddEditClaimStatusChangeStatusCommand request);

        #region Claim status Workstation notes

        Task<IResult<List<GetClaimStatusWorkstationNotesResponse>>> GetClaimStatusWorkstationNotesByClaimTransactionId(int claimTransactionId);

        Task<IResult<GetNotesByIdResponse>> GetClaimStatusWorkstationNotesById(int id);
        Task<IResult<int>> SaveClaimStatusWorkstationNotes(AddEditClaimStatusWorkstationNotesCommand request);

        Task<IResult<int>> DeleteClaimStatusWorkstationNotesById(int id);

        #endregion

    }
}
