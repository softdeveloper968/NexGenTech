using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ARAgingReport;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
using MediatR;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query
{
    public class GetSavedCustomReportQuery : IRequest<string>
    {
    }
    public class GetSavedCustomReportQueryHandler : IRequestHandler<GetSavedCustomReportQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IARAgingReportQueryService _reportQueryService;
        private readonly IStringLocalizer<GetSavedCustomReportQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetSavedCustomReportQueryHandler(IExcelService excelService, IARAgingReportQueryService queryService, IStringLocalizer<GetSavedCustomReportQueryHandler> localizer, IMapper mapper, IMediator mediator)
        {
            _excelService = excelService;
            _reportQueryService = queryService;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<string> Handle(GetSavedCustomReportQuery request, CancellationToken cancellationToken)
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
