using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query
{
    public class GetSavedCustomReportQueryByReportTypeQuery : IRequest<string>
    {
    }
    public class GetSavedCustomReportQueryByReportTypeQueryHandler : IRequestHandler<GetSavedCustomReportQueryByReportTypeQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IARAgingReportQueryService _reportQueryService;
        private readonly IStringLocalizer<GetSavedCustomReportQueryByReportTypeQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetSavedCustomReportQueryByReportTypeQueryHandler(IExcelService excelService, IARAgingReportQueryService queryService, IStringLocalizer<GetSavedCustomReportQueryByReportTypeQueryHandler> localizer, IMapper mapper, IMediator mediator)
        {
            _excelService = excelService;
            _reportQueryService = queryService;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<string> Handle(GetSavedCustomReportQueryByReportTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
