using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Commands.Create;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Commands.Update;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetById;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetRecent;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ChargeEntry
{
    public interface IChargeEntryBatchManager : IManager
    {
        Task<IResult<List<GetAllChargeEntryBatchesResponse>>> GetAllAsync();

        Task<IResult<GetChargeEntryBatchByIdResponse>> GetByIdAsync(int id);

        Task<IResult<int>> CreateAsync(CreateChargeEntryBatchCommand command);

        //Task<IResult<int>> UpdateAsync(UpdateChargeEntryBatchCommand command);

        Task<IResult<int>> UpdateCompletedAsync(int batchId);

        Task<IResult<int>> UpdateAbortedAsync(int batchId);

        //Task<IResult<int>> UpdateDeletedAsync(UpdateChargeEntryBatchCommand command);

        //Task<IResult<int>> DeleteAsync(int batchId);

        Task<IResult<int>> UpdateProcessStartDateTimeAsync(UpdateChargeEntryBatchCommand command);

        Task<IResult<List<GetRecentChargeEntryBatchesByClientIdResponse>>> GetRecentForClientIdAsync();

    }
}
