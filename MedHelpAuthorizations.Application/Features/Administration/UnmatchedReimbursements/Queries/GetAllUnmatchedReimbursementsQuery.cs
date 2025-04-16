using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.UnmatchedReimbursements.Queries
{
    public class GetAllUnmatchedReimbursementsQuery : IRequest<PaginatedResult<GetAllUnmatchedReimbursementResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; } = string.Empty;

        public GetAllUnmatchedReimbursementsQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetAllUnmatchedReimbursementsQueryHandler : IRequestHandler<GetAllUnmatchedReimbursementsQuery, PaginatedResult<GetAllUnmatchedReimbursementResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetAllUnmatchedReimbursementsQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllUnmatchedReimbursementResponse>> Handle(GetAllUnmatchedReimbursementsQuery request, CancellationToken cancellationToken)
        {

            try
            {
                // Retrieve the latest claim IDs per procedure code
                var latestClaimIdsPerProcedureCode = await _unitOfWork.Repository<ClaimStatusBatchClaim>().Entities
                                                .Specify(new UnmatchedReimbursementsSpecification(_clientId))
                                                .GroupBy(x => x.ProcedureCode)
                                                .Select(group => group.OrderByDescending(e => e.CreatedOn).First())
                                                .ToListAsync();

                var paginatedData = latestClaimIdsPerProcedureCode
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(e => new GetAllUnmatchedReimbursementResponse
                    {
                        Id = e.Id,
                        ClientId = e.ClientId,
                        ProcedureCode = e.ProcedureCode,
                        TotalAllowedAmount = e.ClaimStatusTransaction.TotalAllowedAmount ?? 0.0m,
                        ClientFeeScheduleEntryId = e.ClientFeeScheduleEntryId ?? 0,
                        PayerName = e.ClientInsurance.Name,
                        FeeScheduleName = e.ClientFeeScheduleEntry.ClientFeeSchedule.Name,
                        Fee = e.ClientFeeScheduleEntry.Fee,
                        ClientFeeScheduleId = e.ClientFeeScheduleEntry.ClientFeeScheduleId
                    })
                    .ToList();

                return new PaginatedResult<GetAllUnmatchedReimbursementResponse>(paginatedData)
                {
                    TotalCount = latestClaimIdsPerProcedureCode.Count(),
                    CurrentPage = request.PageNumber,
                    PageSize = request.PageSize
                };


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
