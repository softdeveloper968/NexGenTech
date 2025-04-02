using MedHelpAuthorizations.Application.Interfaces.Repositories;
using FluentValidation;

namespace MedHelpAuthorizations.Application.Features.Persons.Commands.CreatePerson
{
    public class CreatePersonCommandValidator : PersonCommandValidator<CreatePersonCommand>
    {
        public CreatePersonCommandValidator(IPersonRepositoryAsync personRepository) : base(personRepository)
        {

        }
    }
}
