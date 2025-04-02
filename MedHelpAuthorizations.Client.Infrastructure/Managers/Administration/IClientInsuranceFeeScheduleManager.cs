using MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule.Command.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsuranceFeeSchedule.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.UpdateClientFeeSchedule;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientInsuranceFeeScheduleManager : IManager
    {
        Task<PaginatedResult<GetAllClientInsuranceFeeScheduleResponse>> GetAllClientInsuranceFeeSchedule(GetAllClientInsuranceFeeScheduleRequest request);
        Task<IResult<List<GetAllClientInsuranceFeeScheduleResponse>>> GetClientInsuranceFeeScheduleByClientFeeScheduleId(int clientFeeScheduleId);
        Task<IResult<int>> SaveAsync(AddEditClientInsuranceFeeScheduleCommand request);
		Task<IResult<int>> UpdateAsync(UpdateClientFeeScheduleCommand query);
	}
}
    