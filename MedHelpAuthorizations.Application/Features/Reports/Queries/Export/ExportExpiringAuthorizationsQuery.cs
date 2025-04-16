using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using AutoMapper;
using MedHelpAuthorizations.Application.Features.Reports.Queries.GetExpiringAuthorizations;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using System.Linq;

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.Export
{
    public class ExportExpiringAuthorizationsQuery : IRequest<string>
    {
        public int ClientId { get; set; }
    }

    public class ExportExpiringAuthorizationsQueryHandler : IRequestHandler<ExportExpiringAuthorizationsQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IStringLocalizer<ExportExpiringAuthorizationsQueryHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        private int _clientId => _currentUserService.ClientId;

        public ExportExpiringAuthorizationsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IAuthorizationRepository authorizationRepository
            , IStringLocalizer<ExportExpiringAuthorizationsQueryHandler> localizer
            , IMapper mapper
            , ICurrentUserService currentUserService)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _authorizationRepository = authorizationRepository;
            _localizer = localizer;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<string> Handle(ExportExpiringAuthorizationsQuery request, CancellationToken cancellationToken)
        {
            request.ClientId = _clientId;
            var expiringQuery = _mapper.Map<GetPagedExpiringAuthorizationsQuery>(request);

            var queryable = await _authorizationRepository.GetExpiringWithProperNameQueryable(expiringQuery);

            List<GetAllPagedAuthorizationsResponse> expiringData = await queryable.ToListAsync();
            var exportQueryResponse = expiringData.Select(x => new ExportQueryResponse
            {
                PagedAuthorizationId = x.Id,
                AuthNumber = x.AuthNumber,
                AuthorizationStatusId = x.AuthorizationStatusId,
                AuthTypeId = x.AuthTypeId,
                AuthTypeName = x.AuthTypeName,
                ClientId = x.ClientId,
                CompleteDate = x.CompleteDate,
                CreatedOn = x.CreatedOn,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                DischargedOn = x.DischargedOn,
                CreateUserId = x.CreateUserId,
                PatientName = x.PatientName,
                PatientAccountNumber = x.AccountNumber,
                PatientDateOfBirth = x.PatientDateOfBirth,
                PatientId = x.PatientId,
                Units = x.Units,
                Documents = x.Documents,
                NeededDocumentTypes = x.NeededDocumentTypes
            });

            var data = await _excelService.ExportAsync(exportQueryResponse, mappers: new Dictionary<string, Func<ExportQueryResponse, object>>()
            {
                { _localizer["Id"], item => item.PagedAuthorizationId },
                { _localizer["AuthType Name"], item => item.AuthTypeName },
                { _localizer["Patient Name"], item => item.PatientName },
                { _localizer["Patient Date Of Birth"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.PatientDateOfBirth?.ToShortDateString())},
                { _localizer["Created On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.CreatedOn.ToShortDateString() )},
                { _localizer["Created By"], item => item.CreateUserId },
                { _localizer["CompleteDate"], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.CompleteDate?.ToShortDateString())},
                { _localizer["Complete By"], item => item.Completeby },
                { _localizer["Auth Number"], item => item.AuthNumber },
                { _localizer["Units"], item => item.Units },
                { _localizer["Start Date"], item =>  _excelService.AddTypePrefix(ExportHelper.DateType, item.StartDate?.ToShortDateString() )},
                { _localizer["End Date"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.EndDate ?.ToShortDateString()) },
                { _localizer["Discharged On"], item => _excelService.AddTypePrefix(ExportHelper.DateType, item.DischargedOn ?.ToShortDateString()) },
                { _localizer["DischargedBy"], item => item.DischargedBy },
                { _localizer["Has Documents"], item => item.HasDocuments },
                { _localizer["Needs Documents"], item => item.NeedsDocuments },
                { _localizer["Authorization Status"], item => item.AuthorizationStatusId.ToString() },

            }, sheetName: _localizer["ExpiringAuthorizations"]);

            return data;
        }
    }
}