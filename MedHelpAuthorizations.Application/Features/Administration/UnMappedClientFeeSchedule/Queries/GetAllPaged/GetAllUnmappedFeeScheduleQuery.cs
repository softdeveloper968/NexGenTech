using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.UnMappedClientFeeSchedule.Queries.GetAllPaged
{
    public class GetAllUnmappedFeeScheduleQuery : IRequest<PaginatedResult<GetAllUnmappedFeeScheduleResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; } = string.Empty;

        public GetAllUnmappedFeeScheduleQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetAllUnmappedFeeScheduleQueryHandler : IRequestHandler<GetAllUnmappedFeeScheduleQuery, PaginatedResult<GetAllUnmappedFeeScheduleResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IClientFeeScheduleService _clientFeeScheduleService;
        private readonly IMapper _mapper;
        private int _clientId => _currentUserService.ClientId;

        public GetAllUnmappedFeeScheduleQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService, IClientFeeScheduleService clientFeeScheduleService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _clientFeeScheduleService = clientFeeScheduleService;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllUnmappedFeeScheduleResponse>> Handle(GetAllUnmappedFeeScheduleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Fetch the data asynchronously
                var query = _unitOfWork.Repository<UnmappedFeeScheduleCpt>().Entities
                   .Include(x => x.ClientInsurance)
                   .Include(x => x.ClientCptCode)
                   .Specify(new GenericByClientIdSpecification<UnmappedFeeScheduleCpt>(_clientId))
                   .Specify(new SearchUnmappedFeeScheduleCptSpecification(request.SearchString));

                var paginatedList = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);

                // Map results and calculate LastReimbursement asynchronously outside the query
                var resultList = new List<GetAllUnmappedFeeScheduleResponse>();
                foreach (var entity in paginatedList.Data)
                {
                    var mappedResult = _mapper.Map<GetAllUnmappedFeeScheduleResponse>(entity);
                    mappedResult.LastReimbursement = await _clientFeeScheduleService.GetLatestPaidAmountForPayerCptDos(
                        entity.ClientInsuranceId, entity.ClientCptCodeId, entity.ClientId, entity.ReferencedDateOfServiceFrom);

                    resultList.Add(mappedResult);
                }

                // Return a paginated result with the new data
                return new PaginatedResult<GetAllUnmappedFeeScheduleResponse>(resultList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
