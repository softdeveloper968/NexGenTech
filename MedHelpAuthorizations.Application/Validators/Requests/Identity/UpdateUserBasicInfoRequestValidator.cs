using FluentValidation;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Requests.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Requests.Identity
{
    public class UpdateUserBasicInfoRequestValidator : AbstractValidator<UpdateUserBasicInfoRequest>
    {
        public UpdateUserBasicInfoRequestValidator(IStringLocalizer<TokenRequestValidator> localizer, IUserService userService)
        {
            RuleFor(x => x.UserId).Must(x => !string.IsNullOrEmpty(x))
                     .Must((model, serverName, context) =>
                     {
                         var user = userService.GetAsync(model.UserId).Result;
                         return user.Data != null;
                     })
                .WithMessage("Invalid User Id");

            RuleFor(x => x.FirstName)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(x => localizer["First Name is required"]);

            RuleFor(x => x.LastName)
             .Must(x => !string.IsNullOrWhiteSpace(x))
             .WithMessage(x => localizer["Last Name is required"]);

            RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid Email Address")
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage(x => localizer["Email is required"])
            .Must((model, serverName, context) =>
            {
                var user = userService.GetAsync(model.UserId).Result;
                if (user.Data.Email != model.Email)
                {
                    return userService.CheckEmailAvailable(model.Email).Result;
                }
                return true;
            })
            .WithMessage("Email already in use.");

            RuleFor(x => x.UserName)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Username is required"])
            .Must((model, serverName, context) =>
            {
                var user = userService.GetAsync(model.UserId).Result;
                if (user.Data.UserName != model.UserName)
                {
                    return userService.CheckUsernameAvailable(model.UserName).Result;
                }
                return true;
            })
            .WithMessage("Username already in use.");
        }
    }
}
