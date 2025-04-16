using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class GetCustomClientReportFilterDetailsByReportIdQuery : IRequest<Result<ClientCustomReportFilterDetails>>
    {
        public int ReportId { get; set; }
    }
    public class GetCustomClientReportFilterDetailsByReportIdQueryHandler : IRequestHandler<GetCustomClientReportFilterDetailsByReportIdQuery, Result<ClientCustomReportFilterDetails>>
    {
        private readonly IExcelService _excelService;
        private readonly IClientReportFilterService _reportQueryService;
        private readonly IStringLocalizer<GetCustomClientReportFilterDetailsByReportIdQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetCustomClientReportFilterDetailsByReportIdQueryHandler(IExcelService excelService, IClientReportFilterService queryService, IStringLocalizer<GetCustomClientReportFilterDetailsByReportIdQueryHandler> localizer, IMapper mapper, IMediator mediator)
        {
            _excelService = excelService;
            _reportQueryService = queryService;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Result<ClientCustomReportFilterDetails>> Handle(GetCustomClientReportFilterDetailsByReportIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _reportQueryService.GetCustomReportClientReportFiltersByReportId(request.ReportId);
                return await Result<ClientCustomReportFilterDetails>.SuccessAsync(result);
            }
            catch (Exception e)
            {
                return await Result<ClientCustomReportFilterDetails>.FailAsync(_localizer[$"Exception in ClientReportFilterDetailsQueryHandle : {Environment.NewLine} {e.Message}"]);
                throw;
            }
        }
    }
}
