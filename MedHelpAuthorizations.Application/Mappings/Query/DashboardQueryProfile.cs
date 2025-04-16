using AutoMapper;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Shared.Models;

namespace MedHelpAuthorizations.Application.Mappings.Query
{
    public class DashboardQueryProfile : Profile
    {
        public DashboardQueryProfile()
        {
            CreateMap<ClaimStatusDashboardQuery, ClaimStatusTotalsDateWiseQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardQuery, DenialsByInsuranceDateWiseQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardQuery, ClaimInProcessDateWiseQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardQuery, AvgAllowedAmtDateWiseQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardQuery, GetFinancialSummaryDataQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardQuery, GetClaimsSummaryDataQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardQuery, GetAverageDaysToPayByPayerQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardQuery, ChargesByPayerQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardQuery, DenialMonthlyComparisonQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardQuery, GetAverageDaysToPayByProviderQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardQuery, ChargesByProviderQuery>().ReverseMap();

            CreateMap<ClaimStatusDashboardQuery, ReimbursementByLocationQuery>().ReverseMap(); //EN-229
            CreateMap<ClaimStatusDashboardQuery, ReimbursementByProviderQuery>().ReverseMap(); //EN-229
            CreateMap<ClaimStatusDashboardQuery, GetProceduresByProviderQuery>().ReverseMap(); //EN-241
            CreateMap<ClaimStatusDashboardQuery, GetInsurancesByProviderQuery>().ReverseMap(); //EN-250
            CreateMap<ClaimStatusDashboardQuery, GetDenialReasonsByProviderQuery>().ReverseMap(); //EN-252
            CreateMap<ClaimStatusDashboardQuery, GetProcedureReimbursementByProviderQuery>().ReverseMap(); //EN-254
            CreateMap<ClaimStatusDashboardQuery, GetPayerReimbursementByProviderQuery>().ReverseMap(); //EN-257
            CreateMap<ClaimStatusDashboardQuery, GetDenialsByProcedureQuery>().ReverseMap(); //EN-289
            CreateMap<ClaimStatusDashboardQuery, GetDenialReasonsByInsuranceQuery>().ReverseMap(); //EN-289

            //ClientInsurance Dashboard
            CreateMap<ClaimStatusDashboardQuery, GetProviderTotalsByPayerQuery>().ReverseMap(); //EN-278
            CreateMap<ClaimStatusDashboardQuery, GetPaymentsByInsuranceQuery>().ReverseMap(); //EN-278
            CreateMap<ClaimStatusDashboardQuery, GetDenialsByInsuranceQuery>().ReverseMap(); //EN-278

            //Initial claim status dashbaord
            CreateMap<GetClaimsSummaryDataQuery, GetInitialClaimSummaryDataQuery>().ReverseMap(); //EN-295
            CreateMap<DenialsByInsuranceDateWiseQuery, GetInitialDenialsByInsuranceQuery>().ReverseMap(); //EN-295
            CreateMap<GetInitialInProcessClaimsQuery, ClaimInProcessDateWiseQuery>().ReverseMap(); //EN-295

            //Location Dashboard
            CreateMap<ClaimStatusDashboardQuery, GetProcedureTotalsByLocationQuery>().ReverseMap(); //EN-312
            CreateMap<ClaimStatusDashboardQuery, GetInsuranceTotalsByLocationQuery>().ReverseMap(); //EN-312
            CreateMap<ClaimStatusDashboardQuery, GetDenialReasonsByLocationsQuery>().ReverseMap(); //EN-312
            CreateMap<ClaimStatusDashboardQuery, GetProcedureReimbursementByLocationQuery>().ReverseMap(); //EN-312
            CreateMap<ClaimStatusDashboardQuery, GetPayerReimbursementByLocationQuery>().ReverseMap(); //EN-312
            CreateMap<ClaimStatusDashboardQuery, GetAverageDaysToPayByLocationQuery>().ReverseMap(); //EN-312
            CreateMap<ClaimStatusDashboardQuery, GetChargesByLocationQuery>().ReverseMap(); //EN-312

            //Provider Level Dashboard
            CreateMap<ClaimStatusDashboardQuery, GetProviderByProcedureQuery>().ReverseMap(); //EN-334
            CreateMap<ClaimStatusDashboardQuery, GetInsuranceByProcedureCodeQuery>().ReverseMap(); //EN-334
            CreateMap<ClaimStatusDashboardQuery, GetDenialReasonsByProcedureCodeQuery>().ReverseMap(); //EN-334
            CreateMap<ClaimStatusDashboardQuery, GetPayerReimbursementByProcedureCodeQuery>().ReverseMap(); //EN-334
            CreateMap<ClaimStatusDashboardQuery, GetProviderReimbursementByProcedureCodeQuery>().ReverseMap(); //EN-334
            CreateMap<ClaimStatusDashboardQuery, ReimbursementByProcedureCodeQuery>().ReverseMap(); //EN-334
            CreateMap<ClaimStatusDashboardQuery, ChargesByProcedureCodeQuery>().ReverseMap(); //EN-334
            CreateMap<ClaimStatusDashboardQuery, GetAverageDaysToPayByProcedureCodeQuery>().ReverseMap(); //EN-334
                                                                                                          
            CreateMap<ClaimStatusDashboardQuery, GetPayerTotalsQuery>().ReverseMap();
            CreateMap<GetPayerTotalsQuery, GetProviderTotalsByPayerQuery>().ReverseMap();
            CreateMap<GetProviderByProcedureQuery, GetProviderTotalsByProcedureCodeQuery>().ReverseMap();
            CreateMap<GetProviderProcedureTotalQuery, GetProceduresByProviderQuery>().ReverseMap();
            CreateMap<GetDenialReasonTotalsByProviderIdQuery, GetDenialReasonsByProviderQuery>().ReverseMap();
            CreateMap<GetDenialReasonTotalsByProviderIdQuery, ClaimStatusDashboardQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardDetailsQuery, ExportClaimStatusReportQuery>().ReverseMap();
            CreateMap<ExportClaimStatusDenialsReportQuery, ClaimStatusDashboardDetailsQuery>().ReverseMap();//EN-584
            CreateMap<ClaimStatusDashboardQuery , PageStateSnapshot>().ReverseMap();
        }
    }
}
