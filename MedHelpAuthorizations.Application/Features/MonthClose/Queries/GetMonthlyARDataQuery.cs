using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.MonthClose.Queries
{
    public class GetMonthlyARDataQuery : IMonthCloseDashboardQuery, IRequest<Result<List<MonthlyARData>>>
    {
        public int ClientId { get; set; }
        public string ClientLocationId { get; set; }
        public string ClientProviderId { get; set; }
        public string ClientInsuranceId { get; set; }
        public string CptCodeId { get; set; }
    }

    public class GetMonthlyARDataQueryHandler : IRequestHandler<GetMonthlyARDataQuery, Result<List<MonthlyARData>>>
    {
        private readonly IMapper _mapper;
        private readonly IMonthCloseQueryService _monthCloseQueryService;

        public GetMonthlyARDataQueryHandler(IMapper mapper, IMonthCloseQueryService monthCloseQueryService)
        {
            _mapper = mapper;
            _monthCloseQueryService = monthCloseQueryService;
        }

        public async Task<Result<List<MonthlyARData>>> Handle(GetMonthlyARDataQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _monthCloseQueryService.GetMonthlyARDataAsync(query);
                // var result = ItemList;
                return await Result<List<MonthlyARData>>.SuccessAsync(result.ToList());
            }
            catch (Exception ex)
            {
                // Handle the exception or log it here
                throw new InvalidOperationException("Error while retrieving monthly AR data", ex);
            }
        }
    }

}
