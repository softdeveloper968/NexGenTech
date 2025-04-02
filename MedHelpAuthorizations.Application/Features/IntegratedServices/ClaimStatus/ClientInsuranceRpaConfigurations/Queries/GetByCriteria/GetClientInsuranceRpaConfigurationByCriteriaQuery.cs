using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByCriteria
{
    public class GetSingleClientInsuranceRpaConfigurationByCriteriaQuery : IRequest<Result<GetClientInsuranceRpaConfigurationByCriteriaResponse>>
    {
        public int ClientId { get; set; }
        public int RpaInsuranceId { get; set; }
        public TransactionTypeEnum TransactionTypeId { get; set; }
        public int? AuthTypeId { get; set; }
        public int? ClientLocationId {  get; set; }
            
        public class GetClientInsuranceRpaConfigurationByIdQueryHandler : IRequestHandler<GetSingleClientInsuranceRpaConfigurationByCriteriaQuery, Result<GetClientInsuranceRpaConfigurationByCriteriaResponse>>
        {
            private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;
            private readonly IMapper _mapper;

            public GetClientInsuranceRpaConfigurationByIdQueryHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, IMapper mapper)
            {
                _clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository;
                _mapper = mapper;
            }

            public async Task<Result<GetClientInsuranceRpaConfigurationByCriteriaResponse>> Handle(GetSingleClientInsuranceRpaConfigurationByCriteriaQuery query, CancellationToken cancellationToken)
            {
                var clientInsuranceRpaConfiguration = await _clientInsuranceRpaConfigurationRepository.GetByCriteriaAsync(query.ClientId, query.RpaInsuranceId, query.TransactionTypeId, query.AuthTypeId, query.ClientLocationId);
                var mappedClientInsuranceRpaConfiguration = _mapper.Map<GetClientInsuranceRpaConfigurationByCriteriaResponse>(clientInsuranceRpaConfiguration);
                return await Result<GetClientInsuranceRpaConfigurationByCriteriaResponse>.SuccessAsync(mappedClientInsuranceRpaConfiguration);
            }
        }
    }
}
