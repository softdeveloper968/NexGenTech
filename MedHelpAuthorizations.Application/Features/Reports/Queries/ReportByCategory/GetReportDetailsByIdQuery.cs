using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
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
    public class GetReportDetailsByIdQuery : IRequest<string>
    {
    }
    public class GetReportDetailsByIdQueryHandler : IRequestHandler<GetReportDetailsByIdQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IReportService _reportQueryService;
        private readonly IStringLocalizer<GetReportDetailsByIdQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetReportDetailsByIdQueryHandler(IExcelService excelService, IReportService queryService, IStringLocalizer<GetReportDetailsByIdQueryHandler> localizer, IMapper mapper, IMediator mediator)
        {
            _excelService = excelService;
            _reportQueryService = queryService;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }

        public Task<string> Handle(GetReportDetailsByIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
