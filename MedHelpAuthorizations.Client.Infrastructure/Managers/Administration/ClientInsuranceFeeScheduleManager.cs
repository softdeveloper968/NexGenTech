using MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule.Command.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.UpdateClientFeeSchedule;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class ClientInsuranceFeeScheduleManager : IClientInsuranceFeeScheduleManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;
        public ClientInsuranceFeeScheduleManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient; ;
        }
        public async Task<PaginatedResult<GetAllClientInsuranceFeeScheduleResponse>> GetAllClientInsuranceFeeSchedule(GetAllClientInsuranceFeeScheduleRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientInsuranceFeeScheduleEndPoints.GetAllPaged(request.PageNumber, request.PageSize, request.ClientInsuranceId));
            return await response.ToPaginatedResult<GetAllClientInsuranceFeeScheduleResponse>();
        }

        public async Task<IResult<List<GetAllClientInsuranceFeeScheduleResponse>>> GetClientInsuranceFeeScheduleByClientFeeScheduleId(int clientFeeScheduleId)
        {
            var response = await _tenantHttpClient.GetAsync(ClientInsuranceFeeScheduleEndPoints.GetAllByClientFeeScheduleId(clientFeeScheduleId));
            return await response.ToResult<List<GetAllClientInsuranceFeeScheduleResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditClientInsuranceFeeScheduleCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientInsuranceFeeScheduleEndPoints.Save, request);
            return await response.ToResult<int>();
        }

		public async Task<IResult<int>> UpdateAsync(UpdateClientFeeScheduleCommand request)
		{
			var response = await _tenantHttpClient.PostAsJsonAsync(ClientInsuranceFeeScheduleEndPoints.Update, request);
			return await response.ToResult<int>();
		}
	}
}
