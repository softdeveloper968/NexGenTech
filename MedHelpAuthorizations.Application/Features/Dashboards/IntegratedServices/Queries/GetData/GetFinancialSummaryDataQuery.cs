using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetFinancialSummaryDataQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<FinancialSummaryData>>> //AA-330
    {
    }

    public class GetFinancialSummaryDataQueryHandler : IRequestHandler<GetFinancialSummaryDataQuery, Result<List<FinancialSummaryData>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetFinancialSummaryDataQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetFinancialSummaryDataQueryHandler(IStringLocalizer<GetFinancialSummaryDataQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<FinancialSummaryData>>> Handle(GetFinancialSummaryDataQuery query, CancellationToken cancellationToken)
        {
            // Retrieve claim status revenue totals using the claimStatusQueryService.
            var response = await _claimStatusQueryService.GetFinancialSummaryDataAsync(query) ?? new();

            

            var financialSummaryMapped = new List<FinancialSummaryData>()
                    {
                        new FinancialSummaryData()
                        {
                            Name = StoredProcedureColumnsHelper.ArBeginning,
                            Totals = response.ARBegining,
                            Visits = response.ARBeginingVisits,
                            Order = 0
                        },
                        new FinancialSummaryData()
                        {
                            Name = StoredProcedureColumnsHelper.Charged,
                            Totals = response.ChargedTotals,
                            Visits = response.ChargedVisits,
                            Order = 1
                        },
                        new FinancialSummaryData()
                        {
                            Name = StoredProcedureColumnsHelper.Payment,
                            Totals = response.PaymentTotals,
                            Visits = response.PaymentVisits,
                            Order = 2
                        },
                        new FinancialSummaryData()
                        {
                            Name = StoredProcedureColumnsHelper.Contractual,
                            Totals = response.ContractualTotals,
                            Visits = response.ContractualVisits,
                            Order = 3
                        },
                        new FinancialSummaryData()
                        {
                            Name = StoredProcedureColumnsHelper.WriteOff,
                            Totals = response.WriteOffAmountSum,
                            Visits = response.WriteOffVisits,
                            Order = 4
                        }
                        ,new FinancialSummaryData()
                        {
                            Name = StoredProcedureColumnsHelper.ArEnding,
                            Totals = response.AREnding,
                            Visits = response.AREndingVisits,
                            Order = 5
                        },
                    };


            // Return a successful result with the response data.
            return await Result<List<FinancialSummaryData>>.SuccessAsync(financialSummaryMapped ?? new List<FinancialSummaryData>());
        }
    }
}
