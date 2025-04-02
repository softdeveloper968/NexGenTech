using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class GetClientReportFilterDetailsByReportIdQuery : IRequest<Result<List<GetClientReportFilterResponse>>>
    {
        public int ReportId { get; set; }
    }
    public class GetClientReportFilterDetailsByReportIdQueryHandler : IRequestHandler<GetClientReportFilterDetailsByReportIdQuery, Result<List<GetClientReportFilterResponse>>>
    {
        private readonly IExcelService _excelService;
        private readonly IClientReportFilterService _reportQueryService;
        private readonly IStringLocalizer<GetClientReportFilterDetailsByReportIdQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetClientReportFilterDetailsByReportIdQueryHandler(IExcelService excelService, IClientReportFilterService queryService, IStringLocalizer<GetClientReportFilterDetailsByReportIdQueryHandler> localizer, IMapper mapper, IMediator mediator)
        {
            _excelService = excelService;
            _reportQueryService = queryService;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Result<List<GetClientReportFilterResponse>>> Handle(GetClientReportFilterDetailsByReportIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _reportQueryService.GetClientReportFiltersByReportId(request.ReportId);
                return await Result<List<GetClientReportFilterResponse>>.SuccessAsync(result);
            }
            catch (Exception e)
            {
                return await Result<List<GetClientReportFilterResponse>>.FailAsync(_localizer[$"Exception in ClientReportFilterDetailsQueryHandle : {Environment.NewLine} {e.Message}"]);
                throw;
            }
        }
    }
}
