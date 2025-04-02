using MedHelpAuthorizations.Application.Features.Administration.ClientDocuments.Command.AddEdit;

namespace MedHelpAuthorizations.Application.Validators.Features.Administration.ClientDocuments
{
    public class ClientDocumentValidator : AbstractValidator<AddEditClientDocumentCommand>
    {
        public ClientDocumentValidator(IStringLocalizer<AddEditClientDocumentCommand> localizer)
        {
            RuleFor(request => request.Title)
                .NotEmpty().WithMessage(localizer["Title is required!"]);

            RuleFor(request => request.Title)
                .MaximumLength(100).WithMessage(localizer["Title cannot Exceed 100 characters."]);

            RuleFor(request => request.Comments)
                .MaximumLength(500).WithMessage(localizer["Comments cannot exceed 500 characters."]);

            RuleFor(request => request.DocumentDate)
                .NotNull().WithMessage(localizer["Document Date is required!"]);
           
        }
    }

}
