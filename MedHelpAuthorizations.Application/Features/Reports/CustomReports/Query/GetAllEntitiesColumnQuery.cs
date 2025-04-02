using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Helpers;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query
{

    public class GetAllFilterColumnsBasedOnReportTypeQuery : IRequest<Result<CustomReportTypeEntity>>
    {
        public int ClientId { get; set; } = 0;
        public CustomReportTypeEnum CustomReportType { get; set; }
    }
    public class GetAllFilterColumnsBasedOnReportTypeQueryHandler : IRequestHandler<GetAllFilterColumnsBasedOnReportTypeQuery, Result<CustomReportTypeEntity>>
    {
        private readonly ICustomReportService _customReportService;
        private readonly IStringLocalizer<GetAllFilterColumnsBasedOnReportTypeQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetAllFilterColumnsBasedOnReportTypeQueryHandler(ICustomReportService customReportService, IStringLocalizer<GetAllFilterColumnsBasedOnReportTypeQueryHandler> localizer, IMapper mapper, ICurrentUserService userService)
        {
            _customReportService = customReportService;
            _localizer = localizer;
            _mapper = mapper;
            _currentUserService = userService;
        }

        public async Task<Result<CustomReportTypeEntity>> Handle(GetAllFilterColumnsBasedOnReportTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ClientId == 0)
                {
                    request.ClientId = _currentUserService.ClientId;
                }
                CustomReportTypeEntity customAttributeEntityDetails = new();
                switch (request.CustomReportType)
                {
                    case CustomReportTypeEnum.Claim:
                        {
                            customAttributeEntityDetails = _customReportService.GetContextDetailForClaimReportType();
                            break;
                        }
                    default: break;
                }

                if (customAttributeEntityDetails is not null)
                {
                    var headerEntityName = customAttributeEntityDetails.MainEntityName;
                    List<CustomAttributeForEntitesDataItem> chooseColumnsDetails = new();
                    List<CustomReportSetFilterColumns> SetFilterColumnsDetails = new();
                    
                    _customReportService.GetChooseDisplayColumns(headerEntityName, customAttributeEntityDetails, out chooseColumnsDetails);
                    _customReportService.GetSetFilterDisplayColumns(headerEntityName, customAttributeEntityDetails,out SetFilterColumnsDetails);

                    if(chooseColumnsDetails.Any())
                    {
                        customAttributeEntityDetails.ChooseColumnsDetails = chooseColumnsDetails;
                    }
                    if (SetFilterColumnsDetails.Any())
                    {
                        customAttributeEntityDetails.SetFilterColumnsDetails = SetFilterColumnsDetails;
                    }
                }
                return await Result<CustomReportTypeEntity>.SuccessAsync(customAttributeEntityDetails);
            }
            catch (Exception e)
            {
                return await Result<CustomReportTypeEntity>.FailAsync(_localizer[$"Exception in GetAllFilterColumnsBasedOnReportTypeQueryHandler : {Environment.NewLine} {e.Message}"]);
                throw;
            }
        }

    }
}
