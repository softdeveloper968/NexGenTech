using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByPatientId;
using MedHelpAuthorizations.Application.Features.Reports.GetCurrentAuthorizations;
using MedHelpAuthorizations.Application.Features.Reports.Queries.GetExpiringAuthorizations;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IAuthorizationRepository : IRepositoryAsync<Authorization, int>
    {
        Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetAllWithProperName(GetAllAuthorizationsQuery request);
        Task<PaginatedResult<GetByCriteriaPagedAuthorizationsResponse>> GetByCriteriaWithProperName(GetByCriteriaPagedAuthorizationsQuery request);
        Task<GetAuthorizationByIdResponse> GetByIdWithProperName(GetAuthorizationByIdQuery request);
        Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetByPatientIdWithProperName(GetAuthorizationByPatientIdQuery request);
        Task<Authorization> GetExpiringForPatientIdAndAuthType(int patientId, int authTypeId, int clientId, DateTime? createdOn);
        Task<IQueryable<GetAllPagedAuthorizationsResponse>> GetExpiringWithProperNameQueryable(GetPagedExpiringAuthorizationsQuery request);
        Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> GetExpiringWithProperNamePaginated(GetPagedExpiringAuthorizationsQuery request);
        Task<IQueryable<GetAllPagedAuthorizationsResponse>> GetExpiringWithProperNameAllClientQueryable(int expiringDays = 30);
        Task<IQueryable<GetCurrentAuthorizationsResponse>> GetCurrentWithProperNameQueryable(GetPagedCurrentAuthorizationsQuery request);
        Task<PaginatedResult<GetCurrentAuthorizationsResponse>> GetCurrentWithProperNamePaginated(GetPagedCurrentAuthorizationsQuery request);
        Task<IQueryable<GetCurrentAuthorizationsResponse>> GetCurrentWithProperNameAllClientQueryable();

    }
}
