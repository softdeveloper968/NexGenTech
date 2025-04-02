using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetAll
{
	public class GetPagedClientRpaInsuranceConfigurations : IRequest<PaginatedResult<GetAllClientInsuranceRpaConfigurationsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        //  public string SearchString { get; set; }
        public int ClientId { get; set; }

        public GetPagedClientRpaInsuranceConfigurations(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            // SearchString = searchString;
            // ClientId = clientId;
        }
    }

    public class GetPagedClientRpaInsuranceConfigurationsQueryHandler : IRequestHandler<GetPagedClientRpaInsuranceConfigurations, PaginatedResult<GetAllClientInsuranceRpaConfigurationsResponse>>
    {
        private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork; 
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetPagedClientRpaInsuranceConfigurationsQueryHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, IMapper mapper, IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllClientInsuranceRpaConfigurationsResponse>> Handle(GetPagedClientRpaInsuranceConfigurations request, CancellationToken cancellationToken)
        {
            try
            {
                //AA-23
                Expression<Func<ClientInsuranceRpaConfiguration, GetAllClientInsuranceRpaConfigurationsResponse>> expression = e => new GetAllClientInsuranceRpaConfigurationsResponse
                {
                    Id = e.Id,
                    ClientId = e.ClientInsurance.ClientId,
                    ClientInsuranceId = e.ClientInsuranceId,
                    RpaInsuranceGroupId = e.ClientInsurance.RpaInsurance.RpaInsuranceGroupId,
                    RpaInsuranceId = e.ClientInsurance.RpaInsurance.Id,
                    ClientRpaCredentialConfigId = e.ClientRpaCredentialConfigurationId,
					AlternateClientRpaCredentialConfigId = e.AlternateClientRpaCredentialConfigurationId,
					TransactionTypeId = e.TransactionTypeId,
                    AuthTypeName = e.AuthType.Description,
                    AuthTypeId = e.AuthTypeId,
                    Username = e.ClientRpaCredentialConfiguration != null ? e.ClientRpaCredentialConfiguration.Username : string.Empty,
                    Password = e.ClientRpaCredentialConfiguration != null ? e.ClientRpaCredentialConfiguration.Password : string.Empty,
					AlternateUsername = e.AlternateClientRpaCredentialConfiguration != null ? e.AlternateClientRpaCredentialConfiguration.Username : string.Empty,
					AlternatePassword = e.AlternateClientRpaCredentialConfiguration != null ? e.AlternateClientRpaCredentialConfiguration.Password : string.Empty,
					DefaultTargetUrl = e.ClientRpaCredentialConfiguration != null ? e.ClientRpaCredentialConfiguration.RpaInsuranceGroup != null ? e.ClientRpaCredentialConfiguration.RpaInsuranceGroup.DefaultTargetUrl : string.Empty : string.Empty,
                    TargetUrl = e.ClientInsurance.RpaInsurance != null ? e.ClientInsurance.RpaInsurance.TargetUrl ?? string.Empty : string.Empty,
                    IsCredentialInUse = e.ClientRpaCredentialConfiguration != null ? e.ClientRpaCredentialConfiguration.IsCredentialInUse : false,
                    UseOffHoursOnly = e.ClientRpaCredentialConfiguration != null ? e.ClientRpaCredentialConfiguration.UseOffHoursOnly : false,
                    ClientInsuranceName = e.ClientInsurance.Name,
                    ExternalId = e.ClientInsurance.ExternalId,
                    ExpiryWarningReported = e.ClientRpaCredentialConfiguration != null ? e.ClientRpaCredentialConfiguration.ExpiryWarningReported : false,
                    FailureReported = e.ClientRpaCredentialConfiguration != null ? e.ClientRpaCredentialConfiguration.FailureReported : false,
                    FailureMessage = e.ClientRpaCredentialConfiguration != null ? e.ClientRpaCredentialConfiguration.FailureMessage : string.Empty,
                    OtpForwardFromEmail = e.ClientRpaCredentialConfiguration != null ? e.ClientRpaCredentialConfiguration.OtpForwardFromEmail : string.Empty,
                    ClientLocationId = e.ClientLocationId,
                    ClientLocationName = e.ClientLocation.Name ?? string.Empty,
				};

                var data = await _unitOfWork.Repository<ClientInsuranceRpaConfiguration>()
                                    .Entities
                                    .Include(c => c.AuthType)
                                    .Include(c => c.ClientLocation)
                                    .Include(c => c.ClientRpaCredentialConfiguration)
                                    .Include(c => c.ClientInsurance)
                                        .ThenInclude(d => d.RpaInsurance)
                                        .ThenInclude(e => e.RpaInsuranceGroup)
                                    .Where(c => c.ClientInsurance.ClientId == _clientId && !c.IsDeleted)
                                    .Select(expression)
                                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);

                return data;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
