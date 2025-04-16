using FluentValidation;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusWorkstation;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.IntegratedServices.ClaimStatusWorkstation
{
    public class AddEditClaimStatusChangeStatusCommandValidator : AbstractValidator<AddEditClaimStatusChangeStatusCommand>
    {
        public AddEditClaimStatusChangeStatusCommandValidator(ClaimLineItemStatusEnum? previousStatusValue = null)
        {
            RuleFor(transaction => transaction.ClaimLineItemStatusId)
                    .Must(x => x != null).WithMessage("Claim Transaction Status is Required!")
                    .NotEqual(previousStatusValue).WithMessage("Claim Status can not be same as previous!");

            RuleFor(transaction => transaction.ExceptionReasonCategoryId)
                    .Must(t => t != null).WithMessage("Exception Reason Category is Required!")
                    .When(c => c.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Denied).WithMessage("Please select Exception Reason Category!");


            RuleFor(transaction => transaction.WorkstationTransactionNote.NoteContent)
                    .Must(x => x != null).WithMessage("Claim Status Workstation Reason Note is Required!")
                    .Must(x => !string.IsNullOrEmpty(x.Trim()) || !string.IsNullOrWhiteSpace(x.Trim())).WithMessage("Please mention Reason Note is Required!");
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AddEditClaimStatusChangeStatusCommand>.CreateWithOptions((AddEditClaimStatusChangeStatusCommand)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
