using AutoMapper;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
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
    public class GetClientReportFilterDetailsByClientIdQuery : IRequest<Result<List<GetClientReportFilterResponse>>>
    {
        public int? ClientId { get; set; } = null;
    }
    public class ClientReportFilterDetailsQueryHandler : IRequestHandler<GetClientReportFilterDetailsByClientIdQuery, Result<List<GetClientReportFilterResponse>>>
    {
        private readonly IExcelService _excelService;
        private readonly IClientReportFilterService _reportQueryService;
        private readonly IStringLocalizer<ClientReportFilterDetailsQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public ClientReportFilterDetailsQueryHandler(IExcelService excelService, IClientReportFilterService queryService, IStringLocalizer<ClientReportFilterDetailsQueryHandler> localizer, IMapper mapper, ICurrentUserService userService)
        {
            _excelService = excelService;
            _reportQueryService = queryService;
            _localizer = localizer;
            _mapper = mapper;
            _currentUserService = userService;
        }

        public async Task<Result<List<GetClientReportFilterResponse>>> Handle(GetClientReportFilterDetailsByClientIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ClientId is null)
                {
                    request.ClientId = _currentUserService.ClientId;
                }
                var result = await _reportQueryService.GetClientReportFilterDetailsByClientId((int)request.ClientId);
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
