using AutoMapper;
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

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.ReportByCategory
{
    public class GetReportByCategoryIdQuery : IRequest<Result<List<GetAllReportsResponse>>>
    {
        public ReportCategoryEnum ReportCategoryId { get; set; }
    }
    public class GetReportByCategoryIdQueryHandler : IRequestHandler<GetReportByCategoryIdQuery, Result<List<GetAllReportsResponse>>>
    {
        private readonly IExcelService _excelService;
        private readonly IReportService _reportQueryService;
        private readonly IStringLocalizer<GetReportByCategoryIdQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetReportByCategoryIdQueryHandler(IExcelService excelService, IReportService queryService, IStringLocalizer<GetReportByCategoryIdQueryHandler> localizer, IMapper mapper, IMediator mediator)
        {
            _excelService = excelService;
            _reportQueryService = queryService;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Result<List<GetAllReportsResponse>>> Handle(GetReportByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _reportQueryService.GetAllReportsByCategoryId(request.ReportCategoryId);
                return await Result<List<GetAllReportsResponse>>.SuccessAsync(result);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
