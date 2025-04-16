using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Queries.GetById
{
    public class GetClientLocationInsuranceIdentifierByIdQuery : IRequest<Result<GetClientLocationInsuranceIdentifierByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetClientLocationInsuranceIdentifierByIdQueryHandler : IRequestHandler<GetClientLocationInsuranceIdentifierByIdQuery, Result<GetClientLocationInsuranceIdentifierByIdResponse>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private int _clientId => _currentUserService.ClientId;

        public GetClientLocationInsuranceIdentifierByIdQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<GetClientLocationInsuranceIdentifierByIdResponse>> Handle(GetClientLocationInsuranceIdentifierByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var locationinsuranceidentifier = await _unitOfWork.Repository<Domain.Entities.ClientLocationInsuranceIdentifier>().Entities
                          .Include(x => x.ClientLocation)
                          .Include(x => x.ClientInsurance)
                          .Specify(new GenericByClientIdSpecification<Domain.Entities.ClientLocationInsuranceIdentifier>(_clientId))
                            .FirstOrDefaultAsync(x => x.Id == query.Id);

                var mapped = _mapper.Map<GetClientLocationInsuranceIdentifierByIdResponse>(locationinsuranceidentifier);
                return await Result<GetClientLocationInsuranceIdentifierByIdResponse>.SuccessAsync(mapped);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
