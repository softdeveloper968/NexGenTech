using MedHelpAuthorizations.Application.BaseFeatures.Persons;
using MedHelpAuthorizations.Application.Interfaces.Repositories;

namespace MedHelpAuthorizations.Application.Features.Persons.Commands.UpdatePerson
{
    public class UpdatePersonCommandValidator : PersonCommandValidator<UpdatePersonCommand>
    {
        public UpdatePersonCommandValidator(IPersonRepositoryAsync personRepository) : base(personRepository)
        {

        }
    }
}
