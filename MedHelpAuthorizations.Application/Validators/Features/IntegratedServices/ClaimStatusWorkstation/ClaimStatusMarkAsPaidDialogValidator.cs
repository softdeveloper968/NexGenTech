using FluentValidation;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusWorkstation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.IntegratedServices.ClaimStatusWorkstation
{
    public class ClaimStatusMarkAsPaidDialogValidator : AbstractValidator<AddEditClaimStatusMarkAsPaidCommand>
    {
        public ClaimStatusMarkAsPaidDialogValidator()
        {
            RuleFor(transaction => transaction.CheckNumber)
                       .Must(x => x != null).WithMessage("Check Number is Required!");

            RuleFor(transaction => transaction.CheckDate)
                    .Must(x => x != null).WithMessage("Check Date is Required!")
                    .Must(x => x != DateTime.MinValue).WithMessage("Check Date is Required!")
                    .Must(x => x != DateTime.MaxValue).WithMessage("Check Date is Required!")
                    .Must(date => date != default(DateTime)).WithMessage("Start date is required");

            RuleFor(transaction => transaction.LineItemPaidAmount)
                    .Must(x => x != null).WithMessage("Paid Amount is Required!")
                    .GreaterThan(0).WithMessage("Paid Amount is greater than 0.");

            RuleFor(transaction => transaction.TotalAllowedAmount)
                    .Must(x => x != null).WithMessage("Allowed Amount is Required!")
                    .GreaterThan(0).WithMessage("Allowed Amount is greater than 0.");

            RuleFor(transaction => transaction.ClaimLineItemStatusId)
                    .Must(x => x != null).WithMessage("Claim Status is Required!");


            ///As per discussion with kevin currently  not checking for these fields when validate Model.
            //RuleFor(transaction => transaction.CopayAmount).Must(x => x != null).WithMessage(x => localizer["Copay Amount is Required!"]);
            //RuleFor(transaction => transaction.CobAmount).Must(x => x != null).WithMessage(x => localizer["COB Amount is Required!"]);
            //RuleFor(transaction => transaction.CoinsuranceAmount).Must(x => x != null).WithMessage(x => localizer["Co-Insurance Amount is Required!"]);
        }
        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<AddEditClaimStatusMarkAsPaidCommand>.CreateWithOptions((AddEditClaimStatusMarkAsPaidCommand)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };

    }
}
