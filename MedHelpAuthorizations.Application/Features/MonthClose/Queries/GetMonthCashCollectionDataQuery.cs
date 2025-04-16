using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetAll;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.MonthClose.Queries
{
    public class GetMonthCashCollectionDataQuery : IMonthCloseDashboardQuery, IRequest<Result<List<MonthlyCashCollectionData>>>
    {
        public int ClientId { get; set; }
        public string ClientLocationId { get; set; }
        public string ClientProviderId { get; set; }
        public string ClientInsuranceId { get; set; }
        public string CptCodeId { get; set; }
    }

    public class GetMonthCashCollectionDataQueryHandler : IRequestHandler<GetMonthCashCollectionDataQuery, Result<List<MonthlyCashCollectionData>>>
    {
        private readonly IMapper _mapper;
        private readonly IMonthCloseQueryService _monthCloseQueryService;

        public GetMonthCashCollectionDataQueryHandler(IMapper mapper, IMonthCloseQueryService monthCloseQueryService)
        {
            _mapper = mapper;
            _monthCloseQueryService = monthCloseQueryService;
        }

        public async Task<Result<List<MonthlyCashCollectionData>>> Handle(GetMonthCashCollectionDataQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _monthCloseQueryService.GetMonthCashCollectionDataAsync(query);

                return await Result<List<MonthlyCashCollectionData>>.SuccessAsync(result.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

}
