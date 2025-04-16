using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.GetById;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Commands.Upsert;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ChargeEntry
{
    public interface IChargeEntryTransactionManager : IManager
    {
        Task<IResult<List<GetAllChargeEntryTransactionsResponse>>> GetAllAsync();

        Task<IResult<GetChargeEntryTransactionByIdResponse>> GetByIdAsync(int id);

        Task<IResult<List<GetChargeEntryTransactionsByBatchIdResponse>>> GetByChargeEntryBatchId(int chargeEntryBatchId);

        Task<IResult<int>> UpsertAsync(UpsertChargeEntryTransactionCommand command);
    }
}
