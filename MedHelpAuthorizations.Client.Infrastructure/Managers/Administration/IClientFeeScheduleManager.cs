using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.AddEdit;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientFeeScheduleManager : IManager
    {
        Task<PaginatedResult<ClientFeeScheduleDto>> GetAllClientFeeSchdulePagedAsync(GetAllPagedClientFeeSchduleRequest request);
        Task<IResult<int>> SaveAsync(AddEditClientFeeScheduleCommand request);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<List<ClientFeeScheduleDto>>> GetClientFeeScheduleByInsuranceIdAsync(int clientInsuranceId, int clientId);
        Task<IResult<List<ClientFeeScheduleDto>>> GetAllFeeSchedule();

        //EN-155
        Task<IResult<List<ClientFeeScheduleDto>>> GetClientFeeScheduleByCriteria(int clientInsuranceId, DateTime dateOfService);

	}
}
