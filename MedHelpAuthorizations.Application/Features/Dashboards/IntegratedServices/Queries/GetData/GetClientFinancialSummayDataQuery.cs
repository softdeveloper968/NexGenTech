using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetClientFinancialSummayDataQuery : ICorporateDashboardQueryBase, IRequest<Result<List<FinancialSummaryData>>>
    {
        public string TenantClientString { get; set; }
    }
    public class GetClientFinancialSummayDataQueryHandler : IRequestHandler<GetClientFinancialSummayDataQuery, Result<List<FinancialSummaryData>>>
    {
        private readonly ICorporateDashboardService _corporateService;
        public GetClientFinancialSummayDataQueryHandler(ICorporateDashboardService corporateService)
        {
            _corporateService = corporateService;
        }

        public async Task<Result<List<FinancialSummaryData>>> Handle(GetClientFinancialSummayDataQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _corporateService.GetClientFinancialSummaryDataAsync(request) ?? new ClaimSummary();
                var financialSummaryMapped = new List<FinancialSummaryData>()
                    {
                        new FinancialSummaryData()
                        {
                            Name = StoredProcedureColumnsHelper.ArBeginning,
                            Totals = 0.00m,
                            Visits = 0,
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
                            Totals = 0.00m,
                            Visits = 0,
                            Order = 5
                        },
                    };


                // Return a successful result with the response data.
                return await Result<List<FinancialSummaryData>>.SuccessAsync(financialSummaryMapped ?? new List<FinancialSummaryData>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}
