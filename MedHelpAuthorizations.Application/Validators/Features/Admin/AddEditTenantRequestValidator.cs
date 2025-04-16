using FluentValidation;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Application.Requests.Admin;
using MedHelpAuthorizations.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Validators.Features.Admin
{
    public class AddEditTenantRequestValidator: AbstractValidator<AddEditTenantRequest>
    {
        public AddEditTenantRequestValidator(IAdminUnitOfWork adminUnitOfWork)
        {
            RuleFor(x=>x.TenantIdentifier)
                .NotEmpty().WithMessage("Tenant Identifier is required")
                .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Only numbers and Alphabets and (-,_) are allowed")
                .Must((model, serverName, context) =>
                {
                    return !adminUnitOfWork.Repository<Tenant, int>().Entities
                    .Any(t => t.Id != model.TenantId && t.Identifier.Equals(model.TenantIdentifier));
                }).WithMessage("Tenant Identifier already in use.");

            RuleFor(x=>x.TenantName)
                .NotEmpty().WithMessage("Tenant Name is required")
                .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Only numbers and Alphabets and (-,_) are allowed")
                .Must((model, serverName, context) =>
                {
                    return !adminUnitOfWork.Repository<Tenant, int>().Entities
                    .Any(t => t.Id != model.TenantId  && t.TenantName.Equals(model.TenantName));
                }).WithMessage("Tenant Name already in use.");

            RuleFor(x => x.DatabaseServerId)
            .NotNull()
            .NotEqual(0)
            .Must((model, serverName, context) =>
            {
                    var isExists = adminUnitOfWork.Repository<Server, int>().Entities.Any(x => x.Id == model.DatabaseServerId);
                    if (!isExists)
                    {
                        context.AddFailure("Invalid Database Server.");
                        return false;
                    }
                    return true;
                });


            RuleFor(x=>x.DatabaseName)
                .NotEmpty().WithMessage("Database Name is required")
                .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Only numbers and Alphabets and (-,_) are allowed");

            RuleFor(x => x.ValidUpto)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Date should be greater than today");

            RuleFor(x => x.AdminEmail).NotEmpty().WithMessage("Admin Email is required")
                .EmailAddress().WithMessage("Invalid Email Address");

            RuleFor(x => x.IsActive)
                .NotNull();
        }
    }
}
