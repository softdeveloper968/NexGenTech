using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class GetClientReportFilterDetailsByReportIdAndFilterNameQuery : IRequest<Result<List<GetClientReportFilterResponse>>>
    {
        public int ReportId { get; set; }
        public string FilterName { get; set; }
    }
    public class GetClientReportFilterDetailsByReportIdAndFilterNameQueryHandler : IRequestHandler<GetClientReportFilterDetailsByReportIdAndFilterNameQuery, Result<List<GetClientReportFilterResponse>>>
    {
        private readonly IExcelService _excelService;
        private readonly IClientReportFilterService _reportQueryService;
        private readonly IStringLocalizer<GetClientReportFilterDetailsByReportIdAndFilterNameQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetClientReportFilterDetailsByReportIdAndFilterNameQueryHandler(IExcelService excelService, IClientReportFilterService queryService, IStringLocalizer<GetClientReportFilterDetailsByReportIdAndFilterNameQueryHandler> localizer, IMapper mapper, IMediator mediator)
        {
            _excelService = excelService;
            _reportQueryService = queryService;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Result<List<GetClientReportFilterResponse>>> Handle(GetClientReportFilterDetailsByReportIdAndFilterNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _reportQueryService.GetClientReportFiltersByReportIdAndFilterName(request.ReportId, request.FilterName);
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
