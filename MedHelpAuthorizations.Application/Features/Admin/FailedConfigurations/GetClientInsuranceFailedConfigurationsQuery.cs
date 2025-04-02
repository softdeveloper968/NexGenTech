using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetFailed;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.FailedConfigurations
{
    public class GetClientInsuranceFailedConfigurationsQuery : IRequest<Result<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>>
    {
        public int TenantId { get; set; }
        public int ClientId { get; set; }
    }
    public class GetClientInsuranceFailedConfigurationsHandler : IRequestHandler<GetClientInsuranceFailedConfigurationsQuery, Result<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly IMapper _mapper;

        public GetClientInsuranceFailedConfigurationsHandler(ITenantRepositoryFactory tenantRepositoryFactory, IMapper mapper)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _mapper = mapper;
        }
        public async Task<Result<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>> Handle(GetClientInsuranceFailedConfigurationsQuery request, CancellationToken cancellationToken)
        {
            if (request.TenantId > 0 && request.ClientId > 0)
            {
                var clientInsuranceRpaConfigurationRepository = _tenantRepositoryFactory.Get<IClientInsuranceRpaConfigurationRepository>(request.TenantId);
                var clientInsuranceRpaConfigurationList = await clientInsuranceRpaConfigurationRepository.GetExpiryWarningOrFailedClientInsuranceRpaConfigByClientIdAsync(request.ClientId);
                var mappedClientInsuranceRpaConfigurations = _mapper.Map<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>(clientInsuranceRpaConfigurationList);
                return await Result<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>.SuccessAsync(mappedClientInsuranceRpaConfigurations);
            }

            else if (request.TenantId > 0)
            {
                var clientInsuranceRpaConfigurationRepository = _tenantRepositoryFactory.Get<IClientInsuranceRpaConfigurationRepository>(request.TenantId);
                var clientInsuranceRpaConfigurationList = await clientInsuranceRpaConfigurationRepository.GetFailedClientInsuranceRpaConfigurationsAsync();
                var mappedClientInsuranceRpaConfigurations = _mapper.Map<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>(clientInsuranceRpaConfigurationList);
                return await Result<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>.SuccessAsync(mappedClientInsuranceRpaConfigurations);
            }

            return null;
        }
    }
}
