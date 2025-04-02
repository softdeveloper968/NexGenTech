using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetById;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class FeeScheduleEntryManager : IFeeScheduleEntryManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public FeeScheduleEntryManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{FeeScheduleEntryEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<GetAllFeeScheduleEntryResponse>> GetAllClientFeeSchdulePagedAsync(GetAllPagedFeeScheduleEntryRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(FeeScheduleEntryEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.ClientFeeScheduleId, request.SearchString, request.SortLabel, request.SortDirection));
            return await response.ToPaginatedResult<GetAllFeeScheduleEntryResponse>();
        }

        public async Task<IResult<GetAllFeeScheduleEntryResponse>> GetFeeScheduleEntryByIdAsync(GetFeeScheduleEntryByIdQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(FeeScheduleEntryEndpoints.GetById(request.Id));
            return await response.ToResult<GetAllFeeScheduleEntryResponse>();
        }

        public async Task<IResult<int>> SaveImportDataAsync(AddEditImportClientFeeScheduleEntryCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(FeeScheduleEntryEndpoints.SaveImportData, request);
            return await response.ToResult<int>();
        }

		public async Task<IResult<int>> SaveAsync(AddEditFeeScheduleEntryCommand request)
		{
			var response = await _tenantHttpClient.PostAsJsonAsync(FeeScheduleEntryEndpoints.Save, request);
			return await response.ToResult<int>();
		}

		public async Task<IResult<List<GetAllFeeScheduleEntryResponse>>> GetAllClientFeeScheduleEntries()
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(FeeScheduleEntryEndpoints.GetAllFeeScheduleEntry());
                return await response.ToResult<List<GetAllFeeScheduleEntryResponse>>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public async Task<IResult<int>> SaveCopyDataAsync(AddEditCopyClientFeeScheduleEntryCommand request)
		{
			var response = await _tenantHttpClient.PostAsJsonAsync(FeeScheduleEntryEndpoints.SaveCopyData, request);
			return await response.ToResult<int>();
		}

		public async Task<IResult<List<GetAllFeeScheduleEntryResponse>>> GetByClientFeeScheduleId(int id)
		{
			try
			{
				var response = await _tenantHttpClient.GetAsync(FeeScheduleEntryEndpoints.GetByClientFeeScheduleId(id));
				return await response.ToResult<List<GetAllFeeScheduleEntryResponse>>();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<IResult<int>> AutoCreateFeeSchedule(AutoCreateFeeScheduleEntriesCommand request)
		{
			var response = await _tenantHttpClient.PostAsJsonAsync(FeeScheduleEntryEndpoints.AutoCreateFeeSchedule, request);
			return await response.ToResult<int>();
		}
	}
}
