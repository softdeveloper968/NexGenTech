using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using Microsoft.Extensions.Localization;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.Extensions.Configuration;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.Data.SqlClient;
using System.Data;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Application.Features.Reports.ARAgingReport;
using static MudBlazor.CategoryTypes;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ARAgingReport;


namespace MedHelpAuthorizations.Infrastructure.Services
{
    public partial class ClaimStatusBatchClaimInstanceManager : IClaimStatusBatchClaimInstanceManager
    {
        private readonly ApplicationContext _context;
        private readonly IDbContextFactory<ApplicationContext> _contextFactory;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
        private readonly IStringLocalizer<ClaimStatusQueryService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private int _clientId => _currentUserService.ClientId;

        public ClaimStatusBatchClaimInstanceManager(
            ApplicationContext context,
            IDbContextFactory<ApplicationContext> contextFactory,
            IUnitOfWork<int> unitOfWork,
            IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository,
            IMapper mapper,
            IStringLocalizer<ClaimStatusQueryService> localizer,
            ICurrentUserService currentUserService,
            IConfiguration configuration
            )
        {
            _context = context;
            _contextFactory = contextFactory;
            _unitOfWork = unitOfWork;
            _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
            _mapper = mapper;
            _configuration = configuration;
            _localizer = localizer;
            this._currentUserService = currentUserService;
        }
        
    }
}