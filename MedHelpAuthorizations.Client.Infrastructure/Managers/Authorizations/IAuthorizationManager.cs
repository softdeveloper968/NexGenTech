using MedHelpAuthorizations.Application.Features.Authorizations.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetAllPaged;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Requests.Authorizations;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByPatientId;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Application.Features.Reports.Queries.GetExpiringAuthorizations;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Authorizations
{
    public interface IAuthorizationManager : IManager
    {
        Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetAuthorizationsAsync(GetAllPagedAuthorizationsRequest request);
        Task<IResult<int>> SaveAsync(AddEditAuthorizationCommand request);
        Task<IResult<int>> DeleteAsync(int id);
        Task<string> ExportToExcelAsync();
        Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetAuthorizationsByPatientIdAsync(GetAuthorizationByPatientIdQuery request);
        Task<IResult<GetAuthorizationByIdResponse>> GetAuthorizationByIdAsync(int id);
        Task<PaginatedResult<GetByCriteriaPagedAuthorizationsResponse>> GetAuthorizationsByCriteriaAsync(GetByCriteriaPagedAuthorizationsQuery request);
    }
}