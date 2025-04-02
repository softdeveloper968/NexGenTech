using FluentValidation;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Requests.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Requests.Identity
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator(IStringLocalizer<TokenRequestValidator> localizer, IUserService userService)
        {
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
                 return userService.CheckEmailAvailable(model.Email).Result;
             })
            .WithMessage("Email already in use.");

            RuleFor(x => x.UserName)
            .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Username is required"])
            .Must((model, serverName, context) =>
             {
                 return userService.CheckUsernameAvailable(model.UserName).Result;
             })
            .WithMessage("Username already in use.");

            RuleFor(x => x.Password)
              .Must(x => !string.IsNullOrWhiteSpace(x))
             .WithMessage(x => localizer["Password is required"]);

            RuleFor(x => x.TenantClients)
                .Must((model, serverName, context) =>
                {
                    var tenantIds = model.TenantClients.Keys;
                    foreach (var tenantId in tenantIds)
                    {
                        if (!model.Tenants.Contains(tenantId))
                        {
                            return false;
                        }
                    }
                    return true;
                })
               .WithMessage("Invalid selection tenant and clients");
        }
    }
}
