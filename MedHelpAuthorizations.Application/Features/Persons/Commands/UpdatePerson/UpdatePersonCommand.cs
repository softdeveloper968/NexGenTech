using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using System.Threading;
using AutoMapper;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Interfaces.Services;

namespace MedHelpAuthorizations.Application.Features.Persons.Commands.UpdatePerson
{
    public class UpdatePersonCommand : PersonCommand, IRequest<Result<int>>
    {       
        public class UpdatePersonCommandHandler : PersonCommandHandler<UpdatePersonCommand>
        {
            private readonly IPersonRepository _personRepository;
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IMapper _mapper;
            private readonly IStringLocalizer<UpdatePersonCommandHandler> _localizer;
            private readonly ICurrentUserService _currentUserService;
            private int _clientId => _currentUserService.ClientId;

            public UpdatePersonCommandHandler(IPersonRepository personRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IStringLocalizer<UpdatePersonCommandHandler> localizer)
            {
                _personRepository = personRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _currentUserService = currentUserService;
                _localizer = localizer;
            }

            public override async Task<Result<int>> Handle(UpdatePersonCommand updateRequest, CancellationToken cancellationToken)
            {
                //var person = await _personRepository.GetByIdAsync(updateRequest.PersonId);
                Person person;
                person = await _unitOfWork.Repository<Person>().GetByIdAsync(updateRequest.PersonId);
                
                if (person == null)
                {
                    throw new ApiException($"Person Not Found.");
                }
                else
                {
                    //person = _mapper.Map(updateRequest, person);
                    //await _personRepository.UpdateAsync(person);                   
                    person = _mapper.Map<Person>(updateRequest);
                    person.ClientId = _clientId; // EN-798 setting ClientId from current Client
                    await _unitOfWork.Repository<Person>().UpdateAsync(person);
                    await _unitOfWork.Commit(cancellationToken);
                    
                    return await Result<int>.SuccessAsync(person.Id, _localizer["Person Updated"]);
                }
            }
        }
    }
}
