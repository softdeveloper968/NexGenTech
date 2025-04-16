using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.AddEdit;
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
	public class ClientFeeScheduleManager : IClientFeeScheduleManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ClientFeeScheduleManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{ClientFeeScheduleEndPoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<ClientFeeScheduleDto>> GetAllClientFeeSchdulePagedAsync(GetAllPagedClientFeeSchduleRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientFeeScheduleEndPoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<ClientFeeScheduleDto>();
        }
        public async Task<IResult<List<ClientFeeScheduleDto>>> GetClientFeeScheduleByInsuranceIdAsync(int clientInsuranceId, int clientId)
        {
            var response = await _tenantHttpClient.GetAsync(ClientFeeScheduleEndPoints.GetByInsuranceId(clientInsuranceId, clientId));
            return await response.ToResult<List<ClientFeeScheduleDto>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditClientFeeScheduleCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientFeeScheduleEndPoints.Save, request);
            return await response.ToResult<int>();
        }

        //EN-155
        public async Task<IResult<List<ClientFeeScheduleDto>>> GetAllFeeSchedule()
        {
            var manager = await _tenantHttpClient.GetAsync(ClientFeeScheduleEndPoints.GetAllFeeSchedule());
            return await manager.ToResult<List<ClientFeeScheduleDto>>();

        }

		//EN-155
		public async Task<IResult<List<ClientFeeScheduleDto>>> GetClientFeeScheduleByCriteria(int clientInsuranceId, DateTime dateOfService)
        {
            var response = await _tenantHttpClient.GetAsync(ClientFeeScheduleEndPoints.GetAllByCriteria(clientInsuranceId, dateOfService));
            return await response.ToResult<List<ClientFeeScheduleDto>>();
        }

	}
}

