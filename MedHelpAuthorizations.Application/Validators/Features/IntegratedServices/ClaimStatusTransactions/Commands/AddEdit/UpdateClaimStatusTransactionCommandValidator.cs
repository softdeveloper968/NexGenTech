using FluentValidation;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Update;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Validators.Features.IntegratedServices.ClaimStatusTransactions.Commands.AddEdit
{
    public class UpdateClaimStatusTransactionCommandValidator : AbstractValidator<UpdateClaimStatusTransactionCommand>
    {
        public UpdateClaimStatusTransactionCommandValidator(IStringLocalizer<UpdateClaimStatusTransactionCommandValidator> localizer)
        {
            RuleFor(request => request.ClaimStatusBatchClaimId)
               .Must(x => x > 0).WithMessage(x => localizer["ClaimStatusBatchClaimId is required!"]);
            RuleFor(request => request.TotalClaimStatusId)
                .Must(x => x != null)
                .WithMessage(x => localizer["TotalClaimStatusId is Required. If there is not one to record - record ClaimStatusEnum.None"]);
            RuleFor(request => request.ClaimLineItemStatusId)
                .Must(x => x != null)
                .When(x => x.TotalClaimStatusId == null)
                .WithMessage(x => localizer["LineItemStatusId is Required if there is not a ClaimStatusId"]);
        }
    }
}