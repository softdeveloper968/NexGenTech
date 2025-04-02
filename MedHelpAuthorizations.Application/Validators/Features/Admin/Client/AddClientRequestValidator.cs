using FluentValidation;
using MedHelpAuthorizations.Application.Requests.Admin.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.Admin.Client
{
    public class AddClientRequestValidator: AbstractValidator<AddEditAdminClientRequest>
    {
        public AddClientRequestValidator()
        {
            RuleFor(x => x.TenantId)
                .NotNull()
                .WithMessage("Tenant Id is required");

            RuleFor(x => x.ClientName)
                .NotNull()
                .NotEmpty()
                .WithMessage("Client Name is required");

            RuleFor(x => x.ClientCode)
                .NotNull()
                .NotEmpty()
                .WithMessage("Client Code is required");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .NotNull()
                .WithMessage("Phone Number is required");
            
            RuleFor(x => x.FaxNumber)
                .NotEmpty()
                .NotNull()
                .WithMessage("Fax Number is required");
        }
    }
}
