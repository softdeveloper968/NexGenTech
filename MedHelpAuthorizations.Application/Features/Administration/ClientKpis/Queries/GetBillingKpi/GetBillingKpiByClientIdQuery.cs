using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Queries.GetClientKpiByClientId;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Queries.GetBillingKpi
{
    public class GetBillingKpiByClientIdQuery : IRequest<Result<GetBillingKpiByClientIdResponse>>
    {
        public DateTime? ClaimBilledFrom { get; set; }
        public DateTime? ClaimBilledTo { get; set; }
    }

    public class GetBillingKpiByClientIdQueryHandler : IRequestHandler<GetBillingKpiByClientIdQuery, Result<GetBillingKpiByClientIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IKpiDashboardService _kpiDashboardService;

        public GetBillingKpiByClientIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IKpiDashboardService kpiDashboardService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _kpiDashboardService = kpiDashboardService;
        }

        public async Task<Result<GetBillingKpiByClientIdResponse>> Handle(GetBillingKpiByClientIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var billingKpiData = await _kpiDashboardService.GetBillingKpiByClientIdAsync(request);

                return await Result<GetBillingKpiByClientIdResponse>.SuccessAsync(billingKpiData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
