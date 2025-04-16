using MedHelpAuthorizations.Application.Requests.Identity;
using System.Linq;

namespace MedHelpAuthorizations.Application.Validators.Requests.Identity
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IStringLocalizer<RegisterRequestValidator> localizer)
        {
            RuleFor(request => request.FirstName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["First Name is required"]);
            RuleFor(request => request.LastName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Last Name is required"]);
            RuleFor(request => request.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Email is required"]);

            RuleFor(request => request.PhoneNumber)
                .Must(x => x.HasValue && (x.Value.ToString().Length == 10 || x.Value.ToString().Length == 11))
                .When(x => x.PhoneNumber.HasValue)
                .WithMessage(x => localizer["PhoneNumber not valid"]);                
            RuleFor(request => request.UserName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["UserName is required"])
                .MinimumLength(6).WithMessage(localizer["UserName must be at least of length 6"]);
            RuleFor(request => request.Password)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Password is required!"])
                .MinimumLength(8).WithMessage(localizer["Password must be at least of length 8"])
                .Matches(@"[A-Z]").WithMessage(localizer["Password must contain at least one capital letter"])
                .Matches(@"[a-z]").WithMessage(localizer["Password must contain at least one lowercase letter"])
                .Matches(@"[0-9]").WithMessage(localizer["Password must contain at least one digit"]);
            RuleFor(request => request.ConfirmPassword)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Password Confirmation is required!"])
                .Equal(request => request.Password).WithMessage(x => localizer["Passwords don't match"]);

            //RuleFor(request => request.ClientIds)
            //   .Must(x => x != null && x.ToList().Count > 0).WithMessage(x => localizer["At lease one client required"]);

            RuleFor(request => request.TenantIdentifiers)
               .Must(x => x != null && x.ToList().Count > 0).WithMessage(x => localizer["At lease one tenant required"]);

            RuleFor(request => request.EmployeeNumber)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["EmployeeNumber is required"])
                .When(x => x.CreateEmployee == true);

            RuleFor(request => request.DefaultEmployeeRoleId)
               .Must(x => x != null).WithMessage(x => localizer["At lease one role required"])
               .When(x => x.CreateEmployee == true);

            RuleFor(request => request.Pin)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Pin is required!"])
             .MaximumLength(5).WithMessage(localizer["Pin must be a 5 character Alpha-numeric value!"])
                .MinimumLength(5).WithMessage(localizer["Pin must be a 5 character Alpha-numeric value!"]);

            RuleFor(request => request.ConfirmPin)
				.Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Pin Confirmation is required!"])
				.Equal(request => request.Pin).WithMessage(x => localizer["Pin don't match"]);
		}
    }
}