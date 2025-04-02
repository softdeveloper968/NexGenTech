using MedHelpAuthorizations.Application.Requests.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Requests.Identity
{
    public class ToggleUserStatusRequestValidator : AbstractValidator<ToggleUserStatusRequest>
    {
        public ToggleUserStatusRequestValidator(IStringLocalizer<ToggleUserStatusRequestValidator> localizer)
        {
            RuleFor(request => request.EmployeeNumber)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["EmployeeNumber is required"])
                .When(x => x.CreateEmployee == true);

            RuleFor(request => request.DefaultEmployeeRoleId)
               .Must(x => x != null).WithMessage(x => localizer["At lease one role required"])
               .When(x => x.CreateEmployee == true);

        }
    }
}
