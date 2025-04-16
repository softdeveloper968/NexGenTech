using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByRpaInsurance
{
    public class GetClientInsuranceRpaConfigurationByRpaInsuranceQuery : IRequest<Result<List<GetClientInsuranceRpaConfigurationByRpaInsuranceResponse>>>
    {
        public int RpaInsuranceId { get; set; }
        public TransactionTypeEnum TransactionTypeId { get; set; }

        public class GetClientInsuranceRpaConfigurationByRpaInsuranceQueryHandler : IRequestHandler<GetClientInsuranceRpaConfigurationByRpaInsuranceQuery, Result<List<GetClientInsuranceRpaConfigurationByRpaInsuranceResponse>>>
        {
            private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;
            private readonly IMapper _mapper;

            public GetClientInsuranceRpaConfigurationByRpaInsuranceQueryHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, IMapper mapper)
            {
                _clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository;
                _mapper = mapper;
            }

            public async Task<Result<List<GetClientInsuranceRpaConfigurationByRpaInsuranceResponse>>> Handle(GetClientInsuranceRpaConfigurationByRpaInsuranceQuery query, CancellationToken cancellationToken)
            {
                var clientInsuranceRpaConfigurationList = await _clientInsuranceRpaConfigurationRepository.GetByRpaInsuranceIdAsync(query.RpaInsuranceId, query.TransactionTypeId);
                var mappedClientInsuranceRpaConfigurations = _mapper.Map<List<GetClientInsuranceRpaConfigurationByRpaInsuranceResponse>>(clientInsuranceRpaConfigurationList);
                return Result<List<GetClientInsuranceRpaConfigurationByRpaInsuranceResponse>>.Success(mappedClientInsuranceRpaConfigurations);
            }
        }
    }
}
