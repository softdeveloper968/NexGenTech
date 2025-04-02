using AutoMapper;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.CreateAddress;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Persons.Commands.CreatePerson
{
    public class CreatePersonCommand : PersonCommand
    { 
    }
    public class CreatePersonCommandHandler : PersonCommandHandler<CreatePersonCommand>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateAddressCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public CreatePersonCommandHandler(IPersonRepository personRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IStringLocalizer<CreateAddressCommandHandler> localizer)
        {
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public override async Task<Result<int>> Handle(CreatePersonCommand createRequest, CancellationToken cancellationToken)
        {
            //var person = _mapper.Map<Person>(createRequest);
            //await _personRepository.InsertAsync(person);
            var person = _mapper.Map<Person>(createRequest);
            person.ClientId = _clientId;
            
            await _unitOfWork.Repository<Person>().AddAsync(person);
            await _unitOfWork.Commit(cancellationToken);
            
            return await Result<int>.SuccessAsync(person.Id, _localizer["Person Created"]);
        }
    }
}
