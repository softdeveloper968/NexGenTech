using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.MonthClose.Queries
{
    public class GetMonthlyReceivablesDataQuery : IMonthCloseDashboardQuery, IRequest<Result<List<MonthlyReceivablesData>>>
    {
        public int ClientId { get; set; }
        public string ClientLocationId { get; set; }
        public string ClientProviderId { get; set; }
        public string ClientInsuranceId { get; set; }
        public string CptCodeId { get; set; }
    }
    public class GetMonthlyReceivablesDataQueryHandler : IRequestHandler<GetMonthlyReceivablesDataQuery, Result<List<MonthlyReceivablesData>>>
    {
        private readonly IMapper _mapper;
        private readonly IMonthCloseQueryService _monthCloseQueryService;

        public GetMonthlyReceivablesDataQueryHandler(IMapper mapper, IMonthCloseQueryService monthCloseQueryService)
        {
            _mapper = mapper;
            _monthCloseQueryService = monthCloseQueryService;
        }

        public async Task<Result<List<MonthlyReceivablesData>>> Handle(GetMonthlyReceivablesDataQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _monthCloseQueryService.GetMonthlyReceivablesDataAsync(query);
                return await Result<List<MonthlyReceivablesData>>.SuccessAsync(result.ToList());
            }
            catch (Exception ex)
            {
                // Handle the exception or log it here
                throw new InvalidOperationException("Error while retrieving monthly receivables data", ex);
            }
        }

    }

}
