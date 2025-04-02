using FluentValidation;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Application.Requests.Admin;
using MedHelpAuthorizations.Domain.IdentityEntities;
using MedHelpAuthorizations.Domain.IdentityEntities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.Admin
{
    public class AddEditServerRequestValidator : AbstractValidator<AddEditServerRequest>
    {
        public AddEditServerRequestValidator(IAdminUnitOfWork adminUnitOfWork)
        {

            RuleFor(x => x.ServerName)
                .Must((model, serverName, context) =>
                {
                    var isExists = adminUnitOfWork.Repository<Server, int>().Entities.Any(x => x.Id != model.ServerId && x.ServerName.ToLower().Equals(serverName));
                    if (isExists)
                    {
                        context.AddFailure("Server Name Already Exists.");
                        return false;
                    }
                    return true;
                });

            RuleFor(x => x.ServerAddress)
                .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Server address is required");

            RuleFor(x => x.ServerType)
                .NotNull().WithMessage("Server Type is required");

            RuleFor(x => x.AuthenticationType)
                .NotNull().WithMessage("Authentication Type is required");

            When(x => x.AuthenticationType == (int)AuthenticationType.Credentials, () =>
            {
                RuleFor(x => x.Username)
                    .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Username is required");

                RuleFor(x => x.Password)
                    .Must(x => !string.IsNullOrEmpty(x)).WithMessage("Password is required");
            });
        }
    }
}
