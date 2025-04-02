using FluentValidation;
using MedHelpAuthorizations.Application.Features.Notes.Commands.AddEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.Notes.Commands.AddEdit
{
    public class AddEditNotesCommandValidator : AbstractValidator<AddEditNotesCommand>
    {
        public AddEditNotesCommandValidator()
        {
            RuleFor(request => request.NoteContent)
                .Must(x => !string.IsNullOrEmpty(x))
                .WithMessage("At least one character required");
            RuleFor(request => request.NoteUserId)
                .Must(x => !string.IsNullOrEmpty(x))
                .WithMessage("User required");
        }
    }
}
