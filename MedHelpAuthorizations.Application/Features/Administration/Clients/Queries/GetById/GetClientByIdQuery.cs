using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetById
{
    public class GetClientByIdQuery : IRequest<Result<GetClientByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, Result<GetClientByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        private int _clientId => _currentUserService.ClientId;

        public GetClientByIdQueryHandler(IUnitOfWork<int> unitOfWork, IClientRepository clientRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _clientRepository = clientRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<GetClientByIdResponse>> Handle(GetClientByIdQuery query, CancellationToken cancellationToken)
        {
            //TODO: Turn into an SP --- Done in EN-704
            try
            {
                if(query.Id == 0)
                {
                    query.Id = _clientId;
                }

                var client = await _clientRepository.GetClientById(query.Id);

                return await Result<GetClientByIdResponse>.SuccessAsync(data: client);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}