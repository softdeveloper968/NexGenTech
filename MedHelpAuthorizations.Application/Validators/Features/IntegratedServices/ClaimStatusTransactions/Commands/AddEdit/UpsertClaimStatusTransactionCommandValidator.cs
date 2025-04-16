using FluentValidation;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Upsert;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Validators.Features.IntegratedServices.ClaimStatusTransactions.Commands.AddEdit
{
    public class UpsertClaimStatusTransactionCommandValidator : AbstractValidator<UpsertClaimStatusTransactionCommand>
    {
        public UpsertClaimStatusTransactionCommandValidator(IStringLocalizer<UpsertClaimStatusTransactionCommandValidator> localizer)
        {
            RuleFor(request => request.ClaimStatusBatchClaimId)
               .Must(x => x > 0).WithMessage(x => localizer["ClaimStatusBatchClaimId is required!"]);
            RuleFor(request => request.ClaimLineItemStatusId)
                .Must(x => x != null)
                .WithMessage(x => localizer["ClaimineItemStatusId is Required!."]);
            RuleFor(request => request.ExceptionReason)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .When(x => x.ClaimLineItemStatusId == Domain.Entities.Enums.ClaimLineItemStatusEnum.Denied 
                    || x.ClaimLineItemStatusId == Domain.Entities.Enums.ClaimLineItemStatusEnum.UnMatchedProcedureCode
                    || x.ClaimLineItemStatusId == Domain.Entities.Enums.ClaimLineItemStatusEnum.NotOnFile
                    || x.ClaimLineItemStatusId == Domain.Entities.Enums.ClaimLineItemStatusEnum.MemberNotFound
                    || x.ClaimLineItemStatusId == Domain.Entities.Enums.ClaimLineItemStatusEnum.Ignored
                    || x.ClaimLineItemStatusId == Domain.Entities.Enums.ClaimLineItemStatusEnum.Rejected)
                .WithMessage(x => localizer["ExceptionReason is Required when ClaimLineItemStatus is Denied, UnMatchedProcedureCode, NotOnFile, MemberNotFound, Ignored, or Rejected!"]); 
        }
    }
}