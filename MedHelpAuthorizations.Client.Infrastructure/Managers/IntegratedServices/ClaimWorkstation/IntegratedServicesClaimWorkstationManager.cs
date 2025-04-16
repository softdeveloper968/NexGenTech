using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using static MedHelpAuthorizations.Client.Shared.Models.DashboardPresets.DashboardPresets;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetNotesByAuthorizationId;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetById;
using System.Collections.Generic;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusWorkstation;
using MedHelpAuthorizations.Application.Validators.Features.IntegratedServices.ClaimStatusWorkstation;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using System.Runtime.CompilerServices;
using static MedHelpAuthorizations.Shared.Constants.Permission.Permissions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Application.Helpers;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ClaimWorkstation
{
    public class IntegratedServicesClaimWorkstationManager : IIntegratedServicesClaimWorkstationManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public IntegratedServicesClaimWorkstationManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        public async Task<PaginatedResult<ClaimWorkstationDetailsResponse>> GetClaimWorkStationDataByCriteria(DateTime? claimStatusTransactionChangeStartDate, DateTime? lastClaimStatusCharged, PresetFilterTypeEnum? presetFilterTypeSelection, ClaimLineItemStatusEnum? previousStatus, ClaimLineItemStatusEnum? currentStatus, string clientInsuranceIds = default(string), string exceptionReasonCategoryIds = default(string), string serviceTypeIds = default(string), string procedureCodes = default(string), string locationIds = default(string), string providerIds = default(string), string claimReportCategory = default(string), ClaimWorkstationSearchOptions claimWorkstationSearchOptions = null, int pageNumber = 0, int pageSize = 10, int? patientId=null)
        {
            try
            {
                var request = new ClaimWorkstationDetailsQuery(pageNumber, pageSize, claimWorkstationSearchOptions)
                {
                    ClaimStatusTransactionChangeStartDate = claimStatusTransactionChangeStartDate,
                    LastClaimStatusCharged = lastClaimStatusCharged,
                    PresetFilterTypeSelectionType = presetFilterTypeSelection,
                    PreviousStatus = previousStatus,
                    CurrentStatus = currentStatus,
                    ClientInsuranceIds = clientInsuranceIds,
                    ExceptionReasonCategoryIds = exceptionReasonCategoryIds,
                    AuthTypeIds = serviceTypeIds,
                    ProcedureCodes = procedureCodes,
                    ClientLocationIds = locationIds,
                    ClientProviderIds = providerIds,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    ClaimWorkstationSearchOptions = claimWorkstationSearchOptions,
                    ClaimStatusCategory = claimReportCategory,
                    PatientId = patientId
                };
                var response = await _tenantHttpClient.PostAsJsonAsync(IntegratedServicesClaimWorkstationEndPoints.GetClaimWorkStationDataByCriteriaPostEndPoint, request);
                return await response.ToPaginatedResult<ClaimWorkstationDetailsResponse>();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error logged when trigger GetClaimWorkStationDataByCriteria: {e.StackTrace}");
                throw;
            }
        }

        public async Task<IResult<int>> SaveAsync(AddEditClaimStatusWorkstationCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(IntegratedServicesClaimWorkstationEndPoints.Save, request);
            return await response.ToResult<int>();
        }
        public async Task<IResult<int>> SaveClaimStatusMarkAsPaidAsync(AddEditClaimStatusMarkAsPaidCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(IntegratedServicesClaimWorkstationEndPoints.SaveClaimStatusMarkAsPaid, request);
            return await response.ToResult<int>();
        }
        public async Task<IResult<int>> SaveClaimStatusChangeStatusAsync(AddEditClaimStatusChangeStatusCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(IntegratedServicesClaimWorkstationEndPoints.SaveClaimStatusChangeStatus, request);
            return await response.ToResult<int>();
        }

        #region Claim Status Workstation Notes
        public async Task<IResult<List<GetClaimStatusWorkstationNotesResponse>>> GetClaimStatusWorkstationNotesByClaimTransactionId(int claimTransactionId)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(IntegratedServicesClaimWorkstationEndPoints.GetClaimStatusWorkstationNotesByClaimTransactionId(claimTransactionId));
                return await response.ToResult<List<GetClaimStatusWorkstationNotesResponse>>();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IResult<GetNotesByIdResponse>> GetClaimStatusWorkstationNotesById(int id)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(IntegratedServicesClaimWorkstationEndPoints.GetClaimStatusWorkstationNotesById(id));
                return await response.ToResult<GetNotesByIdResponse>();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<IResult<int>> SaveClaimStatusWorkstationNotes(AddEditClaimStatusWorkstationNotesCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(IntegratedServicesClaimWorkstationEndPoints.SaveClaimStatusWorkstationNotes(), request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteClaimStatusWorkstationNotesById(int id)
        {
            try
            {
                var response = await _tenantHttpClient.DeleteAsync(IntegratedServicesClaimWorkstationEndPoints.DeleteClaimStatusWorkstationNotesById(id));
                return await response.ToResult<int>();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

    }
}
