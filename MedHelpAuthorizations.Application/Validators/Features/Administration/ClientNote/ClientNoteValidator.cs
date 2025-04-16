using MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Command.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.Administration.ClientNote
{
    public class ClientNoteValidator : AbstractValidator<AddEditClientNoteCommand>
    {
        public ClientNoteValidator(IStringLocalizer<AddEditClientNoteCommand> localizer)
        {
            RuleFor(request => request.Title)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Title is required!"]);
            RuleFor(request => request.Note)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Note is required!"]);
        }
    }
}
