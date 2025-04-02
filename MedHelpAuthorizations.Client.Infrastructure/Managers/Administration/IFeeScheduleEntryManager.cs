using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IFeeScheduleEntryManager : IManager
    {
        Task<PaginatedResult<GetAllFeeScheduleEntryResponse>> GetAllClientFeeSchdulePagedAsync(GetAllPagedFeeScheduleEntryRequest request);
        Task<IResult<int>> SaveAsync(AddEditFeeScheduleEntryCommand request);
        Task<IResult<GetAllFeeScheduleEntryResponse>> GetFeeScheduleEntryByIdAsync(GetFeeScheduleEntryByIdQuery request);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<List<GetAllFeeScheduleEntryResponse>>> GetAllClientFeeScheduleEntries();
        Task<IResult<int>> SaveImportDataAsync(AddEditImportClientFeeScheduleEntryCommand request);

        Task<IResult<int>> SaveCopyDataAsync(AddEditCopyClientFeeScheduleEntryCommand request);

        Task<IResult<List<GetAllFeeScheduleEntryResponse>>> GetByClientFeeScheduleId(int id);

        Task<IResult<int>> AutoCreateFeeSchedule(AutoCreateFeeScheduleEntriesCommand request);
	}
}
