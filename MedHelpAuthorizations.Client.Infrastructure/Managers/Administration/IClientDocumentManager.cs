using MedHelpAuthorizations.Application.Features.Administration.ClientDocuments.Command.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientDocuments.Queries;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientDocumentManager :IManager
    {
        Task<PaginatedResult<GetAllPagedClientDocumentsResponse>> GetClientDocumentsAsync(GetAllPagedClientDocumentRequest request);
        Task<IResult<int>> SaveAsync(AddEditClientDocumentCommand request);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
