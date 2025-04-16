using FluentValidation;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetById;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace MedHelpAuthorizations.Application.Validators.Features.IntegratedServices.ClaimStatusWorkstation
{
    public class ClaimStatusAddEditNotesDialogValidator : AbstractValidator<GetClaimStatusWorkstationNotesResponse>
    {
        public ClaimStatusAddEditNotesDialogValidator()
        {
            RuleFor(note => note.NoteContent)
                    .NotEmpty().WithMessage("Note Content is Required!")
                    .Must(x => !string.IsNullOrWhiteSpace(x) && x.Length == 250).WithMessage("Note Content must be with-in 250 characters");
        }
        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<GetClaimStatusWorkstationNotesResponse>.CreateWithOptions((GetClaimStatusWorkstationNotesResponse)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
