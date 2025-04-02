using MedHelpAuthorizations.Shared.Extensions;
using MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Commands.AddEdit;
using MedHelpAuthorizations.Application.Validators.Features.Documents.Commands.AddEdit;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Validators.Features.IntegratedServices.InputDocuments.Commands.AddEdit
{
    public class AddEditInputDocumentCommandValidator : AbstractValidator<AddEditInputDocumentCommand>
    {
        public AddEditInputDocumentCommandValidator(IStringLocalizer<AddEditDocumentCommandValidator> localizer)
        {
            RuleFor(request => request.InputDocumentTypeId)
               .Must(x => x > 0).WithMessage(x => localizer["InputDocumentType is required!"]);
            RuleFor(request => request.Title)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Title is required!"]);
            RuleFor(request => request.DocumentDate)
                .Must(x => x != null).WithMessage(x => localizer["Document Date is required!"]);
            //RuleFor(request => request.AuthTypeId)
            //    .Must(x => x != 0 && x != null)
            //    .When(x => x.InputDocumentTypeId == InputDocumentTypeEnum.ClaimStatusInput)
            //    .WithMessage(x => localizer[$"Service Type is required when InputDocumentType = {InputDocumentTypeEnum.ClaimStatusInput.GetDescription()}"]);
            RuleFor(request => request.ClientInsuranceId)
                .Must(x => x != 0 && x != null)
                .When(x => x.InputDocumentTypeId == InputDocumentTypeEnum.ClaimStatusInput)
                .WithMessage(x => localizer[$"Client Insurance is required when InputDocumentType = {InputDocumentTypeEnum.ClaimStatusInput.GetDescription()}"]);
        }
    }
}