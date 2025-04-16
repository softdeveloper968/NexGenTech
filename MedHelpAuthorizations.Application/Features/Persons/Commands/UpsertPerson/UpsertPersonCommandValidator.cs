using FluentValidation;
using MedHelpAuthorizations.Application.BaseFeatures.Persons;
using MedHelpAuthorizations.Application.Interfaces.Repositories;

namespace MedHelpAuthorizations.Application.Features.Persons.Commands.UpsertPerson
{
    public class UpsertPersonCommandValidator : PersonCommandValidator<UpsertPersonCommand>
    {
        public UpsertPersonCommandValidator(IPersonRepositoryAsync personRepository) :base(personRepository)
        {

        }
    }
}
