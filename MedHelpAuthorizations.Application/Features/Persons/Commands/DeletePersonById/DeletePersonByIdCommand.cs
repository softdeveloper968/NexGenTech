using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Persons.Commands.DeletePersonById
{
    public class DeletePersonByIdCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public class DeletePersonByIdCommandHandler : IRequestHandler<DeletePersonByIdCommand, Result<int>>
        {
            private readonly IPersonRepository _personRepository;
            public DeletePersonByIdCommandHandler(IPersonRepository personRepository)
            {
                _personRepository = personRepository;
            }
            public async Task<Result<int>> Handle(DeletePersonByIdCommand command, CancellationToken cancellationToken)
            {
                var person = await _personRepository.GetByIdAsync(command.Id);
                if (person == null) 
                    throw new ApiException($"Person Not Found.");                
                
                await _personRepository.DeleteAsync(person);
                
                return await Result<int>.SuccessAsync(person.Id); 
            }
        }
    }
}
