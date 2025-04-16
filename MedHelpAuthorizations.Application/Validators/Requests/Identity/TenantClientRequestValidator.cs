using MedHelpAuthorizations.Shared.Requests.Identity;

namespace MedHelpAuthorizations.Application.Validators.Requests.Identity
{
    public class TenantClientRequestValidator : CustomValidator<TenantClientRequest>
    {
        public TenantClientRequestValidator(IStringLocalizer<TenantClientRequestValidator> localizer) //, string currentTenantIdentifier, string currentClientCode
        {
            //RuleFor(request => request.ClientCode)
            //    .Must(x => x != currentClientCode)
            //        .When(x => x.TenantIdentifier == currentTenantIdentifier)
            //        .WithMessage(x => localizer["Must select different Tenant/Client combination than current."]); 
        }
    }
}
