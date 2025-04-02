using AutoMapper;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.Base;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ClaimStatus;

namespace MedHelpAuthorizations.Application.Mappings
{
    public class ClaimStatusDashboardProfile : Profile
    {
        public ClaimStatusDashboardProfile()
        {
            CreateMap<ClaimStatusDashboardDetailsQuery, ExportClaimStatusDetailsQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardDetailsQuery, ExportClaimStatusDenialDetailsQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardDenialsDetailQuery, ExportClaimStatusDenialDetailsQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardDetailsQuery, ExportClaimStatusInProcessDetailsQuery>().ReverseMap();

            CreateMap<ClaimStatusDashboardDetailsQuery, ExportInitialClaimStatusDetailsQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardDetailsQuery, ExportInitialClaimStatusInProcessDetailsQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardDenialsDetailQuery, ExportInitialClaimStatusDenialDetailsQuery>().ReverseMap();
            CreateMap<GetClaimStatusBatchByIdResponse, GetClaimStatusBatchesBaseResponse>();
            
            CreateMap<ClaimStatusDashboardDetailsResponse, ExportQueryResponse>();
            CreateMap<ClaimStatusDashboardDetailsQuery, FinicalSummaryExportDetailQuery>().ReverseMap();
            CreateMap<ClaimStatusDashboardDetailsQuery, ExportCustomPaymentAndProcedureCodeQuery>().ReverseMap();
        }
    }
}
