using MedHelpAuthorizations.Application.Features.Documents.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Validators.Features.Documents.Commands.AddEdit
{
    public class AddEditDocumentCommandValidator : AbstractValidator<AddEditDocumentCommand>
    {
        public AddEditDocumentCommandValidator(IStringLocalizer<AddEditDocumentCommandValidator> localizer)
        {
            RuleFor(request => request.DocumentTypeId)
               .Must(x => x > 0).WithMessage(x => localizer["DocumentType is required!"]);
            RuleFor(request => request.URL)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["File is required!"]);
            RuleFor(request => request.DocumentDate)
                .Must(x => x != null).WithMessage(x => localizer["Document Date is required!"]);
        }
    }
}