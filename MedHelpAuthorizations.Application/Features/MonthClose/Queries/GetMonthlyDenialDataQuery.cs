using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.MonthClose.Queries
{
    public class GetMonthlyDenialDataQuery : IMonthCloseDashboardQuery, IRequest<Result<List<MonthlyDenialData>>>
    {
        public int ClientId { get; set; }
        public string ClientLocationId { get; set; }
        public string ClientProviderId { get; set; }
        public string ClientInsuranceId { get; set; }
        public string CptCodeId { get; set; }
    }
    public class GetMonthlyDenialDataQueryHandler : IRequestHandler<GetMonthlyDenialDataQuery, Result<List<MonthlyDenialData>>>
    {
        private readonly IMapper _mapper;
        private readonly IMonthCloseQueryService _monthCloseQueryService;

        public GetMonthlyDenialDataQueryHandler(IMapper mapper, IMonthCloseQueryService monthCloseQueryService)
        {
            _mapper = mapper;
            _monthCloseQueryService = monthCloseQueryService;
        }

        public async Task<Result<List<MonthlyDenialData>>> Handle(GetMonthlyDenialDataQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _monthCloseQueryService.GetMonthlyDenialDataAsync(query);
                return await Result<List<MonthlyDenialData>>.SuccessAsync(result.ToList());
            }
            catch (Exception ex)
            {
                // Handle the exception or log it here
                throw new InvalidOperationException("Error while retrieving monthly denial data", ex);
            }
        }
    }

}
