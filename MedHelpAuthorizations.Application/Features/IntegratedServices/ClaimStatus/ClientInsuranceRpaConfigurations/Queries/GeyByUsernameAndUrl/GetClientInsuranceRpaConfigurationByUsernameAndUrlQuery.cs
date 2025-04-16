using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByUserrnameAndUrl
{
    public class GetClientInsuranceRpaConfigurationsByUsernameAndUrlQuery : IRequest<Result<List<GetClientInsuranceRpaConfigurationsByUsernameAndUrlResponse>>>
    {
        public string Username { get; set; }
        public string TargetUrl { get; set; }
        public TransactionTypeEnum TransactionTypeId { get; set; }

        public class GetClientInsuranceRpaConfigurationByUsernameAndUrlQueryHandler : IRequestHandler<GetClientInsuranceRpaConfigurationsByUsernameAndUrlQuery, Result<List<GetClientInsuranceRpaConfigurationsByUsernameAndUrlResponse>>>
        {
            private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;
            private readonly IMapper _mapper;

            public GetClientInsuranceRpaConfigurationByUsernameAndUrlQueryHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, IMapper mapper)
            {
                _clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository;
                _mapper = mapper;
            }

            public async Task<Result<List<GetClientInsuranceRpaConfigurationsByUsernameAndUrlResponse>>> Handle(GetClientInsuranceRpaConfigurationsByUsernameAndUrlQuery query, CancellationToken cancellationToken)
            {
                var clientInsuranceRpaConfiguration = await _clientInsuranceRpaConfigurationRepository.GetByUsernameAndUrlAsync(query.Username, query.TargetUrl, query.TransactionTypeId);
                var mappedClientInsuranceRpaConfiguration = _mapper.Map<List<GetClientInsuranceRpaConfigurationsByUsernameAndUrlResponse>>(clientInsuranceRpaConfiguration);

                return await Result<List<GetClientInsuranceRpaConfigurationsByUsernameAndUrlResponse>>.SuccessAsync(mappedClientInsuranceRpaConfiguration);
            }
        }
    }
}
