using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetByClientId;
using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.ReportByCategory
{
    public class GetAllReportsByCategoryDetailsQuery : IRequest<Result<List<GetAllReportsResponse>>>
    {
    }
    public class GetAllReportsByCategoryDetailsQueryHandler : IRequestHandler<GetAllReportsByCategoryDetailsQuery, Result<List<GetAllReportsResponse>>>
    {
        private readonly IReportService _reportQueryService;
        private readonly IStringLocalizer<GetAllReportsByCategoryDetailsQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetAllReportsByCategoryDetailsQueryHandler(IReportService queryService, IStringLocalizer<GetAllReportsByCategoryDetailsQueryHandler> localizer, IMapper mapper, IMediator mediator)
        {
            _reportQueryService = queryService;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Result<List<GetAllReportsResponse>>> Handle(GetAllReportsByCategoryDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _reportQueryService.GetAllReportsByCategory();
                return await Result<List<GetAllReportsResponse>>.SuccessAsync(result);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
